﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MF.Data.Song;
using MusicFy.Data;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using X.PagedList;

namespace MusicFy.Controllers
{
    public class AlbumsController : Controller
    {
        private readonly MusicFyDbContext _context;

        public AlbumsController(MusicFyDbContext context)
        {
            _context = context;
        }

        // GET: Albums
        public async Task<IActionResult> Index(string searchTitle, int? searchAuthorId, string sortOrder, int? page)
        {
            int pageCurrent = page ?? 1; //page == null ? 1 : page
            int pageMaxSize = 4;

            var albums = _context.Albums.Include(a => a.Author).AsQueryable();

            ViewBag.Authors = new SelectList(_context.Authors, "Id", "Username");

            ViewBag.TitleSearch = searchTitle;
            if (!string.IsNullOrEmpty(searchTitle))
                albums = albums.Where(x => x.Name.Contains(searchTitle));

            ViewBag.AlbumIdSearch = searchAuthorId.ToString();
            if (searchAuthorId.HasValue)
                albums = albums.Where(x => x.AuthorId == searchAuthorId);

            ViewBag.SortOrder = sortOrder;
            ViewBag.TitleSortParam = string.IsNullOrEmpty(sortOrder) ? "title-desc" : "";

            switch (sortOrder)
            {
                case "title-desc":
                    albums = albums.OrderByDescending(x => x.Name);
                    break;
                default:
                    albums = albums.OrderBy(x => x.Name);
                    break;

            }

            return View(await albums.ToPagedListAsync(pageCurrent, pageMaxSize));
        }

        // GET: Albums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Albums == null)
            {
                return NotFound();
            }

            var album = await _context.Albums
                .Include(a => a.Author)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (album == null)
            {
                return NotFound();
            }

            ViewBag.SelectedSongs = _context.Songs.Where(x => x.AlbumId == id);

            return View(album);
        }

        // GET: Albums/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Set<Author>(), "Id", "Username");
            ViewData["AllSongs"] = new SelectList(_context.Songs, "Id", "Name");

            return View();
        }

        // POST: Albums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AuthorId,Name,isPublic,Songs,Pole")] Album album)
        {
            if (album.Author == null && album.AuthorId == null)
            {
                ModelState.AddModelError("Author", "Choose an author for the album.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(album);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Username", album.AuthorId);
            ViewData["AddedSongs"] = new SelectList(_context.Songs.Where(x => x.AlbumId == album.Id), "Id", "Name", album.Songs);

            return View(album);
        }

        // GET: Albums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Albums == null)
            {
                return NotFound();
            }

            var album = await _context.Albums.FindAsync(id);

            if (album == null)
            {
                return NotFound();
            }

            ViewData["AuthorId"] = new SelectList(_context.Set<Author>(), "Id", "Username", album.AuthorId);
            ViewData["AllSongs"] = new SelectList(_context.Songs.Where(x => x.AlbumId == id), "Id", "Name", album.Songs);

            return View(album);
        }

        // POST: Albums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AuthorId,Name,isPublic,Songs")] Album album)
        {
            if (id != album.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(album);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlbumExists(album.Id))
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

            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Username", album.AuthorId);
            ViewData["AllSongs"] = new SelectList(_context.Songs, "Id", "Name", album.Songs);

            return View(album);
        }

        // GET: Albums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Albums == null)
            {
                return NotFound();
            }

            var album = await _context.Albums
                .Include(a => a.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        // POST: Albums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Albums == null)
            {
                return Problem("Entity set 'MusicFyDbContext.Albums'  is null.");
            }
            var album = await _context.Albums.FindAsync(id);
            if (album != null)
            {
                _context.Albums.Remove(album);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // -------------------------------------------------------------------------------
        private bool AlbumExists(int id)
        {
            return (_context.Albums?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
