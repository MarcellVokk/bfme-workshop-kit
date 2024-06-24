using BfmeWorkshopKit.Data;
using BfmeWorkshopKit.Utils;
using Newtonsoft.Json;

namespace BfmeWorkshopKit.Logic
{
    public static class BfmeWorkshopLibraryManager
    {
        public static async Task<List<BfmeWorkshopEntry>> Search(string keyword = "", int game = -1, int page = 0)
        {
            return await Task.Run(async () =>
            {
                string libraryDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BFME Workshop", "Library");
                List<BfmeWorkshopEntry> libraryEntries = new List<BfmeWorkshopEntry>();

                if(!Directory.Exists(libraryDirectory))
                    Directory.CreateDirectory(libraryDirectory);

                foreach (var file in Directory.GetFiles(libraryDirectory).OrderBy(x => new FileInfo(x).CreationTime))
                {
                    try
                    {
                        libraryEntries.Add(JsonConvert.DeserializeObject<BfmeWorkshopEntry>(File.ReadAllText(file)));
                    }
                    catch { }
                }

                if (!libraryEntries.Any(x => x.Guid == $"original-{(game < 2 ? $"BFME{game + 1}" : "RotWK")}"))
                {
                    await AddToLibrary($"original-{(game < 2 ? $"BFME{game + 1}" : "RotWK")}");
                    libraryEntries.Add(JsonConvert.DeserializeObject<BfmeWorkshopEntry>(File.ReadAllText(Path.Combine(libraryDirectory, $"original-{(game < 2 ? $"BFME{game + 1}" : "RotWK")}.json"))));
                }
                if (!libraryEntries.Any(x => x.Guid == $"official-{game}"))
                {
                    await AddToLibrary($"official-{game}");
                    libraryEntries.Add(JsonConvert.DeserializeObject<BfmeWorkshopEntry>(File.ReadAllText(Path.Combine(libraryDirectory, $"official-{game}.json"))));
                }

                libraryEntries = libraryEntries.OrderByDescending(x => x.Guid.StartsWith("official-")).OrderByDescending(x => x.Guid.StartsWith("original-")).Where(x => (keyword.Length > 0 ? (x.Name.ToLower().Contains(keyword) || x.Description.ToLower().Contains(keyword)) : true) && (game != -1 ? x.Game == game : true)).ToList();

                if (libraryEntries.Count - page * 25 > 0)
                    return libraryEntries.GetRange(page * 25, Math.Min(25, libraryEntries.Count - page * 25));
                else
                    return new List<BfmeWorkshopEntry>();
            });
        }

        public static async Task AddToLibrary(string entryGuid)
        {
            string libraryDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BFME Workshop", "Library");

            if (!Directory.Exists(libraryDirectory))
                Directory.CreateDirectory(libraryDirectory);

            string response = await ApiRequestManagger.Get(BfmeWorkshopAuthInfo.Unauthenticated, "workshop/download", new Dictionary<string, string>() { { "guid", entryGuid } });
            File.WriteAllText(Path.Combine(libraryDirectory, $"{entryGuid}.json"), JsonConvert.SerializeObject(JsonConvert.DeserializeObject(response), Formatting.Indented));
        }
    }
}
