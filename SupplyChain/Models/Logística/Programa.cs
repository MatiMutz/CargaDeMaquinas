using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SupplyChain
{
    public class Programa
    {
        [Key]
        public decimal REGISTRO { get; set; } = 0;
        public int PEDIDO { get; set; } = 0;
        public int CG_ESTADO { get; set; } = 0;
        public int CG_ESTADOCARGA { get; set; } = 0;
        public DateTime FE_CIERRE { get; set; }
    }
}
