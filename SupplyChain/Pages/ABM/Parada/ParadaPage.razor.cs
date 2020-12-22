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

namespace SupplyChain.Pages.Paradax
{
    public class ParadaxPageBase : ComponentBase
    {
        [Inject] protected CustomHttpClient Http { get; set; }
        [Inject] protected IJSRuntime JsRuntime { get; set; }
        protected SfGrid<Parada> Grid;

        public bool Enabled = true;
        public bool Disabled = false;

        protected List<Parada> paradas = new List<Parada>();

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
            paradas = await Http.GetFromJsonAsync<List<Parada>>("api/Parada");

            await base.OnInitializedAsync();
        }

        public void ActionBeginHandler(ActionEventArgs<Parada> args)
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
        public async Task ActionBegin(ActionEventArgs<Parada> args)
        {
            if (args.RequestType == Syncfusion.Blazor.Grids.Action.Save)
            {
                HttpResponseMessage response;
                bool found = paradas.Any(o => o.CP== args.Data.CP);
                Orificio ur = new Orificio();

                if (!found)
                {
                    args.Data.CP = paradas.Max(s => s.CP) + 1;
                    response = await Http.PostAsJsonAsync("api/Parada", args.Data);
                }
                else
                {
                    response = await Http.PutAsJsonAsync($"api/Parada/{args.Data.CP}", args.Data);
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

        private async Task EliminarCeldas(ActionEventArgs<Parada> args)
        {
            try
            {
                if (args.Data != null)
                {
                    bool isConfirmed = await JsRuntime.InvokeAsync<bool>("confirm", "Seguro de que desea eliminar la Areas?");
                    if (isConfirmed)
                    {
                        //servicios.Remove(servicios.Find(m => m.PEDIDO == args.Data.PEDIDO));
                        await Http.DeleteAsync($"api/Parada/{args.Data.CP}");
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
                    foreach (Parada selectedRecord in this.Grid.SelectedRecords)
                    {
                        bool isConfirmed = await JsRuntime.InvokeAsync<bool>("confirm", "Seguro de que desea copiar el area?");
                        if (isConfirmed)
                        {
                            Parada Nuevo = new Parada();

                            Nuevo.CP = paradas.Max(s => s.CP) + 1;
                            Nuevo.DESCRIP = selectedRecord.DESCRIP;
                            Nuevo.CG_CIA = selectedRecord.CG_CIA;
                         
               

                            var response = await Http.PostAsJsonAsync("api/Parada", Nuevo);

                            if (response.StatusCode == System.Net.HttpStatusCode.Created)
                            {
                                Grid.Refresh();
                                var para = await response.Content.ReadFromJsonAsync<Parada>();
                                await InvokeAsync(StateHasChanged);
                                Nuevo.CP = para.CP;
                                paradas.Add(Nuevo);
                                var itemsJson = JsonSerializer.Serialize(para);
                                Console.WriteLine(itemsJson);
                                paradas.OrderByDescending(o => o.CP);
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
