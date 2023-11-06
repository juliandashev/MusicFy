using MF.Data.Song;
using Microsoft.EntityFrameworkCore;

namespace MusicFy.Data
{
    public class MusicFyDbContext : DbContext
    {
        public DbSet<Album> Albums { get; set; }
        public DbSet<Author> Authors{ get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Song> Songs{ get; set; }

        public MusicFyDbContext(DbContextOptions<MusicFyDbContext> options) : base(options)
        {
            
        }
    }
}
