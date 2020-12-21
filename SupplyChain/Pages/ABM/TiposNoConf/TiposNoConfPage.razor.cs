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

namespace SupplyChain.Pages.TiposNoConfx
{
    public class TiposNoConfxPageBase : ComponentBase
    {
        [Inject] protected CustomHttpClient Http { get; set; }
        [Inject] protected IJSRuntime JsRuntime { get; set; }
        protected SfGrid<TiposNoConf> Grid;

        public bool Enabled = true;
        public bool Disabled = false;

        protected List<TiposNoConf> tipos = new List<TiposNoConf>();

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
            tipos = await Http.GetFromJsonAsync<List<TiposNoConf>>("api/TiposNoConf");

            await base.OnInitializedAsync();
        }

        public void ActionBeginHandler(ActionEventArgs<TiposNoConf> args)
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
        public async Task ActionBegin(ActionEventArgs<TiposNoConf> args)
        {
            if (args.RequestType == Syncfusion.Blazor.Grids.Action.Save)
            {
                HttpResponseMessage response;
                bool found = tipos.Any(o => o.Cg_TipoNc == args.Data.Cg_TipoNc);
                Orificio ur = new Orificio();

                if (!found)
                {
                    args.Data.Cg_TipoNc = tipos.Max(s => s.Cg_TipoNc) + 1;
                    response = await Http.PostAsJsonAsync("api/TiposNoConf", args.Data);
                }
                else
                {
                    response = await Http.PutAsJsonAsync($"api/TiposNoConf/{args.Data.Cg_TipoNc}", args.Data);
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

        private async Task EliminarCeldas(ActionEventArgs<TiposNoConf> args)
        {
            try
            {
                if (args.Data != null)
                {
                    bool isConfirmed = await JsRuntime.InvokeAsync<bool>("confirm", "Seguro de que desea eliminar la Areas?");
                    if (isConfirmed)
                    {
                        //servicios.Remove(servicios.Find(m => m.PEDIDO == args.Data.PEDIDO));
                        await Http.DeleteAsync($"api/TiposNoConf/{args.Data.Cg_TipoNc}");
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
                    foreach (TiposNoConf selectedRecord in this.Grid.SelectedRecords)
                    {
                        bool isConfirmed = await JsRuntime.InvokeAsync<bool>("confirm", "Seguro de que desea copiar el area?");
                        if (isConfirmed)
                        {
                            TiposNoConf Nuevo = new TiposNoConf();

                            Nuevo.Cg_TipoNc = tipos.Max(s => s.Cg_TipoNc) + 1;
                            Nuevo.Cg_TipoNc = selectedRecord.Des_TipoNc;

                            Nuevo.CG_CIA = selectedRecord.CG_CIA;
                         
               

                            var response = await Http.PostAsJsonAsync("api/TiposNoConf", Nuevo);

                            if (response.StatusCode == System.Net.HttpStatusCode.Created)
                            {
                                Grid.Refresh();
                                var tipo = await response.Content.ReadFromJsonAsync<TiposNoConf>();
                                await InvokeAsync(StateHasChanged);
                                Nuevo.Cg_TipoNc = tipo.Cg_TipoNc;
                                tipos.Add(Nuevo);
                                var itemsJson = JsonSerializer.Serialize(tipo);
                                Console.WriteLine(itemsJson);
                                tipos.OrderByDescending(o => o.Cg_TipoNc);
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
