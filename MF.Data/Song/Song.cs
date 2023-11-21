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
        [StringLength(20, MinimumLength = 3)]
        public string Name { get; set; }

        [Required(ErrorMessage = "AuthorId is required")]
        public int? AuthorId { get; set; }

        [ForeignKey("AuthorId")]
        [InverseProperty("Songs")]
        public Author? Author { get; set; }

        [Required(ErrorMessage = "AlbumId is required")]
        public int? AlbumId { get; set; }

        [ForeignKey("AlbumId")]
        [InverseProperty("Songs")]
        public Album? Album { get; set; }

        [Display(Name = "Song duration in seconds")]
        public ushort Duration { get; private set; } = 0;

        [Display(Name = "All time listeners")]
        public uint Listeners { get; private set; } = 0;

        [Display(Name = "Date added")]
        [DataType(dataType: DataType.Date)]
        public DateTime DateCreated { get; private set; } = DateTime.UtcNow;

        public string? ImageFileName { get; set; }

        [Display(Name = "Upload song banner")]
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}