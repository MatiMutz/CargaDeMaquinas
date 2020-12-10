using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupplyChain.Shared.Models
{
    [Table("TiposNoConf")]
    public class TiposNoConf
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ColumnaGridViewAtributo(Name = "Codigo tipo no Conforme")]
        public int Cg_TipoNc { get; set; } = 0;
        [ColumnaGridViewAtributo(Name = "Descripcion tipo no conforme")]
        public string Des_TipoNc { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Codigo de compañia")]
        public int Cg_cia { get; set; } = 0;
    }
}
