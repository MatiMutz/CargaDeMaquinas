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
using Microsoft.Extensions.Configuration;
using System.IO;
using Syncfusion.Blazor.Inputs;
using Newtonsoft.Json;

namespace SupplyChain.Pages.Prev
{
    public class PrevisionesPageBase : ComponentBase
    {
        [Inject] protected CustomHttpClient Http { get; set; }
        [Inject] protected IJSRuntime JsRuntime { get; set; }
        protected SfGrid<PresAnual> Grid;
        protected SfGrid<Prod> Grid2;
        protected bool VisibleProperty { get; set; } = false;

        public bool Enabled = true;
        public bool Disabled = false;
        public bool Showgrid = true;

        protected List<PresAnual> previsiones = new List<PresAnual>();
        protected List<PresAnual> prueba = new List<PresAnual>();
        protected List<Prod> CG_PRODlist = new List<Prod>();
        protected List<Prod> DES_PRODlist = new List<Prod>();
        protected List<Prod> Busquedalist = new List<Prod>();
        protected List<Prod> Agregarlist = new List<Prod>();
        protected string CgString = "";
        protected string DesString = "";
        protected int CantidadMostrar = 100;
        protected bool IsVisible { get; set; } = false;

        protected DialogSettings DialogParams = new DialogSettings { MinHeight = "400px", Width = "500px" };

        protected List<Object> Toolbaritems = new List<Object>(){
        "Search",
        "Delete",
        "Print",
        "ExcelExport"
    };
        protected override async Task OnInitializedAsync()
        {
            previsiones = await Http.GetFromJsonAsync<List<PresAnual>>("api/Prevision");

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
            if (args.Item.Text == "Delete")
            {
                bool isConfirmed = await JsRuntime.InvokeAsync<bool>("confirm", "Seguro de que desea eliminar el producto?");
                if (isConfirmed)
                {
                    await Http.GetFromJsonAsync<List<PresAnual>>($"api/Prevision/BorrarPrevision/{this.Grid.GetSelectedRecords().Result.FirstOrDefault().REGISTRO}");
                    previsiones = await Http.GetFromJsonAsync<List<PresAnual>>("api/Prevision");
                    Grid.Refresh();
                }
            }
        }

        public async Task ActionBegin(ActionEventArgs<PresAnual> args)
        {
            if (args.RequestType == Syncfusion.Blazor.Grids.Action.Save)
            {
                HttpResponseMessage response;
                response = await Http.PutAsJsonAsync($"api/Prevision/PutPrev/{args.Data.REGISTRO}", args.Data);
                previsiones = await Http.GetFromJsonAsync<List<PresAnual>>("api/Prevision");
                Grid.Refresh();
            }
            if (args.RequestType == Syncfusion.Blazor.Grids.Action.Delete)
            {

            }
        }

        public void OnSelected()
        {
            CgString = this.Grid2.GetSelectedRecords().Result.FirstOrDefault().CG_PROD; // return the details of selected record
            DesString = this.Grid2.GetSelectedRecords().Result.FirstOrDefault().DES_PROD; // return the details of selected record
            CantidadMostrar = 0;
            IsVisible = false;
        }

        protected async Task OnInputCG_PROD(InputEventArgs args)
        {
            if (args.Value != "")
            {
                CG_PRODlist = await Http.GetFromJsonAsync<List<Prod>>($"api/Prevision/BuscarPorCG_PROD/{args.Value}");
                if (CG_PRODlist.Count > 0)
                {
                    DesString = CG_PRODlist.FirstOrDefault().DES_PROD;
                }
                else
                {
                    DesString = "";
                }
            }
        }

        protected async Task OnInputDES_PROD(InputEventArgs args)
        {
            if (args.Value != "")
            {
                CG_PRODlist = await Http.GetFromJsonAsync<List<Prod>>($"api/Prevision/BuscarPorDES_PROD/{args.Value}");
                if (CG_PRODlist.Count > 0)
                {
                    CgString = CG_PRODlist.FirstOrDefault().CG_PROD;
                }
                else
                {
                    CgString = "";
                }
            }
        }

