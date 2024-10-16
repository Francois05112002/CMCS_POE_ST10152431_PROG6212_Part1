using CMCS_POE_ST10152431_PROG6212_Part1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;

namespace CMCS_POE_ST10152431_PROG6212_Part1.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Lecturer> Lecturers { get; set; }
    }
}
