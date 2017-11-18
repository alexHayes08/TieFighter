using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using TieFighter.Models;
using TieFighter.Services;
using TieFighter.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using TieFighterV2.Models;

namespace TieFighter
{
    public class Startup
    {
        public const string signedInPolicyName = "signedIn";
        private const string customGoogleSignInSchemeName = "google";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TieFighterContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<TieFighterContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication()
            .AddOpenIdConnect(customGoogleSignInSchemeName, options =>
            {
                options.Authority = "https://accounts.google.com";
                options.CallbackPath = "/signin-google";
                options.ClientId = Configuration["Authentication:Google:ClientId"];
                options.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                options.Scope.Add("profile");
                options.Scope.Add("email");
                options.Scope.Add("openid");
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    NameClaimType = "name",
                    RoleClaimType = "role",
                    ValidateIssuer = false
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(signedInPolicyName, policy =>
                {
                    policy.AuthenticationSchemes.Add(customGoogleSignInSchemeName);
                    policy.AuthenticationSchemes.Add(new AuthenticationOptions().DefaultAuthenticateScheme);
                    policy.RequireAuthenticatedUser();
                });
            });

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseAuthentication();

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
