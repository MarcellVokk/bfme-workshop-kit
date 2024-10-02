using BfmeFoundationProject.RegistryKit.Data;
using BfmeFoundationProject.RegistryKit;
using BfmeFoundationProject.WorkshopKit.Data;
using BfmeFoundationProject.WorkshopKit.Utils;
using System.Runtime.Versioning;

namespace BfmeFoundationProject.WorkshopKit.Logic
{
    [SupportedOSPlatform("windows")]
    public static class BfmeWorkshopStateManager
    {
        public static async Task<BfmeWorkshopEntry?> GetActivePatch(int game) => await Task.Run(() => FileUtils.ReadJson<BfmeWorkshopEntry?>(Path.Combine(ConfigUtils.ConfigDirectory, $"active_patch_{game}.json"), null));
        public static async Task<Dictionary<string, BfmeWorkshopEntry>> GetActiveEnhancements(int game) => await Task.Run(() => FileUtils.ReadJson(Path.Combine(ConfigUtils.ConfigDirectory, $"active_enhancements_{game}.json"), new Dictionary<string, BfmeWorkshopEntry>()));

        public static bool IsPatchActive(int game, string entryGuid) => FileUtils.Contains(Path.Combine(ConfigUtils.ConfigDirectory, $"active_patch_{game}.json"), $"\"Guid\": \"{entryGuid}\"");
        public static bool IsEnhancementActive(int game, string entryGuid) => FileUtils.Contains(Path.Combine(ConfigUtils.ConfigDirectory, $"active_enhancements_{game}.json"), $"\"Guid\": \"{entryGuid}\"");

        public static async Task<string?> GetActiveModPath(int game)
        {
            try
            {
                BfmeWorkshopEntry? activePatch = await GetActivePatch(game);
                if (activePatch != null && activePatch.Value.Type == 1)
                {
                    if (activePatch.Value.Files.Any(x => x.Guid == "mod_folder_redirect"))
                        return activePatch.Value.Files.First(x => x.Guid == "mod_folder_redirect").Url;
                    else
                        return Path.Combine(string.Join("\\", BfmeRegistryManager.GetKeyValue(game, BfmeRegistryKey.InstallPath).Trim('\\').Trim('/').Split(['\\', '/']).SkipLast(1)), "BFME Workshop", "Mods", $"{string.Join("", activePatch.Value.Name.Select(x => Path.GetInvalidPathChars().Contains(x) ? '_' : x))}-{activePatch.Value.Guid}");
                }
            }
            catch { }
            return null;
        }
    }
}
