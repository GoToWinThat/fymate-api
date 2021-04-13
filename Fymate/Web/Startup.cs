using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Fymate.Core;
using Fymate.Core.Base.Interfaces;
using FluentValidation.AspNetCore;
using Fymate.Infrastructure;
using Fymate.Infrastructure.Identity;
using Fymate.Infrastructure.Persistance.DatabaseContext;
using Fymate.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Fymate.Web.Filters;
using Fymate.Web.Services;

namespace Fymate.Web
{
    public class Startup
    {
        readonly string AllowPolicy = "FymatePolicy";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {


            #region Init
            services.AddApplication();
            services.AddInfrastructure(Configuration);
            var settingsSection = Configuration.GetSection("AppSettings"); //TODO: change to configuration class?
            var settings = settingsSection.Get<AppSettings>();
            services.Configure<AppSettings>(settingsSection);
            #endregion

            services.AddHttpContextAccessor();

            services.AddControllersWithViews(options =>
                options.Filters.Add<ApiExceptionFilterAttribute>())
                    .AddFluentValidation();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });




            #region Identity Server and Auth

            services.AddSingleton<ICurrentUserService, CurrentUserService>();


            //Tell token handler not to map to legacy XML names
            //see: https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/415
            //This way we can use User.Indentity.Name, instead of finding name each time

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddIdentityServerJwt()
            .AddJwtBearer(options =>
            {
                // IdentityServer emits a typ header by default, recommended extra check
                options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };

                //SET ONLY IN-DEV TODO: make this automatic
                options.RequireHttpsMetadata = false;

                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.AuthKey)),
                    ValidateIssuer = false, //TODO: support this
                    ValidateAudience = false, //TODO: support this
                    ValidateLifetime = true,

                };
            });


            services.AddSingleton<IAuthorizationHandler, IsOwnerAuthorizationHandler>();

            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder() //Add fallback policy, so that method without [AllowAnonymous] or explicit Authorize attribute must be authenticated
                //This is to protect new/in-dev, endpoints. 
                    .RequireAuthenticatedUser()
                    .Build();

                options.AddPolicy("IsOwnerPolicy", policy =>
                    policy.Requirements.Add(new IsOwnerAuthorizationRequirement()));

                options.AddPolicy("IsAdmin", policy =>
                    policy.RequireRole("Admin"));
            });


            #endregion


            #region Our services
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<IProfileService, ProfileService>();
            #endregion


            services.AddCors(options =>
            {
                options.AddPolicy(name: AllowPolicy,
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseRouting();


            app.UseAuthentication(); //Add auth middleware to request pipeline
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

            });
        }
    }
}
