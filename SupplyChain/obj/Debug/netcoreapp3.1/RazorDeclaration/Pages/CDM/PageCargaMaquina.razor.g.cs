#pragma checksum "E:\ProyectoSupplychain\CargaDeMaquinas\SupplyChain\Pages\CDM\PageCargaMaquina.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "7e1f767a729e2748b43d1d5b354cc8140aee219d"
// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 0414
#pragma warning disable 0649
#pragma warning disable 0169

namespace SupplyChain.Pages.CDM
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "E:\ProyectoSupplychain\CargaDeMaquinas\SupplyChain\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "E:\ProyectoSupplychain\CargaDeMaquinas\SupplyChain\_Imports.razor"
using Microsoft.AspNetCore.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "E:\ProyectoSupplychain\CargaDeMaquinas\SupplyChain\_Imports.razor"
using Microsoft.AspNetCore.Components.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "E:\ProyectoSupplychain\CargaDeMaquinas\SupplyChain\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "E:\ProyectoSupplychain\CargaDeMaquinas\SupplyChain\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "E:\ProyectoSupplychain\CargaDeMaquinas\SupplyChain\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "E:\ProyectoSupplychain\CargaDeMaquinas\SupplyChain\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "E:\ProyectoSupplychain\CargaDeMaquinas\SupplyChain\_Imports.razor"
using SupplyChain;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "E:\ProyectoSupplychain\CargaDeMaquinas\SupplyChain\_Imports.razor"
using SupplyChain.Shared;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "E:\ProyectoSupplychain\CargaDeMaquinas\SupplyChain\Pages\CDM\PageCargaMaquina.razor"
using Syncfusion.Blazor.Popups;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "E:\ProyectoSupplychain\CargaDeMaquinas\SupplyChain\Pages\CDM\PageCargaMaquina.razor"
using Syncfusion.Blazor.Notifications;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "E:\ProyectoSupplychain\CargaDeMaquinas\SupplyChain\Pages\CDM\PageCargaMaquina.razor"
using Syncfusion.Pdf;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "E:\ProyectoSupplychain\CargaDeMaquinas\SupplyChain\Pages\CDM\PageCargaMaquina.razor"
using Syncfusion.Pdf.Graphics;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "E:\ProyectoSupplychain\CargaDeMaquinas\SupplyChain\Pages\CDM\PageCargaMaquina.razor"
using Syncfusion.Pdf.Grid;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "E:\ProyectoSupplychain\CargaDeMaquinas\SupplyChain\Pages\CDM\PageCargaMaquina.razor"
using System.Drawing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "E:\ProyectoSupplychain\CargaDeMaquinas\SupplyChain\Pages\CDM\PageCargaMaquina.razor"
using System.IO;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "E:\ProyectoSupplychain\CargaDeMaquinas\SupplyChain\Pages\CDM\PageCargaMaquina.razor"
using Syncfusion.Blazor.Inputs;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "E:\ProyectoSupplychain\CargaDeMaquinas\SupplyChain\Pages\CDM\PageCargaMaquina.razor"
using Syncfusion.Blazor.Calendars;

#line default
#line hidden
#nullable disable
#nullable restore
#line 11 "E:\ProyectoSupplychain\CargaDeMaquinas\SupplyChain\Pages\CDM\PageCargaMaquina.razor"
using Syncfusion.Blazor.DropDowns;

#line default
#line hidden
#nullable disable
#nullable restore
#line 12 "E:\ProyectoSupplychain\CargaDeMaquinas\SupplyChain\Pages\CDM\PageCargaMaquina.razor"
using System.Text.Json;

#line default
#line hidden
#nullable disable
#nullable restore
#line 15 "E:\ProyectoSupplychain\CargaDeMaquinas\SupplyChain\Pages\CDM\PageCargaMaquina.razor"
using Syncfusion.Pdf.Tables;

