using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SupplyChain.Shared.Models
{
    [Table("Unidades")]
    public class Unidades
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ColumnaGridViewAtributo (Name = "Unidad")]
        public string UNID { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Descripcion unidad")]
        public string DES_UNID { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Tipo de unidad")]
        public string TIPOUNID { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Codigo de Densidad Basica")]
        public decimal CG_DENBASICA { get; set; } = 0;
        [ColumnaGridViewAtributo(Name = "Código")]
        public int CODIGO { get; set; } = 0;
        [ColumnaGridViewAtributo(Name = "Código de compañía")]
        public int CG_CIA { get; set; } = 0;
    }
}




      