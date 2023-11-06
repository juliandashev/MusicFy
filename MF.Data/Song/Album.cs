using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MF.Data.Song
{
    public class Album
    {
        [Key]
        public int Id { get; set; }

        public int AuthorId { get; set; }

        [Display(Name = "Albums name")]
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }
       
        [Display(Name = "Songs that are in the album")]
        public virtual ICollection<Song> Songs { get; set; }
    }
}
