using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MF.Data.Song
{
    public class Album
    {
        [Key]
        public int Id { get; set; }

        public int? AuthorId { get; set; }

        [Display(Name = "Author")]
        [ForeignKey("AuthorId")]
        [InverseProperty("Albums")]
        public Author? Author { get; set; }

        [Display(Name = "Album")]
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [Display(Name = "Album Songs")]
        [InverseProperty("Album")]
        public virtual ICollection<Song>? Songs { get; set; }

        [Display(Name = "Make Public")]
        [Required]
        public bool isPublic { get; set; }
    }
}
