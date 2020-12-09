using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SupplyChain
{
    [Table("Pedidos")]
    public class Pedidos
    {
        [Key]
        public int REGISTRO { get; set; } = 0;
        public int PEDIDO { get; set; } = 0;
        public string REMITO { get; set; } = "";
        public decimal FLAG { get; set; } = 0;
        public int CG_ORDF { get; set; } = 0;
        public int TIPOO { get; set; } = 0;
        public int CG_TIRE { get; set; } = 0;
    }
}
