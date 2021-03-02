using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupplyChain.HelperService
{
    public static class JSRuntimeExtensionMethods
    {
        public static ValueTask<object> SetInLocalStorage(this IJSRuntime js, string key, string content)
           => js.InvokeAsync<object>(
               "localStorage.setItem",
               key, content
       );

        public static ValueTask<string> GetFromLocalStorage(this IJSRuntime js, string key)
            => js.InvokeAsync<string>(
                "localStorage.getItem",
                key
                );

        public static ValueTask<object> RemoveItem(this IJSRuntime js, string key)
            => js.InvokeAsync<object>(
                "localStorage.removeItem",
                key);

        public static ValueTask<object> SetInSessionStorage(this IJSRuntime js, string key, string content)
           => js.InvokeAsync<object>(
               "sessionStorage.setItem",
               key, content
       );

        public static ValueTask<string> GetFromSessionStorage(this IJSRuntime js, string key)
            => js.InvokeAsync<string>(
                "sessionStorage.getItem",
                key
                );

        public static ValueTask<object> RemoveSessionItem(this IJSRuntime js, string key)
            => js.InvokeAsync<object>(
                "sessionStorage.removeItem",
                key);
    }

   
}

