using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SupplyChain.Shared.Models
{
    [Table("CateOperarios")]
    public class CatOpe
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ColumnaGridViewAtributo(Name = "Codigo Categoria Operario")]
        public string CG_CATEOP { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Descripcion Operario")]
        public string DES_CATEOP { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Valor por Hora")]
        public decimal VALOR_HORA { get; set; } = 0;
        [ColumnaGridViewAtributo(Name = "Moneda")]
        public string MONEDA { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Codigo de compañia")]
        public int CG_CIA { get; set; } = 0;
        [ColumnaGridViewAtributo(Name = "Usuario")]
        public string USUARIO { get; set; } = "";
    }
}
