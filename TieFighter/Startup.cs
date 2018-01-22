using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using TieFighter.Models;
using TieFighter.Services;

namespace TieFighter
{
    public class Startup
    {
        public const string SignedInPolicyName = "signedIn";

        private const string customGoogleSignInSchemeName = "google";
        private const string googleProjectId = "tiefighter-imperialremnant";
        private IHostingEnvironment _hostingEnvironment;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _hostingEnvironment = env;
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

            // Add file services
            var physicalProvider = _hostingEnvironment.ContentRootFileProvider;
            services.AddSingleton<IFileProvider>(physicalProvider);

            // Add Google Datastore Singleton
            var datastoreDb = new TieFighterDatastoreContext(googleProjectId);
            services.AddSingleton<TieFighterDatastoreContext>(datastoreDb);
            IDatastoreEntityAndJsonBinding.DatastoreDbReference = datastoreDb;

            // Add custom string resources service
            var stringResourceConfig = new StringResourceService();
            var result = stringResourceConfig.TrySetConfigFileLocation("C:\\Users\\alexc\\Source\\Repos\\TieFighter\\TieFighter\\wwwroot\\StringResources.xml");
            services.AddSingleton<IStringResourceService, StringResourceService>();
            services.AddSingleton(stringResourceConfig);
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

            app.UseWebSockets();

            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/ws")
                {
                    if (context.WebSockets.IsWebSocketRequest)
                    {
                        WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                        await Echo(context, webSocket);
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                    }
                }
                else
                {
                    await next();
                }

            });

            //// Set up custom content types -associating file extension to MIME type
            //var provider = new FileExtensionContentTypeProvider();
            //// Add new mappings
            //provider.Mappings[".myapp"] = "application/x-msdownload";

            app.UseStaticFiles(new StaticFileOptions()
            {
                ServeUnknownFileTypes = true,
                DefaultContentType = "application/text"
            });

            // FIXME: This is for testing purposes only! Remove later.
            //app.UseDirectoryBrowser();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areaRoute",
                    template: "{area:exists}/{controller}/{action}/{id?}"
                );
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}"
                );
            });
        }

        private async Task Echo(HttpContext context, WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
                await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
    }
}
