using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SupplyChain
{
    [Table("TipoMat")]
    public class TipoMat
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string TIPO { get; set; } = "";//


        public int CG_CIA { get; set; } = 0;//

    }
}




      