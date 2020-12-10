using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SupplyChain.Shared.Models
{
    [Table("Areas")]
    public class Area
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ColumnaGridViewAtributo (Name = "Codigo Área")]
        public int CG_AREA { get; set; } = 0;
        [ColumnaGridViewAtributo(Name = "Descripcion Area")]
        public string DES_AREA { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Código de tipo de área")]
        public int CG_TIPOAREA { get; set; } = 0;
        [ColumnaGridViewAtributo(Name = "Código de proveedor")]
        public int CG_PROVE { get; set; } = 0;
        [ColumnaGridViewAtributo(Name = "Código de compañía")]
        public int CG_CIA { get; set; } = 0;
    }
}




      