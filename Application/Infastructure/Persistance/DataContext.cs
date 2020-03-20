using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace Application.Infastructure.Persistance
{
    public class DataContext : DbContext
    {

        protected readonly IConfiguration Configuration;

        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var ActiveDatabase = Configuration["ActiveDatabase"];

            switch (ActiveDatabase)
            {
                case "Sqlite":
                    optionsBuilder.UseSqlite(Configuration["Database:Sqlite"]);
                    break;
                default:
                    optionsBuilder.UseSqlite(Configuration["Database:Sqlite"]);
                    break;
            }
        }
    }
}
