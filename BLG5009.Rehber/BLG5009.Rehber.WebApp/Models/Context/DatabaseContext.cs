using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLG5009.Rehber.WebApp.Models.Context
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Telephone> Telephones { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Email> Emails { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(x => x.FirstName).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<User>().Property(x => x.LastName).HasMaxLength(50).IsRequired();

            modelBuilder.Entity<User>().Property(x => x.NickName).HasMaxLength(20);

            modelBuilder.Entity<Telephone>().Property(x => x.Number).HasMaxLength(15).IsRequired();
            modelBuilder.Entity<Address>().Property(x => x.AddressText).HasMaxLength(500).IsRequired();
            modelBuilder.Entity<Email>().Property(x => x.EmailAddress).HasMaxLength(150).IsRequired();

        }
    }
}
