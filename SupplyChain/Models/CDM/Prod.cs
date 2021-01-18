using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SupplyChain
{
    public class Prod
    {
        [Key]
        public string CG_PROD { get; set; } = "";
        public string CAMPOCOM1 { get; set; } = "";
        public string CAMPOCOM2 { get; set; } = "";
        public string CAMPOCOM3 { get; set; } = "";
        public string CAMPOCOM4 { get; set; } = "";
        public string CAMPOCOM5 { get; set; } = "";
        public string CAMPOCOM6 { get; set; } = "";
        //public decimal PESO { get; set; } = 0;
    }
}
