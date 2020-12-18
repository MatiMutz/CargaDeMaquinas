using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SupplyChain;
using Syncfusion.Blazor.FileManager;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;
using Syncfusion.Blazor.RichTextEditor.Internal;
using Syncfusion.ExcelExport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System.IO;
using Syncfusion.Pdf.Grid;
using Syncfusion.Blazor.Diagrams;
using Syncfusion.Pdf.Tables;
using System.Data;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Drawing;
using Microsoft.AspNetCore.Http;

namespace SupplyChain.Pages.Logins
{
    public class LoginPageBase : ComponentBase
    {
        [Inject] protected CustomHttpClient Http { get; set; }
        protected Usuarios usuarios = new Usuarios();
        protected override async Task OnInitializedAsync()
        {
            usuarios = await Http.GetFromJsonAsync<Usuarios>("api/Usuarios");

            await base.OnInitializedAsync();
        }
        protected void OnValidSubmit()
        {
        }
    }
}
