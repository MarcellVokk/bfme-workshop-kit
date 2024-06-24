using BfmeWorkshopKit.Data;
using BfmeWorkshopKit.Utils;
using Newtonsoft.Json;

namespace BfmeWorkshopKit.Logic
{
    public static class BfmeWorkshopQueryManager
    {
        public static async Task<List<BfmeWorkshopEntry>> Search(string keyword = "", int game = -1, int sortMode = 0, int page = 0)
        {
            string response = await ApiRequestManagger.Get(BfmeWorkshopAuthInfo.Unauthenticated, "workshop/query", new Dictionary<string, string>()
            {
                { "keywordFilter", keyword },
                { "gameFilter", game.ToString() },
                { "sortMode", sortMode.ToString() },
                { "page", page.ToString() }
            });
            return JsonConvert.DeserializeObject<List<BfmeWorkshopEntry>>(response) ?? new List<BfmeWorkshopEntry>();
        }

        public static async Task<List<BfmeWorkshopEntry>> Search(string ownerUuid)
        {
            string response = await ApiRequestManagger.Get(BfmeWorkshopAuthInfo.Unauthenticated, "workshop/manage", new Dictionary<string, string>() { { "ownerUuid", ownerUuid } });
            return JsonConvert.DeserializeObject<List<BfmeWorkshopEntry>>(response) ?? new List<BfmeWorkshopEntry>();
        }

        public static async Task<(BfmeWorkshopEntry entry, BfmeWorkshopEntryMetadata metadata)> Get(string entryGuid)
        {
            string response = await ApiRequestManagger.Get(BfmeWorkshopAuthInfo.Unauthenticated, "workshop/query", new Dictionary<string, string>() { { "guid", entryGuid } });
            var responseObject = JsonConvert.DeserializeObject<QueryGetResponse>(response);
            return (responseObject.entry, responseObject.metadata);
        }

        private struct QueryGetResponse
        {
            public BfmeWorkshopEntry entry;
            public BfmeWorkshopEntryMetadata metadata;
        }
    }
}
