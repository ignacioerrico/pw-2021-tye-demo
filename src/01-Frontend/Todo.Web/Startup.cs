using Mapster;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Todo.Core.Dto;
using Todo.Core.Util;
using Todo.Web.Business;
using Todo.Web.Business.Models;
using Todo.Web.Entities;

namespace Todo.Web
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

            services.AddHttpClient<TodoHttpClient>(httpClient =>
            {
                httpClient.BaseAddress = Configuration.GetServiceUri("todo-api");
            });

            services.AddScoped<IIndexFacade, IndexFacade>();
            services.AddScoped<IAddFacade, AddFacade>();
            services.AddScoped<IStatsFacade, StatsFacade>();

            services.AddSingleton<IKeywordFinder, KeywordFinder>();
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

            TypeAdapterConfig<TodoNote, TodoNoteDto>
                .NewConfig()
                .TwoWays();
            TypeAdapterConfig<TodoNote, TodoNoteForUpdateDto>
                .NewConfig();
            TypeAdapterConfig<StatsDto, StatsModel>
                .NewConfig();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
