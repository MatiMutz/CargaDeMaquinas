using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SupplyChain
{
    public class ModeloOrdenFabricacion
    {
        [Key]
        public int CG_ORDF { get; set; }
        public DateTime FE_ENTREGA { get; set; }
        public string CG_PROD { get; set; }
        public string DES_PROD { get; set; }
        public int CG_FORM { get; set; }
        public string PROCESO { get; set; }
        public string CG_CELDA { get; set; }
        public int CG_ORDFORIG { get; set; }
        public int ULTIMAORDENASOCIADA { get; set; }
        public int CG_ORDFASOC { get; set; }
        public decimal CANT { get; set; }
        public int CG_ESTADOCARGA { get; set; }
        public decimal CANTFAB { get; set; }
        public decimal AVANCE { get; set; }
        public decimal DIASFAB { get; set; }
        public decimal HORASFAB { get; set; }
        public bool EXIGEOA { get; set; }
        public int PEDIDO { get; set; }
        public DateTime FECHA_PREVISTA_FABRICACION { get; set; }
        public DateTime FECHA_INICIO_REAL_FABRICACION { get; set; }
        public DateTime FE_CIERRE { get; set; }
        //public int CG_OPER { get; set; }
        //public string DES_OPER { get; set; }
    }
}
