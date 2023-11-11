﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public int AuthorId { get; set; }
        public virtual Author Author { get; set; }
        
        public int AlbumId { get; set; }
        public virtual Album Album { get; set; }

        [Display(Name = "Song duration in seconds")]
        [StringLength(20, MinimumLength = 1)]
        public string Duration { get; set; }

        [Display(Name = "All time listeners")]
        public int Listeners{ get; set; }

        [Display(Name = "Date added")]
        [DataType(dataType: DataType.Date)]
        public DateTime DateAdded { get; set; }
    }
}