using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace MusicFy.Models.FileModels
{
    public class FileUploadController : Controller
    {
        public IActionResult Index()
        {
            SingleFileModel model = new SingleFileModel();

            return View(model);
        }
    }
}
