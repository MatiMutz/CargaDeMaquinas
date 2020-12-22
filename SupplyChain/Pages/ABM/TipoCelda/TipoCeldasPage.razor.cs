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

namespace SupplyChain.Pages.TipoCeldas
{
    public class TipoCeldasPageBase : ComponentBase
    {
        [Inject] protected CustomHttpClient Http { get; set; }
        [Inject] protected IJSRuntime JsRuntime { get; set; }
        protected SfGrid<TipoCelda> Grid;

        public bool Enabled = true;
        public bool Disabled = false;

        protected List<TipoCelda> tipoceldas = new List<TipoCelda>();

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
            tipoceldas = await Http.GetFromJsonAsync<List<TipoCelda>>("api/TipoCelda");

            await base.OnInitializedAsync();
        }

        public void ActionBeginHandler(ActionEventArgs<TipoCelda> args)
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
        public async Task ActionBegin(ActionEventArgs<TipoCelda> args)
        {
            if (args.RequestType == Syncfusion.Blazor.Grids.Action.Save)
            {
                HttpResponseMessage response;
                bool found = tipoceldas.Any(o => o.CG_TIPOCELDA == args.Data.CG_TIPOCELDA);
                Orificio ur = new Orificio();

                if (!found)
                {
                    args.Data.CG_TIPOCELDA = tipoceldas.Max(s => s.CG_TIPOCELDA) + 1;
                    response = await Http.PostAsJsonAsync("api/TipoCelda", args.Data);
                }
                else
                {
                    response = await Http.PutAsJsonAsync($"api/TipoCelda/{args.Data.CG_TIPOCELDA}", args.Data);
                }

                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {

                }
            }

            if (args.RequestType == Syncfusion.Blazor.Grids.Action.Delete)
            {
                await EliminarCeldas(args);
            }
        }

        private async Task EliminarCeldas(ActionEventArgs<TipoCelda> args)
        {
            try
            {
                if (args.Data != null)
                {
                    bool isConfirmed = await JsRuntime.InvokeAsync<bool>("confirm", "Seguro de que desea eliminar la Areas?");
                    if (isConfirmed)
                    {
                        //servicios.Remove(servicios.Find(m => m.PEDIDO == args.Data.PEDIDO));
                        await Http.DeleteAsync($"api/TipóCelda/{args.Data.CG_TIPOCELDA}");
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
                    foreach (TipoCelda selectedRecord in this.Grid.SelectedRecords)
                    {
                        bool isConfirmed = await JsRuntime.InvokeAsync<bool>("confirm", "Seguro de que desea copiar el area?");
                        if (isConfirmed)
                        {
                            TipoCelda Nuevo = new TipoCelda();

                            Nuevo.CG_TIPOCELDA = tipoceldas.Max(s => s.CG_TIPOCELDA) + 1;
                            Nuevo.DES_TIPOCELDA = selectedRecord.DES_TIPOCELDA;
                            Nuevo.CG_CIA = selectedRecord.CG_CIA;
                         
               

                            var response = await Http.PostAsJsonAsync("api/TipoCelda", Nuevo);

                            if (response.StatusCode == System.Net.HttpStatusCode.Created)
                            {
                                Grid.Refresh();
                                var tipocelda = await response.Content.ReadFromJsonAsync<TipoCelda>();
                                await InvokeAsync(StateHasChanged);
                                Nuevo.CG_TIPOCELDA = tipocelda.CG_TIPOCELDA;
                                tipoceldas.Add(Nuevo);
                                var itemsJson = JsonSerializer.Serialize(tipocelda);
                                Console.WriteLine(itemsJson);
                                tipoceldas.OrderByDescending(o => o.CG_TIPOCELDA);
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
