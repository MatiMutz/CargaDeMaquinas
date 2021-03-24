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
namespace SupplyChain.Pages.Fab
{
    public class FabricPageBase : ComponentBase
    {
        [Inject] protected CustomHttpClient Http { get; set; }
        [Inject] protected IJSRuntime JsRuntime { get; set; }
        protected SfGrid<Fabricacion> Grid;
        protected bool VisibleProperty { get; set; } = false;

        public bool Enabled = true;
        public bool Disabled = false;

        protected DialogSettings DialogParams = new DialogSettings { MinHeight = "400px", Width = "500px" };

        //protected List<CatOpe> catopes = new List<CatOpe>();
        protected List<Fabricacion> listaFab = new List<Fabricacion>();

        protected List<Object> Toolbaritems = new List<Object>(){
        "Search",
        "Print",
        "ExcelExport"
    };

        protected override async Task OnInitializedAsync()
        {
            listaFab = await Http.GetFromJsonAsync<List<Fabricacion>>("api/Fabricacion");

            await base.OnInitializedAsync();
        }

        public async Task ClickHandler(Syncfusion.Blazor.Navigations.ClickEventArgs args)
        {
            if (args.Item.Text == "Excel Export")
            {
                await this.Grid.ExcelExport();
            }
            if (args.Item.Text == "Print")
            {
                await this.Grid.Print();
            }
        }

        public async Task ActionBegin(ActionEventArgs<Fabricacion> args)
        {
            if (args.RequestType == Syncfusion.Blazor.Grids.Action.Save)
            {

            }
            if (args.RequestType == Syncfusion.Blazor.Grids.Action.Delete)
            {

            }
        }
    

        public void QueryCellInfoHandler(QueryCellInfoEventArgs<Fabricacion> args)
        {
            /*if (args.Column.Field == "CG_ESTADOCARGA")
            {
                args.Cell.AddClass(new string[] { "gris" });
            }
            if (args.Column.Field == "FE_ENTREGA")
            {
                args.Cell.AddClass(new string[] { "gris" });
            }
            if (args.Column.Field == "FE_EMIT")
            {
                args.Cell.AddClass(new string[] { "gris" });
            }
            if (args.Column.Field == "FE_PLAN")
            {
                args.Cell.AddClass(new string[] { "gris" });
            }
            if (args.Column.Field == "FE_FIRME")
            {
                args.Cell.AddClass(new string[] { "gris" });
            }
            if (args.Column.Field == "FE_CURSO")
            {
                args.Cell.AddClass(new string[] { "gris" });
            }
            if (args.Column.Field == "CANT")
            {
                args.Cell.AddClass(new string[] { "gris" });
            }*/
        }
    }
}
