﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SupplyChain
{
    [Table("Unidades")]
    public class Unidades
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string UNID { get; set; } = "";
        public string DES_UNID { get; set; } = "";
        public string TIPOUNID { get; set; } = "";
        public decimal CG_DENBASICA { get; set; } = 0;
        public bool BASICA { get; set; } = false;
        public int CODIGO { get; set; } = 0;
        //sdasdas
        //sdasdas
        //sdasdas

        //sdasdas
        //sdasdas
        //sdasdas
        //sdasdas
        //borrar xd
        //sdasdas//sdasdas

        //sdasdas
    }
}




      