using AuthenticationApi.Application.Interfaces;
using AuthenticationApi.Infrastructure.Data;
using AuthenticationApi.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationApi.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddServices<TContext>(this IServiceCollection services, IConfiguration config) where TContext : DbContext
        {
            //uses the string connection to create the migrations and update the database
            services.AddDbContext<TContext>(option => option.UseSqlServer(
               config.GetConnectionString("dbconnection"), sqlServerOption => sqlServerOption.EnableRetryOnFailure()));

            //add the jwt config
            JWTAuthenticationScheme.AddJWTSchemeCollection(services, config);

            return services;
        }

        //api gateway service
        /*public static IApplicationBuilder UseSharedPolicies(this IApplicationBuilder app)
        {
            app.UseMiddleware<GlobalException>();
            app.UseMiddleware<ListenToApiGateway>();
            return app;
        }*/


        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
        {
            //add the database string 
            AddServices<AuthenticationDbContext>(services, config);
            //implements the interface and the repository to use the controller in cors 
            services.AddScoped<IUser, AuthenticationRepository>();

            return services;
        }
    }
}
