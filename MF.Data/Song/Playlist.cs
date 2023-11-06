using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MF.Data.Song
{
    public class Playlist
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Playlist name")]
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [Display(Name = "Album description")]
        [StringLength(150, MinimumLength = 3)]
        public string? Description { get; set; }

        [Display(Name = "Playlist status")]
        [Required]
        public bool isPublic { get; set; }
    }
}
