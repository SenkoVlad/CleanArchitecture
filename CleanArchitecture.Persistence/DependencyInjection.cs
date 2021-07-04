using CleanArchitecture.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration["DbConnection"];
            services.AddDbContext<NotesDbContext>(options =>
                    options.UseSqlite(connectionString));
            services.AddScoped<INotesDbContext>(provider =>
                    provider.GetService<NotesDbContext>());

            return services;
        }
    }
}
