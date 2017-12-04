using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using TieFighter.Models;
using TieFighter.Services;

namespace TieFighter
{
    public class Startup
    {
        public static readonly TieFighterDatastoreContext DatastoreDb = new TieFighterDatastoreContext(googleProjectId);

        public const string SignedInPolicyName = "signedIn";
        private const string customGoogleSignInSchemeName = "google";
        private const string googleProjectId = "tiefighter-imperialremnant";

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
                options.AddPolicy(SignedInPolicyName, policy =>
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

            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
                ReceiveBufferSize = 4 * 1024
            };
            app.UseWebSockets(webSocketOptions);

            // Set up custom content types -associating file extension to MIME type
            var provider = new FileExtensionContentTypeProvider();
            // Add new mappings
            provider.Mappings[".myapp"] = "application/x-msdownload";

            app.UseStaticFiles(new StaticFileOptions()
            {
                ServeUnknownFileTypes = true,
                DefaultContentType = "application/text"
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "stringId",
                    template: "{area:exists}/{controller}/{action}/{id?}",
                    defaults: new { area = "Admin", controller = "Home", action = "Index"}
                );
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}"
                );
            });
        }
    }
}
