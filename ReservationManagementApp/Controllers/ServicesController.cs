using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReservationManagementApp.Enum;
using ReservationManagementApp.Authorize;
using ReservationManagementApp.Models;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace ReservationManagementApp.Controllers
{
    [Authorize(Policy = "Admin")]
    public class ServicesController : Controller
    {
        private readonly ReservationManagementDbContext _context;

        public ServicesController(ReservationManagementDbContext context)
        {
            _context = context;
        }

        // GET: Services
        public async Task<IActionResult> Index()
        {
            return View(await _context.Services.ToListAsync());
        }

        // GET: Services/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var services = await _context.Services
                .FirstOrDefaultAsync(m => m.Id == id);
            if (services == null)
            {
                return NotFound();
            }

            return View(services);
        }

        // GET: Services/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Services/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Image,Description")] Services service, [FromForm] IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    //upload files to wwwroot
                    var fileName = Path.GetFileName(image.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory() + "/wwwroot/Resources/", "Images", fileName);
                    service.Image = "/Resources/Images/" + fileName;
                    using (var fileSteam = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(fileSteam);
                    }
                }
                _context.Add(service);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(service);
        }

        // GET: Services/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var services = await _context.Services.FindAsync(id);
            if (services == null)
            {
                return NotFound();
            }
            return View(services);
        }

        // POST: Services/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Image,Description")] Services service, IFormFile image)
        {
            if (id != service.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (image != null)
                    {
                        //upload files to wwwroot
                        var fileName = Path.GetFileName(image.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory() + "/wwwroot/Resources/", "Images", fileName);
                        service.Image = "/Resources/Images/" + fileName;
                        using (var fileSteam = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(fileSteam);
                        }
                    }
                    _context.Update(service);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServicesExists(service.Id))
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
            return View(service);
        }

        // GET: Services/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var services = await _context.Services
                .FirstOrDefaultAsync(m => m.Id == id);
            if (services == null)
            {
                return NotFound();
            }

            return View(services);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var services = await _context.Services.FindAsync(id);
            _context.Services.Remove(services);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServicesExists(int id)
        {
            return _context.Services.Any(e => e.Id == id);
        }


    }
}
