using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MBLabs.Models;

namespace EuVou.Data
{
    public class EuVouContext : DbContext
    {
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
       // {
        //    optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=EuVou;Trusted_Connection=True;MultipleActiveResultSets=true");
        //}
        public EuVouContext (DbContextOptions<EuVouContext> options)
            : base(options)
        {
        }

        public DbSet<MBLabs.Models.Client> Client { get; set; }

        public DbSet<MBLabs.Models.Event> Event { get; set; }

        public DbSet<MBLabs.Models.Ticket> Ticket { get; set; }
    }
}
