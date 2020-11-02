using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SupplyChain
{
    public class ModeloOrdenFabricacionEncabezado
    {
        [Key]
        public int CG_ORDF { get; set; }
        public DateTime FECHA_PREVISTA_FABRICACION { get; set; }
        public decimal DIASFAB { get; set; }
        public decimal HORASFAB { get; set; }
        public decimal AVANCE { get; set; }
        public int PEDIDO { get; set; }
        public DateTime ENTRPREV { get; set; }
        public int CG_CLI { get; set; }
        public string DES_CLI { get; set; }
        public string CG_ART { get; set; }
        public string DES_ART { get; set; }
        public decimal CANTPED { get; set; }
        public string TITULO_CAMPOCOM1 { get; set; }
        public string TITULO_CAMPOCOM2 { get; set; }
        public string TITULO_CAMPOCOM3 { get; set; }
        public string TITULO_CAMPOCOM4 { get; set; }
        public string TITULO_CAMPOCOM5 { get; set; }
        public string TITULO_CAMPOCOM6 { get; set; }
        public string CAMPOCOM1 { get; set; }
        public string CAMPOCOM2 { get; set; }
        public string CAMPOCOM3 { get; set; }
        public string CAMPOCOM4 { get; set; }
        public string CAMPOCOM5 { get; set; }
        public string CAMPOCOM6 { get; set; }
        public string OBSERITEM { get; set; }
        public string DIRENT { get; set; }
        public int CG_TRANS { get; set; }
        public string DES_TRANS { get; set; }
        public string DIRTRANS { get; set; }
    }
}
