using System;
using System.Collections.Generic;
using System.Text;

using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http;
using System.Net.Http.Headers;

using TaxCalculator.Models;
using TaxCalculator.Infrastructure;

namespace TaxCalculator.Helpers
{
    internal static class TaxJarHttpClientProvider
    {
        internal static async Task<T> GetAsync<T>(string uri, string apiKey = "")
        {
            using (var httpClient = CreateHttpClient(apiKey))
            {
                var response = await httpClient.GetAsync(uri);

                var responseContent = await response.Content.ReadAsStringAsync();

                CheckIfErrorMessage(response, responseContent);

                return JsonConvert.DeserializeObject<T>(responseContent, JsonSerializerSettings());
            }
        }

        internal static async Task<TReturnT> PostAsync<TReturnT, TPostType>(string uri, TPostType postObject, string apiKey = "")
        {
            using (var httpClient = CreateHttpClient(apiKey))
            {
                var json = JsonConvert.SerializeObject(postObject, JsonSerializerSettings());

                var body = new StringContent(json, Encoding.Default, "application/json");

                var response = await httpClient.PostAsync(uri, body);

                var responseContent = await response.Content.ReadAsStringAsync();

                CheckIfErrorMessage(response, responseContent);

                return JsonConvert.DeserializeObject<TReturnT>(responseContent, JsonSerializerSettings());
            }
        }

        private static JsonSerializerSettings JsonSerializerSettings()
        {
            return new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            };
        }

        private static HttpClient CreateHttpClient(string apiKey = "")
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(Constants.TAXJAR_API_BASE_URI)
            };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrWhiteSpace(apiKey))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            }

            return client;
        }

        private static void CheckIfErrorMessage(HttpResponseMessage response, string responseContent)
        {
            if ((int)response.StatusCode >= 400)
            {
                var taxJarError = JsonConvert.DeserializeObject<TaxJarErrorResponse>(responseContent, JsonSerializerSettings());
                var error = $"{taxJarError.Error } - {taxJarError.Detail}";
                throw new TaxCalculatorException(error);
            }
        }

    }

    public class TaxJarErrorResponse
    {
        public string Error { get; set; }
        public string Detail { get; set; }
        public string Status { get; set; }
    }
}
