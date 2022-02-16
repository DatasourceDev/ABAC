using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABAC.DAL;
using ABAC.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using ABAC.Identity;

namespace ABAC
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
            services.AddControllersWithViews();
            services.AddRazorPages().AddRazorRuntimeCompilation();

            services.AddMvc().AddSessionStateTempDataProvider();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(1);
            });


            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<SpuContext>(options => options.UseSqlServer(connectionString));

            var connectionString2 = Configuration.GetConnectionString("mmsConnection");
            services.AddDbContext<MmsContext>(options => options.UseSqlServer(connectionString2));

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<ILoginServices, LoginServices>();
            services.AddTransient<LoginServices>();

            services.AddScoped<IViewRenderService, ViewRenderService>();
            services.Configure<FormOptions>(options =>
            {
                options.ValueCountLimit = 100000; // 10000 items max
                options.ValueLengthLimit = 1024 * 1024 * 500; // 100MB max len form data
            });
            var portal = Configuration["SystemConf:Portal"];
            if (portal == Portal.admin)
            {
                services.AddMvc().AddRazorPagesOptions(options =>
                {
                    options.Conventions.AddPageRoute("/Auth/Login", "");
                });

                services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
                {
                    options.LoginPath = "/Auth/Login";
                    options.LogoutPath = "/Auth/Logout";
                    options.ExpireTimeSpan = TimeSpan.FromHours(1);
                });


                services.ConfigureApplicationCookie(options =>
                {
                    options.AccessDeniedPath = "/Auth/Login";
                    options.ExpireTimeSpan = TimeSpan.FromHours(1);
                });
            }
            else
            {
                services.AddMvc().AddRazorPagesOptions(options =>
                {
                    options.Conventions.AddPageRoute("/Auth/LoginUser", "");
                });

                services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
                {
                    options.LoginPath = "/Auth/LoginUser";
                    options.LogoutPath = "/Auth/Logout";
                });


                services.ConfigureApplicationCookie(options =>
                {
                    options.AccessDeniedPath = "/Auth/LoginUser";
                });
            }
            services.Configure<SystemConf>(Configuration.GetSection("SystemConf"));
            services.AddScoped<IUserProvider, AdUserProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler("/Home/Error");

            app.UseAdMiddleware();

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<SpuContext>();
                context.Database.EnsureCreated();
                context.EnsureSeedData();
            }
           

            string enUSCulture = "en-US";
            var supportedCultures = new[]
            {
                new CultureInfo("th-TH"),
                new CultureInfo("th"),
            };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(enUSCulture),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            app.UseStaticFiles();

            app.UseSession();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            var portal = Configuration["SystemConf:Portal"];
            if (portal == Portal.admin)
            {
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=auth}/{action=login}/{id?}");
                });
            }
            else
            {
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=auth}/{action=loginuser}/{id?}");
                });
            }
           
        }
    }


    public class SystemConf
    {
        public string Portal { get; set; }
        public string OU_VIP { get; set; }
        public string OU_TEMP { get; set; }
        public string OU_OFFICE { get; set; }
        public string DomainGmail { get; set; }
        public string DomainOffice365 { get; set; }
        public string Env { get; set; }


    }
}
