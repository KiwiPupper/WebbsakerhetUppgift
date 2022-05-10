using Microsoft.EntityFrameworkCore;
using WebbsäkerhetInlämning.Models;

namespace WebbsäkerhetInlämning.Data
{
    public class SqlContext : DbContext
    {
        public SqlContext()
        {

        }

        public SqlContext(DbContextOptions<SqlContext> options) : base(options)
        {

        }

        public DbSet<Message> Messages { get; set; }
    }
}
