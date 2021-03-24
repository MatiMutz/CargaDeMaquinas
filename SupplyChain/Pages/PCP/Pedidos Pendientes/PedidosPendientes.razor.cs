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
using SupplyChain.Shared.Models;
namespace SupplyChain.Pages.PedPend
{
    public class PedPendPageBase : ComponentBase
    {
        [Inject] protected CustomHttpClient Http { get; set; }
        [Inject] protected IJSRuntime JsRuntime { get; set; }
        protected SfGrid<ModeloPedidosPendientes> Grid;
        protected bool VisibleProperty { get; set; } = false;

        public bool Enabled = true;
        public bool Disabled = false;


        //protected List<CatOpe> catopes = new List<CatOpe>();
        protected List<ModeloPedidosPendientes> listaPedPend = new List<ModeloPedidosPendientes>();

        protected List<Object> Toolbaritems = new List<Object>(){
        "Search",
        "Print",
        "ExcelExport"
    };

        protected override async Task OnInitializedAsync()
        {
            listaPedPend = await Http.GetFromJsonAsync<List<ModeloPedidosPendientes>>("api/PedidosPendientes");
            await base.OnInitializedAsync();
        }

        public async Task ClickHandler(Syncfusion.Blazor.Navigations.ClickEventArgs args)
        {
            if (args.Item.Text == "Excel Export")
            {
                //BORRARESTO = Grid.Columns.FirstOrDefault().Width.ToString();
                await this.Grid.ExcelExport();
            }
            if (args.Item.Text == "Print")
            {
                await this.Grid.Print();
            }
        }

        public void QueryCellInfoHandler(QueryCellInfoEventArgs<ModeloPedidosPendientes> args)
        {
            if (args.Data.DES_ESTADOCARGA == "EN CURSO")
            {
                args.Cell.AddClass(new string[] { "verdes" });
            }
        }
    }
}
