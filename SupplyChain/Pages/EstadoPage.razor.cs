using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SupplyChain;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace SupplyChain.Pages.Modelos
{
    public class ModeloPageBase : ComponentBase
    {
        [Inject] protected CustomHttpClient Http { get; set; }
        [Inject] protected IJSRuntime JsRuntime { get; set; }
        protected SfGrid<Solution> Grid;
        public bool Enabled = true;
        public bool Disabled = false;

        protected List<Solution> Pedclis = new List<Solution>();
        protected override async Task OnInitializedAsync()
        {
            
            Pedclis = await Http.GetFromJsonAsync<List<Solution>>("api/Solution");

            await base.OnInitializedAsync();
        }
    }
}
