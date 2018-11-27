using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace TreeColor.Utils
{
    public static class HttpUtil
    {
        private static int MaxTries => 3;
        private static HttpClient _httpClient;
        /// <summary>
        /// Object to execute http queries
        /// </summary>
        public static HttpClient HttpClient => _httpClient ?? (_httpClient = CreateHttpClient());

        /// <summary>
        /// Performs a GET request.
        /// </summary>
        /// <typeparam name="T">a type of the items</typeparam>
        /// <param name="url">requested url</param>
        /// <param name="locale">En/Ru locale</param>
        public static async Task<T> GetAsync<T>(string url)
            where T : class
        {
            T result = default(T);
            string json = null;

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);

            HttpResponseMessage response = null;
            int tryNumber = 0;

            while (tryNumber < MaxTries)
            {
                try
                {
                    response = await HttpClient.SendAsync(request).ConfigureAwait(false);
                    if (response.IsSuccessStatusCode) tryNumber = MaxTries;
                    else
                    {
                        var oldRequest = request;
                        request = request.Clone();
                        oldRequest.Dispose();
                    }
                }
                catch (HttpRequestException ex)
                {
                    throw ex;
                }
                finally
                {
                    tryNumber++;
                }
            }

            if (response.IsSuccessStatusCode)
            {
                json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                result = JsonConvert.DeserializeObject<T>(json);
            }

            return result;
        }

        /// <summary>
        /// To try HTTP request again
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static HttpRequestMessage Clone(this HttpRequestMessage req)
        {
            HttpRequestMessage clone = new HttpRequestMessage(req.Method, req.RequestUri);

            clone.Content = req.Content;
            clone.Version = req.Version;

            foreach (KeyValuePair<string, object> prop in req.Properties)
            {
                clone.Properties.Add(prop);
            }

            foreach (KeyValuePair<string, IEnumerable<string>> header in req.Headers)
            {
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return clone;
        }

        /// <summary>
        /// Returns a configured http client
        /// </summary>
        /// <returns></returns>
        private static HttpClient CreateHttpClient()
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return httpClient;
        }

    }
}