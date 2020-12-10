using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupplyChain.Shared.Models
{
    [Table("TipoArea")]
	public class TipoArea
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[ColumnaGridViewAtributo(Name = "Codigo tipo de area")]
		public int CG_TIPOAREA { get; set; } = 0;
		[ColumnaGridViewAtributo(Name = "Descripcion tipo de area")]
		public string DES_TIPOAREA { get; set; } = "";
	}
}
