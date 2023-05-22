using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using testing1.Models;

namespace ASM.Data
{
    public class ASMContext : DbContext
    {
        public ASMContext (DbContextOptions<ASMContext> options)
            : base(options)
        {
        }

        public DbSet<testing1.Models.User> User { get; set; } = default!;

        public DbSet<testing1.Models.Admin> Admin { get; set; } = default!;

        public DbSet<testing1.Models.Category> Category { get; set; } = default!;

        public DbSet<testing1.Models.ColorDetail> ColorDetail { get; set; } = default!;

        public DbSet<testing1.Models.Customer> Customer { get; set; } = default!;

        public DbSet<testing1.Models.Order> Order { get; set; } = default!;

        public DbSet<testing1.Models.OrderDetail> OrderDetail { get; set; } = default!;

        public DbSet<testing1.Models.Product> Product { get; set; } = default!;

        public DbSet<testing1.Models.Size> Size { get; set; } = default!;

        public DbSet<testing1.Models.Supplier> Supplier { get; set; } = default!;
    }
}
