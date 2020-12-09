using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Json;

namespace SupplyChain.Extensions
{
    public static class HttpContentJsonExtensions
    {
        //public static Task<object?> ReadFromJsonAsync(this HttpContent content, Type type,
        //    JsonSerializerOptions? options = default, CancellationToken cancellationToken = default)
        //{
        //    string myContent = JsonConvert.SerializeObject(content);
        //    StringContent stringContent = new StringContent(myContent, Encoding.UTF8, "application/json");
        //    return stringContent.ReadFromJsonAsync<type>();
        //}

        public static async Task<T> ReadFromJsonAsyncCustom<T>(this HttpContent? content, JsonSerializerOptions options = default,
            CancellationToken cancellationToken = default)
        {
            string myContent = JsonConvert.SerializeObject(content);
            StringContent stringContent = new StringContent(myContent, Encoding.UTF8, "application/json");
            return await stringContent.ReadFromJsonAsync<T>();
        }
    }
}
