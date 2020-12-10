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
        public int CG_AREA { get; set; } = 0;
        public int CG_CATEOP { get; set; } = 0;
        public bool ILIMITADA { get; set; } = false;
        public decimal COEFI { get; set; } = 0;
        public int CG_PROVE { get; set; } = 0;
        public decimal VALOR_AMOR { get; set; } = 0;
        public decimal VALOR_MERC { get; set; } = 0;
        public string MONEDA { get; set; } = "";
        public decimal CANT_ANOS { get; set; } = 0;
        public decimal CANT_UNID { get; set; } = 0;
        public decimal REP_ANOS { get; set; } = 0;
        public decimal M2 { get; set; } = 0;
        public decimal ENERGIA { get; set; } = 0;
        public decimal COMBUST { get; set; } = 0;
        public decimal AIRE_COMP { get; set; } = 0;
        public int CG_TIPOCELDA { get; set; } = 0;
        public int CG_DEPOSM { get; set; } = 0;
        public int CG_CIA { get; set; } = 0;
    }
}




      