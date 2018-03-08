﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicStore.Data;
using MusicStore.Models;


namespace MusicStore.Controllers
{
    public class TracksController : Controller
    {
        private readonly MusicStoreContext _context;

        public TracksController(MusicStoreContext context)
        {
            _context = context;
        }

        // GET: Tracks
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            IQueryable<Track> tracks = _context.Tracks
                .Include(t => t.Album)
                    .ThenInclude(a => a.Artist);

            if (!String.IsNullOrEmpty(searchString))
            {
                tracks = tracks.Where(t => t.Title.Contains(searchString));
            }

            if (!String.IsNullOrEmpty(sortOrder))
            {
                switch (sortOrder)
                {
                    case "titles_asc":
                        tracks = tracks.OrderBy(t => t.Title);
                        break;
                    case "titles_desc":
                        tracks = tracks.OrderByDescending(t => t.Title);
                        break;
                    case "artists_asc":
                        tracks = tracks.OrderBy(t => t.Album.Artist.Name);
                        break;
                    case "artists_desc":
                        tracks = tracks.OrderByDescending(t => t.Album.Artist.Name);
                        break;
                    case "albums_asc":
                        tracks = tracks.OrderBy(t => t.Album.Title);
                        break;
                    case "albums_desc":
                        tracks = tracks.OrderByDescending(t => t.Album.Title);
                        break;
                    default:
                        break;
                }
            }

            tracks.AsNoTracking();
            return View(await tracks.ToListAsync());
        }

        // GET: Tracks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var track = await _context.Tracks
                .Include(t => t.Album)
                .ThenInclude(a => a.Artist)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (track == null)
            {
                return NotFound();
            }

            return View(track);
        }

        // GET: Tracks/Create
        public IActionResult Create()
        {
            ViewData["Albums"] = new SelectList(_context.Albums, "ID", "Title");
            return View();
        }

        // POST: Tracks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,AlbumID,Title")] Track track)
        {
            if (ModelState.IsValid)
            {
                _context.Add(track);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AlbumID"] = new SelectList(_context.Albums, "ID", "ID", track.AlbumID);
            return View(track);
        }

        // GET: Tracks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var track = await _context.Tracks.SingleOrDefaultAsync(m => m.ID == id);
            if (track == null)
            {
                return NotFound();
            }
            ViewData["Albums"] = new SelectList(_context.Albums, "ID", "Title");
            return View(track);
        }

        // POST: Tracks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,AlbumID,Title")] Track track)
        {
            if (id != track.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(track);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrackExists(track.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", new { id = track.ID });
            }
            ViewData["AlbumID"] = new SelectList(_context.Albums, "ID", "ID", track.AlbumID);
            return View(track);
        }

        // GET: Tracks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var track = await _context.Tracks
                .Include(t => t.Album)
                .ThenInclude(a => a.Artist)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (track == null)
            {
                return NotFound();
            }

            return View(track);
        }

        // POST: Tracks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var track = await _context.Tracks.SingleOrDefaultAsync(m => m.ID == id);
            _context.Tracks.Remove(track);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrackExists(int id)
        {
            return _context.Tracks.Any(e => e.ID == id);
        }
    }
}
