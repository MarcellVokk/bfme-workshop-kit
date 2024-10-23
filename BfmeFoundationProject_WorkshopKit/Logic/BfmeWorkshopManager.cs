using BfmeFoundationProject.RegistryKit.Data;
using BfmeFoundationProject.RegistryKit;
using BfmeFoundationProject.WorkshopKit.Data;
using BfmeFoundationProject.WorkshopKit.Utils;
using System.Runtime.Versioning;

namespace BfmeFoundationProject.WorkshopKit.Logic
{
    public static class BfmeWorkshopManager
    {
        public static string WorkshopServerHost = "https://bfmeladder.com";
        public static string WorkshopFilesHost = "https://workshop-files.bfmeladder.com";

        [SupportedOSPlatform("windows")]
        public static async Task<BfmeWorkshopEntry?> GetActivePatch(int game) => await Task.Run(() => FileUtils.ReadJson<BfmeWorkshopEntry?>(Path.Combine(ConfigUtils.ConfigDirectory, $"active_patch_{game}.json"), null));
        [SupportedOSPlatform("windows")]
        public static async Task<Dictionary<string, BfmeWorkshopEntry>> GetActiveEnhancements(int game) => await Task.Run(() => FileUtils.ReadJson(Path.Combine(ConfigUtils.ConfigDirectory, $"active_enhancements_{game}.json"), new Dictionary<string, BfmeWorkshopEntry>()));

        [SupportedOSPlatform("windows")]
        public static bool IsPatchActive(int game, string entryGuid) => FileUtils.Contains(Path.Combine(ConfigUtils.ConfigDirectory, $"active_patch_{game}.json"), $"\"Guid\": \"{entryGuid}\"");
        [SupportedOSPlatform("windows")]
        public static bool IsEnhancementActive(int game, string entryGuid) => FileUtils.Contains(Path.Combine(ConfigUtils.ConfigDirectory, $"active_enhancements_{game}.json"), $"\"Guid\": \"{entryGuid}\"");

        [SupportedOSPlatform("windows")]
        public static async Task<string?> GetActiveModPath(int game)
        {
            try
            {
                BfmeWorkshopEntry? activePatch = await GetActivePatch(game);
                if (activePatch != null && activePatch.Value.Type == 1)
                {
                    if (activePatch.Value.ModFolderRedirect() != "")
                        return activePatch.Value.ModFolderRedirect();
                    else
                        return Path.Combine(string.Join("\\", BfmeRegistryManager.GetKeyValue(game, BfmeRegistryKey.InstallPath).Trim('\\').Trim('/').Split(['\\', '/']).SkipLast(1)), "BFME Workshop", "Mods", $"{string.Join("", activePatch.Value.Name.Select(x => Path.GetInvalidPathChars().Contains(x) ? '_' : x))}-{activePatch.Value.Guid}");
                }
            }
            catch { }
            return null;
        }
    }
}
