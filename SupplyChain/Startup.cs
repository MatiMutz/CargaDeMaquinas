using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SupplyChain.Auth;
using SupplyChain.HelperService;
using Syncfusion.Blazor;

namespace SupplyChain
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddAuthenticationCore(options =>
            {
                // I believe this may be where the problem is: I can't specify both cookie auth AND JWT auth; it has to be one or the other.
                //options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                //options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                //options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            });
            services.AddRazorPages();
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddServerSideBlazor().AddCircuitOptions(options => { options.DetailedErrors = true; });
            //services.AddScoped<AuthenticationStateProvider, ProveedorAutenticacion>();
            //services.AddScoped<ILoginService, ProveedorAutenticacion>();
            services.AddScoped<ProveedorAutenticacion>();
            services.AddScoped<AuthenticationStateProvider, ProveedorAutenticacion>(
                provider => provider.GetRequiredService<ProveedorAutenticacion>());

            services.AddScoped<ILoginService, ProveedorAutenticacion>(
                provider => provider.GetRequiredService<ProveedorAutenticacion>());
            services.AddSingleton<CustomHttpClient>();
            services.AddSyncfusionBlazor();
            services.AddServerSideBlazor().AddHubOptions(o => { o.MaximumReceiveMessageSize = 102400000; });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDA2NDQ2QDMxMzgyZTM0MmUzMEM0SisvUHFWRDRQeGVidDlVTFRqM2c3cHpvaEtjWi9OMGRsLzNYWEFnRVk9");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");

            });
        }
    }
}
