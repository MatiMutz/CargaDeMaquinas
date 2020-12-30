using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SupplyChain;
using Syncfusion.Blazor.FileManager;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;
using Syncfusion.Blazor.RichTextEditor.Internal;
using Syncfusion.ExcelExport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System.IO;
using Syncfusion.Pdf.Grid;
using Syncfusion.Blazor.Diagrams;
using Syncfusion.Pdf.Tables;
using System.Data;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Drawing;
using Syncfusion.Blazor.Buttons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Inputs;

namespace SupplyChain.Pages.Servicios
{
    public class ServicioPageBase : ComponentBase
    {
        [Inject] protected CustomHttpClient Http { get; set; }
        [Inject] protected IJSRuntime JsRuntime { get; set; }
        [Inject] protected Microsoft.JSInterop.IJSRuntime JS { get; set; }

        [Parameter]
        public string pedido { get; set; } = "";

        protected SfGrid<Service> Grid;
        public string NroPedido = "";
        public bool Enabled = true;
        public bool Disabled = false;
        public bool Visible { get; set; } = true;

        public void toolbarClick(ToolbarClickEventArgs args)
        {
            if (args.Item.Text == "Custom")
            {
                // Perform the operation based on your requirement.
                System.Diagnostics.Debug.Write("Custom item clicked");
            }
        }
        public void menuClick(MenuClickEventArgs args)
        {
            if (args.Item.Text == "Custom")
            {
                // Perform the operation based on your requirement.
                System.Diagnostics.Debug.WriteLine("Custom item clicked");
            }
        }
        public class SIoNO
        {
            public string Text { get; set; }
        }
        public List<SIoNO> SIoNOData = new List<SIoNO> {
            new SIoNO() {Text= "SI"},
            new SIoNO() {Text= "NO"}};

        public class Sobrepresiones
        {
            public string Text { get; set; }
        }
        public List<Sobrepresiones> SobrepresionData = new List<Sobrepresiones> {
            new Sobrepresiones() {Text= "3"},
            new Sobrepresiones() {Text= "10"},
            new Sobrepresiones() {Text= "16"},
            new Sobrepresiones() {Text= "21"},
            new Sobrepresiones() {Text= "25"}
        };
        public class Tipos
        {
            public string Text { get; set; }
        }
        public List<Tipos> TipoData = new List<Tipos> {
            new Tipos() {Text= "Cte"},
            new Tipos() {Text= "VAR"}
        };

        public class Estados
        {
            public string Text { get; set; }
        }
        public List<Estados> EstadosData = new List<Estados> {
            new Estados() {Text= "BUENO"},
            new Estados() {Text= "REGULAR"},
            new Estados() {Text= "MUY DETERIORADO"}
        };

        protected List<Service> servicios = new List<Service>();
        protected List<Medida> medidas = new List<Medida>();
        protected List<Serie> series = new List<Serie>();
        protected List<Orificio> orificios = new List<Orificio>();
        protected List<Sobrepresion> sobrepresiones = new List<Sobrepresion>();
        protected List<Tipo> tipos = new List<Tipo>();
        protected List<Estado> estados = new List<Estado>();
        protected List<Trabajosefec> trabajosEfectuados = new List<Trabajosefec>();
        protected List<Marca> marcas = new List<Marca>();
        protected List<Operario> operarios = new List<Operario>();
        protected IEnumerable<Operario> opers;
        protected List<Celdas> celdas = new List<Celdas>();
        protected List<Solution> rutas;
        protected List<Service> servDesc;
        protected string pedant;

        protected DialogSettings DialogParams = new DialogSettings { MinHeight = "400px", Width = "1100px" };

        protected List<Object> Toolbaritems = new List<Object>(){
        "Edit",
        "Delete",
        "Print",
        new Syncfusion.Blazor.Navigations.ItemModel { Text = "PdfExport", TooltipText = "PdfExport", PrefixIcon = "e-copy", Id = "PdfExport" },
        new Syncfusion.Blazor.Navigations.ItemModel { Text = "Copy", TooltipText = "Copy", PrefixIcon = "e-copy", Id = "copy" },
        "ExcelExport"
    };

        protected override async Task OnInitializedAsync()
        {
            servicios = await Http.GetFromJsonAsync<List<Service>>("api/Servicios");
            if (!string.IsNullOrEmpty(pedido))
            {
                servDesc = servicios.Where(s => s.PEDIDO == pedido).ToList();
            }
            else
            {
                servDesc = servicios.OrderByDescending(s => s.PEDIDO).ToList();
            }
            medidas = await Http.GetFromJsonAsync<List<Medida>>("api/Medida");
            series = await Http.GetFromJsonAsync<List<Serie>>("api/Serie");
            orificios = await Http.GetFromJsonAsync<List<Orificio>>("api/Orificio");
            sobrepresiones = await Http.GetFromJsonAsync<List<Sobrepresion>>("api/Sobrepresion");
            tipos = await Http.GetFromJsonAsync<List<Tipo>>("api/Tipo");
            estados = await Http.GetFromJsonAsync<List<Estado>>("api/Estado");
            trabajosEfectuados = await Http.GetFromJsonAsync<List<Trabajosefec>>("api/TrabajosEfec");
            marcas = await Http.GetFromJsonAsync<List<Marca>>("api/Marca");
            operarios = await Http.GetFromJsonAsync<List<Operario>>("api/Operario");
            opers = from opers in (IEnumerable<Operario>)operarios
                    where opers.ACTIVO == true
                    select opers;
            celdas = await Http.GetFromJsonAsync<List<Celdas>>("api/Celdas");
            rutas = await Http.GetFromJsonAsync<List<Solution>>("api/Solution");
            await base.OnInitializedAsync();
        }

