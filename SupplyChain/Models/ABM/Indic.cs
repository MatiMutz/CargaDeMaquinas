using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SupplyChain
{
    [Table("Indic")]
    public class Indic
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int REGISTRO { get; set; } = 0;//
        
        public decimal VA_INDIC { get; set; } = 0;//
        public decimal VA_COMPRA { get; set; } = 0;//
        public DateTime FE_INDIC { get; set; }//

        public string DES_IND { get; set; } = "";//
        

       
    }
}




      