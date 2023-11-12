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

        [ForeignKey("AuthorId")]
        public int AuthorId { get; set; }
        public Author Author { get; set; }

        [Display(Name = "Album name")]
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }
       
        [Display(Name = "Album songs")]
        public virtual ICollection<Song> Songs { get; set; }

        [Display(Name = "Album status")]
        [Required]
        public bool isPublic { get; set; }
    }
}
