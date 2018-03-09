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
    public class ArtistsController : Controller
    {
        private readonly MusicStoreContext _context;

        public ArtistsController(MusicStoreContext context)
        {
            _context = context;
        }

        // GET: Artists
        public async Task<IActionResult> Index(string sortOrder, string searchString, string currentSearch, int? page, int? pageSize)
        {
            IQueryable<Artist> artists = _context.Artists;

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
                artists = artists.Where(a => a.Name.Contains(searchString));
            }

            ViewData["CurrentSearch"] = searchString;

            //Perform sort
            if (!String.IsNullOrEmpty(sortOrder))
            {
                switch (sortOrder)
                {
                    case "name_asc":
                        artists = artists.OrderBy(a => a.Name);
                        break;
                    case "name_desc":
                        artists = artists.OrderByDescending(a => a.Name);
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

            return View(await PaginatedList<Artist>.CreateAsync(artists.AsNoTracking(), page ?? 1, pageSize ?? DefaultValues.PageSize));
        }

        // GET: Artists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artist = await _context.Artists
                .Include(a => a.Albums)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (artist == null)
            {
                return NotFound();
            }

            return View(new ArtistAlbumViewModel(artist, new Album()));
        }

        // GET: Artists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Artists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name")] Artist artist)
        {
            if (ModelState.IsValid)
            {
                _context.Add(artist);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(artist);
        }

        // GET: Artists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artist = await _context.Artists.SingleOrDefaultAsync(m => m.ID == id);
            if (artist == null)
            {
                return NotFound();
            }
            return View(artist);
        }

        // POST: Artists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name")] Artist artist)
        {
            if (id != artist.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(artist);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtistExists(artist.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", new { id = artist.ID });
            }
            return View(artist);
        }

        // GET: Artists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artist = await _context.Artists
                .SingleOrDefaultAsync(m => m.ID == id);
            if (artist == null)
            {
                return NotFound();
            }

            return View(artist);
        }

        // POST: Artists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var artist = await _context.Artists.SingleOrDefaultAsync(m => m.ID == id);
            _context.Artists.Remove(artist);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArtistExists(int id)
        {
            return _context.Artists.Any(e => e.ID == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAlbum([Bind("ID,ArtistID,Title")] Album album)
        {
            if (ModelState.IsValid)
            {
                _context.Albums.Add(album);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = album.ArtistID });
            }
            return RedirectToAction("Create", "Albums");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAlbum(int id)
        {
            var album = await _context.Albums.SingleOrDefaultAsync(t => t.ID == id);
            if (album != null)
            {
                _context.Albums.Remove(album);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = album.ArtistID });
            }
            else
                return NotFound();
        }
    }
}
