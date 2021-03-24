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
using SupplyChain.Shared.Models;
using System.IO;
using Syncfusion.Blazor.Notifications;
using Syncfusion.Blazor.Lists;

namespace SupplyChain.Pages.CargaDeMaquina
{
    public class CDMPageBase : ComponentBase
    {
        [Inject] protected CustomHttpClient Http { get; set; }
        [Inject] protected Microsoft.JSInterop.IJSRuntime JS { get; set; }
        protected List<ModeloCarga> dbCarga;
        protected int extensionDias = 365;
        protected int totalDiasNoLaborables = 0;
        protected int CantidadColumnasPorPeriodo = 8;
        protected int xHora = 1;
        protected int xColumna;
        protected string xCg_celda;
        protected int xColumnaInicialBarra;
        protected bool isOrdenDialogVisible = false;
        protected bool isScrapDialogVisible = false;
        protected bool sabadosLaborables = false;
        protected bool domingosLaborables = false;
        protected SfToast ToastObj;
        protected int ordenNumero = 0;
        protected string ordenTitulo = "";
        protected DateTime fechaInicial;
        protected int ordenAbuscar = 0;
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
        protected List<ModeloGenericoStringString> dbDiasFestivos;
        protected List<ModeloGenericoIntString> dbScrap;
        protected int? scrapSeleccionado;
        protected string Usuario = "USER";
        protected int OrdenDeFabAlta;
        public SfListView<ModeloGenericoIntString> listViewScrap;
        protected string scrapSeleccionadoMensaje { get; set; } = "";
        protected IEnumerable<Operario> operariosBE3;
        protected List<Solution> rutas;

        protected List<Operario> operariosList = new List<Operario>();
        protected List<PedCli> PedCliList = new List<PedCli>();
        protected List<Prod> prodList = new List<Prod>();

        protected override async Task OnInitializedAsync()
        {
            dbEstadoCarga = new List<ModeloGenericoIntString>();
            dbEstadoCarga.Add(new ModeloGenericoIntString() { ID = 2, TEXTO = "FIRME" });
            dbEstadoCarga.Add(new ModeloGenericoIntString() { ID = 3, TEXTO = "EN CURSO" });
            dbEstadoCarga.Add(new ModeloGenericoIntString() { ID = 4, TEXTO = "CERRADA" });
            dbEstadoCarga.Add(new ModeloGenericoIntString() { ID = 5, TEXTO = "ANULADA" });

            operariosList = await Http.GetFromJsonAsync<List<Operario>>("api/Operario");
            operariosBE3 = from operariosBE3 in (IEnumerable<Operario>)operariosList
                           where operariosBE3.CG_OPER == 51 || operariosBE3.CG_OPER == 131 || operariosBE3.CG_OPER == 135 || operariosBE3.CG_OPER == 139 || operariosBE3.CG_OPER == 144
                           select operariosBE3;
            PedCliList = await Http.GetFromJsonAsync<List<PedCli>>("api/PedCli/GetPedidos");
            prodList = await Http.GetFromJsonAsync<List<Prod>>("api/Prod/GetPedidos");

            rutas = await Http.GetFromJsonAsync<List<Solution>>("api/Solution");

            await Refrescar();
        }

        protected async Task Refrescar()
        {
            try
            {
                dbCarga = await Http.GetFromJsonAsync<List<ModeloCarga>>("api/Cargas");
                // turno
                List<ModeloGenericoIntString> xTurno = await Http.GetFromJsonAsync<List<ModeloGenericoIntString>>("api/ModelosGenericosIntString/Select Top 1 CONVERT(INT, ValorN) ID, '' TEXTO From Solution Where Campo = 'HORASDIA'");
                CantidadColumnasPorPeriodo = xTurno.FirstOrDefault().ID;
                // Dias de calendario
                List<ModeloGenericoIntString> xDias = await Http.GetFromJsonAsync<List<ModeloGenericoIntString>>("api/ModelosGenericosIntString/Select Top 1 CONVERT(INT, ValorN) ID, '' TEXTO From Solution Where Campo = 'DIASCARGA'");
                extensionDias = xDias.FirstOrDefault().ID;
                // Sabados laborables
                List<ModeloGenericoIntString> xSabadosLaborables = await Http.GetFromJsonAsync<List<ModeloGenericoIntString>>("api/ModelosGenericosIntString/Select Top 1 CONVERT(INT, ValorN) ID, '' TEXTO From Solution Where Campo = 'SABADOSLABORABLES'");
                sabadosLaborables = (xSabadosLaborables.FirstOrDefault().ID == 1) ? true : false;
                // Domingos laborables
                List<ModeloGenericoIntString> xDomingosLaborables = await Http.GetFromJsonAsync<List<ModeloGenericoIntString>>("api/ModelosGenericosIntString/Select Top 1 CONVERT(INT, ValorN) ID, '' TEXTO From Solution Where Campo = 'DOMINGOSLABORABLES'");
                domingosLaborables = (xDomingosLaborables.FirstOrDefault().ID == 1) ? true : false;
                // Dias laborables
                dbDiasFestivos = await Http.GetFromJsonAsync<List<ModeloGenericoStringString>>("api/ModelosGenericosStringString/select distinct convert(char(8), Fecha, 112) ID, '' TEXTO from CalendarioFestivos");
                //Busca Scrap
                dbScrap = await Http.GetFromJsonAsync<List<ModeloGenericoIntString>>("api/ModelosGenericosIntString/SELECT convert(int, cg_scrap) ID, des_scrap TEXTO FROM scrap ORDER BY Cg_scrap");
                // fecha inicial
                if (dbCarga.Where(x => x.FE_CURSO.Year != 1900 && x.CG_ESTADOCARGA == 3).ToList().Count > 0)
                    fechaInicial = dbCarga.Where(x => x.FE_CURSO.Year != 1900 && x.CG_ESTADOCARGA == 3).Min(x => x.FE_CURSO);
                else
                    fechaInicial = DateTime.Now;
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

        protected void OrdenFabricacionClose(Object args)
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

        protected async Task OrdenFabricacionOk(Object args)
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
                    ordenFabricacion = null;
                    await Refrescar();
                }
                else if (ordenFabricacion.CG_ESTADOCARGA == 4)
                {
                    if (ordenFabricacion.CANTFAB == 0)
                    {
                        await this.ToastObj.Show(new ToastModel { Title = "AVISO!", Content = "Órden sin indicar cantidad fabricada. Se continuará igualmente.", CssClass = "e-toast-warning", Icon = "e-warning toast-icons" });
                    }

                    if (dbScrap != null)
                    {
                        isScrapDialogVisible = true;
                        StateHasChanged();
                    }
                    else
                    {
                        await CerrarOrdenFabricacion();
                    }
                }
                else if (ordenFabricacion.CG_ESTADOCARGA == 5)
                {
                    string sqlCommandString = "EXEC NET_PCP_Anular_OrdenFabricacion " + ordenFabricacion.CG_ORDF.ToString() + ", '" + Usuario + "'";
                    await Http.PutAsJsonAsync("api/SQLgenericCommandString/" + sqlCommandString, ordenFabricacion);
                    StateHasChanged();
                    await this.ToastObj.Show(new ToastModel { Title = "AVISO!", Content = "Órden anulada.", CssClass = "e-toast-warning", Icon = "e-warning toast-icons" });
                    ordenFabricacion = null;
                    await Refrescar();
                }
            }
            catch
            {
                throw;
            }
        }

