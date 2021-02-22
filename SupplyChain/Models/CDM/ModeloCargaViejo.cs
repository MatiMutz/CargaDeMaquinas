using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SupplyChain
{
    public class ModeloCargaViejo
    {
        [Key]
        public int CG_ORDF { get; set; }
        public int CG_ORDEN { get; set; }
        public int CG_ESTADOCARGA { get; set; }
        public int CG_ORDFORIG { get; set; }
        public int CG_ORDFASOC { get; set; }
        public string CG_CELDA { get; set; }
        public string DES_CELDA { get; set; }
        public string CG_PROD { get; set; }
        public string DES_PROD { get; set; }
        public string PROCESO { get; set; }
        public int CG_CLI { get; set; }
        public string DES_CLI { get; set; }
        public DateTime FECHA_PREVISTA_FABRICACION { get; set; }
        public string HORA { get; set; }
        public decimal HORASFAB { get; set; }
        public decimal MINUTOSFAB { get; set; }
        public DateTime FE_ENTREGA { get; set; }
        public decimal DIASFAB { get; set; }
        public decimal CANT { get; set; }
        public decimal CANTFAB { get; set; }
        public decimal AVANCE { get; set; }
        public string BACKGROUND { get; set; }
        public string CLASE { get; set; }
        public bool INSUMOS_ENTREGADOS_A_PLANTA { get; set; }
        public int COLUMNA { get; set; }
        public int COLUMNSPAN { get; set; }
        public int FLAG_DEPENDENCIAS { get; set; }
        public bool EXIGEOA { get; set; }
        public int PEDIDO { get; set; }
    }
}
