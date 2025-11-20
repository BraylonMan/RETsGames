using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using RETsGames.Data;
using RETsGames.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RETsGames.Controllers
{
    [Authorize]
    public class GamesController : Controller
    {
        private readonly RETsGamesContext _context;
        private readonly IConfiguration _configuration;
        private readonly BlobContainerClient _containerClient;

        public GamesController(RETsGamesContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;


            var connectionString = _configuration["azureStorage"];
            var containerName = "gamesupload";
            _containerClient = new BlobContainerClient(connectionString, containerName);
            _configuration = configuration;
        }

        // GET: Games
        public async Task<IActionResult> Index()
        {
            return View(await _context.Game.ToListAsync());
        }

        // GET: Games/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game
                .Include(g => g.Purchases)   // ⭐ Load all related purchases
                .FirstOrDefaultAsync(m => m.GameId == id);

            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // GET: Games/Create
        public IActionResult Create()
        {

            ViewData["CategoryId"] = new SelectList(
             _context.Category.Select(c => new
             {
                 Id = c.CategoryId,
                 Display = c.CategoryId + " (" + c.CategoryName + ")"
             }),
            "Id", "Display"
    );

            return View();
        }

        // POST: Games/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GameId,title,Description,Location,FileName,Owner,CreationDate,CategoryId,FormFile")] Game game)
        {
            if (ModelState.IsValid)
            {
                if (game.FormFile != null && game.FormFile.Length > 0)
                {

                    var fileUpload = game.FormFile;
                    string blobName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(fileUpload.FileName);
                    var blobClient = _containerClient.GetBlobClient(blobName);

                    using (var stream = fileUpload.OpenReadStream())
                    {
                        await blobClient.UploadAsync(stream, new BlobHttpHeaders
                        {
                            ContentType = fileUpload.ContentType
                        });
                    }

                    // Save blob info to database
                    string fileURL = blobClient.Uri.ToString();
                    game.FileName = fileURL;

                }

                game.CreationDate = DateTime.UtcNow;
                _context.Add(game);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(game);
        }


        // GET: Games/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            ViewData["CategoryId"] = new SelectList(
        _context.Category.Select(c => new
        {
            Id = c.CategoryId,
            Display = c.CategoryId + " (" + c.CategoryName + ")"
        }),
        "Id",
        "Display",
        game.CategoryId // pre-select current category
    );

            return View(game);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GameId,title,Description,Location,FileName,Owner,CreationDate,CategoryId,FormFile")] Game game)
        {
            if (id != game.GameId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingGame = await _context.Game.FindAsync(id);
                    if (existingGame == null)
                    {
                        return NotFound();
                    }

                    existingGame.title = game.title;
                    existingGame.Description = game.Description;
                    existingGame.Location = game.Location;
                    existingGame.Owner = game.Owner;
                    existingGame.CategoryId = game.CategoryId;

                    if (game.FormFile != null && game.FormFile.Length > 0)
                    {
                        var fileUpload = game.FormFile;
                        string blobName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(fileUpload.FileName);
                        var blobClient = _containerClient.GetBlobClient(blobName);

                        using (var stream = fileUpload.OpenReadStream())
                        {
                            await blobClient.UploadAsync(stream, new BlobHttpHeaders
                            {
                                ContentType = fileUpload.ContentType
                            });
                        }

                        // Save blob info to database
                        string fileURL = blobClient.Uri.ToString();
                        existingGame.FileName = fileURL;

                    }

                    // Optionally add ModifiedDate instead of touching CreationDate
                    existingGame.CreationDate = existingGame.CreationDate; // preserve original


                    // Save changes
                    _context.Update(existingGame);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Game.Any(e => e.GameId == game.GameId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            
        

    



               
            // Repopulate dropdown if returning view
            ViewData["CategoryId"] = new SelectList(
                _context.Category.Select(c => new
                {
                    Id = c.CategoryId,
                    Display = c.CategoryId + " (" + c.CategoryName + ")"
                }),
                "Id", "Display",
                game.CategoryId
            );

            return View(game);
}

    // GET: Games/Delete/5
    public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game
                .FirstOrDefaultAsync(m => m.GameId == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var game = await _context.Game.FindAsync(id);
            if (game != null)
            {
                _context.Game.Remove(game);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameExists(int id)
        {
            return _context.Game.Any(e => e.GameId == id);
        }
    }
}
