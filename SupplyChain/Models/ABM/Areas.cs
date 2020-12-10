using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SupplyChain
{
    [Table("Areas")]
    public class Areas
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CG_AREA { get; set; } = 0;
        public string DES_AREA { get; set; } = "";
        public string RESP { get; set; } = "";
        public string CONTROLES { get; set; } = "";
        public decimal TARA { get; set; } = 0;
        public int CG_TIPOAREA { get; set; } = 0;
        public int CG_PROVE { get; set; } = 0;
        public int CG_CIA { get; set; } = 0;
        public int CG_DEP { get; set; } = 0;
        public int CG_COS { get; set; } = 0;

    }
}




      