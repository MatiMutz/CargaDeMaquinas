using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SupplyChain
{
    public class Cliente
    {
        [Key]
        public int CG_CLI { get; set; } = 0;
        public string DES_CLI { get; set; } = "";
        public string DIRECC { get; set; } = "";
        public string LOCALIDAD { get; set; } = "";
        public string TELEFONO { get; set; } = "";
        public string EMAIL { get; set; } = "";
        public string DES_PROV { get; set; } = "";
        public int CG_POST { get; set; } = 0;
    }
}
