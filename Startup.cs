using DotNetCore_MultipleJWTSchemas.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DotNetCore_MultipleJWTSchemas
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var signingConfigurations = new SigningConfigurations();
            services.AddSingleton(signingConfigurations);

            services.AddAuthentication()
                .AddJwtBearer("BearerToken1", bearerOptions =>
                {
                    bearerOptions.TokenValidationParameters.IssuerSigningKey = signingConfigurations.Key;
                    bearerOptions.TokenValidationParameters.ValidateAudience = true;
                    bearerOptions.TokenValidationParameters.ValidAudience = "AudienceToken1";
                    bearerOptions.TokenValidationParameters.ValidateIssuer = true;
                    bearerOptions.TokenValidationParameters.ValidIssuer = "IssuerToken1";
                    bearerOptions.TokenValidationParameters.ValidateIssuerSigningKey = true;
                    bearerOptions.TokenValidationParameters.ValidateLifetime = true;
                    bearerOptions.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
                })
                .AddJwtBearer("BearerToken2", bearerOptions =>
                {
                    bearerOptions.TokenValidationParameters.IssuerSigningKey = signingConfigurations.Key;
                    bearerOptions.TokenValidationParameters.ValidateAudience = true;
                    bearerOptions.TokenValidationParameters.ValidAudience = "AudienceToken2";
                    bearerOptions.TokenValidationParameters.ValidateIssuer = true;
                    bearerOptions.TokenValidationParameters.ValidIssuer = "IssuerToken2";
                    bearerOptions.TokenValidationParameters.ValidateIssuerSigningKey = true;
                    bearerOptions.TokenValidationParameters.ValidateLifetime = true;
                    bearerOptions.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
                });

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("PolicyToken1", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes("BearerToken1")
                    .RequireAuthenticatedUser()
                    .Build());

                auth.AddPolicy("PolicyToken2", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes("BearerToken2")
                    .RequireAuthenticatedUser()
                    .Build());
            });
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

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
