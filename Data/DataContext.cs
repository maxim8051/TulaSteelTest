using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MvcTest.Models;

namespace MvcTest.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Department> Department { get; set; }
    }
}
