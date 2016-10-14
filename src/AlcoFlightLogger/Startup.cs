using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AlcoFlightLogger.Data;
using AlcoFlightLogger.Models;
using AlcoFlightLogger.Models.FlightViewModels;
using AlcoFlightLogger.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using Microsoft.AspNetCore.Mvc;

namespace AlcoFlightLogger
{
    public class Startup
    {
        private IHostingEnvironment env;
        public Startup(IHostingEnvironment env)
        {
            this.env = env;
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.private.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<Pilot, IdentityRole<int>>(config =>
            {
                config.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents()
                {
                    OnRedirectToLogin = ctx =>
                    {
                        if (ctx.Request.Path.StartsWithSegments("/api") &&
                            ctx.Response.StatusCode == 200)
                        {
                            ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        }
                        else
                        {
                            ctx.Response.Redirect(ctx.RedirectUri);
                        }

                        return Task.FromResult(0);
                    }
                };
            })
                .AddEntityFrameworkStores<ApplicationDbContext, int>()
                .AddDefaultTokenProviders();

            services.AddMvc()
                .AddJsonOptions(opt =>
                {
                    opt.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind;
                    opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });
            services.AddScoped<IFlightsRepository, FlightsRepository>();

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            //TODO: uncomment when ssl sorted out

            //Add ssl
            if (this.env.IsEnvironment("sslEnabled"))
                services.Configure<MvcOptions>(options =>
                {
                    options.Filters.Add(new RequireHttpsAttribute());
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<Flight, FlightViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.FlightId))
                .ReverseMap();
                config.CreateMap<FuelPoint, FuelPointViewModel>().ReverseMap();
            });

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));

            if (this.env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
                loggerFactory.AddDebug(LogLevel.Information);
            }
            else
            {
                loggerFactory.AddDebug(LogLevel.Error);
            }

            app.UseStaticFiles();

            app.UseIdentity();

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715
            
            //TODO: uncomment when ssl sorted out
            if (this.env.IsEnvironment("sslEnabled"))
                app.UseFacebookAuthentication(new FacebookOptions()
                {
                    AppId = Environment.GetEnvironmentVariable("FACEBOOKAUTH_APPID"),
                    AppSecret = Environment.GetEnvironmentVariable("FACEBOOKAUTH_APPSECRET"),
                });


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
