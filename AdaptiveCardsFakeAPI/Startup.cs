using AdaptiveCardsFakeAPI.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdaptiveCardsFakeAPI
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

            services.AddControllers();

            #region DB Connection Config
            services.AddDbContext<AppDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            #endregion

            #region services ingection
            services.AddScoped<Services.IAdaptiveCardsService, Services.AdaptiveCardsServices>();
            services.AddScoped<Services.IUserServices, Services.UserServices>();
            services.AddScoped<Helpers.IInteractiveBrowserCredential, Helpers.InteractiveBrowserCredential>();
            #endregion

            #region swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AdaptiveCardsFakeAPI", Version = "v1" });
            });
            #endregion

            services.AddCors(opts => opts.AddPolicy("AllowAny", b => b.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AdaptiveCardsFakeAPI v1"));
            }

            //allow current endpoint to access this api
            string[] origins = new string[] { "http://localhost:4200" };
            app.UseCors(builder => builder.WithOrigins(origins).AllowAnyHeader().AllowAnyMethod());


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
