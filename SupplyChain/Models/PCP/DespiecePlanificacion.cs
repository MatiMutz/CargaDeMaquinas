using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SupplyChain.Shared.Models
{
    public class DespiecePlanificacion
    {
        [Key]
        public string CG_PROD { get; set; } = "";
        public string CG_SE { get; set; } = "";
        public string CG_MAT { get; set; } = "";
        public string DES_PROD { get; set; } = "";
        public string UNID { get; set; } = "";
        public decimal CG_FORM { get; set; } = 0;
        public decimal STOCK { get; set; } = 0;
        public decimal CANT_MAT { get; set; } = 0;
        public decimal SALDO { get; set; } = 0;
        public decimal CANT_PLANEADAS { get; set; } = 0;
        public decimal SALDO_PLANEADAS { get; set; } = 0;
        public decimal CANT_TOTAL { get; set; } = 0;
        public decimal SALDO_TOTAL { get; set; } = 0;
    }
}