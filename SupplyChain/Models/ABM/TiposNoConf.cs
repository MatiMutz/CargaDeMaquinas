using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupplyChain
{
    [Table("TiposNoConf")]
    public class TiposNoConf
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Cg_TipoNc { get; set; } = 0;//
        public string Des_TipoNc { get; set; } = "";//
        //cg_cia?
     //   public bool TipoNoconf { get; set; } = false;
     //   public string Origen { get; set; } = "";
     //   public string? Datos { get; set; } = "";
     //   public string Observaciones { get; set; } = "";
   //     public string Nombre_Reporte { get; set; } = "";
     //   public bool Calipro { get; set; } = false;
     //   public decimal Puntos { get; set; } = 0;
     //   public string TipoDemerito { get; set; } = "";
    }
}
