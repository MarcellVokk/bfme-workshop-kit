using BfmeWorkshopKit.Data;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Security.Cryptography;

namespace BfmeWorkshopKit.Logic
{
    public static class BfmeWorkshopSyncManager
    {
        private static HttpClient HttpClient = new HttpClient() { Timeout = Timeout.InfiniteTimeSpan };

        public static Action<BfmeWorkshopEntry>? OnSyncBegin;
        public static Action<int>? OnSyncUpdate;
        public static Action? OnSyncEnd;

        private static Dictionary<int, (string gameLanguage, string gameDirectory)> GetVirtualRegistry() => new Dictionary<int, (string gameLanguage, string gameDirectory)>
        {
            { 0, (LanguageToLanguageCode(GetRegistryValue(0, "Language")), GetRegistryValue(0, "Install Dir")) },
            { 1, (LanguageToLanguageCode(GetRegistryValue(1, "Language")), GetRegistryValue(1, "Install Dir")) },
            { 2, (LanguageToLanguageCode(GetRegistryValue(2, "Language")), GetRegistryValue(2, "Install Dir")) }
        };

        public static async Task Sync(BfmeWorkshopEntry entry, Action<int> OnProgressUpdate, Action<string, int> OnDownloadUpdate)
        {
            OnProgressUpdate.Invoke(0);
            OnSyncBegin?.Invoke(entry);

            try
            {
                File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BFME Workshop", $"active-patch-{entry.Game}.json"), JsonConvert.SerializeObject(entry, Formatting.Indented));

                List<(BfmeWorkshopFile file, int game, bool isGameFile)> files = entry.Files.Select(x => (x, entry.Game, false)).ToList();
                double progress = 0;
                int reportedProgress = 0;
                var virtualRegistry = GetVirtualRegistry();

                if (entry.Game == 0)
                    files.AddRange((await BfmeWorkshopQueryManager.Get("original-BFME1")).entry.Files.Select(x => (file: x, game: 0, isGameFile: true)).Where(x => !files.Any(y => y.game == x.game && y.file.Name == x.file.Name)));
                if (entry.Game == 1 || entry.Game == 2)
                    files.AddRange((await BfmeWorkshopQueryManager.Get("original-BFME2")).entry.Files.Select(x => (file: x, game: 1, isGameFile: true)).Where(x => !files.Any(y => y.game == x.game && y.file.Name == x.file.Name)));
                if (entry.Game == 2)
                    files.AddRange((await BfmeWorkshopQueryManager.Get("original-RotWK")).entry.Files.Select(x => (file: x, game: 2, isGameFile: true)).Where(x => !files.Any(y => y.game == x.game && y.file.Name == x.file.Name)));

                files = files.Where(x => x.file.Language == "ALL" || x.file.Language == "" || x.file.Language == virtualRegistry[x.game].gameLanguage).ToList();

                if (files.Select(x => x.game).Distinct().Any(x => virtualRegistry[x].gameDirectory == ""))
                    throw new BfmeWorkshopGameNotInstalledSyncException("One or more bfme game required by this mod/patch is not installed.");

                foreach (Process p in Process.GetProcessesByName("game.dat"))
                    p.Kill();
                foreach (Process p in Process.GetProcessesByName("lotrbfme"))
                    p.Kill();
                foreach (Process p in Process.GetProcessesByName("lotrbfme2"))
                    p.Kill();
                foreach (Process p in Process.GetProcessesByName("lotrbfme2ep2"))
                    p.Kill();

                foreach (var file in files)
                {
                    await EnsurePatchFileIdentity(file.file, file.game, file.isGameFile, virtualRegistry, OnDownloadUpdate);

                    progress++;
                    if (reportedProgress != (int)(progress / files.Count * 100d))
                    {
                        reportedProgress = (int)(progress / files.Count * 100d);
                        OnProgressUpdate.Invoke(reportedProgress);
                        OnSyncUpdate?.Invoke(reportedProgress);
                    }
                }

                foreach (var game in files.Select(x => x.game).Distinct())
                    foreach (string file in Directory.GetFiles(virtualRegistry[game].gameDirectory, "*.*", SearchOption.AllDirectories))
                        if (!files.Any(x => file == Path.Combine(virtualRegistry[game].gameDirectory, x.file.Name)))
                            File.Delete(file);
            }
            catch
            {
                throw;
            }
            finally
            {
                OnSyncEnd?.Invoke();
            }
        }

        public static BfmeWorkshopEntry? GetActivePatch(int game)
        {
            try
            {
                if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BFME Workshop", $"active-patch-{game}.json")))
                    return JsonConvert.DeserializeObject<BfmeWorkshopEntry>(File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BFME Workshop", $"active-patch-{game}.json")));
            }
            catch { }
            return null;
        }

        private static async Task EnsurePatchFileIdentity(BfmeWorkshopFile file, int game, bool isGameFile, Dictionary<int, (string gameLanguage, string gameDirectory)> virtualRegistry, Action<string, int> OnDownloadUpdate)
        {
            string gameDirectory = virtualRegistry[game].gameDirectory;
            string cacheDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BFME Workshop", "Cache");

            if (!Directory.Exists(cacheDirectory))
                Directory.CreateDirectory(cacheDirectory);

            if (!Directory.Exists(Path.GetDirectoryName(Path.Combine(gameDirectory, file.Name))))
                Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(gameDirectory, file.Name)) ?? "");

