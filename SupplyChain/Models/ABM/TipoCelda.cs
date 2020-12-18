using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupplyChain
{
    [Table("TipoCelda")]
	public class TipoCelda
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int CG_TIPOCELDA { get; set; } = 0;
		public string DES_TIPOCELDA { get; set; } = "";
		//cg_cia
	}
}