        protected void Scrap_Selection(ModeloGenericoIntString args)
        {
            try
            {
                scrapSeleccionadoMensaje = "";
                scrapSeleccionado = args.ID;
            }
            catch
            {
                throw;
            }
        }

        protected async Task DialogScrapClose(Object args)
        {
            try
            {
                isScrapDialogVisible = false;
                await CerrarOrdenFabricacion();
            }
            catch
            {
                throw;
            }
        }

        protected async Task DialogScrapOk(Object args)
        {
            try
            {
                if (scrapSeleccionado == null)
                {
                    scrapSeleccionadoMensaje = "No seleccionó ningún Item";
                }
                else
                {
                    isScrapDialogVisible = false;
                    await CerrarOrdenFabricacion();
                }
            }
            catch
            {
                throw;
            }
        }

        protected async Task CerrarOrdenFabricacion()
        {
            string sqlCommandString = "EXEC NET_PCP_Cerrar_OrdenFabricacion " + ordenFabricacion.CG_ORDF.ToString() + ", '" + Usuario + "', " + scrapSeleccionado.ToString();
            await Http.PutAsJsonAsync("api/SQLgenericCommandString/" + sqlCommandString, ordenFabricacion);
            if (ordenFabricacion.CG_ORDF == ordenFabricacion.ULTIMAORDENASOCIADA)
            {
                await this.ToastObj.Show(new ToastModel { Title = "AVISO!", Content = "Se dió de alta el producto terminado.", CssClass = "e-toast-success", Icon = "e-success toast-icons" });
            }
            ordenFabricacion = null;
            scrapSeleccionado = null;
            await Refrescar();
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

        protected async Task BuscarOrden_Click()
        {
            await Refrescar();
        }

        protected async Task IrAServicio(string pedido)
        {
            //NavigationManager.NavigateTo($"sc/servicio/list/{pedido}");
            await JS.InvokeVoidAsync("open", new object[2] { $"sc/servicio/list/{pedido}", $"sc/servicio/list/{pedido}" });
        }

        protected async Task DownloadText()
        {
            string presion = ordenFabricacionEncabezado.CAMPOCOM4.Trim();
            if (String.IsNullOrEmpty(presion))
            {
                presion = ordenFabricacionEncabezado.CAMPOCOM1.Trim();
            }
            presion = presion.Remove((presion.Length - 5));
            presion = presion.Replace(',', '.');
            // Generate a text file
            //byte[] file;
            if (String.IsNullOrEmpty(ordenFabricacion.DES_OPER))
            {
                await this.ToastObj.Show(new ToastModel { Title = "AVISO!", Content = "Antes de generar el archivo debe asignar un operario", CssClass = "e-toast-danger", Icon = "e-success toast-icons" });
            }
            else
            {
                foreach(Solution ruta in rutas)
                {
                    if (ruta.CAMPO == "RUTADATOS")
                    {
                        string fileName = ruta.VALORC + ordenFabricacion.PEDIDO + ".txt";
                        try
                        {
                            // Check if file already exists. If yes, delete it.
                            if (File.Exists(fileName))
                            {
                                File.Delete(fileName);
                            }

                            // Create a new file
                            using (StreamWriter sw = File.CreateText(fileName))
                            {
                                sw.WriteLine($"{ordenFabricacionEncabezado.DES_CLI.Trim()}");
                                sw.WriteLine($"{ordenFabricacion.PEDIDO}");
                                sw.WriteLine($"{presion}");
                                sw.WriteLine($"{ordenFabricacion.DES_OPER}");
                            }
                            await this.ToastObj.Show(new ToastModel { Title = "AVISO!", Content = "Archivo generado con éxito", CssClass = "e-toast-success", Icon = "e-success toast-icons" });
                        }
                        catch (Exception Ex)
                        {
                            Console.WriteLine(Ex.ToString());
                        }

                    }
                }
            }
        }
    }
}
