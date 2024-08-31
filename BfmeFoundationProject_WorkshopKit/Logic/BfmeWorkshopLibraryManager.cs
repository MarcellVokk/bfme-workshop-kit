using BfmeFoundationProject.WorkshopKit.Data;
using BfmeFoundationProject.WorkshopKit.Utils;
using Newtonsoft.Json;
using System.Runtime.Versioning;

namespace BfmeFoundationProject.WorkshopKit.Logic
{
    [SupportedOSPlatform("windows")]
    public static class BfmeWorkshopLibraryManager
    {
        public static async Task<List<BfmeWorkshopEntryPreview>> Query(string keyword = "", int game = -1, int type = -1, int page = 0)
        {
            return await Task.Run(async () =>
            {
                List<BfmeWorkshopEntryPreview> libraryEntries = new List<BfmeWorkshopEntryPreview>();

                if(!Directory.Exists(ConfigUtils.LibraryDirectory)) Directory.CreateDirectory(ConfigUtils.LibraryDirectory);

                foreach (var file in Directory.GetFiles(ConfigUtils.LibraryDirectory).OrderBy(x => new FileInfo(x).CreationTime))
                    try { libraryEntries.Add(JsonConvert.DeserializeObject<BfmeWorkshopEntryPreview>(File.ReadAllText(file))); } catch { }

                for (int i = 0; i < 3; i++)
                {
                    if (!libraryEntries.Any(x => x.Guid == $"original-{(i < 2 ? $"BFME{i + 1}" : "RotWK")}"))
                    {
                        AddOrUpdate(await BfmeWorkshopDownloadManager.Download($"original-{(i < 2 ? $"BFME{i + 1}" : "RotWK")}"));
                        libraryEntries.Add(JsonConvert.DeserializeObject<BfmeWorkshopEntryPreview>(File.ReadAllText(Path.Combine(ConfigUtils.LibraryDirectory, $"original-{(i < 2 ? $"BFME{i + 1}" : "RotWK")}.json"))));
                    }
                    if (!libraryEntries.Any(x => x.Guid == $"official-{i}"))
                    {
                        AddOrUpdate(await BfmeWorkshopDownloadManager.Download($"official-{i}"));
                        libraryEntries.Add(JsonConvert.DeserializeObject<BfmeWorkshopEntryPreview>(File.ReadAllText(Path.Combine(ConfigUtils.LibraryDirectory, $"official-{i}.json"))));
                    }
                }

                libraryEntries = libraryEntries.OrderByDescending(x => x.Guid.StartsWith("official-")).OrderByDescending(x => x.Guid.StartsWith("original-")).OrderByDescending(x => x.Guid.StartsWith("exp-original-")).Where(x => (keyword.Length > 0 ? (x.Name.ToLower().Contains(keyword) || x.Description.ToLower().Contains(keyword)) : true) && (game != -1 ? x.Game == game : true) && (type != -1 ? (type == -2 ? (x.Type == 0 || x.Type == 1) : (type == -3 ? (x.Type == 2 || x.Type == 3) : x.Type == type)) : true)).ToList();

                if(page == -1)
                    return libraryEntries;

                if (libraryEntries.Count - page * 25 > 0)
                    return libraryEntries.GetRange(page * 25, Math.Min(25, libraryEntries.Count - page * 25));
                else
                    return new List<BfmeWorkshopEntryPreview>();
            });
        }

        public static async Task<BfmeWorkshopEntry?> Get(string entryGuid)
        {
            if (File.Exists(Path.Combine(ConfigUtils.LibraryDirectory, $"{entryGuid}.json")))
                return await Task.Run(() => JsonConvert.DeserializeObject<BfmeWorkshopEntry>(File.ReadAllText(Path.Combine(ConfigUtils.LibraryDirectory, $"{entryGuid}.json"))));

            return null;
        }

        public static void AddOrUpdate(BfmeWorkshopEntry entry)
        {
            string keybindsDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BFME Workshop", "Keybinds", $"{entry.GameName()}-{entry.Name}");

            if (!Directory.Exists(ConfigUtils.LibraryDirectory)) Directory.CreateDirectory(ConfigUtils.LibraryDirectory);
            if (entry.Type <= 1 && !Directory.Exists(keybindsDirectory)) Directory.CreateDirectory(keybindsDirectory);

            File.WriteAllText(Path.Combine(ConfigUtils.LibraryDirectory, $"{entry.Guid}.json"), JsonConvert.SerializeObject(entry, Formatting.Indented));
        }

        public static void Remove(string entryGuid)
        {
            if (File.Exists(Path.Combine(ConfigUtils.LibraryDirectory, $"{entryGuid}.json")))
                File.Delete(Path.Combine(ConfigUtils.LibraryDirectory, $"{entryGuid}.json"));
        }

        public static bool IsInLibrary(string entryGuid)
        {
            return File.Exists(Path.Combine(ConfigUtils.LibraryDirectory, $"{entryGuid}.json"));
        }
    }
}
