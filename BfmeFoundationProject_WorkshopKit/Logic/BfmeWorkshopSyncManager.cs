using BfmeFoundationProject.RegistryKit;
using BfmeFoundationProject.WorkshopKit.Data;
using BfmeFoundationProject.WorkshopKit.Utils;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Versioning;

namespace BfmeFoundationProject.WorkshopKit.Logic
{
    [SupportedOSPlatform("windows")]
    public static class BfmeWorkshopSyncManager
    {
        private static HttpClient HttpClient = new HttpClient() { Timeout = Timeout.InfiniteTimeSpan };
        private static FileSystemWatcher ActivePatchWatcher;

        public static bool Syncing { get; private set; } = false;
        public static Action<BfmeWorkshopEntry>? OnSyncBegin;
        public static Action<int>? OnSyncUpdate;
        public static Action? OnSyncEnd;

        static BfmeWorkshopSyncManager()
        {
            if (!Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BFME Workshop", "Config")))
                Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BFME Workshop", "Config"));

            ActivePatchWatcher = new FileSystemWatcher();
            ActivePatchWatcher.IncludeSubdirectories = true;
            ActivePatchWatcher.Path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BFME Workshop", "Config");
            ActivePatchWatcher.NotifyFilter = NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastWrite;
            ActivePatchWatcher.Filter = "*.*";
            ActivePatchWatcher.Created += (s, e) =>
            {
                if (Path.GetFileName(e.FullPath).StartsWith("active_patch_") && Path.GetFileName(e.FullPath).EndsWith(".syncing") && File.Exists(e.FullPath.Replace(".syncing", ".json")))
                    try { Syncing = true; OnSyncBegin?.Invoke(JsonConvert.DeserializeObject<BfmeWorkshopEntry>(File.ReadAllText(e.FullPath.Replace(".syncing", ".json")))); } catch { }
            };
            ActivePatchWatcher.Deleted += (s, e) =>
            {
                if (Path.GetFileName(e.FullPath).StartsWith("active_patch_") && Path.GetFileName(e.FullPath).EndsWith(".syncing") && Syncing)
                    try { Syncing = false; OnSyncEnd?.Invoke(); } catch { }
            };
            ActivePatchWatcher.Changed += (s, e) =>
            {
                if (Path.GetFileName(e.FullPath).StartsWith("active_patch_") && Path.GetFileName(e.FullPath).EndsWith(".syncing") && Syncing)
                    try { OnSyncUpdate?.Invoke(int.Parse(File.ReadAllText(e.FullPath))); } catch { }
            };
            ActivePatchWatcher.EnableRaisingEvents = true;
        }

        public static async Task Sync(BfmeWorkshopEntry entry, List<string>? enhancements = null, Action<int>? OnProgressUpdate = null)
        {
            ActivePatchWatcher.EnableRaisingEvents = false;
            Syncing = true;
            OnProgressUpdate?.Invoke(0);
            OnSyncBegin?.Invoke(entry);

            try
            {
                if (!Directory.Exists(ConfigUtils.ConfigDirectory)) Directory.CreateDirectory(ConfigUtils.ConfigDirectory);

                double progress = 0;
                int reportedProgress = 0;
                var keybindsDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BFME Workshop", "Keybinds", $"{entry.GameName()}-{entry.Name}");
                var virtualRegistry = ConfigUtils.GetVirtualRegistry();
                var activeEnhancements = await BfmeWorkshopStateManager.GetActiveEnhancements(entry.Game);
                var dependencies = new List<BfmeWorkshopEntry>();
                var files = new List<(BfmeWorkshopFile file, BfmeWorkshopEntry entry, bool isBaseFile)>();

                if (entry.Type == 0 || entry.Type == 1 || entry.Type == 4)
                {
                    // This is a patch or a mod, act accordingly
                    await Task.Run(async () =>
                    {
                        // Close the game to avoid any IO operation issues caused by files being locked by the game.
                        KillGame();

                        // Ensure DirectX9 is installed on the system.
                        await DirectXRuntime.DirectXRuntimeManager.EnsureRuntimes();

                        // Ensure all default files and registry keys are created for the game.
                        BfmeRegistryManager.EnsureDefaults(entry.Game);

                        // Update the active patch and signal that a sync has started
                        await ConfigUtils.SaveActivePatch(entry);
                        ConfigUtils.SaveSyncProgress(entry.Game, 0);

                        // If rotwk, add bfme2's enabled enhancements to active enhancements
                        if (entry.Game == 2)
                            activeEnhancements = activeEnhancements.Concat(await BfmeWorkshopStateManager.GetActiveEnhancements(1)).ToDictionary();

                        // If bfme2, add rotwk's enabled enhancements to active enhancements
                        if (entry.Game == 1 && BfmeRegistryManager.IsInstalled(2))
                            activeEnhancements = activeEnhancements.Concat(await BfmeWorkshopStateManager.GetActiveEnhancements(2)).ToDictionary();

                        // Download all dependencies
                        foreach (var dependency in entry.Dependencies)
                            dependencies.Add(await BfmeWorkshopDownloadManager.Download(dependency));

                        // Download all implicit dependencies
                        while (dependencies.SelectMany(x => x.Dependencies).Any(x => !dependencies.Any(y => y.Guid == x.Split(':')[0])))
                            foreach (var dependency in dependencies.SelectMany(x => x.Dependencies).Where(x => !dependencies.Any(y => y.Guid == x.Split(':')[0])).ToList())
                                dependencies.Add(await BfmeWorkshopDownloadManager.Download(dependency));

                        // Download all explicit enhancements. Make sure it isn't already a dependency
                        foreach (var explicitEnhancement in (enhancements ?? new List<string>()).Where(x => !entry.Dependencies.Any(y => y.Split(':')[0] == x.Split(':')[0])))
                            dependencies.Add(await BfmeWorkshopDownloadManager.Download(explicitEnhancement));

                        // If the curent patch is a mod, and a patch (whose game is not the same as the curent patches) was listed as a dependency, set the active patch of that game to it
                        if (entry.Type == 1 && dependencies.Any(x => x.Type == 0 && x.Game != entry.Game)) await ConfigUtils.SaveActivePatch(dependencies.First(x => x.Type == 0 && x.Game != entry.Game));

                        // Disable all enhancements that dont have the curent patch listed as a dependency, meaning they are not compatible (and if rotwk, disable all bfme2 enhancements).
                        // If there are explicit enhancements specified, disable all enhancements except those and the dependencies specified with the curent entry.
                        foreach (var enhancement in activeEnhancements.Values.Where(x => (x.Type == 2 && !x.Dependencies.Contains(entry.Guid) && !x.Dependencies.Contains($"{entry.Guid}:{entry.Version}")) || x.Game != entry.Game || enhancements != null))
                            ConfigUtils.DisableEnhancement(enhancement, activeEnhancements, virtualRegistry);

                        // Enable all enhancements that are listed as dependencies (explicitly or implicitly) of the curent patch
                        foreach (var dependency in dependencies.Where(x => (x.Type == 2 || x.Type == 3) && (x.Dependencies.Contains(entry.Guid) || x.Dependencies.Contains($"{entry.Guid}:{entry.Version}") || dependencies.Any(y => x.Dependencies.Contains(y.Guid)) || dependencies.Any(y => x.Dependencies.Contains($"{y.Guid}:{y.Version}")))))
                            ConfigUtils.EnableEnhancement(dependency, activeEnhancements, virtualRegistry);

                        // Prepare the files needed for this configuration. Files added sooner have priority over files added later (files added sooner overwrite files added later with the same name)
                        // Files in L1 overwrite files in L2, L3 and L4, files in L2 overwrite files in L3 and L4, etc...

                        // L1 - Files from active enhancements
                        foreach (var enhancement in activeEnhancements.Values) AddFiles(enhancement, false, files, virtualRegistry);

                        // L2 - Files from the curent patch
                        AddFiles(entry, entry.IsBaseGame(), files, virtualRegistry);

                        // L3 - Files from patches that are referenced as dependencies
                        foreach (var dependency in dependencies.Where(x => x.Type == 0)) AddFiles(dependency, false, files, virtualRegistry);

                        // L4 - Base game files (and if rotwk, bfme2's base game files. if bfme2, rotwk's base game files if it's installed)
                        if (!entry.IsBaseGame()) AddFiles(await BfmeWorkshopEntry.BaseGame(entry.Game), true, files, virtualRegistry);
                        if (entry.Game == 2) AddFiles(await BfmeWorkshopEntry.BaseGame(1), true, files, virtualRegistry);
                        if (entry.Game == 1 && BfmeRegistryManager.IsInstalled(2)) AddFiles(await BfmeWorkshopEntry.BaseGame(2), true, files, virtualRegistry);

                        // The files are now properly configured.

                        // Ensure all games referenced by this configuration are installed.
                        if (files.Select(x => x.entry.Game).Distinct().Any(x => virtualRegistry[x].gameDirectory == "" || !Directory.Exists(virtualRegistry[x].gameDirectory)))
                            throw new BfmeWorkshopGameNotInstalledSyncException("One or more bfme game required by this mod/patch is not installed.");

                        // Ensure that none of the games referenced by this configuration are installed in a root directory (for example C:/ as this could cause catastrophic damage)
                        if (files.Select(x => x.entry.Game).Distinct().Any(x => virtualRegistry[x].gameDirectory == "" || !Directory.Exists(virtualRegistry[x].gameDirectory) || virtualRegistry[x].gameDirectory.Replace("/", "\\").Trim('\\').Split('\\').Length <= 1))
                            throw new BfmeWorkshopGameNotInstalledSyncException("One or more bfme game required by this mod/patch is installed in a root directory (for example C:/). The sync was aborted as continuing could cause catastrophic damage. Your files are safe, nothing was modified!");

                        // Begin the syncing process.
                        await Parallel.ForEachAsync(files, async (file, ct) =>
                        {
                            await EnsureFileExists(file.file, file.entry, file.isBaseFile, virtualRegistry);

                            progress++;
                            if (reportedProgress != (int)(progress / files.Count * 100d))
                            {
                                reportedProgress = (int)(progress / files.Count * 100d);
                                OnProgressUpdate?.Invoke(reportedProgress);
                                OnSyncUpdate?.Invoke(reportedProgress);
                                ConfigUtils.SaveSyncProgress(entry.Game, reportedProgress);
                            }
                        });

                        // Delete all files that are not required by the curent configuration
                        foreach (var game in files.Select(x => x.entry.Game).Distinct())
                            CleanUpFiles(entry, game, files, virtualRegistry, virtualRegistry[game].gameDirectory);

                        // Ensure the keybinds directory exists
                        if (!Directory.Exists(keybindsDirectory))
                            Directory.CreateDirectory(keybindsDirectory);

                        // Copy all the files from the keybinds folder to the game\lang folder folder
                        foreach (var langFileOverride in Directory.GetFiles(keybindsDirectory, "*.big"))
                            try { File.Copy(langFileOverride, Path.Combine(virtualRegistry[entry.Game].gameDirectory, "lang", Path.GetFileName(langFileOverride)), true); } catch { }

                        if (File.Exists(Path.Combine(keybindsDirectory, "_zzlotr.big")))
                            try { File.Copy(Path.Combine(keybindsDirectory, "_zzlotr.big"), Path.Combine(virtualRegistry[entry.Game].gameDirectory, "_zzlotr.big"), true); } catch { }
                    });
                }
                else
                {
                    // This is an enhancement or map mack, act accordingly
                    await Task.Run(async () =>
                    {
                        // Get the curently active patch, and if it's not set yet, set it to the base vanilla patch
                        BfmeWorkshopEntry activePatch = await BfmeWorkshopStateManager.GetActivePatch(entry.Game) ?? await BfmeWorkshopEntry.BaseGame(entry.Game);

                        await ConfigUtils.SaveActivePatch(activePatch);

                        if (activeEnhancements.ContainsKey(entry.Guid) && activeEnhancements[entry.Guid].Version == entry.Version)
                            ConfigUtils.DisableEnhancement(entry, activeEnhancements, virtualRegistry); // If the enhancement is already enabled, disable it
                        else if (entry.Type != 2 || entry.Dependencies.Count == 0 || entry.Dependencies.Contains(activePatch.Guid))
                            ConfigUtils.EnableEnhancement(entry, activeEnhancements, virtualRegistry); // If the enhancement is not enabled yet, and is compatible with the active patch, enable it
                        else // If it is not compatible, throw an error
                            throw new BfmeWorkshopEnhancementIncompatibleSyncException($"\"{entry.Name}\" is not compatible with \"{activePatch.Name}\"");

                        // Resyncing the curently active patch to apply the changes
                        OnSyncEnd?.Invoke();
                        await Sync(activePatch, enhancements, OnProgressUpdate);
                    });
                    return;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                Syncing = false;
                OnSyncEnd?.Invoke();
                await Task.Run(() => ConfigUtils.SaveSyncProgress(entry.Game, -1));
                ActivePatchWatcher.EnableRaisingEvents = true;
            }
        }

        public static async Task<BfmeWorkshopEntry> CreateSnapshot(int game, bool cloneFiles = true)
        {
            var virtualRegistry = ConfigUtils.GetVirtualRegistry();
            string cacheDirectory = Path.Combine(virtualRegistry[game].gameDirectory.Split(['\\', '/'])[0], "BFME Workshop", "Cache");
            var baseGame = await BfmeWorkshopEntry.BaseGame(game);
            var files = new List<BfmeWorkshopFile>();

            if (!Directory.Exists(cacheDirectory)) Directory.CreateDirectory(cacheDirectory);

            await Task.Run(async () => await scanFolder(virtualRegistry[game].gameDirectory));

            async Task scanFolder(string folder)
            {
                // If bfme2, and this folder is where rotwk is installed, skip this folder
                if (game == 1 && virtualRegistry[2].gameDirectory != "" && Directory.Exists(virtualRegistry[2].gameDirectory) && folder.ToLower().Trim('\\').Trim('/') == virtualRegistry[2].gameDirectory)
                    return;

                if (folder.Replace("/", "\\").Split('\\').Last() == "BFME Workshop")
                    return;

                // Check every file in this folder, and if it has been modified compared to the base game, add it to the modified files list
                foreach (var file in Directory.GetFiles(folder))
                {
                    string name = file.ToLower().Replace(virtualRegistry[game].gameDirectory, "").Trim('\\').Trim('/');
                    var md5 = await FileUtils.Hash(file);
                    if (baseGame.Files.Any(x => x.Name.ToLower() == name && x.Md5 == md5)) continue;
                    if (cloneFiles) File.Copy(file, Path.Combine(cacheDirectory, $"{md5}.pfcache"), true);
                    files.Add(new BfmeWorkshopFile() { Guid = md5, Name = name, Language = "ALL", Md5 = md5, Url = cloneFiles ? Path.Combine(cacheDirectory, $"{md5}.pfcache") : file });
                }

                // Call this method recursively for all subfolders
                foreach (var subFolder in Directory.GetDirectories(folder))
                    await scanFolder(subFolder);
            }

            return new BfmeWorkshopEntry() { Guid = $"snapshot-{(game == 2 ? "RotWK" : $"BFME{game + 1}")}-{Guid.NewGuid()}", Name = $"{(game == 2 ? "RotWK" : $"BFME{game + 1}")} Snapshot", Version = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss"), Description = "", ArtworkUrl = "https://workshop-files.bfmeladder.com/snapshot-artwork.png", Author = "Me", Owner = "", Game = game, Type = 4, CreationTime = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds(), Files = files, Dependencies = new List<string>() };
        }

        private static void AddFiles(BfmeWorkshopEntry entry, bool isBaseEntry, List<(BfmeWorkshopFile file, BfmeWorkshopEntry entry, bool isBaseFile)> files, Dictionary<int, (string gameLanguage, string gameDirectory, string dataDirectory)> virtualRegistry)
        {
            // This function adds the files from this entry to the curent configuration in a way that ensures that files with the same name and the same target destination directory are only added once.
            // Moreover, it also ensures only files that match the curent language are added.
            List<int[]> fileTypeConflictMap = [[0, 2, 4], [1], [0, 2, 4], [3], [0, 2, 4]];
            List<string> conflictingFiles = files.SelectMany(x => fileTypeConflictMap[x.entry.Type].Select(y => $"{x.entry.Game}:{y}:{x.file.Name.ToLower()}")).ToList();

            var entryFiles = entry.Files.DistinctBy(x => x.Name.ToLower()).Select(x => (file: x, entry: entry, isBaseFile: isBaseEntry)).Where(x => !ConfigUtils.IgnoredGameFiles.Contains(Path.GetFileName(x.file.Name).ToLower()) && !conflictingFiles.Contains($"{x.entry.Game}:{x.entry.Type}:{x.file.Name.ToLower()}"));
            if (entryFiles.Any(x => x.file.Language.Split(' ').Contains(virtualRegistry[x.entry.Game].gameLanguage)))
                files.AddRange(entryFiles.Where(x => x.file.Language == "ALL" || x.file.Language == "" || x.file.Language.Split(' ').Contains(virtualRegistry[x.entry.Game].gameLanguage)));
            else
                files.AddRange(entryFiles.Where(x => x.file.Language == "ALL" || x.file.Language == "" || x.file.Language.Split(' ').Contains("EN")));
        }

        private static async Task EnsureFileExists(BfmeWorkshopFile file, BfmeWorkshopEntry entry, bool isBaseFile, Dictionary<int, (string gameLanguage, string gameDirectory, string dataDirectory)> virtualRegistry)
        {
            string fileName = file.Name.TrimStart('\\').TrimStart('/');
            string cacheDirectory = Path.Combine(string.Join('\\', virtualRegistry[entry.Game].gameDirectory.Split(['\\', '/']).SkipLast(1)), "BFME Workshop", "Cache");
            string secondaryCacheDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BFME Workshop", "Cache");
            string destinationDirectory = entry.Type switch
            {
                0 => virtualRegistry[entry.Game].gameDirectory,
                1 => Path.Combine(string.Join('\\', virtualRegistry[entry.Game].gameDirectory.Split(['\\', '/']).SkipLast(1)), "BFME Workshop", "Mods", $"{string.Join("", entry.Name.Select(x => Path.GetInvalidPathChars().Contains(x) ? '_' : x))}-{entry.Guid}"),
                2 => virtualRegistry[entry.Game].gameDirectory,
                3 => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), virtualRegistry[entry.Game].dataDirectory, "Maps"),
                4 => virtualRegistry[entry.Game].gameDirectory,
                _ => virtualRegistry[entry.Game].gameDirectory
            };

            if (file.Guid == "mod_folder_redirect")
                return;

            if (!file.Url.StartsWith("http"))
            {
                if (File.Exists(file.Url))
                    return;

                throw new BfmeWorkshopFileMissingSyncException($"The file {file.Url} is missing.");
            }

            if (!Directory.Exists(cacheDirectory)) Directory.CreateDirectory(cacheDirectory);
            if (!Directory.Exists(Path.GetDirectoryName(Path.Combine(destinationDirectory, fileName)))) Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(destinationDirectory, fileName)) ?? "");

