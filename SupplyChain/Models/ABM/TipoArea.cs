using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupplyChain
{
    [Table("TipoArea")]
	public class TipoArea
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int CG_TIPOAREA { get; set; } = 0;
		public string DES_TIPOAREA { get; set; } = "";
	}
}
