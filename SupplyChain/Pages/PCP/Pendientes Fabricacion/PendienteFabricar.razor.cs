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
namespace SupplyChain.Pages.PendFab
{
    public class PendFabPageBase : ComponentBase
    {
        [Inject] protected CustomHttpClient Http { get; set; }
        [Inject] protected IJSRuntime JsRuntime { get; set; }
        protected SfGrid<ModeloPendientesFabricar> Grid;
        protected bool VisibleProperty { get; set; } = false;

        public bool Enabled = true;
        public bool Disabled = false;

        protected DialogSettings DialogParams = new DialogSettings { MinHeight = "400px", Width = "500px" };

        //protected List<CatOpe> catopes = new List<CatOpe>();
        protected List<ModeloPendientesFabricar> listaPendFab = new List<ModeloPendientesFabricar>();

        protected List<Object> Toolbaritems = new List<Object>(){
        "Search",
        "Print",
        "ExcelExport"
    };

        protected override async Task OnInitializedAsync()
        {
            listaPendFab = await Http.GetFromJsonAsync<List<ModeloPendientesFabricar>>("api/PendientesFabricar");

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

        public async Task ActionBegin(ActionEventArgs<ModeloPendientesFabricar> args)
        {
            if (args.RequestType == Syncfusion.Blazor.Grids.Action.Save)
            {
                if (args.Data.CG_FORM != 1)
                {
                    bool isConfirmed = await JsRuntime.InvokeAsync<bool>("confirm", "El producto no tiene Fórmula o no tiene Fórmula Activa");
                    if (isConfirmed)
                    {
                    }
                }
                else
                {
                    if (args.Data.CANTEMITIR != 0)
                    {
                        HttpResponseMessage response;
                        response = await Http.PutAsJsonAsync($"api/PendientesFabricar/PutPenFab/{Convert.ToInt32(args.Data.REGISTRO)}", args.Data);
                        listaPendFab = await Http.GetFromJsonAsync<List<ModeloPendientesFabricar>>("api/PendientesFabricar");
                        Grid.Refresh();
                    }
                }
            }
            if (args.RequestType == Syncfusion.Blazor.Grids.Action.Delete)
            {

            }
        }
        protected async Task EmitirOrden()
        {
            int xCuantas = listaPendFab.Where(s => s.CANTEMITIR > 0).Count();
            if (xCuantas == 0)
            {
                bool isConfirmed = await JsRuntime.InvokeAsync<bool>("confirm", "No hay productos con 'Cantidad a emitir' en las 'Necesidades de stock' para emitir órdenes de fabricación");
                if (isConfirmed)
                {
                }
            }
            else
            {
                bool isConfirmed = await JsRuntime.InvokeAsync<bool>("confirm", "Va a emitir órdenes de fabricación según necesidades de stock \n\n¿Desea continuar?");
                if (isConfirmed)
                {
                    listaPendFab = await Http.GetFromJsonAsync<List<ModeloPendientesFabricar>>("api/PendientesFabricar/EmitirOrdenes");
                }
            }
        }
        public void QueryCellInfoHandler(QueryCellInfoEventArgs<ModeloPendientesFabricar> args)
        {
            if (args.Data.CG_FORM == 0)
            {
                args.Cell.AddClass(new string[] { "rojas" });
            }
            if (args.Column.Field == "CANTEMITIR")
            {
                args.Cell.AddClass(new string[] { "gris" });
            }
        }
    }
}
