using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Collections.Generic;

namespace SupplyChain
{
    public class CustomHttpClient : HttpClient
    {
        private string serverUri = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build().GetConnectionString("ServerUri");

        public async Task<T> GetFromJsonAsync<T>(string requestUri)
        {
            HttpClient httpClient = new HttpClient();
            var httpContent = await httpClient.GetAsync(serverUri + requestUri);
            string jsonContent = httpContent.Content.ReadAsStringAsync().Result;
            T obj = JsonConvert.DeserializeObject<T>(jsonContent);
            httpContent.Dispose();
            httpClient.Dispose();
            return obj;
        }
        public async Task<HttpResponseMessage> PutAsJsonAsync<T>(string requestUri, T content)
        {
            HttpClient httpClient = new HttpClient();
            string myContent = JsonConvert.SerializeObject(content);
            StringContent stringContent = new StringContent(myContent, Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync(serverUri + requestUri, stringContent);
            httpClient.Dispose();
            return response;
        }

        public async Task<HttpResponseMessage> PostAsJsonAsync<T>(string requestUri, T content)
        {
            HttpClient httpClient = new HttpClient();
            string myContent = JsonConvert.SerializeObject(content);
            StringContent stringContent = new StringContent(myContent, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(serverUri + requestUri, stringContent);
            httpClient.Dispose();
            return response;
        }

        public new async Task<HttpResponseMessage> DeleteAsync(string requestUri)
        {
            HttpClient httpClient = new HttpClient();
            var response = await httpClient.DeleteAsync(serverUri + requestUri);
            httpClient.Dispose();
            return response;
        }
    }
}
