using Amazon.S3;
using Amazon.S3.Transfer;
using BfmeFoundationProject.WorkshopKit.Data;
using BfmeFoundationProject.WorkshopKit.Logic;
using Newtonsoft.Json;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net;
using System.Net.Mime;
using System.Text;

namespace BfmeFoundationProject.WorkshopKit.Utils
{
    internal static class HttpUtils
    {
        internal static async Task<string> Get(BfmeWorkshopAuthInfo authInfo, string apiEndpointPath, Dictionary<string, string>? requestParameters = null)
        {
            NameValueCollection requestQueryParameters = System.Web.HttpUtility.ParseQueryString(string.Empty);
            requestParameters?.ToList().ForEach(x => requestQueryParameters.Add(x.Key, x.Value == "" ? "~" : x.Value));

            using HttpClient client = new HttpClient() { Timeout = TimeSpan.FromSeconds(10) };
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36");
            using HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, $"{BfmeWorkshopManager.WorkshopServerHost}/api/{apiEndpointPath}{(requestQueryParameters.Count > 0 ? $"?{requestQueryParameters.ToString()}" : "")}");

            requestMessage.Headers.Add("AuthAccountUuid", authInfo.Uuid);
            requestMessage.Headers.Add("AuthAccountPassword", authInfo.Password);

            using HttpResponseMessage response = await client.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        internal static async Task<T?> GetJson<T>(BfmeWorkshopAuthInfo authInfo, string apiEndpointPath, Dictionary<string, string>? requestParameters = null) => JsonConvert.DeserializeObject<T>(await Get(authInfo, apiEndpointPath, requestParameters));

        internal static async Task<string> Set(BfmeWorkshopAuthInfo authInfo, string apiEndpointPath, string data)
        {
            using HttpClient client = new HttpClient() { Timeout = TimeSpan.FromSeconds(10) };
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36");
            using HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{BfmeWorkshopManager.WorkshopServerHost}/api/{apiEndpointPath}");

            requestMessage.Headers.Add("AuthAccountUuid", authInfo.Uuid);
            requestMessage.Headers.Add("AuthAccountPassword", authInfo.Password);

            requestMessage.Content = new StringContent(data, Encoding.UTF8, "text/plain");

            using HttpResponseMessage response = await client.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        internal static async Task Upload(BfmeWorkshopAuthInfo authInfo, string source, string id = "", Action<int>? OnProgressUpdate = null)
        {
            NameValueCollection requestQueryParameters = System.Web.HttpUtility.ParseQueryString(string.Empty);
            requestQueryParameters.Add("id", id == "" ? "~" : id);

            using var workshop_files = new AmazonS3Client("b27b3b7fd5c041cd488270b978fc83a1", "927174808adede24038b5acdac2ca386bfa8ae02b245d57f46e2a2b76f9e2c92", new AmazonS3Config { ServiceURL = $"https://32d721493580a1de3fa984779848a30d.r2.cloudflarestorage.com" });
            using var transfer_utility = new TransferUtility(workshop_files);

            var request = new TransferUtilityUploadRequest();
            request.BucketName = "workshop-files";
            request.Key = $"{authInfo.Uuid}/{id}";
            request.InputStream = new FileStream(source, FileMode.Open, FileAccess.Read);
            request.ContentType = "application/octet-stream";
            request.DisablePayloadSigning = true;
            request.PartSize = 6291456;

            int lastProgress = 0;
            request.UploadProgressEvent += (s, e) =>
            {
                if (e.PercentDone != lastProgress)
                {
                    OnProgressUpdate?.Invoke(e.PercentDone);
                    lastProgress = e.PercentDone;
                }
            };

            await transfer_utility.UploadAsync(request);
        }

        internal static async Task<string> Delete(BfmeWorkshopAuthInfo authInfo, string apiEndpointPath, string id = "")
        {
            NameValueCollection requestQueryParameters = System.Web.HttpUtility.ParseQueryString(string.Empty);
            if (id != "") requestQueryParameters.Add("id", id);

            using HttpClient client = new HttpClient() { Timeout = TimeSpan.FromSeconds(10) };
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36");
            using HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Delete, $"{BfmeWorkshopManager.WorkshopServerHost}/api/{apiEndpointPath}?{requestQueryParameters.ToString()}");

            requestMessage.Headers.Add("AuthAccountUuid", authInfo.Uuid);
            requestMessage.Headers.Add("AuthAccountPassword", authInfo.Password);

            using HttpResponseMessage response = await client.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
