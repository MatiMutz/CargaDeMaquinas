using System;
using System.Collections.Generic;
using System.Text;

namespace SupplyChain.Shared.Models
{
    /// <summary>
    /// Especial para las grillas de Devexpress y poder cambiar el nombre de las columna.
    /// </summary>
    public class ColumnaGridViewAtributo: Attribute
    {
        public ColumnaGridViewAtributo()
        {
            
        }

        public string Name {get; set ; }

    }
}
