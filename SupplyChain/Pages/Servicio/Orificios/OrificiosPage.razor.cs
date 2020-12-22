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

namespace SupplyChain.Pages.Orificios
{
    public class OrificioPageBase : ComponentBase
    {
        [Inject] protected CustomHttpClient Http { get; set; }
        [Inject] protected IJSRuntime JsRuntime { get; set; }
        protected SfGrid<Orificio> Grid;

        public bool Enabled = true;
        public bool Disabled = false;

        protected List<Orificio> orificios = new List<Orificio>();

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
            orificios = await Http.GetFromJsonAsync<List<Orificio>>("api/Orificio");

            await base.OnInitializedAsync();
        }

        public void ActionBeginHandler(ActionEventArgs<Orificio> args)
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
        public async Task ActionBegin(ActionEventArgs<Orificio> args)
        {
            if (args.RequestType == Syncfusion.Blazor.Grids.Action.Save)
            {
                HttpResponseMessage response;
                bool found = orificios.Any(o => o.Id == args.Data.Id);
                Orificio ur = new Orificio();

                if (!found)
                {
                    args.Data.Id = orificios.Max(s => s.Id) + 1;
                    args.Data.CG_ORDEN = 1;
                    response = await Http.PostAsJsonAsync("api/Orificio", args.Data);
                }
                else
                {
                    response = await Http.PutAsJsonAsync($"api/Orificio/{args.Data.Id}", args.Data);
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

        private async Task EliminarServicio(ActionEventArgs<Orificio> args)
        {
            try
            {
                if (args.Data != null)
                {
                    bool isConfirmed = await JsRuntime.InvokeAsync<bool>("confirm", "Seguro de que desea eliminar el orificio / la reparacion?");
                    if (isConfirmed)
                    {
                        //servicios.Remove(servicios.Find(m => m.PEDIDO == args.Data.PEDIDO));
                        await Http.DeleteAsync($"api/Orificio/{args.Data.Id}");
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
                    foreach (Orificio selectedRecord in this.Grid.SelectedRecords)
                    {
                        bool isConfirmed = await JsRuntime.InvokeAsync<bool>("confirm", "Seguro de que desea copiar el Orificio / la reparacion?");
                        if (isConfirmed)
                        {
                            Orificio Nuevo = new Orificio();

                            Nuevo.Id = orificios.Max(s => s.Id) + 1;
                            Nuevo.Codigo = selectedRecord.Codigo;
                            Nuevo.Descripcion = selectedRecord.Descripcion;
                            Nuevo.CG_ORDEN = selectedRecord.CG_ORDEN;

                            var response = await Http.PostAsJsonAsync("api/Orificio", Nuevo);

                            if (response.StatusCode == System.Net.HttpStatusCode.Created)
                            {
                                Grid.Refresh();
                                var orificio = await response.Content.ReadFromJsonAsync<Orificio>();
                                await InvokeAsync(StateHasChanged);
                                Nuevo.Id = orificio.Id;
                                orificios.Add(Nuevo);
                                var itemsJson = JsonSerializer.Serialize(orificio);
                                Console.WriteLine(itemsJson);
                                //toastService.ShowToast($"Registrado Correctemente.Vale {StockGuardado.VALE}", TipoAlerta.Success);
                                orificios.OrderByDescending(o => o.Id);
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
