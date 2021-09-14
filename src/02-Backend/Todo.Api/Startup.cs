using System;
using System.IO;
using System.Reflection;
using Mapster;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Todo.Api.Business;
using Todo.Api.Entities;
using Todo.Api.Grpc;
using Todo.Api.Repositories;
using Todo.Core.Dto;
using Todo.Core.Util;

namespace Todo.Api
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
            services.AddGrpcClient<Words.Grpc.Words.WordsClient>(options =>
                options.Address = Configuration.GetServiceUri("words-grpc"));
            services.AddScoped<WordsGrpcService>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Todo Web RESP API",
                    Description = "The API that helps you get things done!",
                    Contact = new OpenApiContact
                    {
                        Name = "Pedro Ignacio Errico",
                        Email = "ignacio.errico@gmail.com",
                        Url = new Uri("https://github.com/ignacioerrico")
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddScoped<ITodoFacade, TodoFacade>();
            services.AddSingleton<ITodoRepository, TodoRepository>();
            services.AddSingleton<IKeywordFinder, KeywordFinder>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo Web REST API v1");
                    c.RoutePrefix = string.Empty;
                });
            }

            TypeAdapterConfig<TodoNote, TodoNoteDto>
                .NewConfig()
                .TwoWays();
            TypeAdapterConfig<TodoNoteForUpdateDto, TodoNote>
                .NewConfig();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
