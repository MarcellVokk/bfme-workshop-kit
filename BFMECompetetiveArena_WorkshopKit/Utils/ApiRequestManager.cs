using BfmeWorkshopKit.Data;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BfmeWorkshopKit.Utils
{
    internal static class ApiRequestManagger
    {
        private static HttpClient HttpClient = new HttpClient() { Timeout = Timeout.InfiniteTimeSpan };

        public static async Task<string> Get(BfmeWorkshopAuthInfo authInfo, string apiEndpointPath, Dictionary<string, string>? requestParameters = null)
        {
            NameValueCollection requestQueryParameters = System.Web.HttpUtility.ParseQueryString(string.Empty);

            if (requestParameters != null)
                requestParameters.ToList().ForEach(x => requestQueryParameters.Add(x.Key, x.Value));

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"https://bfmeladder.com/api/{apiEndpointPath}{(requestQueryParameters.Count > 0 ? $"?{requestQueryParameters.ToString()}" : "")}"))
            {
                requestMessage.Headers.Add("AuthAccountUuid", authInfo.Uuid);
                requestMessage.Headers.Add("AuthAccountPassword", authInfo.Password);

                HttpResponseMessage response = await HttpClient.SendAsync(requestMessage);

                return await response.Content.ReadAsStringAsync();
            }
        }

        public static async Task<string> Set(BfmeWorkshopAuthInfo authInfo, string apiEndpointPath, string data)
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"https://bfmeladder.com/api/{apiEndpointPath}_set"))
            {
                requestMessage.Headers.Add("AuthAccountUuid", authInfo.Uuid);
                requestMessage.Headers.Add("AuthAccountPassword", authInfo.Password);

                requestMessage.Content = new StringContent(data, Encoding.UTF8, "text/plain");

                HttpResponseMessage response = await HttpClient.SendAsync(requestMessage);

                return await response.Content.ReadAsStringAsync();
            }
        }

        public static async Task<string> Delete(BfmeWorkshopAuthInfo authInfo, string apiEndpointPath, string fileId)
        {
            NameValueCollection requestQueryParameters = System.Web.HttpUtility.ParseQueryString(string.Empty);
            requestQueryParameters.Add("fileId", fileId);

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"https://bfmeladder.com/api/{apiEndpointPath}_delete?{requestQueryParameters.ToString()}"))
            {
                requestMessage.Headers.Add("AuthAccountUuid", authInfo.Uuid);
                requestMessage.Headers.Add("AuthAccountPassword", authInfo.Password);

                HttpResponseMessage response = await HttpClient.SendAsync(requestMessage);

                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}
