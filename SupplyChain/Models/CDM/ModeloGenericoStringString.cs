using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SupplyChain
{
    public class ModeloGenericoStringString
    {
        [Key]
        public string ID { get; set; }
        public string TEXTO { get; set; }
    }
}