#line default
#line hidden
#nullable disable
    [Microsoft.AspNetCore.Components.RouteAttribute("/PageCargaMaquina")]
    public partial class PageCargaMaquina : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 748 "E:\ProyectoSupplychain\CargaDeMaquinas\SupplyChain\Pages\CDM\PageCargaMaquina.razor"
 
    protected List<ModeloCarga> dbCarga;
    protected int xExtensionColumnas = 20;
    protected int CantidadColumnasPorPeriodo = 8;
    protected int xHora = 1;
    protected int xColumna;
    protected string xCg_celda;
    protected int xColumnaInicialBarra;
    protected bool isOrdenDialogVisible = false;
    protected SfToast ToastObj;
    protected int ordenNumero = 0;
    protected string ordenTitulo = "";
    protected ModeloOrdenFabricacion ordenFabricacion;
    protected ModeloOrdenFabricacion ordenFabricacionOriginal;
    protected ModeloOrdenFabricacionEncabezado ordenFabricacionEncabezado;
    protected List<ModeloOrdenFabricacionMP> ordenFabricacionMP;
    protected List<ModeloOrdenFabricacionSE> ordenFabricacionSE;
    protected List<ModeloOrdenFabricacionHojaRuta> ordenFabricacionHojaRuta;
    protected List<ModeloGenericoIntString> dbOrdenesDependientes;
    protected List<ModeloGenericoIntString> dbEstadoCarga;
    protected List<ModeloGenericoStringString> dbCeldas;
    protected List<ModeloGenericoStringString> dbProcesos;
    protected string Usuario = "USER";
    protected string responseString;

    [Inject] protected Microsoft.JSInterop.IJSRuntime JS { get; set; }
    public async void exportdata()
    {
        PdfDocument document1 = new PdfDocument();
        //Create a PdfGrid
        PdfGrid pdfGrid1 = new PdfGrid();
        //Create the page
        PdfPage page = document1.Pages.Add();
        //Create PDF graphics for the page.
        PdfGraphics graphics = page.Graphics;
        //Set the standard font.
        PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 10);
        PdfLightTable pdfTable = new PdfLightTable();
        page.Graphics.RotateTransform(-90);
        //Draw the text.
        graphics.DrawString($"        OF ALTA: {ordenFabricacion.CG_ORDF}\r\n            {ordenFabricacion.CG_PROD}\r\n{ordenFabricacion.DES_PROD}\r\nCANTIDAD {ordenFabricacion.CANTFAB}    {ordenFabricacion.FE_CIERRE}", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(-200, 10));
        document1.PageSettings.Margins.All = 0;
        //PdfTemplate template = null;
        MemoryStream xx = new MemoryStream();
        document1.Save(xx);

        //Set the position as '0'
        //xx.Position = 0;
        //Close the document
        document1.Close(true);

        await JS.SaveAs("ETOF" + ordenFabricacion.CG_PROD.Trim() + ".pdf", xx.ToArray());

        /*
        row1.Cells[0].Value = $"OF ALTA: {ordenFabricacion.CG_CELDA}";
        row1.Cells[1].Value = $"Description: {ordenFabricacion.DES_PROD}";
        PdfGridRow row2 = pdfGrid.Rows.Add();
        row2.Cells[0].Value = $"CANTIDAD: {ordenFabricacion.CANT}";
        row1.Cells[1].Value = $"FECHA: {ordenFabricacion.FECHA_INICIO_REAL_FABRICACION}";
        */
    }
    async Task DownloadText()
    {
        string presion = ordenFabricacionEncabezado.CAMPOCOM4.Trim();
        presion = presion.Remove((presion.Length - 5));
        // Generate a text file
        byte[] file = System.Text.Encoding.UTF8.GetBytes($"{ordenFabricacionEncabezado.DES_CLI.Trim()}\r\n{ordenFabricacion.PEDIDO}\r\n{presion}");
        await JS.InvokeVoidAsync("BlazorDownloadFile", $"{ordenFabricacion.PEDIDO}.txt", "text/plain", file);
    }
    protected override async Task OnInitializedAsync()
    {
        dbEstadoCarga = new List<ModeloGenericoIntString>();
        dbEstadoCarga.Add(new ModeloGenericoIntString() { ID = 2, TEXTO = "FIRME" });
        dbEstadoCarga.Add(new ModeloGenericoIntString() { ID = 3, TEXTO = "EN CURSO" });
        dbEstadoCarga.Add(new ModeloGenericoIntString() { ID = 4, TEXTO = "CERRADA" });
        dbEstadoCarga.Add(new ModeloGenericoIntString() { ID = 5, TEXTO = "ANULADA" });
        await Refrescar();
    }

    protected async Task Refrescar()
    {
        try
        {
            dbCarga = await Http.GetFromJsonAsync<List<ModeloCarga>>("api/Cargas");
        }
        catch
        {
            throw;
        }
    }

    protected async Task OrdenFabricacionOpen(int xOrdenFabricacion, bool xExigeOA, int xPedido, string xCgProd, decimal xCantidad)
    {
        try
        {
            isOrdenDialogVisible = true;
            // Titulo
            if (xExigeOA)
            {
                ordenTitulo = "ORDEN DE ARMADO Nº " + xOrdenFabricacion.ToString();
            }
            else
            {
                ordenTitulo = "ORDEN DE FABRICACIÓN Nº " + xOrdenFabricacion.ToString();
            }
            if (xPedido > 0)
            {
                ordenTitulo += " - SERIE / PEDIDO Nº " + xPedido.ToString();
            }
            // Datos de la orden
            ordenNumero = xOrdenFabricacion;
            ordenFabricacion = await Http.GetFromJsonAsync<ModeloOrdenFabricacion>("api/OrdenesFabricacion/" + ordenNumero.ToString());
            ordenFabricacionOriginal = Newtonsoft.Json.JsonConvert.DeserializeObject<ModeloOrdenFabricacion>(Newtonsoft.Json.JsonConvert.SerializeObject(ordenFabricacion));
            // Ordenes dependientes
            string xSQLcommand = String.Format("SELECT 0 ID, CONVERT(varchar, 0) TEXTO UNION SELECT DISTINCT CG_ORDF ID, CONVERT(varchar, CG_ORDF) TEXTO FROM PROGRAMA WHERE CG_ORDFASOC = {0} AND CG_ORDF != {1}",
                                                  ordenFabricacion.CG_ORDFASOC,
                                                  ordenFabricacion.CG_ORDF);
            dbOrdenesDependientes = await Http.GetFromJsonAsync<List<ModeloGenericoIntString>>("api/ModelosGenericosIntString/" + xSQLcommand);
            // Celdas
            xSQLcommand = String.Format("SELECT ltrim(rtrim(CG_CELDA)) ID, DES_CELDA TEXTO FROM CELDAS ORDER BY CG_CELDA");
            dbCeldas = await Http.GetFromJsonAsync<List<ModeloGenericoStringString>>("api/ModelosGenericosStringString/" + xSQLcommand);
            // Procesos
            xSQLcommand = String.Format("SELECT PROCESO ID, DESCRIP TEXTO FROM PROTAB ORDER BY PROCESO");
            dbProcesos = await Http.GetFromJsonAsync<List<ModeloGenericoStringString>>("api/ModelosGenericosStringString/" + xSQLcommand);
            // Datos del encabezado del detalle
            ordenFabricacionEncabezado = await Http.GetFromJsonAsync<ModeloOrdenFabricacionEncabezado>("api/OrdenesFabricacionEncabezado/" + ordenNumero.ToString());
            // Materias primas
            ordenFabricacionMP = await Http.GetFromJsonAsync<List<ModeloOrdenFabricacionMP>>("api/OrdenesFabricacionMP/" + ordenNumero.ToString());
            // Semi elaborados
            ordenFabricacionSE = await Http.GetFromJsonAsync<List<ModeloOrdenFabricacionSE>>("api/OrdenesFabricacionSE/" + ordenNumero.ToString());
            // Semi elaborados
            ordenFabricacionHojaRuta = await Http.GetFromJsonAsync<List<ModeloOrdenFabricacionHojaRuta>>("api/OrdenesFabricacionHojaRuta/" + xCgProd + "/" + xCantidad.ToString());
        }
        catch
        {
            throw;
        }
    }

    private void OrdenFabricacionClose(Object args)
    {
        try
        {
            isOrdenDialogVisible = false;
            ordenFabricacion = null;
        }
        catch
        {
            throw;
        }
    }

    private async Task OrdenFabricacionOk(Object args)
    {
        try
        {
            isOrdenDialogVisible = false;

            await Http.PutAsJsonAsync("api/OrdenesFabricacion/" + ordenFabricacion.CG_ORDF, ordenFabricacion);
            if (ordenFabricacion.CG_ESTADOCARGA == 2 || ordenFabricacion.CG_ESTADOCARGA == 3 && (ordenFabricacion.CG_ESTADOCARGA != ordenFabricacionOriginal.CG_ESTADOCARGA))
            {
                string sqlCommandString = string.Format("UPDATE Programa SET CG_ESTADOCARGA = {0},Fe_curso = GETDATE(), CG_ESTADO = {1} WHERE (Cg_ordf = {2} OR Cg_ordfAsoc = {2})",
                                          ordenFabricacion.CG_ESTADOCARGA,
                                          ordenFabricacionOriginal.CG_ESTADOCARGA,
                                          ordenFabricacion.CG_ORDF);
                await Http.PutAsJsonAsync("api/SQLgenericCommandString/" + sqlCommandString, ordenFabricacion);
            }
            else if (ordenFabricacion.CG_ESTADOCARGA == 4)
            {
                if (ordenFabricacion.CANTFAB == 0)
                {
                    await this.ToastObj.Show(new ToastModel { Title = "AVISO!", Content = "Órden sin indicar cantidad fabricada. Se continuará igualmente.", CssClass = "e-toast-warning", Icon = "e-warning toast-icons" });
                }

                //Busca Scrap
                // FALTA SCRAP

                string sqlCommandString = "EXEC NET_PCP_Cerrar_OrdenFabricacion " + ordenFabricacion.CG_ORDF.ToString() + ", '" + Usuario + "', 0";
                var respuesta = await Http.PutAsJsonAsync("api/SQLgenericCommandString/" + sqlCommandString, ordenFabricacion);
                responseString = await respuesta.Content.ReadAsStringAsync();
                Console.WriteLine(responseString);
                if (ordenFabricacion.CG_ORDF == ordenFabricacion.ULTIMAORDENASOCIADA)
                {
                    await this.ToastObj.Show(new ToastModel { Title = "AVISO!", Content = "Se dió de alta el producto terminado.", CssClass = "e-toast-success", Icon = "e-success toast-icons" });
                }
            }
            else if (ordenFabricacion.CG_ESTADOCARGA == 5)
            {
                string sqlCommandString = "EXEC NET_PCP_Anular_OrdenFabricacion " + ordenFabricacion.CG_ORDF.ToString() + ", '" + Usuario + "'";
                await Http.PutAsJsonAsync("api/SQLgenericCommandString/" + sqlCommandString, ordenFabricacion);
                StateHasChanged();
                await this.ToastObj.Show(new ToastModel { Title = "AVISO!", Content = "Órden anulada.", CssClass = "e-toast-warning", Icon = "e-warning toast-icons" });
            }
            ordenFabricacion = null;
            await Refrescar();
        }
        catch
        {
            throw;
        }
    }

    protected async Task EstadoCarga_Change()
    {
        try
        {
            if (ordenFabricacionOriginal.CG_ESTADOCARGA == 0 && ordenFabricacion.CG_ESTADOCARGA == 2)
            {
                await this.ToastObj.Show(new ToastModel { Title = "ERROR!", Content = "No puede pasar una órden de fabricación EMITIDA a estado EN FIRME.", CssClass = "e-toast-danger", Icon = "e-error toast-icons" });
            }
            else if (ordenFabricacionOriginal.CG_ESTADOCARGA == 0 && ordenFabricacion.CG_ESTADOCARGA == 3)
            {
                await this.ToastObj.Show(new ToastModel { Title = "ERROR!", Content = "No puede pasar una órden de fabricación EMITIDA a estado EN CURSO.", CssClass = "e-toast-danger", Icon = "e-error toast-icons" });
            }
            else if (ordenFabricacionOriginal.CG_ESTADOCARGA == 1 && ordenFabricacion.CG_ESTADOCARGA == 3)
            {
                await this.ToastObj.Show(new ToastModel { Title = "ERROR!", Content = "No puede pasar una órden de fabricación PLANEADA a estado EN CURSO.", CssClass = "e-toast-danger", Icon = "e-error toast-icons" });
            }
        }
        catch
        {
            throw;
        }
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private CustomHttpClient Http { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IJSRuntime JsRuntime { get; set; }
    }
}
#pragma warning restore 1591
