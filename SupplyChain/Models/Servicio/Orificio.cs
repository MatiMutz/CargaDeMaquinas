using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupplyChain
{
    [Table("CAMPOCOM3")]
    public class Orificio
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } = 0;
        public string Codigo { get; set; } = "";
        public string Descripcion { get; set; } = "";
        public int CG_ORDEN { get; set; } = 0;
    }
}
