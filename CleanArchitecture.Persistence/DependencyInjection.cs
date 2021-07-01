using CleanArchitecture.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Persistence
{
    static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services,
                                                        IConfiguration configuration)
        {
            var connectionString = configuration["dbConnection"];
            services.AddDbContext<NotedDbContext>(options =>
                    options.UseSqlite(connectionString));
            services.AddScoped<INotesDbContext>(provider =>
                    provider.GetService<NotedDbContext>());

            return services;
        }
    }
}
