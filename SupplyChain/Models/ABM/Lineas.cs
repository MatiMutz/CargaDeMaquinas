using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SupplyChain.Shared.Models
{
    [Table("Lineas")]
    public class Lineas
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ColumnaGridViewAtributo(Name = "Codigo Linea")]
        public int CG_LINEA { get; set; } = 0;
        [ColumnaGridViewAtributo(Name = "Descripcion Linea")]
        public string DES_LINEA { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Codigo compañia")]
        public int CG_CIA { get; set; } = 0;
    }
}
