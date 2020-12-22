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

namespace SupplyChain.Pages.Indicx
{
    public class IndicxPageBase : ComponentBase
    {
        [Inject] protected CustomHttpClient Http { get; set; }
        [Inject] protected IJSRuntime JsRuntime { get; set; }
        protected SfGrid<Indic> Grid;

        public bool Enabled = true;
        public bool Disabled = false;

        protected List<Indic> indics = new List<Indic>();

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
            indics = await Http.GetFromJsonAsync<List<Indic>>("api/Indic");

            await base.OnInitializedAsync();
        }

        public void ActionBeginHandler(ActionEventArgs<Indic> args)
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
        public async Task ActionBegin(ActionEventArgs<Indic> args)
        {
            if (args.RequestType == Syncfusion.Blazor.Grids.Action.Save)
            {
                HttpResponseMessage response;
                bool found = indics.Any(o => o.REGISTRO == args.Data.REGISTRO);
                Orificio ur = new Orificio();

                if (!found)
                {
                    args.Data.REGISTRO = indics.Max(s => s.REGISTRO) + 1;
                    response = await Http.PostAsJsonAsync("api/Indic", args.Data);
                }
                else
                {
                    response = await Http.PutAsJsonAsync($"api/Indic/{args.Data.REGISTRO}", args.Data);
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

        private async Task EliminarCeldas(ActionEventArgs<Indic> args)
        {
            try
            {
                if (args.Data != null)
                {
                    bool isConfirmed = await JsRuntime.InvokeAsync<bool>("confirm", "Seguro de que desea eliminar la Areas?");
                    if (isConfirmed)
                    {
                        //servicios.Remove(servicios.Find(m => m.PEDIDO == args.Data.PEDIDO));
                        await Http.DeleteAsync($"api/Indic/{args.Data.REGISTRO}");
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
                    foreach (Indic selectedRecord in this.Grid.SelectedRecords)
                    {
                        bool isConfirmed = await JsRuntime.InvokeAsync<bool>("confirm", "Seguro de que desea copiar el area?");
                        if (isConfirmed)
                        {
                            Indic Nuevo = new Indic();

                            Nuevo.REGISTRO = indics.Max(s => s.REGISTRO) + 1;
                            Nuevo.DES_IND = selectedRecord.DES_IND;
                            Nuevo.VA_INDIC = selectedRecord.VA_INDIC;


                            Nuevo.VA_COMPRA = selectedRecord.VA_COMPRA;
                            Nuevo.FE_INDIC= selectedRecord.FE_INDIC;
                         
               

                            var response = await Http.PostAsJsonAsync("api/Indic", Nuevo);

                            if (response.StatusCode == System.Net.HttpStatusCode.Created)
                            {
                                Grid.Refresh();
                                var indi = await response.Content.ReadFromJsonAsync<Indic>();
                                await InvokeAsync(StateHasChanged);
                                Nuevo.REGISTRO = indi.REGISTRO;
                                indics.Add(Nuevo);
                                var itemsJson = JsonSerializer.Serialize(indi);
                                Console.WriteLine(itemsJson);
                                indics.OrderByDescending(o => o.REGISTRO);
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
