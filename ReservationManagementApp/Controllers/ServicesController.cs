using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using ReservationManagementApp.ServicesApp;
using Newtonsoft.Json;
using ReservationManagementApp.Models.Dto;

namespace ReservationManagementApp.Controllers
{
    [Authorize(Policy = "Admin")]
    public class ServicesController : Controller
    {
        private readonly IConfiguration _configuration;
        public HttpServicesReponse _clientService = new HttpServicesReponse();
        public ServicesController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        // GET: Services
        public IActionResult Index()
        {
            string responseBody = this._clientService.GetResponse(this._configuration["AppSettings:ApiRest"] + "api/ServiceApi").GetAwaiter().GetResult();
            List<ServiceDto> list = JsonConvert.DeserializeObject<List<ServiceDto>>(responseBody);
            return View(list);
        }

        // GET: Services/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            string responseBody = await this._clientService.GetResponse(this._configuration["AppSettings:ApiRest"] + "api/ServiceApi/" + id);
            ServiceDto service = JsonConvert.DeserializeObject<ServiceDto>(responseBody);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
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
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Image,Description")] ServiceDto service, [FromForm] IFormFile image)
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
                string responseBody = this._clientService.PostResponse(this._configuration["AppSettings:ApiRest"] + "api/ServiceApi", JsonConvert.SerializeObject(service)).GetAwaiter().GetResult();
                if (!string.IsNullOrEmpty(responseBody))
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

            string responseBody = await this._clientService.GetResponse(this._configuration["AppSettings:ApiRest"] + "api/ServiceApi/" + id);
            ServiceDto service = JsonConvert.DeserializeObject<ServiceDto>(responseBody);
            TempData["Image"] = service.Image;
            if (service == null)
            {
                return NotFound();
            }
            return View(service);
        }

        // POST: Services/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Image,Description")] ServiceDto service, IFormFile image)
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
                        TempData.Remove("Image");
                    }
                    else if (TempData["Image"] != null)
                    {
                        service.Image = TempData["Image"].ToString();
                        TempData.Remove("Image");
                    }
                    string responseBody = this._clientService.PutResponse(this._configuration["AppSettings:ApiRest"] + "api/ServiceApi/" + id, JsonConvert.SerializeObject(service)).GetAwaiter().GetResult();

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

            string responseBody = await this._clientService.GetResponse(this._configuration["AppSettings:ApiRest"] + "api/ServiceApi/" + id);
            ServiceDto service = JsonConvert.DeserializeObject<ServiceDto>(responseBody);

            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await this._clientService.DeleteResponse(this._configuration["AppSettings:ApiRest"] + "api/ServiceApi/" + id);
            return RedirectToAction(nameof(Index));

        }

        private bool ServicesExists(int id)
        {
            string responseBody = this._clientService.GetResponse(this._configuration["AppSettings:ApiRest"] + "api/ServiceApi/" + id).GetAwaiter().GetResult();
            ServiceDto service = JsonConvert.DeserializeObject<ServiceDto>(responseBody);
            return service != null;
        }


    }
}
