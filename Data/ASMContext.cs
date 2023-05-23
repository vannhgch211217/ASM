using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ASM.Models;

namespace ASM.Data
{
    public class ASMContext : DbContext
    {
        public ASMContext (DbContextOptions<ASMContext> options)
            : base(options)
        {
        }

        public DbSet<ASM.Models.User> User { get; set; } = default!;

        public DbSet<ASM.Models.Admin> Admin { get; set; } = default!;

        public DbSet<ASM.Models.Category> Category { get; set; } = default!;

        public DbSet<ASM.Models.ColorDetail> ColorDetail { get; set; } = default!;

        public DbSet<ASM.Models.Customer> Customer { get; set; } = default!;

        public DbSet<ASM.Models.Order> Order { get; set; } = default!;

        public DbSet<ASM.Models.OrderDetail> OrderDetail { get; set; } = default!;

        public DbSet<ASM.Models.Product> Product { get; set; } = default!;

        public DbSet<ASM.Models.Size> Size { get; set; } = default!;

        public DbSet<ASM.Models.Supplier> Supplier { get; set; } = default!;
    }
}
