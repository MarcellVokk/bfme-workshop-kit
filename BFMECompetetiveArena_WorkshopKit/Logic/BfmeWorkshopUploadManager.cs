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
    public static class BfmeWorkshopUploadManager
    {
        public static async Task<BfmeWorkshopEntry> Publish(BfmeWorkshopAuthInfo authInfo, BfmeWorkshopEntry entry)
        {
            string response = await ApiRequestManagger.Set(authInfo, "workshop/manage", JsonConvert.SerializeObject(entry));

            if (!response.StartsWith("{"))
                throw new Exception(response);

            return JsonConvert.DeserializeObject<BfmeWorkshopEntry>(response);
        }

        public static async Task Delete(BfmeWorkshopAuthInfo authInfo, string entryGuid)
        {
            string response = await ApiRequestManagger.Delete(authInfo, "workshop/manage", entryGuid);

            if (response != "")
                throw new Exception(response);
        }
    }
}
