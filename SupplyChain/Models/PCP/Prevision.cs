using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SupplyChain.Shared.Models
{
    [Table("PresAnual")]
    public class PresAnual
    {
        [Key]
        public int REGISTRO { get; set; } = 0;
        public string CG_ART { get; set; } = "";
        public string DES_ART { get; set; } = "";
        public string UNID { get; set; } = "";
        public decimal CANTPED { get; set; } = 0;
        public DateTime FE_PED { get; set; }
    }
}