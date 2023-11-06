using System.ComponentModel.DataAnnotations;

namespace MF.Data.Song
{
    public class Song
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Song name")]
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Name { get; set; }

        [Display(Name = "Author/s")]
        [Required]
        public virtual ICollection<Author> Authors { get; set; }

        [Display(Name = "Album name")]
        [Required]
        public Album Album { get; set; }

        [Display(Name = "Song duration in seconds")]
        [Required]
        [StringLength(20, MinimumLength = 1)]
        public string Duration { get; set; }

        [Display(Name = "All time listeners")]
        public int Listeners{ get; set; }

        [Display(Name = "Date added")]
        [DataType(dataType: DataType.Date)]
        public DateTime DateAdded { get; set; }
    }
}