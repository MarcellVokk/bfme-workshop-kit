using BfmeWorkshopKit.Data;
using BfmeWorkshopKit.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BfmeWorkshopKit.Logic
{
    public static class BfmeWorkshopDownloadManager
    {
        public static async Task DownloadTo(string entryGuid, string destinationFileName)
        {
            string response = await ApiRequestManagger.Get(BfmeWorkshopAuthInfo.Unauthenticated, "workshop/download", new Dictionary<string, string>() { { "guid", entryGuid } });
            File.WriteAllText(destinationFileName, JsonConvert.SerializeObject(JsonConvert.DeserializeObject(response), Formatting.Indented));
        }

        public static async Task<BfmeWorkshopEntry> Download(string entryGuid)
        {
            string response = await ApiRequestManagger.Get(BfmeWorkshopAuthInfo.Unauthenticated, "workshop/download", new Dictionary<string, string>() { { "guid", entryGuid } });
            return JsonConvert.DeserializeObject<BfmeWorkshopEntry>(response);
        }
    }
}