            if (File.Exists(Path.Combine(gameDirectory, file.Name)))
            {
                string hash = await Task.Run(() => GetFileMd5Hash(Path.Combine(gameDirectory, file.Name)));
                if (hash == file.Md5)
                {
                    if (!isGameFile && !File.Exists(Path.Combine(cacheDirectory, $"{file.Guid}.pfcache")))
                        await Task.Run(() => File.Copy(Path.Combine(gameDirectory, file.Name), Path.Combine(cacheDirectory, $"{file.Guid}.pfcache"), true));

                    return;
                }
            }

            if (File.Exists(Path.Combine(cacheDirectory, $"{file.Guid}.pfcache")))
            {
                string hash = await Task.Run(() => GetFileMd5Hash(Path.Combine(cacheDirectory, $"{file.Guid}.pfcache")));
                if (hash == file.Md5)
                {
                    await Task.Run(() => File.Copy(Path.Combine(cacheDirectory, $"{file.Guid}.pfcache"), Path.Combine(gameDirectory, file.Name), true));
                    return;
                }
            }

            OnDownloadUpdate.Invoke($"Downloading {file.Name}", 0);
            await Download(file.Url, isGameFile ? Path.Combine(gameDirectory, file.Name) : Path.Combine(cacheDirectory, $"{file.Guid}.pfcache"), (downloadProgress) =>
            {
                OnDownloadUpdate.Invoke($"Downloading {file.Name}", downloadProgress);
                return false;
            });
            OnDownloadUpdate.Invoke("", 0);

            if (!isGameFile)
                await Task.Run(() => File.Copy(Path.Combine(cacheDirectory, $"{file.Guid}.pfcache"), Path.Combine(gameDirectory, file.Name), true));
        }

        private static string GetRegistryValue(int game, string key)
        {
            string gameRegistry = "";
            if (game == 0)
                gameRegistry = @$"SOFTWARE\{(IntPtr.Size == 8 ? "WOW6432Node" : "")}\EA Games\The Battle for Middle-earth";
            else if (game == 1)
                gameRegistry = @$"SOFTWARE\{(IntPtr.Size == 8 ? "WOW6432Node" : "")}\Electronic Arts\The Battle for Middle-earth II";
            else if (game == 2)
                gameRegistry = @$"SOFTWARE\{(IntPtr.Size == 8 ? "WOW6432Node" : "")}\Electronic Arts\The Lord of the Rings, The Rise of the Witch-king";

            try
            {
                RegistryKey? registryKey = Registry.LocalMachine.OpenSubKey(gameRegistry, false);

                if (registryKey != null)
                {
                    string result = registryKey.GetValue(key) as string ?? "";
                    registryKey.Close();
                    return result;
                }
            }
            catch { }

            return "";
        }

        private static string GetFileMd5Hash(string path)
        {
            using (var md5 = MD5.Create())
            using (var stream = File.OpenRead(path))
            {
                var hash = md5.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }

        private static string LanguageToLanguageCode(string language)
        {
            Dictionary<string, string> languages = new Dictionary<string, string>()
            {
                { "english", "EN" },
                { "french", "FR" },
                { "german", "DE" },
                { "italian", "IT" },
                { "spanish", "ES" },
                { "swedish", "SV" },
                { "dutch", "NL" },
                { "polish", "PL" },
                { "norwegian", "NO" },
                { "russian", "RU" }
            };

            if(languages.ContainsKey(language.ToLower()))
                return languages[language.ToLower()];

            return language.ToLower();
        }

        private static async Task Download(string url, string localPath, Func<int, bool> OnProgressUpdate)
        {
            using (var response = await HttpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();

                var totalBytes = response.Content.Headers.ContentLength ?? -1L;
                var totalBytesRead = 0L;
                var buffer = new byte[4096];
                var isMoreToRead = true;
                int progressInPercent = 0;

                using (var fileStream = new FileStream(localPath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
                using (var stream = await response.Content.ReadAsStreamAsync())
                    do
                    {
                        var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                        if (bytesRead == 0)
                        {
                            isMoreToRead = false;
                            continue;
                        }

                        await fileStream.WriteAsync(buffer, 0, bytesRead);

                        totalBytesRead += bytesRead;
                        int newProgressInPercent = (int)(totalBytesRead * 100 / totalBytes);

                        if (progressInPercent != newProgressInPercent)
                        {
                            progressInPercent = newProgressInPercent;
                            if (OnProgressUpdate.Invoke(newProgressInPercent))
                                return;
                        }
                    }
                    while (isMoreToRead);
            }
        }
    }

    public class BfmeWorkshopGameNotInstalledSyncException : Exception
    {
        public BfmeWorkshopGameNotInstalledSyncException(string message) : base(message) { }
    }
}