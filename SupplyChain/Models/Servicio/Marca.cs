using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupplyChain
{
    [Table("Marcas")]
	public class Marca
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public string MARCA { get; set; } = "";
	}
}
