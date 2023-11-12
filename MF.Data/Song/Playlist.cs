using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MF.Data.Song
{
    public class Playlist
    {
        [Key]
        public int Id { get; set; }

        public int AuthorId { get; set; }

        public Author Author { get; set; }

        [Display(Name = "Playlist name")]
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [Display(Name = "Playlist description")]
        [StringLength(150, MinimumLength = 3)]
        public string? Description { get; set; }

        // Can be nullable, becase a playlist can be empty
        [Display(Name = "Playlist's songs")]
        public virtual ICollection<Song>? Songs { get; set; }

        [Display(Name = "Playlist status")]
        [Required]
        public bool IsPublic { get; set; }
    }
}
