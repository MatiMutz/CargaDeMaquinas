using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SupplyChain
{
    [Table("Celdas")]
    public class Celdas
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string CG_CELDA { get; set; } = "";
        public string DES_CELDA { get; set; } = "";
        //[ColumnaGridViewAtributo(Name = "Código de área")]
        //public int CG_AREA { get; set; } = 0;
        //[ColumnaGridViewAtributo(Name = "Código de categoria Operarios")]
        //public int CG_CATEOP { get; set; } = 0;
        //[ColumnaGridViewAtributo(Name = "Ilimitada")]
        //public bool ILIMITADA { get; set; } = false;
        //[ColumnaGridViewAtributo(Name = "Coeficiente")]
        //public decimal COEFI { get; set; } = 0;
        //[ColumnaGridViewAtributo(Name = "Código de proveedor")]
        //public int CG_PROVE { get; set; } = 0;
        //[ColumnaGridViewAtributo(Name = "Valor de Amortizacion")]
        //public decimal VALOR_AMOR { get; set; } = 0;
        //[ColumnaGridViewAtributo(Name = "Valor de Mercado")]
        //public decimal VALOR_MERC { get; set; } = 0;
        //[ColumnaGridViewAtributo(Name = "Moneda")]
        //public string MONEDA { get; set; } = "";
        //[ColumnaGridViewAtributo(Name = "Cantidad de años")]
        //public decimal CANT_ANOS { get; set; } = 0;
        //[ColumnaGridViewAtributo(Name = "Cantidad de unidades")]
        //public decimal CANT_UNID { get; set; } = 0;
        //[ColumnaGridViewAtributo(Name = "Repuesto años")]
        //public decimal REP_ANOS { get; set; } = 0;
        //[ColumnaGridViewAtributo(Name = "M2")]
        //public decimal M2 { get; set; } = 0;
        //[ColumnaGridViewAtributo(Name = "Energía")]
        //public decimal ENERGIA { get; set; } = 0;
        //[ColumnaGridViewAtributo(Name = "Combustible")]
        //public decimal COMBUST { get; set; } = 0;
        //[ColumnaGridViewAtributo(Name = "Aire comprimido")]
        //public decimal AIRE_COMP { get; set; } = 0;
        //[ColumnaGridViewAtributo(Name = "Código de tipo de celda")]
        //public int CG_TIPOCELDA { get; set; } = 0;
        //[ColumnaGridViewAtributo(Name = "Código de depósito Material")]
        //public int CG_DEPOSM { get; set; } = 0;
        //[ColumnaGridViewAtributo(Name = "Código de compañia")]
        //public int CG_CIA { get; set; } = 0;
    }
}




      