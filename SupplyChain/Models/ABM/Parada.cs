using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SupplyChain
{
    [Table("Paradas")]
    public class Parada
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal CP { get; set; } = 0;//
        public string DESCRIP { get; set; } = "";//
        public int CG_CIA { get; set; } = 0;//
     
    }
}




      