using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SupplyChain
{
    [Table("Operario")]
    public class Operario
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CG_OPER { get; set; } = 0;
        public string DES_OPER { get; set; } = "";
        public int CG_TURNO { get; set; } = 0;
        public decimal RENDIM { get; set; } = 0;
        public DateTime? FE_FINAL { get; set; }
        public int HS_FINAL { get; set; } = 0;
        public string CG_CATEOP { get; set; } = "";
        public decimal VALOR_HORA { get; set; } = 0;
        public string MONEDA { get; set; } = "";
        public bool ACTIVO { get; set; } = false;
        public int CG_CIA { get; set; } = 0;
        public string USUARIO { get; set; } = "";
    }
}
