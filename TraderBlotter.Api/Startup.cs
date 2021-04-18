using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using BatchManager.Services;
using DataAccess.Repository;
using DataAccess.Repository.Data;
using DataAccess.Repository.Infrastructure;
using DataAccess.Repository.Repositories;
using DataAccess.Repository.RepositoryEF;
using DataAccess.Repository.RepositoryEF.IRepositoryEF;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using TraderBlotter.Api.Models.Mapper;

namespace TraderBlotter.Api
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

            services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(Configuration.GetConnectionString("DefaultConnectionString"),
                new MySqlServerVersion(new Version(8, 0, 21)), // use MariaDbServerVersion for MariaDB
                        mySqlOptions => mySqlOptions.CharSetBehavior(CharSetBehavior.NeverAppend)));
            services.AddControllers();
            services.AddScoped<ITradeViewRepository, TradeViewRepository>();
            services.AddScoped<IUserViewRepository, UserViewRepository>();
            services.AddScoped<IRoleViewRepository, RolesViewRepository>();
            services.AddScoped<IConnectionFactory, ConnectionFactory>();
            //services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<object>));
            services.AddScoped<IGenericRepository<TradeView>, GenericRepository<TradeView>>();
            services.AddScoped<IGenericRepository<object>, GenericRepository<object>>();
            services.AddScoped<ITradeViewGenericRepository, TradeViewGenericRepository>();

            //services.AddSingleton<ITradeViewGenericRepository, TradeViewGenericRepository>();
            //services.AddSingleton<IConnectionFactory, ConnectionFactory>();
            //services.AddSingleton<ITradeViewRepository, TradeViewRepository>();
            //services.AddSingleton<IGenericRepository<TradeView>, GenericRepository<TradeView>>();
            services.AddScoped<ITradeViewBseCmRepository, TradeViewBseCmReposiotry>();
            services.AddScoped<ILoadTradeviewData, LoadTradeViewDataBseCm>();

            services.AddAutoMapper(typeof(TraderBlotterMappings));

            services.AddApiVersioning(x =>
            {
                x.DefaultApiVersion = new ApiVersion(1, 0);
                x.AssumeDefaultVersionWhenUnspecified = true;
                x.ReportApiVersions = true;
                x.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("BlotterAPISpec", new OpenApiInfo()
                {
                    Title = "Blotter API",
                    Version = "1",
                    Description = "Blotter API Documentation",
                    Contact = new OpenApiContact
                    {
                        Email = "muniraju021@gmail.com",
                        Name = "Muniraju JAYARAMA",
                        Url = new Uri("http://localhost/8090")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MJ License",
                        Url = new Uri("https://www.linkedin.com/in/muniraj021/")
                    }
                });
                var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var cmlCommentsFullFilePath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
                options.IncludeXmlComments(cmlCommentsFullFilePath);
            });



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddLog4Net();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/BlotterAPISpec/swagger.json", "Blotter API");
                options.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            Task.Run(async () =>
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Configuration.GetSection("BaseUrl").Value);
                    var resp = await client.GetAsync("api/v1/healthcheck");
                }
            });            

        }
    }
}
