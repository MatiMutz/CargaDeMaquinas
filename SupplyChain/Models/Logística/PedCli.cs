using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SupplyChain
{
    public class PedCli
    {
        [Key]
        public int REGISTRO { get; set; } = 0; 
        public int PEDIDO { get; set; } = 0;
        public string DES_CLI { get; set; } = "";
        public string ORCO { get; set; } = "";
        public string CG_ART { get; set; } = "";
        public string DES_ART { get; set; } = "";
        public string OBSERITEM { get; set; } = "";
        public string DIRENT { get; set; } = "";
        public string CG_ESTADO { get; set; } = "";
        public int CG_ESTADPEDCLI { get; set; } = 0;
        public string ESTADO_LOGISTICA { get; set; } = "";

    }
}
