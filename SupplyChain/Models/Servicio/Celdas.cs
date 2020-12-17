using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SupplyChain
{
    [Table("Celdas")]
    public class Celdas
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string CG_CELDA { get; set; } = "";
        public string DES_CELDA { get; set; } = "";
        public int CG_AREA { get; set; } = 0;
        public int CG_SERIE { get; set; } = 0;
        public bool ILIMITADA { get; set; } = false;
        public string RESP { get; set; } = "";
        public string CONTROLES { get; set; } = "";
        public decimal TS1 { get; set; } = 0;
        public decimal TS2 { get; set; } = 0;
        public decimal COEFI { get; set; } = 0;
        public string CG_PROD { get; set; } = "";
        public decimal CANT { get; set; } = 0;
        public int TIEMPOFAB { get; set; } = 0;
        public DateTime FE_INICIAL { get; set; }
        public DateTime FE_REG { get; set; }
        public int HS_INICIAL { get; set; } = 0;
        public DateTime FE_FINAL { get; set; }
        public int HS_FINAL { get; set; } = 0;
        public string OBSERVAC { get; set; } = "";
        public int CG_OPER { get; set; } = 0;
        public int CG_CLI { get; set; } = 0;
        public int CG_PROVE { get; set; } = 0;
        public int CG_COS { get; set; } = 0;
        public int CG_PREDIO { get; set; } = 0;
        public decimal VALOR_AMOR { get; set; } = 0;
        public decimal VALOR_MERC { get; set; } = 0;
        public string MONEDA { get; set; } = "";
        public decimal CANT_ANOS { get; set; } = 0;
        public decimal CANT_UNID { get; set; } = 0;
        public decimal REP_ANOS { get; set; } = 0;
        public decimal M2 { get; set; } = 0;
        public decimal ENERGIA { get; set; } = 0;
        public decimal COMBUST { get; set; } = 0;
        public decimal AIRE_COMP { get; set; } = 0;
        public decimal SETUP_OPT { get; set; } = 0;
        public bool NOMAQ { get; set; } = false;
        public bool PLC { get; set; } = false;
        public int CG_TIPOCELDA { get; set; } = 0;
        public int CG_DEPOSM { get; set; } = 0;
        public bool ACT_PRODIN { get; set; } = false;
        public int CG_OPER_ACTUAL { get; set; } = 0;
        public int CG_SUPER_ACTUAL { get; set; } = 0;
        public int ULTIMO_REGISTRO_TRANSFERIDO_PLC { get; set; } = 0;
    }
}   