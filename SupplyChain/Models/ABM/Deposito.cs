using SupplyChain.Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupplyChain
{
    [Table("Depos")]
    public class Deposito
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? CG_DEP { get; set; }//
        [Required(ErrorMessage ="Ingresar Deposito")]
        public string DES_DEP { get; set; }//
        public string TIPO_DEP { get; set; }//
      //  public DateTime? FE_DES { get; set; }
     //   public string DIRECCD { get; set; }
       // public string LOCALIDADD { get; set; }
       // public string CONTACTOD { get; set; }
       // public string TELEFONOD { get; set; }
      //  public string FAXD { get; set; }
      //  public bool FABR { get; set; }
        public int CG_CLI { get; set; }//
        public int CG_PROVE { get; set; }//
        public int CG_CIA { get; set; }//
      //  public bool ACTS { get; set; }
        //public int CG_PROV { get; set; }
        //public int CG_VEN { get; set; }
    }
}
