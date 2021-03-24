using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SupplyChain
{
    public class ModeloPedidosPendientes
    {
        [Key]
        public int PEDIDO { get; set; } = 0;
        public DateTime FE_MOV { get; set; }
        public decimal CG_CLI { get; set; } = 0;
        public string DES_CLI { get; set; } = "";
        public string CG_ART { get; set; } = "";
        public string DES_ART { get; set; } = "";
        public decimal CANTPED { get; set; } = 0;
        public DateTime ENTRPREV { get; set; }
        public string Obseritem { get; set; } = "";
        public decimal? CG_ESTADOCARGA { get; set; } = 0;
        public string DES_ESTADOCARGA { get; set; } = "";
        public int? CG_ORDF { get; set; } = 0;
        public string CAMPOCOM1 { get; set; } = "";
        public string CAMPOCOM2 { get; set; } = "";
        public string CAMPOCOM3 { get; set; } = "";
        public string CAMPOCOM4 { get; set; } = "";
        public string CAMPOCOM5 { get; set; } = "";
        public string CAMPOCOM6 { get; set; } = "";
        public int Semana { get; set; } = 0;
        public string LOTE { get; set; } = "";
        public decimal REGISTRO_PEDCLI { get; set; } = 0;
    }
}
