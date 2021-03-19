using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SupplyChain.Shared.Models
{
    public class Fabricacion
    {
        [Key]
        public string CG_PROD { get; set; } = "";
        public string DES_PROD { get; set; } = "";
        public int CG_ORDEN { get; set; } = 0;
        public int CG_ORDF { get; set; } = 0;
        public int ULTIMAORDENASOCIADA { get; set; } = 0;
        public string CLASE { get; set; } = "";
        public string CG_R { get; set; } = "";
        public int CG_ESTADOCARGA { get; set; } = 0;
        public decimal CANT { get; set; } = 0;
        public decimal CANTFAB { get; set; } = 0;
        public string UNID { get; set; } = "";
        public string PROCESO { get; set; } = "";
        public bool INSUMOS_ENTREGADOS_A_PLANTA { get; set; } = false;
        public int PEDIDO { get; set; } = 0;
        public decimal DIASFAB { get; set; } = 0;
        public string CG_CELDA { get; set; } = "";
        public int CG_FORM { get; set; } = 0;
        public DateTime? FE_ENTREGA { get; set; }
        public DateTime? FE_EMIT { get; set; }
        public DateTime? FE_PLAN { get; set; }
        public DateTime? FE_FIRME { get; set; }
        public DateTime? FE_CURSO { get; set; }
        public DateTime? FECHA_PREVISTA_FABRICACION { get; set; }
        public int ORDEN { get; set; } = 0;
        public DateTime? FE_ANUL { get; set; }
        public DateTime? FE_CIERRE { get; set; }
    }
}