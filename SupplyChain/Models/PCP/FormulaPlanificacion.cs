using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SupplyChain.Shared.Models
{
    public class FormulaPlanificacion
    {
        [Key]
        public int CG_FORM { get; set; } = 0;
        public string DES_FORM { get; set; } = "";
        public string ACTIVO { get; set; } = "";
        public int REVISION { get; set; } = 0;
    }
}