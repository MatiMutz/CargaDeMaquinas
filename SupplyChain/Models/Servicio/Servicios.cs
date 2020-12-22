using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SupplyChain
{
    [Table("Servicios")]
    public class Service
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string PEDIDO { get; set; } = "";
        public DateTime? FECHA { get; set; }
        public string CLIENTE { get; set; } = "";
        public int CG_CLI { get; set; } = 0;
        public string PLANTA { get; set; } = "";
        public string OCOMPRA { get; set; } = "";
        public string REMITOREC { get; set; } = "";
        public string REMITO { get; set; } = "";
        public string IDENTIFICACION { get; set; } = "";
        public string NSERIE { get; set; } = "";
        public string MARCA { get; set; } = "";
        public string MODELO { get; set; } = "";
        public string MEDIDA { get; set; } = "";
        public string SERIE { get; set; } = "";
        public string ORIFICIO { get; set; } = "";
        public string AREA { get; set; } = "";
        public string FLUIDO { get; set; } = "";
        public string AÑO { get; set; } = "";
        public string PRESION { get; set; } = "";
        public string TEMP { get; set; } = "";
        public string PRESIONBANCO { get; set; } = "";
        public string SOBREPRESION { get; set; } = "";
        public string CONTRAPRESION { get; set; } = "";
        public string TIPO { get; set; } = "";
        public string RESORTE { get; set; } = "";
        public string SERVICIO { get; set; } = "";
        public string ENSRECEP { get; set; } = "";
        public string ESTADO { get; set; } = "";
        public string PRESIONRECEP { get; set; } = "";
        public string FUGAS { get; set; } = "";
        public string PRESIONFUGA { get; set; } = "";
        public string CAMBIOPRESION { get; set; } = "";
        public string PRESIONSOLIC { get; set; } = "";
        public string CAMBIOREPUESTO { get; set; } = "";
        public string CODRESORTE { get; set; } = "";
        public string REPUESTOS { get; set; } = "";
        public string TRABAJOSEFEC { get; set; } = "";
        public string TRABAJOSACCES { get; set; } = "";
        public string MANOMETRO { get; set; } = "";
        public string FECMANTANT { get; set; } = "";
        public string PEDIDOANT { get; set; } = "";
        public string ENSAYOCONTRAP { get; set; } = "";
        public string RESP { get; set; } = "";
        public string CONTROLO { get; set; } = "";
        public string POP { get; set; } = "";
        public string RESPTECNICO { get; set; } = "";
        public string OPDS { get; set; } = "";
        public string ACTA { get; set; } = "";
        public string PRESENCIAINSPEC { get; set; } = "";
        public string DESCARTICULO { get; set; } = "";
        public string OBSERV { get; set; } = "";
        public string OBSERVMANT { get; set; } = "";
        public string CATALOGO { get; set; } = "";
    }
}
