using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeekBurger.Ingredients.Services.Implementation;
using GeekBurger.Ingredients.Services.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GeekBurger.Ingredients
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

            services.AddHttpClient<IProductService, ProductService>(client =>
            {
                client.BaseAddress = new Uri(Configuration["ProductsUrl"]);
            });

            services.AddSingleton<ILabelImaggeAddedService, LabelImageAddedService>();
            services.AddSingleton<IProductChangedService, ProductChangedService>();
            services.AddSwaggerGen();
            //services.AddScoped<IProductService, ProductService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "GeekBurger.Ingredients V1");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            var productChangedService = app.ApplicationServices.GetService<IProductChangedService>();
            productChangedService.ReceiveMessages();

            var labelImaggeAddedService = app.ApplicationServices.GetService<ILabelImaggeAddedService>();
            labelImaggeAddedService.ReceiveMessages();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
