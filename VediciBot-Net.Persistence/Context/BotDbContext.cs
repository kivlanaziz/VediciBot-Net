using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VediciBot_Net.Persistence.Entity;

namespace VediciBot_Net.Persistence.Context
{
    public class BotDbContext : DbContext
    {
        public DbSet<Roles> Roles { get; set; }

        public BotDbContext(DbContextOptions<BotDbContext> options)
               :base(options)
        {
            Database.EnsureCreated();
        }
    }
}
