using System;
using Community.Data.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json.Serialization;
using Community.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Community.Web
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
            
            // ** Add Cookie Authentication via extension method **
            //services.AddCookieAuthentication();
            // add a DatabaseContext instance to dependency injection system  
            services.AddDbContext<DatabaseContext>();
            //options => {
            //options.UseMySQL(Configuration.GetConnectionString("DataConnection"));
            //}
            //);

            // ** Add Cookie and Jwt Authentication via extension method **
            services.AddCookieAndJwtAuthentication(Configuration);

            // ** Enable Cors for and webapi endpoints provided **
            services.AddCors();
            
            // Add Services to DI        
            services.AddTransient<IUserService,UserServiceDb>();
            services.AddTransient<IBusinessService,BusinessServiceDb>();
            services.AddTransient<IPhotoService,PhotoServiceDb>();
            services.AddTransient<IPostService,PostServiceDb>();
            services.AddTransient<INewsService,NewsServiceDb>();
            services.AddTransient<IBookingService,BookingServiceDb>();

            // ** Required to enable asp-authorize Taghelper **            
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider provider)
        {
            if (env.IsDevelopment())
            {
                // seed - using service provider to get Services from DI
                Seeder.Seed(provider.GetService<IUserService>(), provider.GetService<IBusinessService>(),
                provider.GetService<IPhotoService>(), provider.GetService<IPostService>(), provider.GetService<INewsService>(),
                provider.GetService<IBookingService>());
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // ** configure cors to allow full cross origin access to any webapi end points **
            app.UseCors(c => c.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            // ** turn on authentication/authorisation **
            app.UseAuthentication();
            app.UseAuthorization();
 
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