            // If the file exists in the destination directory and the file is the same as requested, do nothing
            if (File.Exists(Path.Combine(destinationDirectory, fileName)))
            {
                string hash = await FileUtils.Hash(Path.Combine(destinationDirectory, fileName));
                if (hash == file.Md5)
                {
                    if (!isBaseFile && entry.Type != 1 && !File.Exists(Path.Combine(cacheDirectory, $"{file.Guid}.pfcache")))
                        await Task.Run(() => File.Copy(Path.Combine(destinationDirectory, fileName), Path.Combine(cacheDirectory, $"{file.Guid}.pfcache"), true));
                    else if (!isBaseFile && entry.Type != 1 && await FileUtils.Hash(Path.Combine(cacheDirectory, $"{file.Guid}.pfcache")) != hash)
                        await Task.Run(() => File.Copy(Path.Combine(destinationDirectory, fileName), Path.Combine(cacheDirectory, $"{file.Guid}.pfcache"), true));
                    return;
                }
            }

            // If this is not a mod, try to load the file from the local cache
            if (entry.Type != 1)
            {
                if (File.Exists(Path.Combine(cacheDirectory, $"{file.Guid}.pfcache")))
                {
                    string hash = await FileUtils.Hash(Path.Combine(cacheDirectory, $"{file.Guid}.pfcache"));
                    if (hash == file.Md5)
                    {
                        await Task.Run(() => File.Copy(Path.Combine(cacheDirectory, $"{file.Guid}.pfcache"), Path.Combine(destinationDirectory, fileName), true));
                        return;
                    }
                }

                if (File.Exists(Path.Combine(secondaryCacheDirectory, $"{file.Guid}.pfcache")))
                {
                    string hash = await FileUtils.Hash(Path.Combine(secondaryCacheDirectory, $"{file.Guid}.pfcache"));
                    if (hash == file.Md5)
                    {
                        await Task.Run(() => File.Copy(Path.Combine(secondaryCacheDirectory, $"{file.Guid}.pfcache"), Path.Combine(destinationDirectory, fileName), true));
                        return;
                    }
                }
            }

