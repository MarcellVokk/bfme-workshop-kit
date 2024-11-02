﻿using BfmeFoundationProject.RegistryKit.Data;
using BfmeFoundationProject.RegistryKit;
using BfmeFoundationProject.WorkshopKit.Data;
using System.Runtime.Versioning;

namespace BfmeFoundationProject.WorkshopKit.Utils
{
    [SupportedOSPlatform("windows")]
    internal static class ConfigUtils
    {
        internal static string ConfigDirectory => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BFME Workshop", "Config");
        internal static string LibraryDirectory => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BFME Workshop", "Library");
        internal static List<string> IgnoredGameFiles = new List<string>()
        {
            "_zzlotr.big",
            "cs01.vp6",
            "nlc_logo.vp6",
            "te_logo.vp6",
            "cs01.vp6",
            "newlinelogo.vp6",
            "ealogo.vp6",
            "242.vp6",
            "intel.vp6",
            "thx.vp6"
        };

        internal static Dictionary<int, (string gameLanguage, string gameDirectory, string dataDirectory)> GetVirtualRegistry() => new Dictionary<int, (string gameLanguage, string gameDirectory, string dataDirectory)>
        {
            { 0, (BfmeRegistryManager.GameLanguageToLanguageCode(BfmeRegistryManager.GetKeyValue(0, BfmeRegistryKey.Language)), BfmeRegistryManager.GetKeyValue(0, BfmeRegistryKey.InstallPath).Trim('\\').Trim('/').ToLower(), BfmeRegistryManager.GetKeyValue(0, BfmeRegistryKey.UserDataLeafName)) },
            { 1, (BfmeRegistryManager.GameLanguageToLanguageCode(BfmeRegistryManager.GetKeyValue(1, BfmeRegistryKey.Language)), BfmeRegistryManager.GetKeyValue(1, BfmeRegistryKey.InstallPath).Trim('\\').Trim('/').ToLower(), BfmeRegistryManager.GetKeyValue(1, BfmeRegistryKey.UserDataLeafName)) },
            { 2, (BfmeRegistryManager.GameLanguageToLanguageCode(BfmeRegistryManager.GetKeyValue(2, BfmeRegistryKey.Language)), BfmeRegistryManager.GetKeyValue(2, BfmeRegistryKey.InstallPath).Trim('\\').Trim('/').ToLower(), BfmeRegistryManager.GetKeyValue(2, BfmeRegistryKey.UserDataLeafName)) }
        };

        internal static void DisableEnhancement(BfmeWorkshopEntry enhancementEntry, Dictionary<string, BfmeWorkshopEntry> activeEnhancements, Dictionary<int, (string gameLanguage, string gameDirectory, string dataDirectory)> virtualRegistry)
        {
            if (activeEnhancements.ContainsKey(enhancementEntry.Guid))
            {
                if (enhancementEntry.Type == 3)
                    foreach (var file in activeEnhancements[enhancementEntry.Guid].Files)
                        if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), virtualRegistry[enhancementEntry.Game].dataDirectory, "Maps", file.Name)))
                            File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), virtualRegistry[enhancementEntry.Game].dataDirectory, "Maps", file.Name));

                activeEnhancements.Remove(enhancementEntry.Guid);

                FileUtils.WriteJson(Path.Combine(ConfigDirectory, $"active_enhancements_{enhancementEntry.Game}.json"), activeEnhancements.Where(x => x.Value.Game == enhancementEntry.Game).ToDictionary());
            }
        }

        internal static void EnableEnhancement(BfmeWorkshopEntry enhancementEntry, Dictionary<string, BfmeWorkshopEntry> activeEnhancements, Dictionary<int, (string gameLanguage, string gameDirectory, string dataDirectory)> virtualRegistry)
        {
            if (activeEnhancements.ContainsKey(enhancementEntry.Guid))
                activeEnhancements[enhancementEntry.Guid] = enhancementEntry;
            else
                activeEnhancements.Add(enhancementEntry.Guid, enhancementEntry);

            FileUtils.WriteJson(Path.Combine(ConfigDirectory, $"active_enhancements_{enhancementEntry.Game}.json"), activeEnhancements.Where(x => x.Value.Game == enhancementEntry.Game).ToDictionary());
        }

        internal static async Task SaveActivePatch(BfmeWorkshopEntry activePatch)
        {
            FileUtils.WriteJson(Path.Combine(ConfigDirectory, $"active_patch_{activePatch.Game}.json"), activePatch);
            if (activePatch.Game == 2) FileUtils.WriteJson(Path.Combine(ConfigDirectory, $"active_patch_{1}.json"), await BfmeWorkshopEntry.BaseGame(1));
            if (activePatch.Game == 1 && BfmeRegistryManager.IsInstalled(2)) FileUtils.WriteJson(Path.Combine(ConfigDirectory, $"active_patch_{2}.json"), await BfmeWorkshopEntry.BaseGame(2));
        }

        internal static void SaveSyncProgress(int game, int progress)
        {
            if (progress >= 0)
            {
                try { using (StreamWriter sw = new StreamWriter(new FileStream(Path.Combine(ConfigDirectory, $"active_patch_{game}.syncing"), FileMode.Create, FileAccess.Write, FileShare.Read))) sw.Write(progress.ToString()); } catch { }
                if (game == 2) try { using (StreamWriter sw = new StreamWriter(new FileStream(Path.Combine(ConfigDirectory, $"active_patch_{1}.syncing"), FileMode.Create, FileAccess.Write, FileShare.Read))) sw.Write(progress.ToString()); } catch { }
            }
            else
            {
                for (int i = 0; i < 100; i++)
                {
                    if (!File.Exists(Path.Combine(ConfigDirectory, $"active_patch_{game}.syncing"))) break;
                    try { File.Delete(Path.Combine(ConfigDirectory, $"active_patch_{game}.syncing")); } catch { }
                }
                if (game == 2)
                {
                    for (int i = 0; i < 100; i++)
                    {
                        if (!File.Exists(Path.Combine(ConfigDirectory, $"active_patch_1.syncing"))) break;
                        try { File.Delete(Path.Combine(ConfigDirectory, $"active_patch_1.syncing")); } catch { }
                    }
                }
            }
        }
    }
}
