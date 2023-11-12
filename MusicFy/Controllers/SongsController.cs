using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MF.Data.Song;
using MusicFy.Data;
using Microsoft.Data.SqlClient;

namespace MusicFy.Controllers
{
    public class SongsController : Controller
    {
        private readonly MusicFyDbContext _context;

        public SongsController(MusicFyDbContext context)
        {
            _context = context;
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

        [HttpPost]
        public IActionResult UploadFile(IFormFile postedFile)
        {
            string fileName = Path.GetFileName(postedFile.FileName);
            string contentType = postedFile.ContentType;

            using (MemoryStream ms = new MemoryStream())
            {
                postedFile.CopyTo(ms);

                // Този ред може да се премахне, тъй като вече използвате MusicFyDbContext вместо IConfiguration
                // string constr = this.Configuration.GetConnectionString("MyConn");

                // Пример за вмъкване на данни чрез EF Core и MusicFyDbContext
                var file = new Image
                {
                    Name = fileName,
                    ContentType = contentType,
                    Data = ms.ToArray()
                };

                _context.Images.Add(file);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }


        // POST: Songs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,AuthorId,AlbumId,Duration,Listeners,Image")] Song song)
        {
            if (ModelState.IsValid)
            {
                _context.Add(song);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,AuthorId,AlbumId,Duration,Listeners,Image")] Song song)
        {
            if (id != song.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
