using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MF.Data.Song
{
    public class Author
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Author's first name")]
        [StringLength(20, MinimumLength = 3)]
        public string? FName { get; set; }

        [Display(Name = "Author's last name")]
        [StringLength(20, MinimumLength = 3)]
        public string? LName { get; set; }

        [Display(Name = "Author's username")]
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Username { get; set; }

        [Display(Name = "Author's albums released")]
        [InverseProperty("Author")]
        public virtual ICollection<Album>? Albums { get; set; }

        [Display(Name = "Author's songs released")]
        [InverseProperty("Author")]
        public virtual ICollection<Song>? Songs { get; set; }

        [Display(Name = "Author's followers")]
        public int Followers { get; private set; } = 0;

        [Display(Name = "Author's total monthly listeners")]
        public int MonthlyListeners { get; private set; } = 0;

        [Display(Name = "Date when account was created")]
        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    }
}
