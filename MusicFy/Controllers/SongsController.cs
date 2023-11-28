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
        public async Task<IActionResult> Index()
        {
            var musicFyDbContext = _context.Songs.Include(s => s.Album).Include(s => s.Author);

            return View(await musicFyDbContext.ToListAsync());
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
        public async Task<IActionResult> Create([Bind("Id,Name,AuthorId,AlbumId,ImageFile")] Song song)
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
                if (song.ImageFile != null)
                {
                    string uniqueFileName =
                        $"{song.AlbumId}_{song.AuthorId}_{DateTime.Now:yyyyMMddHHmmss}{Path.GetExtension(song.ImageFile.FileName)}";

                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "banners", uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await song.ImageFile.CopyToAsync(fileStream);
                    }

                    if (IsImage(filePath))
                    {
                        song.ImageFileName = uniqueFileName;
                    }
                    else
                    {
                        System.IO.File.Delete(filePath);
                        ModelState.AddModelError("ImageFile", "The uploaded file is not a valid image.");
                        return View(song);
                    }
                }

                _context.Add(song);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["AlbumId"] = new SelectList(_context.Albums, "Id", "Name", song.AlbumId);
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Username", song.AuthorId);

            return View(song);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,AuthorId,AlbumId,Duration,Listeners,DateCreated,ImageFile")] Song song)
        {
            if (id != song.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (song.ImageFile != null)
                    {
                        string uniqueFileName =
                            $"{song.AlbumId}_{song.AuthorId}_{DateTime.Now:yyyyMMddHHmmss}{Path.GetExtension(song.ImageFile.FileName)}";

                        string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "banners", uniqueFileName);

                        // delete old image if it exists
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await song.ImageFile.CopyToAsync(fileStream);
                        }

                        if (IsImage(filePath))
                        {
                            song.ImageFileName = uniqueFileName;
                        }
                        else
                        {
                            System.IO.File.Delete(filePath);
                            ModelState.AddModelError("ImageFile", "The uploaded file is not a valid image.");
                            return View(song);
                        }
                    }

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
                _context.Songs.Remove(song);
                System.IO.File.Delete(song.ImageFileName);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SongExists(int id)
        {
            return (_context.Songs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
