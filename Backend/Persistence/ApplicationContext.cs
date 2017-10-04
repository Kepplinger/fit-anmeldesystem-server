using Microsoft.EntityFrameworkCore;

namespace vega.Persistence
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            
        }
    }
}