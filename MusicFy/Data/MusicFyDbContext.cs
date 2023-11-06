using Microsoft.EntityFrameworkCore;

namespace MusicFy.Data
{
    public class MusicFyDbContext : DbContext
    {
        public MusicFyDbContext(DbContextOptions<MusicFyDbContext> options) : base(options)
        {
            
        }
    }
}
