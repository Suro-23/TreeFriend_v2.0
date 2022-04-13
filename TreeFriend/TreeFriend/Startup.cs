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

            //�q�պAŪ���n�J�O�ɳ]�w
            //double loginExpireMinute = this.Configuration.GetValue<double>("LoginExpireMinute");
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => {
                    //options.AccessDeniedPath = "/Home/AccessDeny";//�ڵ��n�J �v������
                    //options.LoginPath = "/Register/Create";//�n�J����
                    //options.LogoutPath = "/Register/Create";//�n�X����
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Register/Create");
                    //options.ExpireTimeSpan = TimeSpan.FromMinutes(loginExpireMinute);//�w�]14��
                    //options.SlidingExpiration = false;
                });

            services.AddControllersWithViews();
            //�[�J�s�u�r��
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

            app.UseAuthentication();//����

            //app.UseMvcWithDefaultRoute();

            app.UseAuthorization();//���v

            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=HomePage}");
            });
        }
    }
}