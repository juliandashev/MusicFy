using Microsoft.AspNetCore.Mvc;

namespace MusicFy.Models.FileModels
{
    public class ResponseModel
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public bool IsResponse { get; set; }
    }
}
