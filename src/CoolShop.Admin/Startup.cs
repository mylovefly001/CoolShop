using System;
using System.IO;
using CoolShop.Core.Extend;
using CoolShop.Core.Library;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;

namespace CoolShop.Admin
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddRazorRuntimeCompilation().AddNewtonsoftJson(options =>
            {
                //格式化日期
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                //首字母小写
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }).AddMvcOptions(options => { options.Filters.Add<Filter>(); });
            services.AddSession(options => { options.IdleTimeout = TimeSpan.FromMinutes(60); });
            services.AddSingleton<DbContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseSession();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Environment.CurrentDirectory, "Assets"))
            });

            app.UseEndpoints(endpoints => { endpoints.MapControllerRoute("default", "{controller=Index}/{action=Index}"); });
        }
    }
}