        protected async Task BuscarProductoPrevision()
        {
            CantidadMostrar = 100;
            if (DesString == "")
            {
                Busquedalist = await Http.GetFromJsonAsync<List<Prod>>($"api/Prevision/BuscarProductoPrevision/{CgString}/Vacio/{CantidadMostrar}");
            }
            else if (CgString == "")
            {
                Busquedalist = await Http.GetFromJsonAsync<List<Prod>>($"api/Prevision/BuscarProductoPrevision/Vacio/{DesString}/{CantidadMostrar}");
            }
            else
            {
                Busquedalist = await Http.GetFromJsonAsync<List<Prod>>($"api/Prevision/BuscarProductoPrevision/{CgString}/{DesString}/{CantidadMostrar}");
            }
            IsVisible = true;
        }
        protected async Task AgregarProductoPrevision()
        {
            previsiones = await Http.GetFromJsonAsync<List<PresAnual>>($"api/Prevision/AgregarProductoPrevision/{CgString}");
            CgString = "";
            DesString = "";
            previsiones = await Http.GetFromJsonAsync<List<PresAnual>>("api/Prevision");
            Grid.Refresh();
        }
        protected async Task AgregarValores()
        {
            CantidadMostrar = CantidadMostrar + 100;
            if (DesString == "")
            {
                Busquedalist = await Http.GetFromJsonAsync<List<Prod>>($"api/Prevision/BuscarProductoPrevision/{CgString}/Vacio/{CantidadMostrar}");
            }
            else if (CgString == "")
            {
                Busquedalist = await Http.GetFromJsonAsync<List<Prod>>($"api/Prevision/BuscarProductoPrevision/Vacio/{DesString}/{CantidadMostrar}");
            }
            else
            {
                Busquedalist = await Http.GetFromJsonAsync<List<Prod>>($"api/Prevision/BuscarProductoPrevision/{CgString}/{DesString}/{CantidadMostrar}");
            }
        }
        protected async Task GuardarDatos(CellSaveArgs<PresAnual> args)
        {
            if (args.ColumnName == "CANTPED")
            {
                previsiones = await Http.GetFromJsonAsync<List<PresAnual>>($"api/Prevision/UpdateCant/{args.RowData.REGISTRO}/{args.Value}");
                await Grid.UpdateCell(args.RowData.REGISTRO, "CANTPED", args.Value);
            }
            else if (args.ColumnName == "FE_PED")
            {
                string Dia = ((DateTime)args.Value).Day.ToString();
                string Mes = ((DateTime)args.Value).Month.ToString();
                string Anio = ((DateTime)args.Value).Year.ToString();
                previsiones = await Http.GetFromJsonAsync<List<PresAnual>>($"api/Prevision/UpdateFecha/{args.RowData.REGISTRO}/{Dia}/{Mes}/{Anio}");
                //await Grid.UpdateCell(args.RowData.REGISTRO, "FE_PED", args.Value);
                Grid.Refresh();
                Showgrid = false;
            }
            //Grid.Refresh();
            //StateHasChanged();
            Showgrid = true;
            Grid.Refresh();
            StateHasChanged();
        }
        public async Task CellSaveHandlerAsync(CellSaveArgs<PresAnual> args)
        {
            if (args.ColumnName == "CANTPED")
            {
                previsiones = await Http.GetFromJsonAsync<List<PresAnual>>($"api/Prevision/UpdateCant/{args.RowData.REGISTRO}/{args.Value}");
            }
            else if (args.ColumnName == "FE_PED")
            {
                string Dia = ((DateTime)args.Value).Day.ToString();
                string Mes = ((DateTime)args.Value).Month.ToString();
                string Anio = ((DateTime)args.Value).Year.ToString();
                prueba = await Http.GetFromJsonAsync<List<PresAnual>>($"api/Prevision/UpdateFecha/{args.RowData.REGISTRO}/{Dia}/{Mes}/{Anio}");
            }
            await Grid.EndEdit();
        }
        public void QueryCellInfoHandler(QueryCellInfoEventArgs<PresAnual> args)
        {
            if (args.Column.Field == "CANTPED")
            {
                args.Cell.AddClass(new string[] { "gris" });
            }
        }
    }
}
