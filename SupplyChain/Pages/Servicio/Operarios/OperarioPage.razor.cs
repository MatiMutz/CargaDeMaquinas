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

namespace SupplyChain.Pages.Operarios
{
    public class OperarioPageBase : ComponentBase
    {
        [Inject] protected CustomHttpClient Http { get; set; }
        [Inject] protected IJSRuntime JsRuntime { get; set; }
        protected SfGrid<Operario> Grid;

        public bool Enabled = true;
        public bool Disabled = false;

        protected List<Operario> operarios = new List<Operario>();

        protected List<Object> Toolbaritems = new List<Object>(){
        "Search",
        "Add",
        "Edit",
        "Delete",
        "Print",
        new ItemModel { Text = "Copy", TooltipText = "Copy", PrefixIcon = "e-copy", Id = "copy" },
        "ExcelExport"
    };

        protected override async Task OnInitializedAsync()
        {
            operarios = await Http.GetFromJsonAsync<List<Operario>>("api/Operario");

            await base.OnInitializedAsync();
        }

        public void ActionBeginHandler(ActionEventArgs<Operario> args)
        {
            if (args.RequestType == Syncfusion.Blazor.Grids.Action.BeginEdit)
            {
                this.Enabled = false;
            }
            else
            {
                this.Enabled = true;
            }
        }
        public async Task ActionBegin(ActionEventArgs<Operario> args)
        {
            if (args.RequestType == Syncfusion.Blazor.Grids.Action.Save)
            {
                HttpResponseMessage response;
                bool found = operarios.Any(o => o.CG_OPER == args.Data.CG_OPER);
                Operario ur = new Operario();

                if (!found)
                {
                    args.Data.CG_OPER = operarios.Max(s => s.CG_OPER) + 1;
                    response = await Http.PostAsJsonAsync("api/Operario", args.Data);
                }
                else
                {
                    response = await Http.PutAsJsonAsync($"api/Operario/{args.Data.CG_OPER}", args.Data);
                }

                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {

                }
            }

            if (args.RequestType == Syncfusion.Blazor.Grids.Action.Delete)
            {
                await EliminarServicio(args);
            }
        }

        private async Task EliminarServicio(ActionEventArgs<Operario> args)
        {
            try
            {
                if (args.Data != null)
                {
                    bool isConfirmed = await JsRuntime.InvokeAsync<bool>("confirm", "Seguro de que desea eliminar el OPERARIO / la reparacion?");
                    if (isConfirmed)
                    {
                        //servicios.Remove(servicios.Find(m => m.PEDIDO == args.Data.PEDIDO));
                        await Http.DeleteAsync($"api/Operario/{args.Data.CG_OPER}");
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async Task ClickHandler(Syncfusion.Blazor.Navigations.ClickEventArgs args)
        {
            if (args.Item.Text == "Copy")
            {
                if (this.Grid.SelectedRecords.Count > 0)
                {
                    foreach (Operario selectedRecord in this.Grid.SelectedRecords)
                    {
                        bool isConfirmed = await JsRuntime.InvokeAsync<bool>("confirm", "Seguro de que desea copiar el OPERARIO / la reparacion?");
                        if (isConfirmed)
                        {
                            Operario Nuevo = new Operario();

                            Nuevo.CG_OPER = operarios.Max(s => s.CG_OPER) + 1;
                            Nuevo.DES_OPER = selectedRecord.DES_OPER;
                            Nuevo.CG_TURNO = selectedRecord.CG_TURNO;
                            Nuevo.RENDIM = selectedRecord.RENDIM;
                            Nuevo.FE_FINAL = selectedRecord.FE_FINAL;
                            Nuevo.HS_FINAL = selectedRecord.HS_FINAL;
                            Nuevo.CG_CATEOP = selectedRecord.CG_CATEOP;
                            Nuevo.VALOR_HORA = selectedRecord.VALOR_HORA;
                            Nuevo.MONEDA = selectedRecord.MONEDA;
                            Nuevo.ACTIVO = selectedRecord.ACTIVO;
                            Nuevo.CG_CIA = selectedRecord.CG_CIA;
                            Nuevo.USUARIO = selectedRecord.USUARIO;


                            var response = await Http.PostAsJsonAsync("api/Operario", Nuevo);

                            if (response.StatusCode == System.Net.HttpStatusCode.Created)
                            {
                                Grid.Refresh();
                                var operario = await response.Content.ReadFromJsonAsync<Operario>();
                                await InvokeAsync(StateHasChanged);
                                Nuevo.CG_OPER = operario.CG_OPER;
                                operarios.Add(Nuevo);
                                var itemsJson = JsonSerializer.Serialize(operario);
                                Console.WriteLine(itemsJson);
                                //toastService.ShowToast($"Registrado Correctemente.Vale {StockGuardado.VALE}", TipoAlerta.Success);
                                operarios.OrderByDescending(o => o.CG_OPER);
                            }

                        }
                    }
                }
            }
            if (args.Item.Text == "Excel Export")
            {
                await this.Grid.ExcelExport();
            }
        }

        public void Refresh()
        {
            Grid.Refresh();

        }
    }
}
