﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SupplyChain
{
    [Table("Lineas")]
    public class Lineas
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CG_LINEA { get; set; } = 0;
        public string DES_LINEA { get; set; } = "";
        public int CG_CIA { get; set; } = 0;//

        //  public string RESP { get; set; } = "";
        //   public decimal FACTOR { get; set; } = 0;

    }
}
