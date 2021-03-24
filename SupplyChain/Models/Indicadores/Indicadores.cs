using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SupplyChain
{
	public class Indicadores
	{
		[Key]
		public int? Pedido { get; set; } = 0;
		public int? Presup { get; set; } = 0;
		public DateTime? FechaPed { get; set; }
		public int? CodCliente { get; set; } = 0;
		public string RazonSoc { get; set; } = "";
		public string DES_CATEG { get; set; } = "";
		public string CodArt { get; set; } = "";
		public string DescripcionArt { get; set; } = "";
		public decimal? Cantped { get; set; } = 0;
		public decimal? CantEnt { get; set; } = 0;
		public decimal? PesosPresupuesto { get; set; } = 0;
		public decimal? DolaresPresupuesto { get; set; } = 0;
		public decimal? PesosPedido { get; set; } = 0;
		public decimal? DolaresPedido { get; set; } = 0;
		public int? OCinterna { get; set; } = 0;
		public DateTime? FechaPrev { get; set; }
		public string Dirent { get; set; } = "";
		public string OrdenCcompra { get; set; } = "";
		public decimal? TC { get; set; } = 0;
		public DateTime? Fe_altaPT { get; set; }
		public string Obseritem { get; set; } = "";
		public string Presion { get; set; } = "";
		public string Resorte { get; set; } = "";
		public string Fluido { get; set; } = "";
		public string PresionenBanco { get; set; } = "";
		public string Temperatura { get; set; } = "";
		public string Contrapresion { get; set; } = "";
		public DateTime? FechaArmado { get; set; }
		public string RemitoVentas { get; set; } = "";
		public int? Semana { get; set; } = 0;
		public bool Confirmado { get; set; } = false;
		public DateTime? FechaConfirmacion { get; set; }
		public decimal? TC_FechaPedido { get; set; } = 0;
		public decimal? TC_FechaPresupuesto { get; set; } = 0;
		public int? MesFactura { get; set; } = 0;
		public string ConFactu { get; set; } = "";
		public string ConPed { get; set; } = "";
		public int? MesPedido { get; set; } = 0;
		public decimal? UnidEqui { get; set; } = 0;
		public int? DiasAtraso { get; set; } = 0;
		public int? DiasDemoraEntrega { get; set; } = 0;
		public int? DiasParaFabricar { get; set; } = 0;
		public string ConRem { get; set; } = "";
		public string Presup_des_obra { get; set; } = "";
		public string Pedido_des_obra { get; set; } = "";
		public DateTime? ENTRREAL { get; set; }
	}
}