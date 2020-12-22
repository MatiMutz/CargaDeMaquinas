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

namespace SupplyChain.Pages.Series
{
    public class SeriePageBase : ComponentBase
    {
        [Inject] protected CustomHttpClient Http { get; set; }
        [Inject] protected IJSRuntime JsRuntime { get; set; }
        protected SfGrid<Serie> Grid;

        public bool Enabled = true;
        public bool Disabled = false;

        protected List<Serie> series = new List<Serie>();

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
            series = await Http.GetFromJsonAsync<List<Serie>>("api/Serie");

            await base.OnInitializedAsync();
        }

        public void ActionBeginHandler(ActionEventArgs<Serie> args)
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
        public async Task ActionBegin(ActionEventArgs<Serie> args)
        {
            if (args.RequestType == Syncfusion.Blazor.Grids.Action.Save)
            {
                HttpResponseMessage response;
                bool found = series.Any(o => o.Id == args.Data.Id);
                Serie ur = new Serie();

                if (!found)
                {
                    args.Data.Id = series.Max(s => s.Id) + 1;
                    args.Data.CG_ORDEN = 1;
                    response = await Http.PostAsJsonAsync("api/Series", args.Data);
                }
                else
                {
                    response = await Http.PutAsJsonAsync($"api/Series/{args.Data.Id}", args.Data);
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

        private async Task EliminarServicio(ActionEventArgs<Serie> args)
        {
            try
            {
                if (args.Data != null)
                {
                    bool isConfirmed = await JsRuntime.InvokeAsync<bool>("confirm", "Seguro de que desea eliminar el serie / la reparacion?");
                    if (isConfirmed)
                    {
                        //servicios.Remove(servicios.Find(m => m.PEDIDO == args.Data.PEDIDO));
                        await Http.DeleteAsync($"api/Serie/{args.Data.Id}");
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
                    foreach (Serie selectedRecord in this.Grid.SelectedRecords)
                    {
                        bool isConfirmed = await JsRuntime.InvokeAsync<bool>("confirm", "Seguro de que desea copiar el Serie / la reparacion?");
                        if (isConfirmed)
                        {
                            Serie Nuevo = new Serie();

                            Nuevo.Id = series.Max(s => s.Id) + 1;
                            Nuevo.Codigo = selectedRecord.Codigo;
                            Nuevo.Descripcion = selectedRecord.Descripcion;
                            Nuevo.CG_ORDEN = selectedRecord.CG_ORDEN;

                            var response = await Http.PostAsJsonAsync("api/Codigo", Nuevo);

                            if (response.StatusCode == System.Net.HttpStatusCode.Created)
                            {
                                Grid.Refresh();
                                var serie = await response.Content.ReadFromJsonAsync<Serie>();
                                await InvokeAsync(StateHasChanged);
                                Nuevo.Id = serie.Id;
                                series.Add(Nuevo);
                                var itemsJson = JsonSerializer.Serialize(serie);
                                Console.WriteLine(itemsJson);
                                //toastService.ShowToast($"Registrado Correctemente.Vale {StockGuardado.VALE}", TipoAlerta.Success);
                                series.OrderByDescending(o => o.Id);
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
