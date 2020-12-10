using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SupplyChain.Shared.Models
{
    [Table("Unidades")]
    public class Unidades
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string UNID { get; set; } = "";
        public string DES_UNID { get; set; } = "";
        public string TIPOUNID { get; set; } = "";
        public decimal CG_DENBASICA { get; set; } = 0;
        public int CODIGO { get; set; } = 0;
        public int CG_CIA { get; set; } = 0;
    }
}




      