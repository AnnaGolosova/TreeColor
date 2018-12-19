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
using System.Web.Configuration;
using System.Text;
using TreeColor.Models;

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
        /// Sets token to headers
        /// </summary>
        /// <param name="token"></param>
        public static void SetAuthorizationHeader(string token)
        {
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        /// <summary>
        /// Performs a GET request.
        /// </summary>
        /// <typeparam name="T">a type of the items</typeparam>
        /// <param name="url">requested url</param>
        public static async Task<ReturnDataModel<T>> GetAsync<T>(string url)
        {
            var dataModel = new ReturnDataModel<T>();
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
                        if (response.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            string token = HttpUtil.GetToken().Result;
                            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        }
                        var oldRequest = request;
                        request = request.Clone();
                        oldRequest.Dispose();
                    }
                }
                catch (HttpRequestException ex)
                {
                    dataModel.LoadException(ex);
                }
                finally
                {
                    tryNumber++;
                }
            }

            if (response.IsSuccessStatusCode)
            {
                json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                dataModel.Data = JsonConvert.DeserializeObject<T>(json);
            }
            else
            {
                dataModel.Message = response.StatusCode + " " + response.ReasonPhrase;
            }

            return dataModel;
        }

        /// <summary>
        /// Performs a GET request.
        /// </summary>
        /// <typeparam name="T">a type of the items</typeparam>
        /// <param name="url">requested url</param>
        public static async Task<ReturnDataModel<T>> GetAsync<T>(object data, string url)
        {
            var dataModel = new ReturnDataModel<T>();

            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Content = content;

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
                        if (response.StatusCode == HttpStatusCode.Forbidden)
                        {
                            string token = HttpUtil.GetToken().Result;
                            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        }
                        var oldRequest = request;
                        request = request.Clone();
                        oldRequest.Dispose();
                    }
                }
                catch (HttpRequestException ex)
                {
                    dataModel.LoadException(ex);
                }
                catch (Exception ex)
                {
                    dataModel.LoadException(ex);
                }
                finally
                {
                    tryNumber++;
                }
            }

            if (response.IsSuccessStatusCode)
            {
                var jsonResult = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                dataModel.Data = JsonConvert.DeserializeObject<T>(jsonResult);
            }
            else
            {
                dataModel.Message = response.StatusCode + " " + response.ReasonPhrase;
            }

            return dataModel;
        }

        /// <summary>
        /// Performs PUT request to the API and returns JSON as an answer
        /// </summary>
        /// <param name="data"></param>
        /// <param name="pathSegment">for example api/Test/All</param>
        /// <returns></returns>
        public static async Task<ReturnDataModel> PutAsync(object data, string pathSegment)
        {
            var dataModel = new ReturnDataModel();

            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, pathSegment);
            request.Content = content;

            HttpResponseMessage responseContent = null;
            try
            {
                responseContent = await HttpClient.SendAsync(request).ConfigureAwait(false);
            }
            catch (HttpRequestException ex)
            {
                dataModel.LoadException(ex);
            }

            if (responseContent.IsSuccessStatusCode)
            {
                var jsonResponce = await responseContent.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
            else
            {
                dataModel.Message = responseContent.StatusCode + " " + responseContent.ReasonPhrase;
            }

            return dataModel;
        }

        /// <summary>
        /// Performs POST request to the API and returns JSON as an answer
        /// </summary>
        /// <param name="data"></param>
        /// <param name="pathSegment">for example api/Test/All</param>
        /// <returns></returns>
        public static async Task<ReturnDataModel<T>> PostAsync<T>(object data, string pathSegment)
        {
            var dataModel = new ReturnDataModel<T>();

            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, pathSegment);
            request.Content = content;

            HttpResponseMessage responseContent = null;
            try
            {
                responseContent = await HttpClient.SendAsync(request).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                dataModel.LoadException(ex);
            }

            if (responseContent.IsSuccessStatusCode)
            {
                var jsonResponce = await responseContent.Content.ReadAsStringAsync().ConfigureAwait(false);
                dataModel.Data = JsonConvert.DeserializeObject<T>(jsonResponce);
            }
            else
            {
                dataModel.Message = responseContent.StatusCode + " " + responseContent.ReasonPhrase;
            }

            return dataModel;
        }
        /// <summary>
        /// Performs POST request to the API and returns JSON as an answer
        /// </summary>
        /// <param name="data"></param>
        /// <param name="pathSegment">for example api/Test/All</param>
        /// <returns></returns>
        public static async Task<ReturnDataModel> PostAsync(object data, string pathSegment)
        {
            var dataModel = new ReturnDataModel();

            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, pathSegment);
            request.Content = content;

            HttpResponseMessage responseContent = null;
            try
            {
                responseContent = await HttpClient.SendAsync(request).ConfigureAwait(false);

                if(!responseContent.IsSuccessStatusCode)
                {
                    dataModel.Message = responseContent.StatusCode + " " + responseContent.ReasonPhrase;
                }
            }
            catch (HttpRequestException ex)
            {
                dataModel.LoadException(ex);
            }

            return dataModel;
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
                clone.Headers.Add(header.Key, header.Value);
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
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "-a-gqUTH2sffClCQ8uWC-_NzO0y9QByZk3kTRaz4M5NJExfut9ejD1_f2BTde7raJTjanZ2yGh5MMtxlVshLY-7Xc1a8k3_15FcbIjTNG0KqEaW9ufQp0tlNnlhg4fjXTcCk82AO8tXj8KrputiAuM9p-PfNh4yETv23fYiQ9UPbckfeHQ1Z5pXK3W2EtUTSArob4bj-35oICxRxIrJqAqWFPbAX_ObIH7-R9Uwj9jwCOmDIGGPEbh-LcB0LIjFY");
            httpClient.BaseAddress = new Uri(ConfigManager.ServerName);


            return httpClient;
        }

        public static async Task<string> GetToken()
        {
            string token = null;
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ConfigManager.ServerName + "token");

            HttpResponseMessage response = null;
            int tryNumber = 0;

            while (tryNumber < MaxTries)
            {
                try
                {
                    var client = new HttpClient();
                    var requestContent = string.Format("username={0}&password={1}&grant_type={2}", Uri.EscapeDataString("adminTest@gsu.by"),
                        Uri.EscapeDataString("adminPassword"), Uri.EscapeDataString("password"));
                    request.Content = new StringContent(requestContent, Encoding.UTF8, "application/x-www-form-urlencoded");
                    
                    response = await client.SendAsync(request);

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
                catch(Exception ex)
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
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                token = JsonConvert.DeserializeObject<TokenModel>(json).access_token;
            }

            return token;
        }
    }
}