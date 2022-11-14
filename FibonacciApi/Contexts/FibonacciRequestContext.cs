using FibonacciApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FibonacciApi.Contexts
{
    public class FibonacciRequestContext : DbContext
    {
        public FibonacciRequestContext(DbContextOptions<FibonacciRequestContext> options)
        : base(options)
        {
        }

        public DbSet<FibonacciRequest> FibonacciRequests { get; set; }
    }
}
