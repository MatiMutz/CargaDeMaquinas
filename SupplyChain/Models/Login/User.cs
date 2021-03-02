using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SupplyChain
{
    public class Usuarios
    {
        [Key]
        public string Usuario { get; set; } = ""; 
        public int Cg_TipoUsu { get; set; } = 0;
        public string Nombre { get; set; } = "";
        public string Email { get; set; } = "";
        public string Tel_Of { get; set; } = "";
        public string Tel_Mov { get; set; } = "";
        public string Tel_Part { get; set; } = "";
        public string Contras { get; set; } = "";
        public int Derechos { get; set; } = 0;
        public int Cg_Cia { get; set; } = 0;
        public string UltimoPuntoVenta { get; set; } = "";
        public int Cg_Area { get; set; } = 0;
        public bool CG_CUENTRAPI { get; set; } = false;
    }
}
