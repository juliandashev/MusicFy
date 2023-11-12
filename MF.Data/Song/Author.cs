﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public virtual ICollection<Album> Albums { get; set; }

        [Display(Name = "Author's songs released")]
        public virtual ICollection<Song> Songs { get; set; }

        private int followers { get; set; }
        [Display(Name = "Author's followers")]
        public int Followers
        {
            get { return followers; }
            set
            {
                if (value < 0)
                    value = 0;

                followers = value;
            }
        }

        private int monthlyListeners { get; set; }

        [Display(Name = "Author's total monthly listeners")]
        public int MonthlyListeners
        {
            get { return monthlyListeners; }
            set
            {
                if (value < 0)
                    value = 0;

                monthlyListeners = value;
            }
        }

        [Display(Name = "Date when account was created")]
        [DataType(dataType: DataType.Date)]
        public static DateTime DateCreated
        {
            get { return DateTime.UtcNow; }
        }
    }
}
