using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicStore.Data;
using MusicStore.Models;
using MusicStore.ViewModels;

namespace MusicStore.Controllers
{
    public class AlbumsController : Controller
    {
        private readonly MusicStoreContext _context;

        public AlbumsController(MusicStoreContext context)
        {
            _context = context;
        }

        // GET: Albums
        public async Task<IActionResult> Index(string sortOrder, string searchString, string currentSearch, int? page, int? pageSize)
        {
            IQueryable<Album> albums = _context.Albums.Include(a => a.Artist);

            //Perform search
            if (!String.IsNullOrEmpty(searchString))    //first search request
            {
                page = 1;
            }
            else
            {
                searchString = currentSearch;
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                albums = albums.Where(t => t.Title.Contains(searchString));
            }

            ViewData["CurrentSearch"] = searchString;

            //Perform sort
            if (!String.IsNullOrEmpty(sortOrder))
            {
                switch (sortOrder)
                {
                    case "titles_asc":
                        albums = albums.OrderBy(a => a.Title);
                        break;
                    case "titles_desc":
                        albums = albums.OrderByDescending(a => a.Title);
                        break;
                    case "artists_asc":
                        albums = albums.OrderBy(a => a.Artist.Name);
                        break;
                    case "artists_desc":
                        albums = albums.OrderByDescending(a => a.Artist.Name);
                        break;
                    case "release_asc":
                        albums = albums.OrderBy(a => a.ReleaseDate);
                        break;
                    case "release_desc":
                        albums = albums.OrderByDescending(a => a.ReleaseDate);
                        break;
                    default:
                        break;
                }
            }

            ViewData["CurrentSort"] = sortOrder;

            //Set page size
            if (pageSize == null || pageSize < DefaultValues.PageSizeMin)
            {
                pageSize = DefaultValues.PageSize;
                page = 1;
            }

            ViewData["CurrentPageSize"] = pageSize;

            return View(await PaginatedList<Album>.CreateAsync(albums.AsNoTracking(), page ?? 1, pageSize ?? DefaultValues.PageSize));
        }

        // GET: Albums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _context.Albums
                .Include(a => a.Artist)
                .Include(a => a.Tracks)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (album == null)
            {
                return NotFound();
            }

            return View(new AlbumTrackViewModel(album, new Track()));
        }

        // GET: Albums/Create
        public IActionResult Create()
        {
            ViewData["Artists"] = new SelectList(_context.Artists, "ID", "Name");
            return View();
        }

        // POST: Albums/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ArtistID,Title,ReleaseDate")] Album album)
        {
            if (ModelState.IsValid)
            {
                _context.Add(album);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ArtistID"] = new SelectList(_context.Artists, "ID", "ID", album.ArtistID);
            return View(album);
        }

        // GET: Albums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _context.Albums.SingleOrDefaultAsync(m => m.ID == id);
            if (album == null)
            {
                return NotFound();
            }
            ViewData["Artists"] = new SelectList(_context.Artists, "ID", "Name", album.ArtistID);
            return View(album);
        }

        // POST: Albums/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ArtistID,Title,ReleaseDate")] Album album)
        {
            if (id != album.ID)
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
                    if (!AlbumExists(album.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", new { id = album.ID });
            }
            ViewData["ArtistID"] = new SelectList(_context.Artists, "ID", "ID", album.ArtistID);
            return View(album);
        }

        // GET: Albums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _context.Albums
                .Include(a => a.Artist)
                .SingleOrDefaultAsync(m => m.ID == id);
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
            var album = await _context.Albums.SingleOrDefaultAsync(m => m.ID == id);
            _context.Albums.Remove(album);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AlbumExists(int id)
        {
            return _context.Albums.Any(e => e.ID == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTrack([Bind("ID,AlbumID,Title")] Track track)
        {
            if (ModelState.IsValid)
            {
                _context.Tracks.Add(track);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = track.AlbumID });
            }
            return RedirectToAction("Create", "Tracks");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTrack(int id)
        {
            var track = await _context.Tracks.SingleOrDefaultAsync(t => t.ID == id);
            if (track != null)
            {
                _context.Tracks.Remove(track);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = track.AlbumID });
            }
            else
                return NotFound();
        }
    }
}