            // If the file was not found in neither the destination directory nor in the local cache, download the file to either the cache or directly to the destination
            await HttpUtils.Download(file.Url, (isBaseFile || entry.Type == 1) ? Path.Combine(destinationDirectory, fileName) : Path.Combine(cacheDirectory, $"{file.Guid}.pfcache"));

            // If the file was downloaded into cache, copy the file from the cache directory to the destination
            if (!isBaseFile && entry.Type != 1)
                await Task.Run(() => File.Copy(Path.Combine(cacheDirectory, $"{file.Guid}.pfcache"), Path.Combine(destinationDirectory, fileName), true));
        }

        private static void CleanUpFiles(BfmeWorkshopEntry entry, int game, List<(BfmeWorkshopFile file, BfmeWorkshopEntry entry, bool isBaseFile)> files, Dictionary<int, (string gameLanguage, string gameDirectory, string dataDirectory)> virtualRegistry, string root)
        {
            // If not rotwk, and this folder is where rotwk is installed, skip this folder (rotwk can be installed inside bfme2's folder, and we don't want to delete it)
            if (game != 2 && virtualRegistry[2].gameDirectory != "" && Directory.Exists(virtualRegistry[2].gameDirectory) && root.ToLower().Trim('\\').Trim('/') == virtualRegistry[2].gameDirectory)
                return;

            if (root.Replace("/", "\\").Split('\\').Last() == "BFME Workshop")
                return;

            // Delete all files in this directory that are not in IgnoredGameFiles, and are not found in the curent file configuration
            foreach (string file in Directory.GetFiles(root))
                if (!ConfigUtils.IgnoredGameFiles.Contains(Path.GetFileName(file).ToLower()) && !files.Any(x => x.entry.Game == game && file.ToLower() == Path.Combine(virtualRegistry[x.entry.Game].gameDirectory, x.file.Name.ToLower().TrimStart('\\').TrimStart('/'))))
                    File.Delete(file);

            // Call this method recursively on all subfolders in this directory
            foreach (string subFolder in Directory.GetDirectories(root))
                CleanUpFiles(entry, game, files, virtualRegistry, subFolder);

            // If this directory is fully empty, delete it
            if (Directory.GetFiles(root).Length == 0 && Directory.GetDirectories(root).Length == 0)
                Directory.Delete(root, true);
        }

        private static void KillGame()
        {
            foreach (Process p in Process.GetProcessesByName("game.dat"))
                try { p.Kill(); } catch { }
            foreach (Process p in Process.GetProcessesByName("lotrbfme"))
                try { p.Kill(); } catch { }
            foreach (Process p in Process.GetProcessesByName("lotrbfme2"))
                try { p.Kill(); } catch { }
            foreach (Process p in Process.GetProcessesByName("lotrbfme2ep1"))
                try { p.Kill(); } catch { }
        }
    }

    public class BfmeWorkshopGameNotInstalledSyncException : Exception
    {
        public BfmeWorkshopGameNotInstalledSyncException(string message) : base(message) { }
    }

    public class BfmeWorkshopEnhancementIncompatibleSyncException : Exception
    {
        public BfmeWorkshopEnhancementIncompatibleSyncException(string message) : base(message) { }
    }

    public class BfmeWorkshopFileMissingSyncException : Exception
    {
        public BfmeWorkshopFileMissingSyncException(string message) : base(message) { }
    }
}