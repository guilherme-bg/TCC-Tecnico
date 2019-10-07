using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TCC.Models {
    public class TCCContext : IdentityDbContext<Usuario> {
        public TCCContext(DbContextOptions<TCCContext> options) : base(options) {
        }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Cidade> Cidade { get; set; }
        protected override void OnModelCreating(ModelBuilder builder) {
            builder.Entity<Usuario>().HasKey(m => m.Id);
            builder.Entity<Cidade>().HasKey(m => m.Id);
            base.OnModelCreating(builder);
        }
    }
}