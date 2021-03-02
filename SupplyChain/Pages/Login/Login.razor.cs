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
using SupplyChain.HelperService;
using SupplyChain.Auth;

namespace SupplyChain.Pages.Logins
{
    public class LoginPageBase : ComponentBase
    {
        [Inject] protected CustomHttpClient Http { get; set; }
        [Inject] ILoginService loginService { get; set; }
        [Inject] IJSRuntime JsRuntime { get; set; }
        [Inject] NavigationManager NavManager { get; set; }
        [Inject] ProveedorAutenticacion MyAuthStateProvider { get; set; }
        protected Usuarios usuario = new Usuarios();

        protected async Task LoginDialogOk(Object args)
        {
            try
            {
                if (usuario.Usuario != "" && usuario.Contras != "")
                {
                    //await SpinnerLogin.Show();

                    Usuarios xUser = await Http.GetFromJsonAsync<Usuarios>($"api/Usuarios/{usuario.Usuario}/{usuario.Contras}");
                    if (xUser != null)
                    {
                        await JsRuntime.InvokeVoidAsync("sessionStorage.setItem", "Usuario", xUser.Usuario);

                        //IsDialogLoginVisible = false;
                        //await this.ToastObj.Show(new ToastModel
                        //{
                        //    Title = $"Bienvenid@ {xUser.Usuario}!",
                        //    Content = "Usuario y contraseña correcta.",
                        //    CssClass = "e-toast-success",
                        //    Icon = "e-success toast-icons"
                        //});

                        await loginService.Login(xUser);
                        NavManager.NavigateTo("/index");
                    }
                    else
                    {
                        //await this.ToastObj.Show(new ToastModel
                        //{
                        //    Title = "Error!",
                        //    Content = "Usuario o contraseña incorrecta.",
                        //    CssClass = "e-toast-danger",
                        //    Icon = "e-error toast-icons"
                        //});
                    }

                    //await SpinnerLogin.Hide();
                }
                else
                {
                    //await this.ToastObj.Show(new ToastModel
                    //{
                    //    Title = "Aviso!",
                    //    Content = "Debe indicar usuario y contraseña.",
                    //    CssClass = "e-toast-warning",
                    //    Icon = "e-warning toast-icons"
                    //});
                }
            }
            catch
            {
                throw;
            }
        }

        protected void UpdateAuthentication(bool? isAuthenticated)
        {
            if (!isAuthenticated.HasValue)
            {
                MyAuthStateProvider.IsAuthenticating = true;
            }
            else
            {
                MyAuthStateProvider.IsAuthenticating = false;
                MyAuthStateProvider.Usuario = usuario;
                MyAuthStateProvider.IsAuthenticated = isAuthenticated.Value;
            }

            MyAuthStateProvider.NotifyAuthenticationStateChanged();
        }

        protected void OnValidSubmit()
        {
        }
    }
}
