using System.Text;
using Core;
using Core.Base.Interfaces;
using FluentValidation.AspNetCore;
using Infrastructure;
using Infrastructure.Identity;
using Infrastructure.Persistance.DatabaseContext;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Web.FymateApi.Filters;
using Web.FymateApi.Services;

namespace Web.FymateApi
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
            services.AddApplication();
            services.AddInfrastructure(Configuration);

            services.AddSingleton<ICurrentUserService, CurrentUserService>();
            services.AddHttpContextAccessor();

            services.AddControllersWithViews(options =>
                options.Filters.Add<ApiExceptionFilterAttribute>())
                    .AddFluentValidation();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });



            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("keykeykeykeykeykeykeykey")),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });


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
