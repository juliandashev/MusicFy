using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MF.Data.Song
{
    public class Song
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Song name")]
        [Required(ErrorMessage = "Song name is required")]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [Required(ErrorMessage = "AuthorId is required")]
        public int? AuthorId { get; set; }

        [ForeignKey("AuthorId")]
        [InverseProperty("Songs")]
        public Author? Author { get; set; }

        public int? AlbumId { get; set; }

        [ForeignKey("AlbumId")]
        [InverseProperty("Songs")]
        public Album? Album { get; set; }

        [Display(Name = "Song duration")]
        public string? Duration { get; set; }

        [Display(Name = "All time listeners")]
        public ulong Listeners { get; private set; } = 0;

        [Display(Name = "Date added")]
        [DataType(dataType: DataType.Date)]
        public DateTime DateCreated { get; private set; } = DateTime.UtcNow;

        public string? ImageFileName { get; set; }

        [Display(Name = "Upload song banner")]
        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        [Display(Name = "Upload song")]
        [NotMapped]
        public IFormFile? MusicFile { get; set; }

        public string? MusicFileName { get; set; }
    }
}