using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SupplyChain.Shared.Models
{
    public class ModeloPendientesFabricar
    {
        [Key]
        public decimal? REGISTRO { get; set; } = 0;
        public decimal? PEDIDO { get; set; } = 0;
        public string CG_ART { get; set; } = "";
        public string DES_ART { get; set; } = "";
        public decimal? PREVISION { get; set; } = 0;
        public decimal? CANTPED { get; set; } = 0;
        public decimal? CALCULADO { get; set; } = 0;
        public decimal? CANTEMITIR { get; set; } = 0;
        public decimal? LOPTIMO { get; set; } = 0;
        public decimal? STOCK { get; set; } = 0;
        public decimal? STOCKMIN { get; set; } = 0;
        public int? CG_FORM { get; set; } = 0;
        public decimal? STOCKENT { get; set; } = 0;
        public decimal? COMP_EMITIDAS { get; set; } = 0;
        public string EXIGEOA { get; set; } = "";
    }
}
