using Microsoft.AspNetCore.Mvc;

namespace MusicFy.Controllers
{
    public class MusicPlayerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string action, int volume)
        {
            if (action == "PlayPause")
            {
                // Your logic to play or pause the music
            }
            else if (action == "Skip")
            {
                // Your logic to skip to the next song
            }
            else if (action == "Replay")
            {
                // Your logic to replay the current song
            }

            // Return a JSON response to update the volume
            return Json(new { Volume = volume });
        }
    }
}
