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
        public string DES_CLI { get; set; } = "";
        public string CG_ART { get; set; } = "";
        public string DES_ART { get; set; } = "";
        public string DESPACHO { get; set; } = "";
        public string LOTE { get; set; } = "";
        public string AVISO { get; set; } = "";
        public DateTime FE_MOV { get; set; }
    }
}
