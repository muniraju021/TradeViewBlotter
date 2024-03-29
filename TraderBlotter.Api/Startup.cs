using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using BatchManager.Services;
using DataAccess.Repository;
using DataAccess.Repository.Data;
using DataAccess.Repository.Infrastructure;
using DataAccess.Repository.Repositories;
using DataAccess.Repository.RepositoryEF;
using DataAccess.Repository.RepositoryEF.IRepositoryEF;
using DataAccess.Repository.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using TraderBlotter.Api.ConfigurationFilters;
using TraderBlotter.Api.Models.Dto;
using TraderBlotter.Api.Models.Mapper;

namespace TraderBlotter.Api
{
    public class Startup
    {
        public static IAutoSyncService _autoSyncservice = null;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddResponseCompression();
            services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(Configuration.GetConnectionString("DefaultConnectionString"),
                new MySqlServerVersion(new Version(8, 0, 21)))); // use MariaDbServerVersion for MariaDB
                        //mySqlOptions => mySqlOptions.cha(CharSet.NeverAppend)));
            services.AddControllers();
            services.AddScoped<ITradeViewRepository, TradeViewRepository>();
            services.AddScoped<IUserViewRepository, UserViewRepository>();
            services.AddScoped<IRoleViewRepository, RolesViewRepository>();
            services.AddScoped<IConnectionFactory, ConnectionFactory>();
            //services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<object>));
            services.AddScoped<IGenericRepository<TradeView>, GenericRepository<TradeView>>();
            services.AddScoped<IGenericRepository<object>, GenericRepository<object>>();
            services.AddScoped<ITradeViewGenericRepository, TradeViewGenericRepository>();
            services.AddScoped<ITradeViewRefRepository, TradeViewRefRepository>();

            services.AddScoped<ITradeViewBseCmRepository, TradeViewBseCmReposiotry>();
            services.AddScoped<ITradeViewNseFoRepository, TradeViewNseFoRepository>();      
            services.AddScoped<ITradeViewNseCmRepository, TradeViewNseCmReposiotry>();


            services.AddScoped<ILoadTradeviewData, LoadTradeViewDataBseCm>();
            services.AddScoped<ILoadTradeviewDataNseFo, LoadTradeviewDataNseFo>();
            services.AddScoped<ILoadTradeviewDataNseCm, LoadTradeViewDataNseCm>();

            services.AddScoped<IAutoSyncService, AutoSyncService>();

            services.AddScoped<IDealerClientMappingRepository, DealerClientMappingRepository>();
            services.AddScoped<IGroupDealerMappingRepository, GroupDealerMappingRepository>();
            services.AddScoped<ICurrentExprityDateRefRepository, CurrentExprityDateRefRepository>();

            services.AddScoped<IFileHelper, FileHelper>();
            services.AddScoped<IGreekNseCmRepository, GreekNseCmRepository>();
            services.AddScoped<IGreekNseFoRepository, GreekNseFoRepository>();
            services.AddScoped<IGreekBseCmRepository, GreekBseCmRepository>();

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

            //Add Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    //ValidAudience = Configuration["JWT:ValidAudience"],
                    //ValidIssuer = Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                };
            });



            services.AddMvc(config =>
            {
                //config.Filters.Add(typeof(CustomExceptionFilter));
                config.Filters.Add(typeof(CustomAuthorizationFilter));
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, Microsoft.Extensions.Hosting.IHostApplicationLifetime applicationLifetime)
        {
            loggerFactory.AddLog4Net();

            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseMiddleware<LoggingMiddleware>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

            app.UseResponseCompression();
            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/BlotterAPISpec/swagger.json", "Blotter API");
                options.RoutePrefix = string.Empty;
            });

            //app.UseExceptionHandler(errorApp =>
            //{
            //    errorApp.Run(async context =>
            //    {
            //        context.Response.StatusCode = 500; // or another Status accordingly to Exception Type
            //        context.Response.ContentType = "application/json";

            //        var error = context.Features.Get<IExceptionHandlerFeature>();
            //        if (error != null)
            //        {
            //            var ex = error.Error;

            //            await context.Response.WriteAsync(new ErrorModel()
            //            {
            //                HttpStatusCode = 500,
            //                Message = ex.Message
            //            }.ToString(), Encoding.UTF8);
            //        }
            //    });
            //});

            

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            //Task.Run(async () =>
            //{
            //    using (var client = new HttpClient())
            //    {
            //        client.BaseAddress = new Uri(Configuration.GetSection("BaseUrl").Value);
            //        var resp = await client.GetAsync("api/v1/healthcheck");
            //    }
            //});

            var scope = app.ApplicationServices.CreateScope();
            _autoSyncservice = scope.ServiceProvider.GetService<IAutoSyncService>();
            _autoSyncservice.StartAutoSyncFromSource();
                    
            applicationLifetime.ApplicationStopping.Register(OnShutDown);

        }

        private void OnShutDown()
        {
            LoadTradeViewDataBseCm.isSyncDataStarted = false;
        }
    }
}
