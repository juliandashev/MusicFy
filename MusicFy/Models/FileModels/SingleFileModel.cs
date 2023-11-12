using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MusicFy.Models.FileModels
{
    public class SingleFileModel : ResponseModel
    {
        [Required(ErrorMessage = "Please enter file name")]
        public string FileName { get; set; }

        [Required(ErrorMessage = "Please select file")]
        public IFormFile File { get; set; }
    }
}
