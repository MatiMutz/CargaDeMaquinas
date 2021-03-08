using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using SupplyChain.HelperService;

namespace SupplyChain.Auth
{
    public class ProveedorAutenticacion : AuthenticationStateProvider, ILoginService
    {

        public static readonly string USER = "Usuario";

        private Task<AuthenticationState> _authenticationStateTask;

        public bool IsAuthenticated { get; set; } = true;
        public bool IsAuthenticating { get; set; }
        public Usuarios Usuario { get; set; }
        private AuthenticationState Anonimo =>
            new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));


        

        public async Task<AuthenticationState> ConstruirAuthenticationStateAsync()
        {

            ClaimsIdentity identity;
            if (IsAuthenticating)
            {
                return null;
            }
            else if (IsAuthenticated)
            {
                identity = new ClaimsIdentity(new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, Usuario.Usuario),
                            new Claim(ClaimTypes.Role, "Administrador")
                        }, "testing");
            }
            else
            {
                identity = new ClaimsIdentity();
            }

            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));
        }

        public void NotifyAuthenticationStateChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task Login(Usuarios userToken)
        {
            Usuario = userToken;
            var authState = ConstruirAuthenticationStateAsync();
            SetAuthenticationState(await Task.FromResult(authState));
        }

        public async Task Logout()
        {
            await Limpiar();
            NotifyAuthenticationStateChanged(Task.FromResult(Anonimo));
        }

        private async Task Limpiar()
        {

        }


        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            _authenticationStateTask = ConstruirAuthenticationStateAsync();
            return _authenticationStateTask
                  ?? throw new InvalidOperationException($"{nameof(GetAuthenticationStateAsync)} was called before {nameof(SetAuthenticationState)}.");
        }

        public void SetAuthenticationState(Task<AuthenticationState> authenticationStateTask)
        {
            _authenticationStateTask = authenticationStateTask ?? throw new ArgumentNullException(nameof(authenticationStateTask));
            NotifyAuthenticationStateChanged(_authenticationStateTask);
        }
    }
}