        public void ActionBeginHandler(ActionEventArgs<Service> args)
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
        public async Task ActionBegin(ActionEventArgs<Service> args)
        {
            if (args.RequestType == Syncfusion.Blazor.Grids.Action.Save)
            {
                HttpResponseMessage response;
                bool found = servicios.Any(o => o.PEDIDO == args.Data.PEDIDO);
                Service ur = new Service();

                if (!found)
                {
                    args.Data.PEDIDO = servicios.Max(s => s.PEDIDO) + 1;
                    response = await Http.PostAsJsonAsync("api/Servicios", args.Data);
                    servDesc = servicios.OrderByDescending(s => s.PEDIDO).ToList();
                }
                else
                {
                    pedant = servicios.Where(s => s.PEDIDO == args.Data.PEDIDO).FirstOrDefault().PEDIDOANT;
                    if (pedant != args.Data.PEDIDOANT)
                    {
                        foreach (var ped in servicios)
                        {
                            if (args.Data.PEDIDOANT == ped.PEDIDO)
                            {
                                bool isConfirmed = await JsRuntime.InvokeAsync<bool>("confirm", "Quiere traer los datos del pedido anterior?");
                                if (isConfirmed)
                                {
                                    if (ped.FECHA.ToString().Substring(3, 1) == "/")
                                    {
                                        args.Data.FECMANTANT = ped.FECHA.ToString().Substring(0, 8);
                                    }
                                    else if (ped.FECHA.ToString().Substring(4, 1) == "/")
                                    {
                                        args.Data.FECMANTANT = ped.FECHA.ToString().Substring(0, 9);
                                    }
                                    else if (ped.FECHA.ToString().Substring(5, 1) == "/")
                                    {
                                        args.Data.FECMANTANT = ped.FECHA.ToString().Substring(0, 10);
                                    }
                                    args.Data.IDENTIFICACION = ped.IDENTIFICACION;
                                    args.Data.MARCA = ped.MARCA;
                                    args.Data.NSERIE = ped.NSERIE;
                                    args.Data.MODELO = ped.MODELO;
                                    args.Data.MEDIDA = ped.MEDIDA;
                                    args.Data.SERIE = ped.SERIE;
                                    args.Data.ORIFICIO = ped.ORIFICIO;
                                    args.Data.AÑO = ped.AÑO;
                                    args.Data.AREA = ped.AREA;
                                    args.Data.FLUIDO = ped.FLUIDO;
                                    args.Data.SOBREPRESION = ped.SOBREPRESION;
                                    args.Data.PRESION = ped.PRESION;
                                    args.Data.CONTRAPRESION = ped.CONTRAPRESION;
                                    args.Data.TIPO = ped.TIPO;
                                    args.Data.TEMP = ped.TEMP;
                                    args.Data.RESORTE = ped.RESORTE;
                                    args.Data.PRESIONBANCO = ped.PRESIONBANCO;
                                    args.Data.SERVICIO = ped.SERVICIO;
                                }
                            }
                        }
                    }
                    args.Data.MARCA = string.IsNullOrEmpty(args.Data.MARCA) ? "" : args.Data.MARCA;
                    args.Data.MODELO = string.IsNullOrEmpty(args.Data.MODELO) ? "" : args.Data.MODELO;
                    args.Data.MEDIDA = string.IsNullOrEmpty(args.Data.MEDIDA) ? "" : args.Data.MEDIDA;
                    args.Data.SERIE = string.IsNullOrEmpty(args.Data.SERIE) ? "" : args.Data.SERIE;
                    args.Data.ORIFICIO = string.IsNullOrEmpty(args.Data.ORIFICIO) ? "" : args.Data.ORIFICIO;
                    args.Data.SOBREPRESION = string.IsNullOrEmpty(args.Data.SOBREPRESION) ? "" : args.Data.SOBREPRESION;
                    args.Data.TIPO = string.IsNullOrEmpty(args.Data.TIPO) ? "" : args.Data.TIPO;
                    args.Data.ENSRECEP = string.IsNullOrEmpty(args.Data.ENSRECEP) ? "" : args.Data.ENSRECEP;
                    args.Data.ESTADO = string.IsNullOrEmpty(args.Data.ESTADO) ? "" : args.Data.ESTADO;
                    args.Data.FUGAS = string.IsNullOrEmpty(args.Data.FUGAS) ? "" : args.Data.FUGAS;
                    args.Data.CAMBIOPRESION = string.IsNullOrEmpty(args.Data.CAMBIOPRESION) ? "" : args.Data.CAMBIOPRESION;
                    args.Data.CAMBIOREPUESTO = string.IsNullOrEmpty(args.Data.CAMBIOREPUESTO) ? "" : args.Data.CAMBIOREPUESTO;
                    args.Data.ENSAYOCONTRAP = string.IsNullOrEmpty(args.Data.ENSAYOCONTRAP) ? "" : args.Data.ENSAYOCONTRAP;
                    args.Data.TRABAJOSEFEC = string.IsNullOrEmpty(args.Data.TRABAJOSEFEC) ? "" : args.Data.TRABAJOSEFEC;
                    args.Data.RESP = string.IsNullOrEmpty(args.Data.RESP) ? "" : args.Data.RESP;
                    args.Data.CONTROLO = string.IsNullOrEmpty(args.Data.CONTROLO) ? "" : args.Data.CONTROLO;
                    args.Data.POP = string.IsNullOrEmpty(args.Data.POP) ? "" : args.Data.POP;
                    args.Data.RESPTECNICO = string.IsNullOrEmpty(args.Data.RESPTECNICO) ? "" : args.Data.RESPTECNICO;
                    args.Data.OPDS = string.IsNullOrEmpty(args.Data.OPDS) ? "" : args.Data.OPDS;
                    args.Data.PRESENCIAINSPEC = string.IsNullOrEmpty(args.Data.PRESENCIAINSPEC) ? "" : args.Data.PRESENCIAINSPEC;
                    args.Data.MANOMETRO = string.IsNullOrEmpty(args.Data.MANOMETRO) ? "" : args.Data.MANOMETRO;
                    response = await Http.PutAsJsonAsync($"api/Servicios/{args.Data.PEDIDO}", args.Data);
                    servDesc = servicios.OrderByDescending(s => s.PEDIDO).ToList();
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

        private async Task EliminarServicio(ActionEventArgs<Service> args)
        {
            try
            {
                if (args.Data != null)
                {
                    bool isConfirmed = await JsRuntime.InvokeAsync<bool>("confirm", $"Seguro de que desea eliminar la reparacion {args.Data.PEDIDO}?");
                    if (isConfirmed)
                    {
                        //servicios.Remove(servicios.Find(m => m.PEDIDO == args.Data.PEDIDO));
                        await Http.DeleteAsync($"api/Servicios/{args.Data.PEDIDO}");
                        servDesc = servicios.OrderByDescending(s => s.PEDIDO).ToList();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void OnInput(InputEventArgs args)
        {
            this.Grid.Search(args.Value);
        }
        public async Task ClickHandler(Syncfusion.Blazor.Navigations.ClickEventArgs args)
        {
            if (args.Item.Text == "Copy")
            {
                if (this.Grid.SelectedRecords.Count > 0)
                {
                    foreach (Service selectedRecord in this.Grid.SelectedRecords)
                    {
                        bool isConfirmed = await JsRuntime.InvokeAsync<bool>("confirm", "Seguro de que desea copiar el Servicios / la reparacion?");
                        if (isConfirmed)
                        {
                            Service Nuevo = new Service();

                            Nuevo.PEDIDO = (servicios.Max(s => Int32.Parse(s.PEDIDO)) + 1).ToString();
                            Nuevo.FECHA = selectedRecord.FECHA;
                            Nuevo.CLIENTE = selectedRecord.CLIENTE;
                            Nuevo.PLANTA = selectedRecord.PLANTA;
                            Nuevo.OCOMPRA = selectedRecord.OCOMPRA;
                            Nuevo.REMITOREC = selectedRecord.REMITOREC;
                            Nuevo.REMITO = selectedRecord.REMITO;
                            Nuevo.IDENTIFICACION = selectedRecord.IDENTIFICACION;
                            Nuevo.NSERIE = selectedRecord.NSERIE;
                            Nuevo.MARCA = selectedRecord.MARCA;
                            Nuevo.MODELO = selectedRecord.MODELO;
                            Nuevo.MEDIDA = selectedRecord.MEDIDA;
                            Nuevo.SERIE = selectedRecord.SERIE;
                            Nuevo.ORIFICIO = selectedRecord.ORIFICIO;
                            Nuevo.AREA = selectedRecord.AREA;
                            Nuevo.FLUIDO = selectedRecord.FLUIDO;
                            Nuevo.AÑO = selectedRecord.AÑO;
                            Nuevo.PRESION = selectedRecord.PRESION;
                            Nuevo.TEMP = selectedRecord.TEMP;
                            Nuevo.PRESIONBANCO = selectedRecord.PRESIONBANCO;
                            Nuevo.SOBREPRESION = selectedRecord.SOBREPRESION;
                            Nuevo.CONTRAPRESION = selectedRecord.CONTRAPRESION;
                            Nuevo.TIPO = selectedRecord.TIPO;
                            Nuevo.RESORTE = selectedRecord.RESORTE;
                            Nuevo.SERVICIO = selectedRecord.SERVICIO;
                            Nuevo.ENSRECEP = selectedRecord.ENSRECEP;
                            Nuevo.ESTADO = selectedRecord.ESTADO;
                            Nuevo.PRESIONRECEP = selectedRecord.PRESIONRECEP;
                            Nuevo.FUGAS = selectedRecord.FUGAS;
                            Nuevo.PRESIONFUGA = selectedRecord.PRESIONFUGA;
                            Nuevo.CAMBIOPRESION = selectedRecord.CAMBIOPRESION;
                            Nuevo.PRESIONSOLIC = selectedRecord.PRESIONSOLIC;
                            Nuevo.CAMBIOREPUESTO = selectedRecord.CAMBIOREPUESTO;
                            Nuevo.CODRESORTE = selectedRecord.CODRESORTE;
                            Nuevo.REPUESTOS = selectedRecord.REPUESTOS;
                            Nuevo.TRABAJOSEFEC = selectedRecord.TRABAJOSEFEC;
                            Nuevo.TRABAJOSACCES = selectedRecord.TRABAJOSACCES;
                            Nuevo.MANOMETRO = selectedRecord.MANOMETRO;
                            Nuevo.FECMANTANT = selectedRecord.FECMANTANT;
                            Nuevo.PEDIDOANT = selectedRecord.PEDIDOANT;
                            Nuevo.ENSAYOCONTRAP = selectedRecord.ENSAYOCONTRAP;
                            Nuevo.RESP = selectedRecord.RESP;
                            Nuevo.CONTROLO = selectedRecord.CONTROLO;
                            Nuevo.POP = selectedRecord.POP;
                            Nuevo.RESPTECNICO = selectedRecord.RESPTECNICO;
                            Nuevo.OPDS = selectedRecord.OPDS;
                            Nuevo.ACTA = selectedRecord.ACTA;
                            Nuevo.PRESENCIAINSPEC = selectedRecord.PRESENCIAINSPEC;
                            Nuevo.DESCARTICULO = selectedRecord.DESCARTICULO;
                            Nuevo.OBSERV = selectedRecord.OBSERV;

                            var response = await Http.PostAsJsonAsync("api/Servicios", Nuevo);
                            servDesc = servicios.OrderByDescending(s => s.PEDIDO).ToList();

                            if (response.StatusCode == System.Net.HttpStatusCode.Created)
                            {
                                Grid.Refresh();
                                var servicio = await response.Content.ReadFromJsonAsync<Service>();
                                await InvokeAsync(StateHasChanged);
                                Nuevo.PEDIDO = servicio.PEDIDO;
                                servicios.Add(Nuevo);
                                var itemsJson = JsonSerializer.Serialize(servicio);
                                Console.WriteLine(itemsJson);
                                //toastService.ShowToast($"Registrado Correctemente.Vale {StockGuardado.VALE}", TipoAlerta.Success);
                                servicios.OrderByDescending(o => o.PEDIDO);
                            }

                        }
                    }
                }
            }
            if (args.Item.Text == "Excel Export")
            {
                if (this.Grid.SelectedRecords.Count > 0)
                {
                    foreach (Service selectedRecord in this.Grid.SelectedRecords)
                    {
                        ExcelExportProperties Props = new ExcelExportProperties();
                        List<ExcelCell> ExcelCells1 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 2, RowSpan = 2, Value = "ARBROS S.A.", Style = new ExcelStyle() { FontColor = "#001C8C", FontSize = 19 , HAlign = ExcelHorizontalAlign.Center, VAlign = ExcelVerticalAlign.Center, Bold = true } },
                            new ExcelCell() {Index = 2, Value = "" },
                            new ExcelCell() { Index = 3, ColSpan = 2, RowSpan = 1, Value = "CERTIFICADO DE MANTENIMIENTO Y CALIBRACIÓN", Style = new ExcelStyle() { FontColor = "#001C8C", FontSize = 12, HAlign = ExcelHorizontalAlign.Center, VAlign = ExcelVerticalAlign.Center, Bold = true } },
                            new ExcelCell() {Index = 4, Value = "" },
                            new ExcelCell() { Index = 5, ColSpan = 2, RowSpan = 1, Value = $"Fecha: {DateTime.Today.Day} / {DateTime.Today.Month} / {DateTime.Today.Year}", Style = new ExcelStyle() { FontColor = "#001C8C", FontSize = 12, HAlign = ExcelHorizontalAlign.Center, VAlign = ExcelVerticalAlign.Center, Bold = true } },
                        };
                        List<ExcelCell> ExcelCells2 = new List<ExcelCell> {
                            new ExcelCell() {Index = 2, Value = "" },
                            new ExcelCell() {Index = 2, Value = "" },
                            new ExcelCell() { Index = 3, ColSpan = 2, RowSpan = 1, Value = "VALVULA DE SEGURIDAD Y ALIVIO", Style = new ExcelStyle() { FontColor = "#001C8C", FontSize = 12, HAlign = ExcelHorizontalAlign.Center, VAlign = ExcelVerticalAlign.Center, Bold = true } },
                            new ExcelCell() {Index = 4, Value = "" },
                            new ExcelCell() { Index = 5, ColSpan = 2, RowSpan = 1, Value = $"Pedido Número: {selectedRecord.PEDIDO}", Style = new ExcelStyle() { FontColor = "#001C8C", FontSize = 12, HAlign = ExcelHorizontalAlign.Center, VAlign = ExcelVerticalAlign.Center, Bold = true } },
                        };
                        List<ExcelCell> ExcelCells3 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 6, Value = "ENSAYOS EFECTUADOS EN BANCO CON PULMÓN HIDRO NEUMÁTICO. FLUIDO DE PRUEBA: AIRE A TEMPERATURA AMBIENTE", Style = new ExcelStyle() { FontColor = "#000000", FontSize = 10, HAlign = ExcelHorizontalAlign.Center, VAlign = ExcelVerticalAlign.Center, Bold = true}}};
                        List<ExcelCell> ExcelCells4 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 6, Value = "GENERALIDADES", Style = new ExcelStyle() { FontColor = "#000000", FontSize = 10, HAlign = ExcelHorizontalAlign.Center, VAlign = ExcelVerticalAlign.Center, Bold = true, Italic = true, Underline = true }}};
                        List<ExcelCell> ExcelCells5 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 3, Value = $"Cliente: {selectedRecord.CLIENTE}"},
                            new ExcelCell() {Index = 2, Value = "" },
                            new ExcelCell() {Index = 3, Value = "" },
                            new ExcelCell() { Index = 4, ColSpan = 3, Value = $"Planta: {selectedRecord.PLANTA}"}};
                        List<ExcelCell> ExcelCells6 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 3, Value = $"Orden de Compra: {selectedRecord.OCOMPRA}"},
                            new ExcelCell() {Index = 2, Value = "" },
                            new ExcelCell() {Index = 3, Value = "" },
                            new ExcelCell() { Index = 4, ColSpan = 3, Value = $"Remito Recepción: {selectedRecord.REMITOREC}"}};
                        List<ExcelCell> ExcelCells7 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 3, Value = $"Observaciones: {selectedRecord.OBSERV}"},
                            new ExcelCell() {Index = 2, Value = "" },
                            new ExcelCell() {Index = 3, Value = "" },
                            new ExcelCell() { Index = 4, ColSpan = 3, Value = $"Descripción Artículo: {selectedRecord.DESCARTICULO}"}};
                        List<ExcelCell> ExcelCells8 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 3, Value = $"Mantenimiento Anterior: {selectedRecord.PEDIDOANT}"},
                            new ExcelCell() {Index = 2, Value = "" },
                            new ExcelCell() {Index = 3, Value = "" },
                            new ExcelCell() { Index = 4, ColSpan = 3, Value = $"Fecha de Mantenimiento Anterior: {selectedRecord.FECMANTANT}"}};
                        List<ExcelCell> ExcelCells9 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 6, Value = $"Remito: {selectedRecord.REMITO}"}};
                        List<ExcelCell> ExcelCells10 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 6, Value = "DATOS DE PLACA", Style = new ExcelStyle() { FontColor = "#000000", FontSize = 10, HAlign = ExcelHorizontalAlign.Center, VAlign = ExcelVerticalAlign.Center, Bold = true, Italic = true, Underline = true }}};
                        List<ExcelCell> ExcelCells11 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 3, Value = $"TAG: {selectedRecord.IDENTIFICACION}"},
                            new ExcelCell() {Index = 2, Value = "" },
                            new ExcelCell() {Index = 3, Value = "" },
                            new ExcelCell() { Index = 4, ColSpan = 3, Value = $"Marca: {selectedRecord.MARCA}"}};
                        List<ExcelCell> ExcelCells12 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 3, Value = $"Número de Serie: {selectedRecord.NSERIE}"},
                            new ExcelCell() {Index = 2, Value = "" },
                            new ExcelCell() {Index = 3, Value = "" },
                            new ExcelCell() { Index = 4, ColSpan = 3, Value = $"Modelo: {selectedRecord.MODELO}"}};
                        List<ExcelCell> ExcelCells13 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 3, Value = $"Medida: {selectedRecord.MEDIDA}"},
                            new ExcelCell() {Index = 2, Value = "" },
                            new ExcelCell() {Index = 3, Value = "" },
                            new ExcelCell() { Index = 4, ColSpan = 3, Value = $"Serie: {selectedRecord.SERIE}"}};
                        List<ExcelCell> ExcelCells14 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 3, Value = $"Orificio: {selectedRecord.ORIFICIO}"},
                            new ExcelCell() {Index = 2, Value = "" },
                            new ExcelCell() {Index = 3, Value = "" },
                            new ExcelCell() { Index = 4, ColSpan = 3, Value = $"Año: {selectedRecord.AÑO}"}};
                        List<ExcelCell> ExcelCells15 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 3, Value = $"Area: {selectedRecord.AREA}"},
                            new ExcelCell() {Index = 2, Value = "" },
                            new ExcelCell() {Index = 3, Value = "" },
                            new ExcelCell() { Index = 4, ColSpan = 3, Value = $"Fluido: {selectedRecord.FLUIDO}"}};
                        List<ExcelCell> ExcelCells16 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 3, Value = $"Sobrepresión: {selectedRecord.SOBREPRESION}"},
                            new ExcelCell() {Index = 2, Value = "" },
                            new ExcelCell() {Index = 3, Value = "" },
                            new ExcelCell() { Index = 4, ColSpan = 3, Value = $"Presión: {selectedRecord.PRESION}"}};
                        List<ExcelCell> ExcelCells17 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 3, Value = $"Contrapresión: {selectedRecord.CONTRAPRESION}"},
                            new ExcelCell() {Index = 2, Value = "" },
                            new ExcelCell() {Index = 3, Value = "" },
                            new ExcelCell() { Index = 4, ColSpan = 3, Value = $"Tipo: {selectedRecord.TIPO}"}};
                        List<ExcelCell> ExcelCells18 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 3, Value = $"Temperatura: {selectedRecord.TEMP}"},
                            new ExcelCell() {Index = 2, Value = "" },
                            new ExcelCell() {Index = 3, Value = "" },
                            new ExcelCell() { Index = 4, ColSpan = 3, Value = $"Resorte: {selectedRecord.RESORTE}"}};
                        List<ExcelCell> ExcelCells19 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 3, Value = $"Presión en Banco: {selectedRecord.PRESIONBANCO}"},
                            new ExcelCell() {Index = 2, Value = "" },
                            new ExcelCell() {Index = 3, Value = "" },
                            new ExcelCell() { Index = 4, ColSpan = 3, Value = $"Servicio: {selectedRecord.SERVICIO}"}};
                        List<ExcelCell> ExcelCells20 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 6, Value = "ENSAYOS A LA RECEPCIÓN", Style = new ExcelStyle() { FontColor = "#000000", FontSize = 10, HAlign = ExcelHorizontalAlign.Center, VAlign = ExcelVerticalAlign.Center, Bold = true, Italic = true, Underline = true }}};
                        List<ExcelCell> ExcelCells21 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 3, Value = $"Ensayo a la Recepcion: {selectedRecord.ENSRECEP}"},
                            new ExcelCell() {Index = 2, Value = "" },
                            new ExcelCell() {Index = 3, Value = "" },
                            new ExcelCell() { Index = 4, ColSpan = 3, Value = $"Estado: {selectedRecord.ESTADO}"}};
                        List<ExcelCell> ExcelCells22 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 3, Value = $"Presión Ensayo Recepción: {selectedRecord.PRESIONRECEP}"},
                            new ExcelCell() {Index = 2, Value = "" },
                            new ExcelCell() {Index = 3, Value = "" },
                            new ExcelCell() { Index = 4, ColSpan = 3, Value = $"Fugas: {selectedRecord.FUGAS}"}};
                        List<ExcelCell> ExcelCells23 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 3, Value = $"Presión de Fuga: {selectedRecord.PRESIONFUGA}"},
                            new ExcelCell() {Index = 2, Value = "" },
                            new ExcelCell() {Index = 3, Value = "" },
                            new ExcelCell() { Index = 4, ColSpan = 3, Value = $"Cambio de Presión: {selectedRecord.CAMBIOPRESION}"}};
                        List<ExcelCell> ExcelCells24 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 6, Value = $"Presión Solicitada: {selectedRecord.PRESIONSOLIC}"}};
                        List<ExcelCell> ExcelCells25 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 6, Value = "TRABAJOS EFECTUADOS", Style = new ExcelStyle() { FontColor = "#000000", FontSize = 10, HAlign = ExcelHorizontalAlign.Center, VAlign = ExcelVerticalAlign.Center, Bold = true, Italic = true, Underline = true }}};
                        List<ExcelCell> ExcelCells26 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 3, Value = $"Cambio de Repuestos: {selectedRecord.CAMBIOREPUESTO}"},
                            new ExcelCell() {Index = 2, Value = "" },
                            new ExcelCell() {Index = 3, Value = "" },
                            new ExcelCell() { Index = 4, ColSpan = 3, Value = $"Repuestos: {selectedRecord.REPUESTOS}"}};
                        List<ExcelCell> ExcelCells27 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 3, Value = $"Código de Resorte: {selectedRecord.CODRESORTE}"},
                            new ExcelCell() {Index = 2, Value = "" },
                            new ExcelCell() {Index = 3, Value = "" },
                            new ExcelCell() { Index = 4, ColSpan = 3, Value = $"Ensayo Contrapresion: {selectedRecord.ENSAYOCONTRAP}"}};
                        List<ExcelCell> ExcelCells28 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 3, Value = $"Trabajos efectuados: {selectedRecord.TRABAJOSEFEC}"},
                            new ExcelCell() {Index = 2, Value = "" },
                            new ExcelCell() {Index = 3, Value = "" },
                            new ExcelCell() { Index = 4, ColSpan = 3, Value = $"Trabajos Accesorios: {selectedRecord.TRABAJOSACCES}"}};
                        List<ExcelCell> ExcelCells29 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 3, Value = $"Responsable: {selectedRecord.RESP}"},
                            new ExcelCell() {Index = 2, Value = "" },
                            new ExcelCell() {Index = 3, Value = "" },
                            new ExcelCell() { Index = 4, ColSpan = 3, Value = $"Controló: {selectedRecord.CONTROLO}"}};
                        List<ExcelCell> ExcelCells30 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 3, Value = $"POP: {selectedRecord.POP}"},
                            new ExcelCell() {Index = 2, Value = "" },
                            new ExcelCell() {Index = 3, Value = "" },
                            new ExcelCell() { Index = 4, ColSpan = 3, Value = $"Responsable Técnico: {selectedRecord.RESPTECNICO}"}};
                        List<ExcelCell> ExcelCells31 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 3, Value = $"OPDS: {selectedRecord.OPDS}"},
                            new ExcelCell() {Index = 2, Value = "" },
                            new ExcelCell() {Index = 3, Value = "" },
                            new ExcelCell() { Index = 4, ColSpan = 3, Value = $"Acta: {selectedRecord.ACTA}"}};
                        List<ExcelCell> ExcelCells32 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 3, Value = $"Presencia Inspector: {selectedRecord.PRESENCIAINSPEC}"},
                            new ExcelCell() {Index = 2, Value = "" },
                            new ExcelCell() {Index = 3, Value = "" },
                            new ExcelCell() { Index = 4, ColSpan = 3, Value = $"Responsable Técnico: {selectedRecord.RESPTECNICO}"}};
                        List<ExcelRow> ExcelRows = new List<ExcelRow> {
                            new ExcelRow() { Index = 1,  Cells = ExcelCells1 },
                            new ExcelRow() { Index = 2,  Cells = ExcelCells2 },
                            new ExcelRow() { Index = 3,  Cells = ExcelCells3 },
                            new ExcelRow() { Index = 4,  Cells = ExcelCells4 },
                            new ExcelRow() { Index = 5,  Cells = ExcelCells5 },
                            new ExcelRow() { Index = 6,  Cells = ExcelCells6 },
                            new ExcelRow() { Index = 7,  Cells = ExcelCells7 },
                            new ExcelRow() { Index = 8,  Cells = ExcelCells8 },
                            new ExcelRow() { Index = 9,  Cells = ExcelCells9 },
                            new ExcelRow() { Index = 10,  Cells = ExcelCells10 },
                            new ExcelRow() { Index = 11,  Cells = ExcelCells11 },
                            new ExcelRow() { Index = 12,  Cells = ExcelCells12 },
                            new ExcelRow() { Index = 13,  Cells = ExcelCells13 },
                            new ExcelRow() { Index = 14,  Cells = ExcelCells14 },
                            new ExcelRow() { Index = 15,  Cells = ExcelCells15 },
                            new ExcelRow() { Index = 16,  Cells = ExcelCells16 },
                            new ExcelRow() { Index = 17,  Cells = ExcelCells17 },
                            new ExcelRow() { Index = 18,  Cells = ExcelCells18 },
                            new ExcelRow() { Index = 19,  Cells = ExcelCells19 },
                            new ExcelRow() { Index = 20,  Cells = ExcelCells20 },
                            new ExcelRow() { Index = 21,  Cells = ExcelCells21 },
                            new ExcelRow() { Index = 22,  Cells = ExcelCells22 },
                            new ExcelRow() { Index = 23,  Cells = ExcelCells23 },
                            new ExcelRow() { Index = 24,  Cells = ExcelCells24 },
                            new ExcelRow() { Index = 25,  Cells = ExcelCells25 },
                            new ExcelRow() { Index = 26,  Cells = ExcelCells26 },
                            new ExcelRow() { Index = 27,  Cells = ExcelCells27 },
                            new ExcelRow() { Index = 28,  Cells = ExcelCells28 },
                            new ExcelRow() { Index = 29,  Cells = ExcelCells29 },
                            new ExcelRow() { Index = 30,  Cells = ExcelCells30 },
                            new ExcelRow() { Index = 31,  Cells = ExcelCells31 },
                            new ExcelRow() { Index = 32,  Cells = ExcelCells32 }};
                        List<ExcelCell> FooterCells1 = new List<ExcelCell> { new ExcelCell() { ColSpan = 6, Value = "Thank you for your business!", Style = new ExcelStyle() { FontColor = "#C67878", HAlign = ExcelHorizontalAlign.Center, Bold = true } } };
                        List<ExcelCell> FooterCells2 = new List<ExcelCell> { new ExcelCell() { ColSpan = 6, Value = "!Visit Again!", Style = new ExcelStyle() { FontColor = "#C67878", HAlign = ExcelHorizontalAlign.Center, Bold = true } } };
                        ExcelHeader Header = new ExcelHeader()
                        {
                            HeaderRows = 32,
                            Rows = ExcelRows
                        };
                        Props.Header = Header;
                        Props.FileName = $"Reporte{selectedRecord.PEDIDO}.xlsx";
                        await this.Grid.ExcelExport(Props);
                    }
                }
                //await this.Grid.ExcelExport();
            }
            if (args.Item.Text == "PdfExport")
            {
                if (this.Grid.SelectedRecords.Count > 0)
                {
                    foreach (Service selectedRecord in this.Grid.SelectedRecords)
                    {
                        //Create a new PDF document
                        PdfDocument document = new PdfDocument();
                        //Create the page
                        PdfPage page = document.Pages.Add();
                        //Create PDF graphics for the page
                        FileStream fontStream = new FileStream("E:\\solution_page\\Servicio\\Servicio\\wwwroot\\Calibri 400.ttf", FileMode.Open, FileAccess.Read);
                        //Create a PdfGrid
                        PdfGrid pdfGrid = new PdfGrid();
                        //Create and customize the string formats
                        PdfStringFormat Centrado = new PdfStringFormat();
                        Centrado.Alignment = PdfTextAlignment.Center;
                        Centrado.LineAlignment = PdfVerticalAlignment.Middle;
                        //Create and customize the string formats
                        PdfStringFormat Izquierda = new PdfStringFormat();
                        Izquierda.Alignment = PdfTextAlignment.Left;
                        Izquierda.LineAlignment = PdfVerticalAlignment.Middle;
                        //Add columns to PdfGrid
                        for (int i = 0; i < 6; i++)
                        {
                            PdfGridColumn column = pdfGrid.Columns.Add();
                            if (i == 0 || i == 1 || i == 4 || i == 5)
                            {
                                column.Width = 64;
                            }
                            if (i == 2 || i == 3)
                            {

                                column.Width = 128;
                            }
                        }
                        //Add rows to PdfGrid
                        for (int i = 0; i < 32; i++)
                        {
                            PdfGridRow row = pdfGrid.Rows.Add();
                            if (i == 0 || i == 1 || i == 2 || i == 3 || i == 9 || i == 19 || i == 23)
                            {
                                row.Height = 26;
                            }
                            else
                            {
                                row.Height = 23;
                            }
                        }
                        //Load the image from the stream 
                        FileStream fs = new FileStream("E:\\solution_page\\Servicio\\Servicio\\wwwroot\\logo_aerre.jpg", FileMode.Open);
                        //Add RowSpan
                        PdfGridCell gridCell = pdfGrid.Rows[0].Cells[0];
                        gridCell.ColumnSpan = 2;
                        gridCell.RowSpan = 2;
                        gridCell.StringFormat = Centrado;
                        gridCell.Value = new PdfBitmap(fs);
                        //Add RowSpan
                        PdfGridCell gridCell2 = pdfGrid.Rows[0].Cells[2];
                        gridCell2.ColumnSpan = 2;
                        gridCell2.StringFormat = Centrado;
                        gridCell2.Value = "CERTIFICADO DE MANTENIMIENTO Y CALIBRACIÓN";
                        //Add RowSpan
                        PdfGridCell gridCell3 = pdfGrid.Rows[1].Cells[2];
                        gridCell3.ColumnSpan = 2;
                        gridCell3.StringFormat = Centrado;
                        gridCell3.Value = "VALVULA DE SEGURIDAD Y ALIVIO";
                        //Add RowSpan
                        PdfGridCell gridCell4 = pdfGrid.Rows[0].Cells[4];
                        gridCell4.ColumnSpan = 2;
                        gridCell4.StringFormat = Centrado;
                        gridCell4.Value = $"Fecha: {DateTime.Today.Day} / {DateTime.Today.Month} / {DateTime.Today.Year}";
                        //Add RowSpan
                        PdfGridCell gridCell5 = pdfGrid.Rows[1].Cells[4];
                        gridCell5.ColumnSpan = 2;
                        gridCell5.StringFormat = Centrado;
                        gridCell5.Value = $"Pedido Número: {selectedRecord.PEDIDO}";
                        //Add RowSpan
                        PdfGridCell gridCell6 = pdfGrid.Rows[2].Cells[0];
                        gridCell6.ColumnSpan = 6;
                        gridCell6.StringFormat = Centrado;
                        gridCell6.Value = "ENSAYOS EFECTUADOS EN BANCO CON PULMÓN HIDRO NEUMÁTICO. FLUIDO DE PRUEBA: AIRE A TEMPERATURA AMBIENTE";
                        //Add RowSpan
                        PdfGridCell gridCell7 = pdfGrid.Rows[3].Cells[0];
                        gridCell7.ColumnSpan = 6;
                        gridCell7.StringFormat = Centrado;
                        gridCell7.Value = "GENERALIDADES";
                        //Add RowSpan
                        PdfGridCell gridCell8 = pdfGrid.Rows[4].Cells[0];
                        gridCell8.ColumnSpan = 3;
                        gridCell8.StringFormat = Izquierda;
                        gridCell8.Value = $"   Cliente: {selectedRecord.CLIENTE}";
                        //Add RowSpan
                        PdfGridCell gridCell9 = pdfGrid.Rows[4].Cells[3];
                        gridCell9.ColumnSpan = 3;
                        gridCell9.StringFormat = Izquierda;
                        gridCell9.Value = $"   Planta: {selectedRecord.PLANTA}";
                        //Add RowSpan
                        PdfGridCell gridCell10 = pdfGrid.Rows[5].Cells[0];
                        gridCell10.ColumnSpan = 2;
                        gridCell10.StringFormat = Izquierda;
                        gridCell10.Value = $"   Remito Recep: {selectedRecord.REMITOREC}";
                        //Add RowSpan
                        PdfGridCell gridCell11 = pdfGrid.Rows[5].Cells[2];
                        gridCell11.ColumnSpan = 2;
                        gridCell11.StringFormat = Izquierda;
                        gridCell11.Value = $"   Orden de Compra: {selectedRecord.OCOMPRA}";
                        //Add RowSpan
                        PdfGridCell gridCell16 = pdfGrid.Rows[5].Cells[4];
                        gridCell16.ColumnSpan = 2;
                        gridCell16.StringFormat = Izquierda;
                        gridCell16.Value = $"   Remito: {selectedRecord.REMITO}";
                        //Add RowSpan
                        PdfGridCell gridCell14 = pdfGrid.Rows[6].Cells[0];
                        gridCell14.ColumnSpan = 3;
                        gridCell14.StringFormat = Izquierda;
                        gridCell14.Value = $"   Mantenimiento anterior: {selectedRecord.PEDIDOANT}";
                        //Add RowSpan
                        PdfGridCell gridCell15 = pdfGrid.Rows[6].Cells[3];
                        gridCell15.ColumnSpan = 3;
                        gridCell15.StringFormat = Izquierda;
                        gridCell15.Value = $"   Fecha de mantenimiento anterior: {selectedRecord.FECMANTANT}";
                        //Add RowSpan
                        PdfGridCell gridCell17 = pdfGrid.Rows[7].Cells[0];
                        gridCell17.ColumnSpan = 6;
                        gridCell17.StringFormat = Centrado;
                        gridCell17.Value = "DATOS DE PLACA";
                        //Add RowSpan
                        PdfGridCell gridCell18 = pdfGrid.Rows[8].Cells[0];
                        gridCell18.ColumnSpan = 3;
                        gridCell18.StringFormat = Izquierda;
                        gridCell18.Value = $"   TAG: {selectedRecord.IDENTIFICACION}";
                        //Add RowSpan
                        PdfGridCell gridCell19 = pdfGrid.Rows[8].Cells[3];
                        gridCell19.ColumnSpan = 3;
                        gridCell19.StringFormat = Izquierda;
                        gridCell19.Value = $"   Marca: {selectedRecord.MARCA}";
                        //Add RowSpan
                        PdfGridCell gridCell20 = pdfGrid.Rows[9].Cells[0];
                        gridCell20.ColumnSpan = 3;
                        gridCell20.StringFormat = Izquierda;
                        gridCell20.Value = $"   Número de serie: {selectedRecord.NSERIE}";
                        //Add RowSpan
                        PdfGridCell gridCell21 = pdfGrid.Rows[9].Cells[3];
                        gridCell21.ColumnSpan = 3;
                        gridCell21.StringFormat = Izquierda;
                        gridCell21.Value = $"   Modelo: {selectedRecord.MODELO}";
                        //Add RowSpan
                        PdfGridCell gridCell22 = pdfGrid.Rows[10].Cells[0];
                        gridCell22.ColumnSpan = 3;
                        gridCell22.StringFormat = Izquierda;
                        gridCell22.Value = $"   Medida: {selectedRecord.MEDIDA}";
                        //Add RowSpan
                        PdfGridCell gridCell23 = pdfGrid.Rows[10].Cells[3];
                        gridCell23.ColumnSpan = 3;
                        gridCell23.StringFormat = Izquierda;
                        gridCell23.Value = $"   Serie: {selectedRecord.SERIE}";
                        //Add RowSpan
                        PdfGridCell gridCell24 = pdfGrid.Rows[11].Cells[0];
                        gridCell24.ColumnSpan = 3;
                        gridCell24.StringFormat = Izquierda;
                        gridCell24.Value = $"   Orificio: {selectedRecord.ORIFICIO}";
                        //Add RowSpan
                        PdfGridCell gridCell25 = pdfGrid.Rows[11].Cells[3];
                        gridCell25.ColumnSpan = 3;
                        gridCell25.StringFormat = Izquierda;
                        gridCell25.Value = $"   Año: {selectedRecord.AÑO}";
                        //Add RowSpan
                        PdfGridCell gridCell26 = pdfGrid.Rows[12].Cells[0];
                        gridCell26.ColumnSpan = 3;
                        gridCell26.StringFormat = Izquierda;
                        gridCell26.Value = $"   Area: {selectedRecord.AREA}";
                        //Add RowSpan
                        PdfGridCell gridCell27 = pdfGrid.Rows[12].Cells[3];
                        gridCell27.ColumnSpan = 3;
                        gridCell27.StringFormat = Izquierda;
                        gridCell27.Value = $"   Fluido: {selectedRecord.FLUIDO}";
                        //Add RowSpan
                        PdfGridCell gridCell28 = pdfGrid.Rows[13].Cells[0];
                        gridCell28.ColumnSpan = 3;
                        gridCell28.StringFormat = Izquierda;
                        gridCell28.Value = $"   Sobrepresión: {selectedRecord.SOBREPRESION}%";
                        //Add RowSpan
                        PdfGridCell gridCell29 = pdfGrid.Rows[13].Cells[3];
                        gridCell29.ColumnSpan = 3;
                        gridCell29.StringFormat = Izquierda;
                        gridCell29.Value = $"   Presión: {selectedRecord.PRESION} Bar";
                        //Add RowSpan
                        PdfGridCell gridCell30 = pdfGrid.Rows[14].Cells[0];
                        gridCell30.ColumnSpan = 3;
                        gridCell30.StringFormat = Izquierda;
                        if (selectedRecord.CONTRAPRESION == "Atm.")
                        {
                            gridCell30.Value = $"   Contrapresión: {selectedRecord.CONTRAPRESION}";
                        }
                        else
                        {
                            gridCell30.Value = $"   Contrapresión: {selectedRecord.CONTRAPRESION} Bar";
                        }
                        //Add RowSpan
                        PdfGridCell gridCell31 = pdfGrid.Rows[14].Cells[3];
                        gridCell31.ColumnSpan = 3;
                        gridCell31.StringFormat = Izquierda;
                        gridCell31.Value = $"   Tipo: {selectedRecord.TIPO}";
                        //Add RowSpan
                        PdfGridCell gridCell32 = pdfGrid.Rows[15].Cells[0];
                        gridCell32.ColumnSpan = 3;
                        gridCell32.StringFormat = Izquierda;
                        gridCell32.Value = $"   Temperatura: {selectedRecord.TEMP}";
                        //Add RowSpan
                        PdfGridCell gridCell33 = pdfGrid.Rows[15].Cells[3];
                        gridCell33.ColumnSpan = 3;
                        gridCell33.StringFormat = Izquierda;
                        gridCell33.Value = $"   Resorte: {selectedRecord.RESORTE}";
                        //Add RowSpan
                        PdfGridCell gridCell34 = pdfGrid.Rows[16].Cells[0];
                        gridCell34.ColumnSpan = 3;
                        gridCell34.StringFormat = Izquierda;
                        gridCell34.Value = $"   Presión en Banco: {selectedRecord.PRESIONBANCO} Bar";
                        //Add RowSpan
                        PdfGridCell gridCell35 = pdfGrid.Rows[16].Cells[3];
                        gridCell35.ColumnSpan = 3;
                        gridCell35.StringFormat = Izquierda;
                        gridCell35.Value = $"   Servicio: {selectedRecord.SERVICIO}";
                        //Add RowSpan
                        PdfGridCell gridCell36 = pdfGrid.Rows[17].Cells[0];
                        gridCell36.ColumnSpan = 6;
                        gridCell36.StringFormat = Centrado;
                        gridCell36.Value = "ENSAYOS A LA RECEPCIÓN";
                        //Add RowSpan
                        PdfGridCell gridCell37 = pdfGrid.Rows[18].Cells[0];
                        gridCell37.ColumnSpan = 3;
                        gridCell37.StringFormat = Izquierda;
                        gridCell37.Value = $"   Ensayo a la Recepción: {selectedRecord.ENSRECEP}";
                        //Add RowSpan
                        PdfGridCell gridCell38 = pdfGrid.Rows[18].Cells[3];
                        gridCell38.ColumnSpan = 3;
                        gridCell38.StringFormat = Izquierda;
                        gridCell38.Value = $"   Estado: {selectedRecord.ESTADO}";
                        //Add RowSpan
                        PdfGridCell gridCell39 = pdfGrid.Rows[19].Cells[0];
                        gridCell39.ColumnSpan = 3;
                        gridCell39.StringFormat = Izquierda;
                        gridCell39.Value = $"   Presión ensayo recepción: {selectedRecord.PRESIONRECEP} Bar";
                        //Add RowSpan
                        PdfGridCell gridCell40 = pdfGrid.Rows[19].Cells[3];
                        gridCell40.ColumnSpan = 3;
                        gridCell40.StringFormat = Izquierda;
                        gridCell40.Value = $"   Fugas: {selectedRecord.FUGAS}";
                        //Add RowSpan
                        PdfGridCell gridCell41 = pdfGrid.Rows[20].Cells[0];
                        gridCell41.ColumnSpan = 3;
                        gridCell41.StringFormat = Izquierda;
                        gridCell41.Value = $"   Presión de fuga: {selectedRecord.PRESIONFUGA} Bar";
                        //Add RowSpan
                        PdfGridCell gridCell42 = pdfGrid.Rows[20].Cells[3];
                        gridCell42.ColumnSpan = 3;
                        gridCell42.StringFormat = Izquierda;
                        gridCell42.Value = $"   Cambio de presión: {selectedRecord.CAMBIOPRESION}";
                        //Add RowSpan
                        PdfGridCell gridCell43 = pdfGrid.Rows[21].Cells[0];
                        gridCell43.ColumnSpan = 6;
                        gridCell43.StringFormat = Centrado;
                        gridCell43.Value = "TRABAJOS EFECTUADOS";
                        //Add RowSpan
                        PdfGridCell gridCell44 = pdfGrid.Rows[22].Cells[0];
                        gridCell44.ColumnSpan = 3;
                        gridCell44.StringFormat = Izquierda;
                        gridCell44.Value = $"   Cambio de repuestos: {selectedRecord.CAMBIOREPUESTO}";
                        //Add RowSpan
                        PdfGridCell gridCell45 = pdfGrid.Rows[22].Cells[3];
                        gridCell45.ColumnSpan = 3;
                        gridCell45.StringFormat = Izquierda;
                        gridCell45.Value = $"   Repuestos: {selectedRecord.REPUESTOS}";
                        //Add RowSpan
                        PdfGridCell gridCell46 = pdfGrid.Rows[23].Cells[0];
                        gridCell46.ColumnSpan = 3;
                        gridCell46.StringFormat = Izquierda;
                        gridCell46.Value = $"   Código de resorte: {selectedRecord.CODRESORTE}";
                        //Add RowSpan
                        PdfGridCell gridCell47 = pdfGrid.Rows[23].Cells[3];
                        gridCell47.ColumnSpan = 3;
                        gridCell47.StringFormat = Izquierda;
                        gridCell47.Value = $"   Ensayo contrapresión: {selectedRecord.ENSAYOCONTRAP}";
                        //Add RowSpan
                        PdfGridCell gridCell48 = pdfGrid.Rows[24].Cells[0];
                        gridCell48.ColumnSpan = 6;
                        gridCell48.StringFormat = Izquierda;
                        if (selectedRecord.TRABAJOSEFEC == "D")
                        {
                            gridCell48.Value = $"   Trabajos efectuados: Desarme, limpieza, revisión de sus componentes, reacondicionamiento de asientos, rearmado, prueba, calibración.";
                        }
                        else if (selectedRecord.TRABAJOSEFEC == "V")
                        {
                            gridCell48.Value = $"   Trabajos efectuados: Unicamente verificación de funcionamiento y calibración.";
                        }
                        else
                        {
                            gridCell48.Value = $"   Trabajos efectuados: {selectedRecord.TRABAJOSEFEC}";
                        }
                        //Add RowSpan
                        PdfGridCell gridCell49 = pdfGrid.Rows[25].Cells[0];
                        gridCell49.ColumnSpan = 6;
                        gridCell49.StringFormat = Izquierda;
                        gridCell49.Value = $"   Trabajos accesorios: {selectedRecord.TRABAJOSACCES}";
                        //Add RowSpan
                        PdfGridCell gridCell12 = pdfGrid.Rows[26].Cells[0];
                        gridCell12.ColumnSpan = 6;
                        gridCell12.StringFormat = Izquierda;
                        gridCell12.Value = $"   Observaciones: {selectedRecord.OBSERVMANT}";
                        //Add RowSpan
                        PdfGridCell gridCell50 = pdfGrid.Rows[27].Cells[0];
                        gridCell50.ColumnSpan = 3;
                        gridCell50.StringFormat = Izquierda;
                        gridCell50.Value = $"   Presión solicitada: {selectedRecord.PRESIONSOLIC} Bar";
                        //Add RowSpan
                        PdfGridCell gridCell51 = pdfGrid.Rows[27].Cells[3];
                        gridCell51.ColumnSpan = 3;
                        gridCell51.StringFormat = Izquierda;
                        gridCell51.Value = $"   Responsable: {selectedRecord.RESP}";
                        //Add RowSpan
                        PdfGridCell gridCell52 = pdfGrid.Rows[28].Cells[0];
                        gridCell52.ColumnSpan = 3;
                        gridCell52.StringFormat = Izquierda;
                        gridCell52.Value = $"   Controló: {selectedRecord.CONTROLO}";
                        //Add RowSpan
                        PdfGridCell gridCell53 = pdfGrid.Rows[28].Cells[3];
                        gridCell53.ColumnSpan = 3;
                        gridCell53.StringFormat = Izquierda;
                        gridCell53.Value = $"   POP: {selectedRecord.POP}";
                        //Add RowSpan
                        PdfGridCell gridCell54 = pdfGrid.Rows[29].Cells[0];
                        gridCell54.ColumnSpan = 3;
                        gridCell54.StringFormat = Izquierda;
                        gridCell54.Value = $"   Responsable técnino: {selectedRecord.RESPTECNICO}";
                        //Add RowSpan
                        PdfGridCell gridCell55 = pdfGrid.Rows[29].Cells[3];
                        gridCell55.ColumnSpan = 3;
                        gridCell55.StringFormat = Izquierda;
                        gridCell55.Value = $"   OPDS: {selectedRecord.OPDS}";
                        //Add RowSpan
                        PdfGridCell gridCell56 = pdfGrid.Rows[30].Cells[0];
                        gridCell56.ColumnSpan = 3;
                        gridCell56.StringFormat = Izquierda;
                        gridCell56.Value = $"   Acta: {selectedRecord.ACTA}";
                        //Add RowSpan
                        PdfGridCell gridCell57 = pdfGrid.Rows[30].Cells[3];
                        gridCell57.ColumnSpan = 3;
                        gridCell57.StringFormat = Izquierda;
                        gridCell57.Value = $"   Presencia inspector: {selectedRecord.PRESENCIAINSPEC}";
                        //Add RowSpan
                        PdfGridCell gridCell58 = pdfGrid.Rows[31].Cells[0];
                        gridCell58.ColumnSpan = 3;
                        gridCell58.StringFormat = Izquierda;
                        gridCell58.Value = "   Firma:";
                        //Add RowSpan
                        PdfGridCell gridCell59 = pdfGrid.Rows[31].Cells[3];
                        gridCell59.ColumnSpan = 3;
                        gridCell59.StringFormat = Izquierda;
                        gridCell59.Value = $"   Aclaración:";
                        //Draw the PdfGrid
                        pdfGrid.Draw(page, new Syncfusion.Drawing.PointF(0, 0));
                        //Saving the PDF to the MemoryStream
                        MemoryStream stream = new MemoryStream();
                        document.Save(stream);
                        //Set the position as '0'
                        stream.Position = 0;
                        //Close the document 
                        document.Close(true);
                        await JS.SaveAs(selectedRecord.PEDIDO + ".pdf", stream.ToArray());
                    }
                }
            }
            if (args.Item.Text == "Edit")
            {
                if (this.Grid.SelectedRecords.Count > 0)
                {
                    foreach (Service selectedRecord in this.Grid.SelectedRecords)
                    {
                        NroPedido = selectedRecord.PEDIDO;
                    }
                }
            }
        }
        public void Refresh()
        {
            Grid.Refresh();
        }
    }
}
