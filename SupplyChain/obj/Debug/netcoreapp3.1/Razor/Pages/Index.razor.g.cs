#pragma checksum "E:\ProyectoSupplychain\CargaDeMaquinas\SupplyChain\Pages\Index.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "47ec873f2206854a393928ac391d009798d80056"
// <auto-generated/>
#pragma warning disable 1591
namespace SupplyChain.Pages
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "E:\ProyectoSupplychain\CargaDeMaquinas\SupplyChain\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "E:\ProyectoSupplychain\CargaDeMaquinas\SupplyChain\_Imports.razor"
using Microsoft.AspNetCore.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "E:\ProyectoSupplychain\CargaDeMaquinas\SupplyChain\_Imports.razor"
using Microsoft.AspNetCore.Components.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "E:\ProyectoSupplychain\CargaDeMaquinas\SupplyChain\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "E:\ProyectoSupplychain\CargaDeMaquinas\SupplyChain\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "E:\ProyectoSupplychain\CargaDeMaquinas\SupplyChain\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "E:\ProyectoSupplychain\CargaDeMaquinas\SupplyChain\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "E:\ProyectoSupplychain\CargaDeMaquinas\SupplyChain\_Imports.razor"
using SupplyChain;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "E:\ProyectoSupplychain\CargaDeMaquinas\SupplyChain\_Imports.razor"
using SupplyChain.Shared;

#line default
#line hidden
#nullable disable
    [Microsoft.AspNetCore.Components.RouteAttribute("/")]
    public partial class Index : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.AddMarkupContent(0, "<div class=\"jumbotron jumbotron-fluid\">\r\n    <div class=\"container\">\r\n        <h1 class=\"display-4\">Arbros S.A.</h1>\r\n        <hr>\r\n        <p class=\"lead\">Sistema de Gestión.</p>\r\n        <hr>\r\n    </div>\r\n</div>\r\n\r\n");
            __builder.AddMarkupContent(1, "<div class=\"row\">\r\n    \r\n    <div class=\"col-md-6 mb-4\">\r\n        \r\n        <div class=\"card gradient-card\">\r\n            <div class=\"card-image\">\r\n                \r\n                <a href=\"PageCargaMaquina\">\r\n                    <div class=\"text-white d-flex h-100 mask blue-gradient-rgba\">\r\n                        <div class=\"first-content align-self-center p-3\">\r\n                            <h3 class=\"card-title\" style=\"color:blue; font-weight:bold;text-shadow: 0 0 3px #F0FFFF, 0 0 5px #F0FFFF;\">Carga de Máquinas</h3>\r\n                            <p class=\"lead mb-0\" style=\"color:blue; font-weight:bold;text-shadow: 0 0 3px #F0FFFF, 0 0 5px #F0FFFF;\">Ingresar al Módulo</p>\r\n                        </div>\r\n                        <div class=\"second-content align-self-center mx-auto text-center\">\r\n                            <i class=\"far fa-money-bill-alt fa-3x\"></i>\r\n                        </div>\r\n                    </div>\r\n                </a>\r\n            </div>\r\n        </div>\r\n        \r\n    </div>\r\n    \r\n    \r\n    <div class=\"col-md-6 mb-4\">\r\n        \r\n        <div class=\"card gradient-card\">\r\n            <div class=\"card-image\">\r\n                \r\n                <a href=\"kanban\">\r\n                    <div class=\"text-white d-flex h-100 mask purple-gradient-rgba\">\r\n                        <div class=\"first-content align-self-center p-3\">\r\n                            <h3 class=\"card-title\" style=\"color:blue; font-weight:bold;text-shadow: 0 0 3px #F0FFFF, 0 0 5px #F0FFFF;\">Logística</h3>\r\n                            <p class=\"lead mb-0\" style=\"color:blue; font-weight:bold;text-shadow: 0 0 3px #F0FFFF, 0 0 5px #F0FFFF;\">Ingresar al Módulo</p>\r\n                        </div>\r\n                        <div class=\"second-content  align-self-center mx-auto text-center\">\r\n                            <i class=\"fas fa-chart-line fa-3x\"></i>\r\n                        </div>\r\n                    </div>\r\n                </a>\r\n            </div>\r\n        </div>\r\n        \r\n    </div>\r\n    \r\n    \r\n    <div class=\"col-md-6 mb-4\">\r\n        \r\n        <div class=\"card gradient-card\">\r\n            <div class=\"card-image\">\r\n                \r\n                <a href=\"servicio/list\">\r\n                    <div class=\"text-white d-flex h-100 mask peach-gradient-rgba\">\r\n                        <div class=\"first-content align-self-center p-3\">\r\n                            <h3 class=\"card-title\" style=\"color:blue; font-weight:bold;text-shadow: 0 0 3px #F0FFFF, 0 0 5px #F0FFFF;\">Servicios - Reparaciones</h3>\r\n                            <p class=\"lead mb-0\" style=\"color:blue; font-weight:bold;text-shadow: 0 0 3px #F0FFFF, 0 0 5px #F0FFFF;\">Ingresar al Módulo</p>\r\n                        </div>\r\n                        <div class=\"second-content  align-self-center mx-auto text-center\">\r\n                            <i class=\"fas fa-chart-pie fa-3x\"></i>\r\n                        </div>\r\n                    </div>\r\n                </a>\r\n            </div>\r\n        </div>\r\n        \r\n    </div>\r\n    \r\n</div>");
        }
        #pragma warning restore 1998
    }
}
#pragma warning restore 1591
