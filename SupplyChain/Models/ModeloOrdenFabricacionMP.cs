using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SupplyChain
{
    public class ModeloOrdenFabricacionMP
    {
        [Key]
        public int REGISTRO { get; set; }
        public string CG_ART { get; set; }
        public string DES_ART { get; set; }
        public decimal STOCK { get; set; }
        public string LOTE { get; set; }
        public string DESPACHO { get; set; }
        public string SERIE { get; set; }
    }
}
