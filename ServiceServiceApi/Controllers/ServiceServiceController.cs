using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceServiceApi.IServicesApi;
using ServiceServiceApi.Models;

namespace ServiceServiceApi.Controllers
{
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = "Key")]
    [ApiController]
    public class ServiceServiceController : ControllerBase
    {

        IServiceService _serviceServices;
        public ServiceServiceController(IServiceService serviceServices)
        {
            this._serviceServices = serviceServices;
        }

        // GET: api/Service
        [HttpGet]
        public IActionResult GetServices()
        {
            List<ServiceDto> services = _serviceServices.GetServices();
            return Ok(services);
        }

        // GET: api/Service/5
        [HttpGet("{id}", Name = "GetService")]
        public IActionResult GetService(int id)
        {
            var result = _serviceServices.GetServiceById(id);
            return Ok(result);
        }

        // POST: api/Service
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Post([FromBody] ServiceDto value)
        {
            var result = _serviceServices.AddService(value);
            return Ok(result);
        }

        // PUT: api/Service/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Put(int id, [FromBody] ServiceDto value)
        {
            var result = _serviceServices.UpdateService(value);
            return Ok(result);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var result = _serviceServices.RemoveService(id);
            return Ok(result);
        }
    }
}
