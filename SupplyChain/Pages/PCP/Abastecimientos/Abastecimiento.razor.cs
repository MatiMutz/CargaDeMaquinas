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
using System.Data;
using Syncfusion.Blazor.ProgressBar;

namespace SupplyChain.Pages.Absto
{
    public class AbastPageBase : ComponentBase
    {
        [Inject] protected CustomHttpClient Http { get; set; }
        [Inject] protected IJSRuntime JsRuntime { get; set; }
        protected SfGrid<ModeloAbastecimiento> GridMP;
        protected SfGrid<ModeloAbastecimiento> GridSE;
        protected string claseSE = "btn btn-sm btn-primary active";
        protected string claseMP = "btn btn-sm btn-outline-primary";

        protected bool Enabled = true;
        protected bool Disabled = false;
        protected bool ShowSemi = false;
        protected bool ShowMatP = false;
        protected bool VisiblePropertyMP { get; set; } = false;
        protected bool VisiblePropertySE { get; set; } = false;

        protected DialogSettings DialogParams = new DialogSettings { MinHeight = "400px", Width = "500px" };

        //protected List<CatOpe> catopes = new List<CatOpe>();
        protected List<ModeloAbastecimiento> listaAbastMP = new List<ModeloAbastecimiento>();
        protected List<ModeloAbastecimiento> listaAbastSE = new List<ModeloAbastecimiento>();

        protected List<Object> ToolbaritemsMP = new List<Object>(){
        "Search",
        "Print",
        "ExcelExport"
    };
        protected List<Object> ToolbaritemsSE = new List<Object>(){
        "Search",
        "Print",
        "ExcelExport"
    };

        protected override async Task OnInitializedAsync()
        {
            listaAbastMP = await Http.GetFromJsonAsync<List<ModeloAbastecimiento>>("api/Abastecimiento/AbastecimientoMP");
            listaAbastSE = await Http.GetFromJsonAsync<List<ModeloAbastecimiento>>("api/Abastecimiento/AbastecimientoSE");

            await base.OnInitializedAsync();
        }

        public async Task ClickHandlerMP(Syncfusion.Blazor.Navigations.ClickEventArgs args)
        {
            if (args.Item.Text == "Excel Export")
            {
                await this.GridMP.ExcelExport();
            }
            if (args.Item.Text == "Print")
            {
                await this.GridMP.Print();
            }
        }
        public async Task ClickHandlerSE(Syncfusion.Blazor.Navigations.ClickEventArgs args)
        {
            if (args.Item.Text == "Excel Export")
            {
                await this.GridSE.ExcelExport();
            }
            if (args.Item.Text == "Print")
            {
                await this.GridSE.Print();
            }
        }
        public async Task ActionBeginMP(ActionEventArgs<ModeloAbastecimiento> args)
        {
            if (args.RequestType == Syncfusion.Blazor.Grids.Action.Save)
            {
                HttpResponseMessage response;
                response = await Http.PutAsJsonAsync($"api/Abastecimiento/PutAbMP/{args.Data.CG_MAT}", args.Data);
                listaAbastMP = await Http.GetFromJsonAsync<List<ModeloAbastecimiento>>("api/Abastecimiento/AbastecimientoMPX");
                GridMP.Refresh();
            }
            if (args.RequestType == Syncfusion.Blazor.Grids.Action.Delete)
            {
            }
        }
        public async Task ActionBeginSE(ActionEventArgs<ModeloAbastecimiento> args)
        {
            if (args.RequestType == Syncfusion.Blazor.Grids.Action.Save)
            {
                HttpResponseMessage response;
                response = await Http.PutAsJsonAsync($"api/Abastecimiento/PutAbSE/{args.Data.CG_MAT}", args.Data);
                listaAbastSE = await Http.GetFromJsonAsync<List<ModeloAbastecimiento>>("api/Abastecimiento/AbastecimientoSEX");
                GridSE.Refresh();
            }
            if (args.RequestType == Syncfusion.Blazor.Grids.Action.Delete)
            {

            }
        }
        protected void Semis()
        {
            ShowSemi = true;
            ShowMatP = false;
        }
        protected async Task MatP()
        {
            bool isConfirmed = await JsRuntime.InvokeAsync<bool>("confirm", "Asegúrese de haber Abastecido Semi-Elaborados primero.");
            if (isConfirmed)
            {
                ShowMatP = true;
                ShowSemi = false;
            }
        }

        protected async Task EmitirMP()
        {
            int xCuantas = listaAbastSE.Where(s => s.CALCULADO != 0).Count();
            if (xCuantas != 0)
            {
                bool isConfirmed = await JsRuntime.InvokeAsync<bool>("confirm", "Hay Semi-Elaborados con Fabricación sugerida. \n\n¿Desea continuar?", "Abastecimiento de Materias Primas");
                if (isConfirmed)
                {
                    bool isConfirmed2 = await JsRuntime.InvokeAsync<bool>("confirm", "Va a ejecutar el proceso de Emitir Preparación de Compras Materias primas. \n\n¿Desea continuar?", "Abastecimiento de Materias Primas");
                    if (isConfirmed2)
                    {
                        listaAbastMP = await Http.GetFromJsonAsync<List<ModeloAbastecimiento>>("api/Abastecimiento/AbastecerMP");
                        listaAbastMP = await Http.GetFromJsonAsync<List<ModeloAbastecimiento>>("api/Abastecimiento/AbastecimientoMPX");
                        GridMP.Refresh();
                    }
                }
            }
        }
        protected async Task EmitirSE()
        {
            bool isConfirmed = await JsRuntime.InvokeAsync<bool>("confirm", "Va a ejecutar el proceso de abastecer. \n\n¿Desea continuar?", "Abastecimiento de Semi-Elaborados");
            if (isConfirmed)
            {
                listaAbastSE = await Http.GetFromJsonAsync<List<ModeloAbastecimiento>>("api/Abastecimiento/AbastecerSE");
                listaAbastSE = await Http.GetFromJsonAsync<List<ModeloAbastecimiento>>("api/Abastecimiento/AbastecimientoSEX");
                GridSE.Refresh();
            }
        }
        public void QueryCellInfoHandler(QueryCellInfoEventArgs<ModeloAbastecimiento> args)
        {
            if (args.Column.Field == "ACOMPRAR")
            {
                args.Cell.AddClass(new string[] { "gris" });
            }
        }
    }
}
