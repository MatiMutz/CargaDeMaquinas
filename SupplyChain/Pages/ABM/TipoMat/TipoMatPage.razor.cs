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

namespace SupplyChain.Pages.TipoMatx
{
    public class TipoMatxPageBase : ComponentBase
    {
        [Inject] protected CustomHttpClient Http { get; set; }
        [Inject] protected IJSRuntime JsRuntime { get; set; }
        protected SfGrid<TipoMat> Grid;

        public bool Enabled = true;
        public bool Disabled = false;

        protected List<TipoMat> tipomats = new List<TipoMat>();

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
            tipomats = await Http.GetFromJsonAsync<List<TipoMat>>("api/TipoMat");

            await base.OnInitializedAsync();
        }

        public void ActionBeginHandler(ActionEventArgs<TipoMat> args)
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
        public async Task ActionBegin(ActionEventArgs<TipoMat> args)
        {
            if (args.RequestType == Syncfusion.Blazor.Grids.Action.Save)
            {
                HttpResponseMessage response;
                bool found = tipomats.Any(o => o.TIPO == args.Data.TIPO);
                Orificio ur = new Orificio();

                if (!found)
                {
                    args.Data.TIPO = tipomats.Max(s => s.TIPO) + 1;
                    response = await Http.PostAsJsonAsync("api/TipoMat", args.Data);
                }
                else
                {
                    response = await Http.PutAsJsonAsync($"api/TipoMat/{args.Data.TIPO}", args.Data);
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

        private async Task EliminarCeldas(ActionEventArgs<TipoMat> args)
        {
            try
            {
                if (args.Data != null)
                {
                    bool isConfirmed = await JsRuntime.InvokeAsync<bool>("confirm", "Seguro de que desea eliminar la Areas?");
                    if (isConfirmed)
                    {
                        //servicios.Remove(servicios.Find(m => m.PEDIDO == args.Data.PEDIDO));
                        await Http.DeleteAsync($"api/TipoMat/{args.Data.TIPO}");
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
                    foreach (TipoMat selectedRecord in this.Grid.SelectedRecords)
                    {
                        bool isConfirmed = await JsRuntime.InvokeAsync<bool>("confirm", "Seguro de que desea copiar el area?");
                        if (isConfirmed)
                        {
                            TipoMat Nuevo = new TipoMat();

                            Nuevo.TIPO = tipomats.Max(s => s.TIPO) + 1;
                            Nuevo.CG_CIA = selectedRecord.CG_CIA;

               

                            var response = await Http.PostAsJsonAsync("api/TipoMat", Nuevo);

                            if (response.StatusCode == System.Net.HttpStatusCode.Created)
                            {
                                Grid.Refresh();
                                var tipo = await response.Content.ReadFromJsonAsync<TipoMat>();
                                await InvokeAsync(StateHasChanged);
                                Nuevo.TIPO = tipo.TIPO;
                                tipomats.Add(Nuevo);
                                var itemsJson = JsonSerializer.Serialize(tipo);
                                Console.WriteLine(itemsJson);
                                tipomats.OrderByDescending(o => o.TIPO);
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
