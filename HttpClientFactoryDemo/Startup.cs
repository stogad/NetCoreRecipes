﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HttpClientFactoryDemo.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace HttpClientFactoryDemo
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
            services.AddScoped<IUserService, UserServiceWithBasicClientUsage>();
            services.AddScoped<IPhotoService, PhotoServiceWithNamedClient>();

           // services.AddHttpClient();
            services.AddHttpClient("photos", c =>
            {
                c.BaseAddress = new Uri("https://jsonplaceholder.typicode.com");
                c.DefaultRequestHeaders.Add("Accept", "application/json");
                c.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample");
            });
            services.AddHttpClient<IAlbumService, AlbumServiceWithTypedClient>(options =>
            {
                options.Timeout = TimeSpan.FromSeconds(5);
            });
;

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new Info
                    {
                        Contact = new Contact { Email = "xxx@o2.pl", Name = "DawidS", Url = "http://google.pl" },
                        Description = "Example API",
                        License = new License { Name = "Licence name" },
                        Title = "My API",
                        Version = "Version 1"
                    });
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Demo API");
            });
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
