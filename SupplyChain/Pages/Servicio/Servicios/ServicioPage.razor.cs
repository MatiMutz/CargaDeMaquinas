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
        protected List<Prod> prods = new List<Prod>();
        protected List<Solution> rutas;
        protected List<Service> servDesc;
        protected string pedant;
        protected List<Cliente> ClienteList = new List<Cliente>();

        protected DialogSettings DialogParams = new DialogSettings { MinHeight = "400px", Width = "1100px" };

        protected List<Object> Toolbaritems = new List<Object>(){
        "Edit",
        new Syncfusion.Blazor.Navigations.ItemModel { Text = "Certificado", TooltipText = "Certificado", PrefixIcon = "e-copy", Id = "Certificado" },
        new Syncfusion.Blazor.Navigations.ItemModel { Text = "OPDS", TooltipText = "OPDS", PrefixIcon = "e-copy", Id = "OPDS" },
        "Exportar grilla en Excel",
        new Syncfusion.Blazor.Navigations.ItemModel { Text = "Seleccionar Columnas", TooltipText = "Seleccionar Columnas", Id = "Seleccionar Columnas" },
        new Syncfusion.Blazor.Navigations.ItemModel { Text = "Search", TooltipText = "OPDS", Id = "Search" }
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
            if (args.Item.Text == "Seleccionar Columnas")
            {
                await Grid.OpenColumnChooser();
            }
            if (args.Item.Text == "Exportar grilla en Excel")
            {
                await this.Grid.ExcelExport();
            }
            if (args.Item.Text == "Certificado")
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
                        FileStream fontStream = new FileStream("wwwroot\\Calibri 400.ttf", FileMode.Open, FileAccess.Read);
                        //Create a PdfGrid
                        PdfGrid pdfGrid = new PdfGrid();

                        PdfGraphics graphics = page.Graphics;
                        //PdfFont font = new PdfStandardFont(PdfFontFamily.Courier, 10, PdfFontStyle.Bold);
                        //PdfFont font = new PdfTrueTypeFont("wwwroot\\Calibri 400.ttf", 24);
                        PdfFont font = new PdfTrueTypeFont(fontStream, 10, PdfFontStyle.Bold);

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
                            if (i == 0 || i == 1 || i == 2 || i == 3 || i == 7 || i == 17 || i == 21)
                            {
                                row.Height = 26;
                            }
                            else if (i == 31)
                            {
                                row.Height = 47;
                            }
                            else
                            {
                                row.Height = 22;
                            }
                        }
                        //Load the image from the stream 
                        FileStream fs = new FileStream("wwwroot\\logo_aerre.jpg", FileMode.Open);
                        FileStream IMR = new FileStream("wwwroot\\IMR.jpg", FileMode.Open);
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
                        gridCell2.Value = new PdfTextElement("CERTIFICADO DE MANTENIMIENTO Y CALIBRACIÓN", font, new PdfPen(PdfColor.Empty), PdfBrushes.Black, Centrado);
                        //Add RowSpan
                        PdfGridCell gridCell3 = pdfGrid.Rows[1].Cells[2];
                        gridCell3.ColumnSpan = 2;
                        gridCell3.StringFormat = Centrado;
                        gridCell3.Value = new PdfTextElement("VALVULA DE SEGURIDAD Y ALIVIO", font, new PdfPen(PdfColor.Empty), PdfBrushes.Black, Centrado);
                        //Add RowSpan
                        PdfGridCell gridCell4 = pdfGrid.Rows[0].Cells[4];
                        gridCell4.ColumnSpan = 2;
                        gridCell4.StringFormat = Centrado;
                        gridCell4.Value = new PdfTextElement($"FECHA: {DateTime.Today.Day} / {DateTime.Today.Month} / {DateTime.Today.Year}", font, new PdfPen(PdfColor.Empty), PdfBrushes.Black, Centrado);
                        //Add RowSpan
                        PdfGridCell gridCell5 = pdfGrid.Rows[1].Cells[4];
                        gridCell5.ColumnSpan = 2;
                        gridCell5.StringFormat = Centrado;
                        gridCell5.Value = new PdfTextElement($"PEDIDO: {selectedRecord.PEDIDO}", font, new PdfPen(PdfColor.Empty), PdfBrushes.Black, Centrado);
                        //Add RowSpan
                        PdfGridCell gridCell6 = pdfGrid.Rows[2].Cells[0];
                        gridCell6.ColumnSpan = 6;
                        gridCell6.StringFormat = Centrado;
                        gridCell6.Value = new PdfTextElement("ENSAYOS EFECTUADOS EN BANCO CON PULMÓN HIDRO NEUMÁTICO. FLUIDO DE PRUEBA: AIRE A TEMPERATURA AMBIENTE", font, new PdfPen(PdfColor.Empty), PdfBrushes.Black, Centrado);
                        //Add RowSpan
                        PdfGridCell gridCell7 = pdfGrid.Rows[3].Cells[0];
                        gridCell7.ColumnSpan = 6;
                        gridCell7.StringFormat = Centrado;
                        gridCell7.Value = new PdfTextElement("GENERALIDADES", font, new PdfPen(PdfColor.Empty), PdfBrushes.Black, Centrado);
                        //Add RowSpan
                        PdfGridCell gridCell8 = pdfGrid.Rows[4].Cells[0];
                        gridCell8.ColumnSpan = 3;
                        gridCell8.StringFormat = Izquierda;
                        gridCell8.Value = new PdfTextElement($"   Cliente: {selectedRecord.CLIENTE}", font, new PdfPen(PdfColor.Empty), PdfBrushes.Black, Izquierda);
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
                        gridCell16.Value = new PdfTextElement($"   Remito: {selectedRecord.REMITO}", font, new PdfPen(PdfColor.Empty), PdfBrushes.Black, Izquierda);
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
                        gridCell17.Value = new PdfTextElement("DATOS DE PLACA", font, new PdfPen(PdfColor.Empty), PdfBrushes.Black, Centrado);
                        //Add RowSpan
                        PdfGridCell gridCell18 = pdfGrid.Rows[8].Cells[0];
                        gridCell18.ColumnSpan = 3;
                        gridCell18.StringFormat = Izquierda;
                        gridCell18.Value = $"   TAG: {selectedRecord.IDENTIFICACION}";
                        //Add RowSpan
                        PdfGridCell gridCell19 = pdfGrid.Rows[8].Cells[3];
                        gridCell19.ColumnSpan = 3;
                        gridCell19.StringFormat = Izquierda;
                        gridCell19.Value = new PdfTextElement($"   Marca: {selectedRecord.MARCA}", font, new PdfPen(PdfColor.Empty), PdfBrushes.Black, Izquierda);
                        //Add RowSpan
                        PdfGridCell gridCell20 = pdfGrid.Rows[9].Cells[0];
                        gridCell20.ColumnSpan = 3;
                        gridCell20.StringFormat = Izquierda;
                        gridCell20.Value = new PdfTextElement($"   Número de serie: {selectedRecord.NSERIE}", font, new PdfPen(PdfColor.Empty), PdfBrushes.Black, Izquierda);
                        //Add RowSpan
                        PdfGridCell gridCell21 = pdfGrid.Rows[9].Cells[3];
                        gridCell21.ColumnSpan = 3;
                        gridCell21.StringFormat = Izquierda;
                        gridCell21.Value = new PdfTextElement($"   Modelo: {selectedRecord.MODELO}", font, new PdfPen(PdfColor.Empty), PdfBrushes.Black, Izquierda);
                        //Add RowSpan
                        PdfGridCell gridCell22 = pdfGrid.Rows[10].Cells[0];
                        gridCell22.ColumnSpan = 3;
                        gridCell22.StringFormat = Izquierda;
                        gridCell22.Value = new PdfTextElement($"   Medida: {selectedRecord.MEDIDA}", font, new PdfPen(PdfColor.Empty), PdfBrushes.Black, Izquierda);
                        //Add RowSpan
                        PdfGridCell gridCell23 = pdfGrid.Rows[10].Cells[3];
                        gridCell23.ColumnSpan = 3;
                        gridCell23.StringFormat = Izquierda;
                        gridCell23.Value = new PdfTextElement($"   Clase: {selectedRecord.SERIE}", font, new PdfPen(PdfColor.Empty), PdfBrushes.Black, Izquierda);
                        //Add RowSpan
                        PdfGridCell gridCell24 = pdfGrid.Rows[11].Cells[0];
                        gridCell24.ColumnSpan = 3;
                        gridCell24.StringFormat = Izquierda;
                        gridCell24.Value = new PdfTextElement($"   Orificio: {selectedRecord.ORIFICIO}", font, new PdfPen(PdfColor.Empty), PdfBrushes.Black, Izquierda);
                        //Add RowSpan
                        PdfGridCell gridCell25 = pdfGrid.Rows[11].Cells[3];
                        gridCell25.ColumnSpan = 3;
                        gridCell25.StringFormat = Izquierda;
                        gridCell25.Value = new PdfTextElement($"   Año: {selectedRecord.AÑO}", font, new PdfPen(PdfColor.Empty), PdfBrushes.Black, Izquierda);
                        //Add RowSpan
                        PdfGridCell gridCell26 = pdfGrid.Rows[12].Cells[0];
                        gridCell26.ColumnSpan = 3;
                        gridCell26.StringFormat = Izquierda;
                        gridCell26.Value = new PdfTextElement($"   Area: {selectedRecord.AREA}", font, new PdfPen(PdfColor.Empty), PdfBrushes.Black, Izquierda);
                        //Add RowSpan
                        PdfGridCell gridCell27 = pdfGrid.Rows[12].Cells[3];
                        gridCell27.ColumnSpan = 3;
                        gridCell27.StringFormat = Izquierda;
                        gridCell27.Value = $"   Fluido: {selectedRecord.FLUIDO.Trim()}";
                        //Add RowSpan
                        PdfGridCell gridCell28 = pdfGrid.Rows[13].Cells[0];
                        gridCell28.ColumnSpan = 3;
                        gridCell28.StringFormat = Izquierda;
                        gridCell28.Value = $"   Sobrepresión: {selectedRecord.SOBREPRESION}%";
                        //Add RowSpan
                        PdfGridCell gridCell29 = pdfGrid.Rows[13].Cells[3];
                        gridCell29.ColumnSpan = 3;
                        gridCell29.StringFormat = Izquierda;
                        gridCell29.Value = new PdfTextElement($"   Presión: {selectedRecord.PRESION.Trim()} Bar", font, new PdfPen(PdfColor.Empty), PdfBrushes.Black, Izquierda);
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
                            gridCell30.Value = $"   Contrapresión: {selectedRecord.CONTRAPRESION.Trim()} Bar";
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
                        gridCell34.Value = new PdfTextElement($"   Presión en Banco: {selectedRecord.PRESIONBANCO.Trim()} Bar", font, new PdfPen(PdfColor.Empty), PdfBrushes.Black, Izquierda);
                        //Add RowSpan
                        PdfGridCell gridCell35 = pdfGrid.Rows[16].Cells[3];
                        gridCell35.ColumnSpan = 3;
                        gridCell35.StringFormat = Izquierda;
                        gridCell35.Value = $"   Servicio: {selectedRecord.SERVICIO}";
                        //Add RowSpan
                        PdfGridCell gridCell36 = pdfGrid.Rows[17].Cells[0];
                        gridCell36.ColumnSpan = 6;
                        gridCell36.StringFormat = Centrado;
                        gridCell36.Value = new PdfTextElement("ENSAYOS A LA RECEPCIÓN", font, new PdfPen(PdfColor.Empty), PdfBrushes.Black, Centrado);
                        //Add RowSpan
                        PdfGridCell gridCell37 = pdfGrid.Rows[18].Cells[0];
                        gridCell37.ColumnSpan = 3;
                        gridCell37.StringFormat = Izquierda;
                        gridCell37.Value = new PdfTextElement($"   Ensayo a la Recepción: {selectedRecord.ENSRECEP}", font, new PdfPen(PdfColor.Empty), PdfBrushes.Black, Izquierda);
                        //Add RowSpan
                        PdfGridCell gridCell38 = pdfGrid.Rows[18].Cells[3];
                        gridCell38.ColumnSpan = 3;
                        gridCell38.StringFormat = Izquierda;
                        gridCell38.Value = new PdfTextElement($"   Estado: {selectedRecord.ESTADO}", font, new PdfPen(PdfColor.Empty), PdfBrushes.Black, Izquierda);
                        //Add RowSpan
                        PdfGridCell gridCell39 = pdfGrid.Rows[19].Cells[0];
                        gridCell39.ColumnSpan = 3;
                        gridCell39.StringFormat = Izquierda;
                        gridCell39.Value = new PdfTextElement($"   Presión ensayo recepción: {selectedRecord.PRESIONRECEP.Trim()} Bar", font, new PdfPen(PdfColor.Empty), PdfBrushes.Black, Izquierda);
                        //Add RowSpan
                        PdfGridCell gridCell40 = pdfGrid.Rows[19].Cells[3];
                        gridCell40.ColumnSpan = 3;
                        gridCell40.StringFormat = Izquierda;
                        gridCell40.Value = $"   Fugas: {selectedRecord.FUGAS}";
                        //Add RowSpan
                        PdfGridCell gridCell41 = pdfGrid.Rows[20].Cells[0];
                        gridCell41.ColumnSpan = 3;
                        gridCell41.StringFormat = Izquierda;
                        gridCell41.Value = $"   Presión de fuga: {selectedRecord.PRESIONFUGA.Trim()} Bar";
                        //Add RowSpan
                        PdfGridCell gridCell42 = pdfGrid.Rows[20].Cells[3];
                        gridCell42.ColumnSpan = 3;
                        gridCell42.StringFormat = Izquierda;
                        gridCell42.Value = new PdfTextElement($"   Cambio de presión: {selectedRecord.CAMBIOPRESION}", font, new PdfPen(PdfColor.Empty), PdfBrushes.Black, Izquierda);
                        //Add RowSpan
                        PdfGridCell gridCell43 = pdfGrid.Rows[21].Cells[0];
                        gridCell43.ColumnSpan = 6;
                        gridCell43.StringFormat = Centrado;
                        gridCell43.Value = new PdfTextElement("TRABAJOS EFECTUADOS", font, new PdfPen(PdfColor.Empty), PdfBrushes.Black, Centrado);
                        //Add RowSpan
                        PdfGridCell gridCell44 = pdfGrid.Rows[22].Cells[0];
                        gridCell44.ColumnSpan = 3;
                        gridCell44.StringFormat = Izquierda;
                        gridCell44.Value = new PdfTextElement($"   Cambio de repuestos: {selectedRecord.CAMBIOREPUESTO}", font, new PdfPen(PdfColor.Empty), PdfBrushes.Black, Izquierda);
                        //Add RowSpan
                        PdfGridCell gridCell45 = pdfGrid.Rows[22].Cells[3];
                        gridCell45.ColumnSpan = 3;
                        gridCell45.StringFormat = Izquierda;
                        gridCell45.Value = new PdfTextElement($"   Repuestos: {selectedRecord.REPUESTOS}", font, new PdfPen(PdfColor.Empty), PdfBrushes.Black, Izquierda);
                        //Add RowSpan
                        PdfGridCell gridCell46 = pdfGrid.Rows[23].Cells[0];
                        gridCell46.ColumnSpan = 3;
                        gridCell46.StringFormat = Izquierda;
                        gridCell46.Value = new PdfTextElement($"   Código de resorte: {selectedRecord.CODRESORTE}", font, new PdfPen(PdfColor.Empty), PdfBrushes.Black, Izquierda);
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
                        gridCell50.Value = new PdfTextElement($"   Presión solicitada: {selectedRecord.PRESIONSOLIC.Trim()} Bar", font, new PdfPen(PdfColor.Empty), PdfBrushes.Black, Izquierda);
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
                        gridCell54.Value = new PdfTextElement($"   Acta: {selectedRecord.ACTA}", font, new PdfPen(PdfColor.Empty), PdfBrushes.Black, Izquierda);
                        //Add RowSpan
                        PdfGridCell gridCell55 = pdfGrid.Rows[29].Cells[3];
                        gridCell55.ColumnSpan = 3;
                        gridCell55.StringFormat = Izquierda;
                        gridCell55.Value = $"   OPDS: {selectedRecord.OPDS}";
                        //Add RowSpan
                        PdfGridCell gridCell56 = pdfGrid.Rows[30].Cells[0];
                        gridCell56.ColumnSpan = 3;
                        gridCell56.StringFormat = Centrado;
                        gridCell56.Value = $"Responsable técnino:";
                        //Add RowSpan
                        PdfGridCell gridCell57 = pdfGrid.Rows[30].Cells[3];
                        gridCell57.ColumnSpan = 3;
                        gridCell57.StringFormat = Izquierda;
                        gridCell57.Value = $"   Presencia inspector: {selectedRecord.PRESENCIAINSPEC}";
                        //Add RowSpan
                        PdfGridCell gridCell58 = pdfGrid.Rows[31].Cells[0];
                        gridCell58.ColumnSpan = 2;
                        gridCell58.StringFormat = Centrado;
                        gridCell58.Value = new PdfBitmap(IMR);
                        //Add RowSpan
                        PdfGridCell gridCell59 = pdfGrid.Rows[31].Cells[2];
                        gridCell59.ColumnSpan = 1;
                        gridCell59.StringFormat = Centrado;
                        gridCell59.Value = "Ing. Iris Mónica Rabboni\r\nNº en OPDS s/res 1126: 188\r\nMatrícula: 47642";
                        //Add RowSpan
                        PdfGridCell gridCell60 = pdfGrid.Rows[31].Cells[3];
                        gridCell60.ColumnSpan = 3;
                        gridCell60.StringFormat = Centrado;
                        gridCell60.Value = "ARBROS SA.\r\nParque Industrial Desarrollo Productivo\r\n Ruta 24 5801, Moreno, Provincia de Buenos Aires\r\nTel.: (+54 9 11) 4497-8011 / 8033 / 8077";
                        //Draw the PdfGrid
                        pdfGrid.Draw(page, new Syncfusion.Drawing.PointF(0, 0));
                        //Saving the PDF to the MemoryStream
                        MemoryStream stream = new MemoryStream();
                        document.Save(stream);
                        //Set the position as '0'
                        stream.Position = 0;
                        //Close the document 
                        document.Close(true);
                        await JS.SaveAs($"{selectedRecord.PEDIDO} Certificado" + ".pdf", stream.ToArray());
                    }
                }
            }
            if (args.Item.Text == "OPDS")
            {
                if (this.Grid.SelectedRecords.Count > 0)
                {
                    foreach (Service selectedRecord in this.Grid.SelectedRecords)
                    {
                        ClienteList = await Http.GetFromJsonAsync<List<Cliente>>($"api/Cliente/BuscarPorCliente/{selectedRecord.CG_CLI}");
                        PdfDocument document1 = new PdfDocument();
                        document1.PageSettings.Size = new Syncfusion.Drawing.SizeF(612, 1009);
                        document1.PageSettings.Margins.All = 0;
                        //Create a PdfGrid
                        PdfGrid pdfGrid1 = new PdfGrid();
                        //Create the page
                        PdfPage page = document1.Pages.Add();
                        //Create PDF graphics for the page.
                        PdfGraphics graphics = page.Graphics;
                        //Set the standard font.

                        PdfFont font = new PdfStandardFont(PdfFontFamily.Courier, 12);
                        PdfLightTable pdfTable = new PdfLightTable();
                        page.Graphics.RotateTransform(-360);
                        string espaciosEmail = "";
                        for (int i = 0; i < (38 - ClienteList.FirstOrDefault().DES_PROV.Trim().Length); i++)
                        {
                            espaciosEmail = espaciosEmail + " ";
                        }
                        string email = "";
                        if (ClienteList.FirstOrDefault().EMAIL.Trim().Length > 29)
                        {
                            email = ClienteList.FirstOrDefault().EMAIL;
                        }
                        string espaciosMedida = "";
                        for (int i = 0; i < (46 - selectedRecord.IDENTIFICACION.Trim().Length); i++)
                        {
                            espaciosMedida = espaciosMedida + " ";
                        }
                        string espaciosSerie = "";
                        for (int i = 0; i < (42 - selectedRecord.MARCA.Trim().Length); i++)
                        {
                            espaciosSerie = espaciosSerie + " ";
                        }
                        string espaciosFluido = "";
                        for (int i = 0; i < (57 - selectedRecord.MODELO.Trim().Length); i++)
                        {
                            espaciosFluido = espaciosFluido + " ";
                        }
                        string espaciosTemperatura = "";
                        for (int i = 0; i < (60 - selectedRecord.MODELO.Trim().Length); i++)
                        {
                            espaciosTemperatura = espaciosTemperatura + " ";
                        }
                        string fugas = "";
                        if (selectedRecord.FUGAS.Trim() == "SI")
                        {
                            fugas = "x";
                        }
                        else
                        {
                            fugas = "     x";
                        }
                        string espaciosfugas = "";
                        for (int i = 0; i < (24 - fugas.Length); i++)
                        {
                            espaciosfugas = espaciosfugas + " ";
                        }
                        string TrEfec;
                        string TrEfec2;
                        if (selectedRecord.TRABAJOSEFEC == "D" || selectedRecord.TRABAJOSEFEC.Contains("reacondicionamiento"))
                        {
                            TrEfec = "Desarme, limpieza, revisión de sus componentes,";
                            TrEfec2 = "reacondicionamiento de asientos, rearmado, prueba, calibración.";
                        }
                        else if (selectedRecord.TRABAJOSEFEC == "V" || selectedRecord.TRABAJOSEFEC.Contains("funcionamiento"))
                        {
                            TrEfec = "Unicamente verificación de funcionamiento";
                            TrEfec2 = "y calibración.";
                        }
                        else
                        {
                            TrEfec = $"{selectedRecord.TRABAJOSEFEC}";
                            TrEfec2 = "";
                        }
                        string presionfuga;
                        if (selectedRecord.PRESIONFUGA.Trim() == "")
                        {
                            presionfuga = "";
                        }
                        else
                        {
                            presionfuga = "Bar";
                        }
                        string presionA;
                        if (selectedRecord.PRESION.Trim() == "")
                        {
                            presionA = "";
                        }
                        else
                        {
                            presionA = "Bar";
                        }
                        string presionSolic;
                        if (selectedRecord.PRESIONSOLIC.Trim() == "")
                        {
                            presionSolic = "";
                        }
                        else
                        {
                            presionSolic = "Bar";
                        }
                        //Draw the text.
                        graphics.DrawString(
                            $"\r\n" +//1
                            $"\r\n" +
                            $"\r\n" +
                            $"\r\n" +
                            $"\r\n" +//5
                            $"\r\n" +
                            $"\r\n" +
                            $"\r\n" +
                            $"\r\n" +
                            $"\r\n" +//10
                            $"         Nro. Serie Rep.: {selectedRecord.PEDIDO.Trim()}                                Fecha: {DateTime.Now.Day}/{DateTime.Now.Month}/{DateTime.Now.Year}\r\n" +
                            $"\r\n" +
                            $"                 {selectedRecord.CLIENTE.Trim()}\r\n" +
                            $"\r\n" +
                            $"                  {ClienteList.FirstOrDefault().DIRECC.Trim()}\r\n" +//15
                            $"                                                                    {ClienteList.FirstOrDefault().CG_POST}\r\n" +//no va
                            $"                  {ClienteList.FirstOrDefault().LOCALIDAD.Trim()}\r\n" +
                            $"                 {ClienteList.FirstOrDefault().DES_PROV.Trim()}{espaciosEmail}{email}\r\n" +
                            $"\r\n" +
                            $"\r\n" +//20
                            $"                         ARBROS S.A.\r\n" +
                            $"                                     08/11\r\n" +
                            $"\r\n" +
                            $"                                                        188\r\n" +
                            $"\r\n" +//25
                            $"\r\n" +
                            $"             {selectedRecord.IDENTIFICACION.Trim()}{espaciosMedida}Med ExS: {selectedRecord.MEDIDA.Trim()}\r\n" +
                            $"\r\n" +
                            $"               {selectedRecord.MARCA.Trim()}{espaciosSerie}Cnx ExS: {selectedRecord.SERIE.Trim()}\r\n" +
                            $"                {selectedRecord.MODELO.Trim()}{espaciosFluido}{selectedRecord.FLUIDO.Trim()}\r\n" +//30
                            $"\r\n" +
                            $"             {selectedRecord.AÑO.Trim()}{espaciosTemperatura}{selectedRecord.TEMP.Trim()}°C\r\n" +
                            $"                                                      {selectedRecord.CONTRAPRESION.Trim()} {selectedRecord.TIPO.Trim()}\r\n" +//DescargaTeorica
                            $"\r\n" +
                            $"\r\n" +//35
                            $"\r\n" +
                            $"                                      {fugas}{espaciosfugas}{selectedRecord.PRESIONFUGA.Trim()} {presionfuga}\r\n" +
                            $"                          {selectedRecord.PRESION.Trim()} {presionA}\r\n" +
                            $"\r\n" +
                            $"\r\n" +//40
                            $"\r\n" +
                            $"\r\n" +
                            $"\r\n" +
                            $"                     x          Lapidado                       x      Helicoidal\r\n" +
                            $"                     x          Lapidado\r\n" +//45
                            $"\r\n" +
                            $"\r\n" +
                            $"\r\n" +
                            $"                     {DateTime.Today.Day}    {DateTime.Today.Month}    {DateTime.Today.Year}\r\n" +
                            $"" +//50
                            $"\r\n" +
                            $"              x\r\n" +
                            $"                          {selectedRecord.PRESIONSOLIC.Trim()} {presionSolic}\r\n" +
                            $"\r\n" +
                            $"                                 {DateTime.Today.Day}    {DateTime.Today.Month}    {DateTime.Today.AddYears(1).Year}\r\n" +
                            $"\r\n" +
                            $"\r\n" +
                            $"                     {TrEfec}\r\n" +
                            $"          {TrEfec2}\r\n" +
                            $"\r\n" +//60
                            $"\r\n" +
                            $"\r\n" +
                            $"\r\n" +
                            $"\r\n" +
                            $"\r\n" +//65
                            $"\r\n" +
                            $"\r\n" +
                            $"\r\n" +
                            $"\r\n" +
                            $"                     p. ARBROS S.A.                 Ing. Iris Mónica Rabboni\r\n" +//70
                            $"                  Ing. Gustavo A. Mutz          N° Insc. en OPDS s/res 1126: 188\r\n" +
                            $"             TALLER HABILITADO OPDS N°08/11             Matricula: 47642\r\n", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(0, 0));

                        MemoryStream xx = new MemoryStream();
                        document1.Save(xx);
                        document1.Close(true);
                        await JS.SaveAs($"{selectedRecord.PEDIDO.Trim()} OPDS {selectedRecord.ACTA.Trim()}" + ".pdf", xx.ToArray());
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
