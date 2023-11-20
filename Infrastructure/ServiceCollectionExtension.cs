using Application.IRepositories;
using Infrastructure.DbContexts;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext(configuration);
            services.RegisterServices();
        }

        public static async void AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = "";

            var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("AppSettings.json", optional: true, reloadOnChange: true);
            IConfiguration _configuration = builder.Build();
            connectionString = _configuration.GetConnectionString("DefaultConnection");


            services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
            using (ApplicationDbContext db = new ApplicationDbContext(connectionString))
            {
                var admin = await db.Users.Include(x => x.Group).FirstOrDefaultAsync(p =>p.Group.Code == Domain.Enums.Position.Admin);
                if (admin == null)
                {
                    admin = new Domain.Entities.User();
                    var newGroup = new Domain.Entities.UserGroup() { Code = Domain.Enums.Position.Admin, Description = ""};
                    var newState = new Domain.Entities.UserState() { Code = Domain.Enums.State.Active, Description = "" };
                    admin.State = newState;
                    admin.Group = newGroup;
                    admin.Login = "admin";
                    admin.Password = "admin";
                    admin.CreatedDate = DateTime.UtcNow;
                    db.Users.Add(admin);
                    await db.SaveChangesAsync();
                }
            }
        }
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IAuthRepository, AuthRepository>();
        }
    }
}
