using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NetCoreReact.Business.Models;
using NetCoreReact.Business.UnitOfWork;
using NetCoreReact.Handlers;
using NetCoreReact.Services;
using System.Text;

namespace NetCoreReact
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
            // Configure DbContext
            services.AddDbContext<SampleContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
            });

            // Add JWT Authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Configuration.GetValue<string>("JWT:SecretKey"))
                        )
                    };
                });

            #region ASP.NET Core - Dependency Injection

            //  Singleton (object always same)
            services.AddSingleton<IAuthService>(
                new AuthService(
                    Configuration.GetValue<string>("JWT:SecretKey"),
                    Configuration.GetValue<int>("JWT:Lifespan")
                )
            );

            // Scoped  (object same within request but diffrent every request)
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Transient (object always diffrent)

            #endregion

            // Configure Swagger
            services.AddSwaggerGen(configuration =>
            {
                configuration.SwaggerDoc("v1", new OpenApiInfo { Title = "API Docs", Version = "v1" });
                configuration.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token. \n \n Example: 'Bearer [your-token]' \n \n",
                    Name = "Authorization",
                    Scheme = "Bearer",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                configuration.OperationFilter<SwaggerOperationFilterHandler>();
            });

            // Configure MVC for handlers
            services.AddControllersWithViews(configuration =>
            {
                configuration.Filters.Add(typeof(RequestFilterHandler));
            })
            .ConfigureApiBehaviorOptions(configuration =>
            {
                configuration.InvalidModelStateResponseFactory = context =>
                {
                    return RequestFilterHandler.CustomErrorResponse(context);
                };
            })
            .AddNewtonsoftJson(configuration =>
            {
                configuration.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
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

            // Provide handle for response format and error format
            app.UseMiddleware<HttpHandler>();

            // Enable middleware to serve generated Swagger as JSON endpoint.
            app.UseSwagger();

            // specifying Swagger JSON endpoint.
            app.UseSwaggerUI(configuration =>
            {
                configuration.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                configuration.RoutePrefix = "api";
                configuration.DefaultModelsExpandDepth(-1);
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
