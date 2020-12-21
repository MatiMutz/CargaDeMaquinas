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

namespace SupplyChain.Pages.Unidad
{
    public class UnidadPageBase : ComponentBase
    {
        [Inject] protected CustomHttpClient Http { get; set; }
        [Inject] protected IJSRuntime JsRuntime { get; set; }
        protected SfGrid<Unidades> Grid;

        public bool Enabled = true;
        public bool Disabled = false;

        protected List<Unidades> unidades = new List<Unidades>();

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
            unidades = await Http.GetFromJsonAsync<List<Unidades>>("api/Unidades");

            await base.OnInitializedAsync();
        }

        public void ActionBeginHandler(ActionEventArgs<Unidades> args)
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
        public async Task ActionBegin(ActionEventArgs<Unidades> args)
        {
            if (args.RequestType == Syncfusion.Blazor.Grids.Action.Save)
            {
                HttpResponseMessage response;
                bool found = unidades.Any(o => o.UNID == args.Data.UNID);
                Orificio ur = new Orificio();

                if (!found)
                {
                    args.Data.UNID = unidades.Max(s => s.UNID) + 1;
                    response = await Http.PostAsJsonAsync("api/Unidades", args.Data);
                }
                else
                {
                    response = await Http.PutAsJsonAsync($"api/Unidades/{args.Data.UNID}", args.Data);
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

        private async Task EliminarCeldas(ActionEventArgs<Unidades> args)
        {
            try
            {
                if (args.Data != null)
                {
                    bool isConfirmed = await JsRuntime.InvokeAsync<bool>("confirm", "Seguro de que desea eliminar la Areas?");
                    if (isConfirmed)
                    {
                        //servicios.Remove(servicios.Find(m => m.PEDIDO == args.Data.PEDIDO));
                        await Http.DeleteAsync($"api/Unidades/{args.Data.UNID}");
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
                    foreach (Unidades selectedRecord in this.Grid.SelectedRecords)
                    {
                        bool isConfirmed = await JsRuntime.InvokeAsync<bool>("confirm", "Seguro de que desea copiar el area?");
                        if (isConfirmed)
                        {
                            Unidades Nuevo = new Unidades();

                            Nuevo.UNID = unidades.Max(s => s.UNID) + 1;
                            Nuevo.DES_UNID = selectedRecord.DES_UNID;
                            
                        
                            Nuevo.TIPOUNID = selectedRecord.TIPOUNID;
                            Nuevo.CG_DENBASICA = selectedRecord.CG_DENBASICA;
                            Nuevo.CODIGO = selectedRecord.CODIGO;
                            Nuevo.CG_CIA = selectedRecord.CG_CIA;



                            var response = await Http.PostAsJsonAsync("api/Unidades", Nuevo);

                            if (response.StatusCode == System.Net.HttpStatusCode.Created)
                            {
                                Grid.Refresh();
                                var unidad = await response.Content.ReadFromJsonAsync<Unidades>();
                                await InvokeAsync(StateHasChanged);
                                Nuevo.UNID = unidad.UNID;
                                unidades.Add(Nuevo);
                                var itemsJson = JsonSerializer.Serialize(unidad);
                                Console.WriteLine(itemsJson);
                                unidades.OrderByDescending(o => o.UNID);
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
