using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DarkAdminPanel.WebUI.ApiClients.Abstract;
using DarkAdminPanel.WebUI.ApiClients.Concrete;
using DarkAdminPanel.WebUI.Extensions;
using DarkAdminPanel.WebUI.Mapping;
using DarkAdminPanel.WebUI.Middlewares;
using DarkAdminPanel.WebUI.Modules;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace MasterExam.WebUI
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddMemoryCache();
            services.AddSession();
            services.AddHttpContextAccessor();

            // Auto Mapper Configurations
            services.AddSingleton(AutoMapperConfig.CreateMapper());

            //Configure DI for application services
            LogicModule.Load(services);

            //Adding Jwt module
            JwtModule.Load(services, _configuration);

            //Adding HttpClient module
            HttpClientModule.Load(services);

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "node_modules")),
                RequestPath = "/modules"
            });

            app.UseSession();

            //Add JWToken to all incoming HTTP Request Header
            app.UseMiddleware<HttpRequestHeaderMiddleware>();

            //Add StatusCode Page middleware
            app.UseStatusCodePages();
            app.UseStatusCodePages(async context =>
                        {
                            var request =  context.HttpContext.Request;
                            var response = context.HttpContext.Response;

                            if (response.StatusCode == (int)HttpStatusCode.Unauthorized ||
                                response.StatusCode == (int)HttpStatusCode.Forbidden)
                            {
                                var returnUrl = String.Format("/{0}/{1}", request.RouteValues["controller"], request.RouteValues["action"]);

                                context.HttpContext.Session.SetJson("JWToken", null);
                                response.Redirect("/Account/Login?ReturnUrl=" + returnUrl);
                            }
                        }
            );

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "AccountSetting",
                    pattern: "Account/Setting/{userName}",
                     defaults: new { Controller = "Account", Action = "Setting" }
                    );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{Controller}/{Action}/{id?}",
                    defaults: new { Controller = "Account", Action = "Login" }
               );
            });
        }
    }
}
