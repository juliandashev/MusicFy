using MF.Data.Song;
using Microsoft.EntityFrameworkCore;

namespace MusicFy.Data
{
    public class MusicFyDbContext : DbContext
    {
        public DbSet<Author> Authors{ get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Song> Songs{ get; set; }

        public DbSet<MF.Data.Song.File> Files { get; set; }
        
        public MusicFyDbContext(DbContextOptions<MusicFyDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Song>()
                .HasOne(s => s.Author)
                .WithMany(a => a.Songs)
                .HasForeignKey(s => s.AuthorId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
