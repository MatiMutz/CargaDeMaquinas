using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupplyChain.Shared.Models
{
    [Table("TipoCelda")]
	public class TipoCelda
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[ColumnaGridViewAtributo(Name = "Codigo tipo de celda")]
		public int CG_TIPOCELDA { get; set; } = 0;
		[ColumnaGridViewAtributo(Name = "Descripcion tipo de celda")]
		public string DES_TIPOCELDA { get; set; } = "";
		[ColumnaGridViewAtributo(Name = "Codigo de Compañia")]
		public int CG_CIA { get; set; } = 0;
	}
}
