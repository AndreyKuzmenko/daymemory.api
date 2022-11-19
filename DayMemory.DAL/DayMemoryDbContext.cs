using DayMemory.Core.Models;
using DayMemory.Core.Models.Common;
using DayMemory.Core.Models.Personal;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace DayMemory.DAL
{
    public class DayMemoryDbContext : IdentityDbContext<IdentityUser>
    {
        public DayMemoryDbContext(DbContextOptions<DayMemoryDbContext> options) : base(options)
        {
        }

        public DayMemoryDbContext()
        {

        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);

        //    var builder = new ConfigurationBuilder()
        //        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        //    IConfigurationRoot config = builder.Build();
        //    var connectionString = config.GetConnectionString("DefaultConnection");
        //    optionsBuilder.UseSqlServer(connectionString);
        //}


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DayMemoryDbContext).Assembly);
        }
    }


    public class DayMemoryContextFactory : IDesignTimeDbContextFactory<DayMemoryDbContext>
    {
        public DayMemoryDbContext CreateDbContext(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile(Path.GetFullPath("..\\DayMemory.API\\appsettings.json"), optional: true, reloadOnChange: true);

            IConfigurationRoot config = builder.Build();
            var connectionString = config.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<DayMemoryDbContext>();
            //
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=DayMemory;Integrated Security=true;");


            return new DayMemoryDbContext(optionsBuilder.Options);
        }
    }
}




