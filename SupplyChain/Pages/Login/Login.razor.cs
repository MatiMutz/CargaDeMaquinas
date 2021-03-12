using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;
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

        public string TxtType = "password";

        public void ShowPassword()
        {
            if (this.TxtType == "password")
            {
                this.TxtType = "text";
            }
            else
            {
                this.TxtType = "password";
            }
        }

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
                        //await JsRuntime.InvokeVoidAsync("sessionStorage.setItem", "Usuario", xUser.Usuario);

                        //IsDialogLoginVisible = false;
                        //await this.ToastObj.Show(new ToastModel
                        //{
                        //    Title = $"Bienvenid@ {xUser.Usuario}!",
                        //    Content = "Usuario y contraseña correcta.",
                        //    CssClass = "e-toast-success",
                        //    Icon = "e-success toast-icons"
                        //});

                        MyAuthStateProvider.Usuario = usuario;
                        var authState = await MyAuthStateProvider.GetAuthenticationStateAsync();
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
