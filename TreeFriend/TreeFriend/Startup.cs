using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TreeFriend.Models;

namespace TreeFriend {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {

            //從組態讀取登入逾時設定
            //double loginExpireMinute = this.Configuration.GetValue<double>("LoginExpireMinute");
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => {
                    //options.AccessDeniedPath = "/Home/AccessDeny";//拒絕登入 權限不足
                    //options.LoginPath = "/Register/Create";//登入頁面
                    //options.LogoutPath = "/Register/Create";//登出頁面
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Register/Create");
                    //options.ExpireTimeSpan = TimeSpan.FromMinutes(loginExpireMinute);//預設14天
                    //options.SlidingExpiration = false;
                });

            services.AddControllersWithViews();
            //加入連線字串
            services.AddDbContext<TreeFriendDbContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("TreeFriend")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCookiePolicy();

            app.UseAuthentication();//驗證

            //app.UseMvcWithDefaultRoute();

            app.UseAuthorization();//授權

            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=HomePage}");
            });
        }
    }
}
