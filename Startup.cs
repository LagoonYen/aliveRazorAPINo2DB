using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AliveStoreTemplate.Repositories;
using AliveStoreTemplate.Repository;
using AliveStoreTemplate.Service;
using AliveStoreTemplate.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace AliveStoreTemplate
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddControllers();

            //swagger相關
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "PTCG卡牌商城 API",
                    Version = "v1",
                    Description = "提供各式各樣API串接"
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                x.IncludeXmlComments(xmlPath);
            });

            //session相關
            // 注入分散式記憶體快取
            services.AddDistributedMemoryCache();
            // 注入Session
            services.AddSession(options => {
                //session過期時間
                options.IdleTimeout = TimeSpan.FromMinutes(60);
                //設置為true表示前端js等腳本無法讀取cookie,防止了xss攻擊(默認為true)
                options.Cookie.HttpOnly = true;
                //Cookies是必須的(默認為false)
                options.Cookie.IsEssential = true;
            });

            // 注入 HttpContextAccessor
            services.AddHttpContextAccessor();

            //// Service 及 Repository啟用
            services.AddScoped<MemberService, MemberServiceImpl>();
            services.AddScoped<MemberRepository, MemberRepositoryImpl>();
            services.AddScoped<ProductService, ProductServiceImpl>();
            services.AddScoped<ProductRepository, ProductRepositoryImpl>();
            //services.AddScoped<ShopCarService, ShopCarServiceImpl>();
            //services.AddScoped<ShopCarRepository, ShopCarRepositoryImpl>();
            //services.AddScoped<OrderService, OrderServiceImpl>();
            //services.AddScoped<OrderRepository, OrderRepositoryImpl>();

            //services.AddTransient<ShopContext>();
        }

            // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
            public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

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

            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "PTCGSHOP API");
                x.RoutePrefix = String.Empty;
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // 使用Session
            app.UseSession();

            //留意寫Code順序，先執行驗證...
            app.UseAuthentication();
            app.UseAuthorization(); //Controller、Action才能加上 [Authorize] 屬性

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }
    }
}
