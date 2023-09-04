using ApiTransation.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiTransation.Data
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }
        public DbSet<CartaoCredito> CartoesCredito { get; set; }
    }
}
