using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MF.Data.Song;
using MusicFy.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Hosting;
using System.Drawing.Imaging;
using System.Drawing;
using NuGet.Packaging.Signing;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NAudio.Wave;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.Text.RegularExpressions;
using X.PagedList;

namespace MusicFy.Controllers
{
    public class SongsController : Controller
    {
        private readonly MusicFyDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SongsController(MusicFyDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Songs
        public async Task<IActionResult> Index(string searchTitle, int? searchAlbumId, string sortOrder, int? page)
        {
            int pageCurrent = page ?? 1; //page == null ? 1 : page
            int pageMaxSize = 3;

            var songs = _context.Songs.Include(s => s.Album).Include(s => s.Author).AsQueryable();
            ViewBag.Albums = new SelectList(_context.Albums, "Id", "Name");

            ViewBag.TitleSearch = searchTitle;
            if (!string.IsNullOrEmpty(searchTitle))
                songs = songs.Where(x => x.Name.Contains(searchTitle));

            ViewBag.AlbumIdSearch = searchAlbumId.ToString();
            if (searchAlbumId.HasValue)
                songs = songs.Where(x => x.AlbumId == searchAlbumId);

            ViewBag.SortOrder = sortOrder;
            ViewBag.TitleSortParam = string.IsNullOrEmpty(sortOrder) ? "title-desc" : "";
            ViewBag.ReleaseDateSortParam = sortOrder == "rdate-desc" ? "rdate-asc" : "rdate-desc";

            switch (sortOrder)
            {
                case "title-desc":
                    songs = songs.OrderByDescending(x => x.Name);
                    break;
                case "rdate-desc":
                    songs = songs.OrderByDescending(x => x.DateCreated);
                    break;
                case "rdate-asc":
                    songs = songs.OrderBy(x => x.DateCreated);
                    break;
                default:
                    songs = songs.OrderBy(x => x.Name);
                    break;

            }

            return View(await songs.ToPagedListAsync(pageCurrent, pageMaxSize));
        }

        // GET: Songs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Songs == null)
            {
                return NotFound();
            }

            var song = await _context.Songs
                .Include(s => s.Album)
                .Include(s => s.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (song == null)
            {
                return NotFound();
            }

            return View(song);
        }

        // GET: Songs/Create
        public IActionResult Create()
        {
            ViewData["AlbumId"] = new SelectList(_context.Albums, "Id", "Name");
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Username");
            return View();
        }

        // POST: Songs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,AuthorId,AlbumId,Duration,ImageFile,MusicFile")] Song song)
        {
            bool isNull =
                song.Album == null && song.AlbumId == null &&
                song.Author == null && song.AuthorId == null;

            if (isNull)
            {
                ModelState.AddModelError("Author", "Choose an author for the album.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(song);
                await _context.SaveChangesAsync();

                await UploadImageAsync(song); // upload an image
                await UploadSongAsync(song); // upload a song

                return RedirectToAction(nameof(Index));
            }

            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(x => x.Errors);

            foreach (var error in allErrors)
            {
                await Console.Out.WriteLineAsync(error + " ");
            }

            ViewData["AlbumId"] = new SelectList(_context.Albums, "Id", "Name", song.AlbumId);
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Username", song.AuthorId);

            return View(song);
        }

        // GET: Songs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Songs == null)
            {
                return NotFound();
            }

            var song = await _context.Songs.FindAsync(id);
            if (song == null)
            {
                return NotFound();
            }
            ViewData["AlbumId"] = new SelectList(_context.Albums, "Id", "Name", song.AlbumId);
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Username", song.AuthorId);

            return View(song);
        }

        // POST: Songs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,AuthorId,AlbumId,Duration,Listeners,DateCreated,ImageFile,MusicFile")] Song song)
        {
            if (id != song.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await UpdateImageAsync(song);
                    await UpdateSongAsync(song);

                    _context.Update(song);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SongExists(song.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AlbumId"] = new SelectList(_context.Albums, "Id", "Name", song.AlbumId);
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Username", song.AuthorId);

            return View(song);
        }

        // GET: Songs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Songs == null)
            {
                return NotFound();
            }

            var song = await _context.Songs
                .Include(s => s.Album)
                .Include(s => s.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (song == null)
            {
                return NotFound();
            }

            return View(song);
        }

        // POST: Songs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Songs == null)
            {
                return Problem("Entity set 'MusicFyDbContext.Songs'  is null.");
            }
            var song = await _context.Songs.FindAsync(id);

            if (song != null)
            {
                DeleteEntity("Image", id);
                DeleteEntity("Song", id);

                _context.Songs.Remove(song);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // --------------------------------------------------------------------------------------------------------------------------

        private void DeleteEntity(string type, int id)
        {
            string pattern = $"^({id})";
            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

            switch (type)
            {
                case "Image":
                    filePath += "/banners/";
                    break;
                case "Song":
                    filePath += "/songs/";
                    break;
                default:
                    filePath += string.Empty;
                    break;
            }

            string[] files = Directory.GetFiles(filePath);

            foreach (string file in files)
            {
                if (Regex.IsMatch(Path.GetFileName(file), pattern))
                {
                    System.IO.File.Delete(file);
                }
            }
        }

        private bool SongExists(int id)
        {
            return (_context.Songs?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private async Task UploadImageAsync(Song song)
        {
            if (song.ImageFile != null)
            {
                string uniqueFileName =
                    $"{song.Id}_{song.AlbumId}_{song.AuthorId}{Path.GetExtension(song.ImageFile.FileName)}";

                string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "banners", uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await song.ImageFile.CopyToAsync(fileStream);
                }

                if (IsImage(filePath))
                {
                    song.ImageFileName = uniqueFileName;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    System.IO.File.Delete(filePath);
                    ModelState.AddModelError("ImageFile", "The uploaded file is not a valid image.");
                }
            }
        }

        private async Task UploadSongAsync(Song song)
        {
            if (song.MusicFile != null)
            {
                string uniqueFileName =
                    $"{song.Id}_{song.AlbumId}_{song.AuthorId}{Path.GetExtension(song.MusicFile.FileName)}";

                string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "songs", uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await song.MusicFile.CopyToAsync(fileStream);
                }

                if (IsMp3(filePath))
                {
                    song.MusicFileName = uniqueFileName;
                    song.Duration = FormatTimeSpan(GetDuration(filePath));
                    await _context.SaveChangesAsync();
                }
                else
                {
                    System.IO.File.Delete(filePath);
                    ModelState.AddModelError("MusicFile", "Invalid file format. Please upload a valid mp3 file.");
                }
            }
        }

        private static string FormatTimeSpan(TimeSpan timeSpan)
        {
            if (timeSpan.Hours > 0)
            {
                return $"{timeSpan.Hours:00}:{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
            }
            else
            {
                return $"{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
            }
        }

        private static TimeSpan GetDuration(string filePath)
        {
            using (var reader = new Mp3FileReader(filePath))
            {
                return reader.TotalTime;
            }
        }

        private async Task UpdateImageAsync(Song song)
        {
            DeleteEntity("Image", song.Id);
            await UploadImageAsync(song);
        }

        private async Task UpdateSongAsync(Song song)
        {
            DeleteEntity("Song", song.Id);
            await UploadSongAsync(song);
        }

        private bool IsMp3(string filePath)
        {
            // Check if the file extension is .mp3
            if (Path.GetExtension(filePath).ToLower() == ".mp3")
            {
                return true;
            }

            // Check if the file's header data matches the MP3 file signature
            byte[] fileHeader = new byte[3];
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                fileStream.Read(fileHeader, 0, 3);
            }

            // The MP3 file signature is '49 44 33' (ASCII characters for 'ID3')
            return fileHeader[0] == 73 && fileHeader[1] == 68 && fileHeader[2] == 51;
        }

        private bool IsImage(string filePath)
        {
            try
            {
                using (var image = Image.FromFile(filePath))
                {
                    return image.RawFormat.Guid == ImageFormat.Jpeg.Guid ||
                           image.RawFormat.Guid == ImageFormat.Png.Guid ||
                           image.RawFormat.Guid == ImageFormat.Gif.Guid;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
