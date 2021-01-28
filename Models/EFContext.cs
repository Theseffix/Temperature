using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace temperature
{

    /*Install-Package Microsoft.Extensions.Configuration
    Install-Package Microsoft.Extensions.Configuration.FileExtensions
    Install-Package Microsoft.Extensions.Configuration.Json
    Microsoft.EntityFrameworkCore.SqlServer
    Microsoft.EntityFrameworkCore.Tool
     
     */

    class EFContext : DbContext
    {
        private string connectionString;

        public EFContext() : base()
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json", optional: false);

            var configuration = builder.Build();
            connectionString = configuration.GetConnectionString("sqlConnection");

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }

        //Bygg nya tables
        public DbSet<Models.Outside> Outside { get; set; }
        public DbSet<Models.Inside> Inside { get; set; }

        //Skjut ut databas genom Add-Migration i Package Manager Console. Ex. Add-Migration "V2"
    }
}