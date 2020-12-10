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

namespace SupplyChain.Pages.Area
{
    public class AreaPageBase : ComponentBase
    {
        [Inject] protected CustomHttpClient Http { get; set; }
        [Inject] protected IJSRuntime JsRuntime { get; set; }
        protected SfGrid<Areas> Grid;

        public bool Enabled = true;
        public bool Disabled = false;

        protected List<Areas> areas = new List<Areas>();

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
            areas = await Http.GetFromJsonAsync<List<Areas>>("api/Areas");

            await base.OnInitializedAsync();
        }

        public void ActionBeginHandler(ActionEventArgs<Areas> args)
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
        public async Task ActionBegin(ActionEventArgs<Areas> args)
        {
            if (args.RequestType == Syncfusion.Blazor.Grids.Action.Save)
            {
                HttpResponseMessage response;
                bool found = areas.Any(o => o.CG_AREA == args.Data.CG_AREA);
                Orificio ur = new Orificio();

                if (!found)
                {
                    args.Data.CG_AREA = areas.Max(s => s.CG_AREA) + 1;
                    response = await Http.PostAsJsonAsync("api/Areas", args.Data);
                }
                else
                {
                    response = await Http.PutAsJsonAsync($"api/Areas/{args.Data.CG_AREA}", args.Data);
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

        private async Task EliminarCeldas(ActionEventArgs<Areas> args)
        {
            try
            {
                if (args.Data != null)
                {
                    bool isConfirmed = await JsRuntime.InvokeAsync<bool>("confirm", "Seguro de que desea eliminar la Areas?");
                    if (isConfirmed)
                    {
                        //servicios.Remove(servicios.Find(m => m.PEDIDO == args.Data.PEDIDO));
                        await Http.DeleteAsync($"api/Areas/{args.Data.CG_AREA}");
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
                    foreach (Areas selectedRecord in this.Grid.SelectedRecords)
                    {
                        bool isConfirmed = await JsRuntime.InvokeAsync<bool>("confirm", "Seguro de que desea copiar el area?");
                        if (isConfirmed)
                        {
                            Areas Nuevo = new Areas();

                            Nuevo.CG_AREA = areas.Max(s => s.CG_AREA) + 1;
                            Nuevo.DES_AREA = selectedRecord.DES_AREA;
                            Nuevo.RESP = selectedRecord.RESP;
                            Nuevo.CONTROLES = selectedRecord.CONTROLES;
                            Nuevo.TARA = selectedRecord.TARA;
                            Nuevo.CG_TIPOAREA = selectedRecord.CG_TIPOAREA;
                            Nuevo.CG_PROVE = selectedRecord.CG_PROVE;
                            Nuevo.CG_CIA = selectedRecord.CG_CIA;
                            Nuevo.CG_DEP = selectedRecord.CG_DEP;
                            Nuevo.CG_COS = selectedRecord.CG_COS;
                            
               

                            var response = await Http.PostAsJsonAsync("api/Areas", Nuevo);

                            if (response.StatusCode == System.Net.HttpStatusCode.Created)
                            {
                                Grid.Refresh();
                                var area = await response.Content.ReadFromJsonAsync<Areas>();
                                await InvokeAsync(StateHasChanged);
                                Nuevo.CG_AREA = area.CG_AREA;
                                areas.Add(Nuevo);
                                var itemsJson = JsonSerializer.Serialize(area);
                                Console.WriteLine(itemsJson);
                                areas.OrderByDescending(o => o.CG_AREA);
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
