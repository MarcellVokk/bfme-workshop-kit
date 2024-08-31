using Amazon.S3;
using Amazon.S3.Transfer;
using BfmeFoundationProject.WorkshopKit.Data;
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
        private static HttpClient HttpClient = new HttpClient() { Timeout = Timeout.InfiniteTimeSpan };

        internal static async Task<string> Get(BfmeWorkshopAuthInfo authInfo, string apiEndpointPath, Dictionary<string, string>? requestParameters = null)
        {
            NameValueCollection requestQueryParameters = System.Web.HttpUtility.ParseQueryString(string.Empty);

            if (requestParameters != null)
                requestParameters.ToList().ForEach(x => requestQueryParameters.Add(x.Key, x.Value == "" ? "~" : x.Value));

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"https://bfmeladder.com/api/{apiEndpointPath}{(requestQueryParameters.Count > 0 ? $"?{requestQueryParameters.ToString()}" : "")}"))
            {
                requestMessage.Headers.Add("AuthAccountUuid", authInfo.Uuid);
                requestMessage.Headers.Add("AuthAccountPassword", authInfo.Password);

                HttpResponseMessage response = await HttpClient.SendAsync(requestMessage);
                return await response.Content.ReadAsStringAsync();
            }
        }

        internal static async Task<T?> GetJson<T>(BfmeWorkshopAuthInfo authInfo, string apiEndpointPath, Dictionary<string, string>? requestParameters = null) => JsonConvert.DeserializeObject<T>(await Get(authInfo, apiEndpointPath, requestParameters));

        internal static async Task<string> Set(BfmeWorkshopAuthInfo authInfo, string apiEndpointPath, string data)
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"https://bfmeladder.com/api/{apiEndpointPath}"))
            {
                requestMessage.Headers.Add("AuthAccountUuid", authInfo.Uuid);
                requestMessage.Headers.Add("AuthAccountPassword", authInfo.Password);

                requestMessage.Content = new StringContent(data, Encoding.UTF8, "text/plain");

                HttpResponseMessage response = await HttpClient.SendAsync(requestMessage);
                return await response.Content.ReadAsStringAsync();
            }
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

        public static async Task Download(string url, string localPath, Action<int>? OnProgressUpdate = null)
        {
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    using (var response = await HttpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
                    {
                        response.EnsureSuccessStatusCode();

                        var totalBytes = response.Content.Headers.ContentLength ?? -1L;
                        var totalBytesRead = 0L;
                        var buffer = new byte[4096];
                        var isMoreToRead = true;
                        int progressInPercent = 0;

                        using (var fileStream = new FileStream(localPath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
                        using (var stream = await response.Content.ReadAsStreamAsync())
                            do
                            {
                                var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                                if (bytesRead == 0)
                                {
                                    isMoreToRead = false;
                                    continue;
                                }

                                await fileStream.WriteAsync(buffer, 0, bytesRead);

                                totalBytesRead += bytesRead;
                                int newProgressInPercent = (int)(totalBytesRead * 100 / totalBytes);

                                if (progressInPercent != newProgressInPercent)
                                {
                                    progressInPercent = newProgressInPercent;
                                    OnProgressUpdate?.Invoke(newProgressInPercent);
                                }
                            }
                            while (isMoreToRead);
                    }
                    break;
                }
                catch (Exception ex)
                {
                    if (i == 2) throw new HttpRequestException($"Error while downloading from {url}\n{ex.ToString()}");
                }
            }
        }

        internal static async Task<string> Delete(BfmeWorkshopAuthInfo authInfo, string apiEndpointPath, string id = "")
        {
            NameValueCollection requestQueryParameters = System.Web.HttpUtility.ParseQueryString(string.Empty);
            requestQueryParameters.Add("id", id == "" ? "~" : id);

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Delete, $"https://bfmeladder.com/api/{apiEndpointPath}?{requestQueryParameters.ToString()}"))
            {
                requestMessage.Headers.Add("AuthAccountUuid", authInfo.Uuid);
                requestMessage.Headers.Add("AuthAccountPassword", authInfo.Password);

                HttpResponseMessage response = await HttpClient.SendAsync(requestMessage);
                return await response.Content.ReadAsStringAsync();
            }
        }
    }

    internal class ProgressableStreamContent : HttpContent
    {
        private readonly Stream content;
        private readonly int bufferSize;
        private readonly Action<int>? progress;
        private int progressInPercent = 0;

        public ProgressableStreamContent(Stream content, int bufferSize, Action<int>? progress)
        {
            if (content == null) throw new ArgumentNullException(nameof(content));
            if (bufferSize <= 0) throw new ArgumentOutOfRangeException(nameof(bufferSize));
            if (progress == null) throw new ArgumentNullException(nameof(progress));

            this.content = content;
            this.bufferSize = bufferSize;
            this.progress = progress;

            if (content.CanSeek)
            {
                Headers.ContentLength = content.Length;
            }
        }

        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext? context)
        {
            var buffer = new byte[bufferSize];
            long uploaded = 0;
            long total = content.Length;
            content.Position = 0; // Reset stream position

            try
            {
                while (true)
                {
                    var length = await content.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
                    if (length == 0) break;

                    await stream.WriteAsync(buffer, 0, length).ConfigureAwait(false);
                    uploaded += length;
                    int newProgressInPercent = (int)((double)uploaded / total * 100);
                    if (progressInPercent != newProgressInPercent)
                    {
                        progressInPercent = newProgressInPercent;
                        progress?.Invoke(newProgressInPercent);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception here to diagnose the issue
                Debug.WriteLine($"An exception occurred: {ex}");
                throw;
            }
        }

        protected override bool TryComputeLength(out long length)
        {
            length = content.Length;
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                content.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
