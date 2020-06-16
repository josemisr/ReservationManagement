using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.IServicesApi;
using Api.Models;
using Api.ServicesApi;
using DataAccess.Models;
using DataAccess.Operations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceApiController : ControllerBase
    {

        IServiceService _serviceServices;
        public ServiceApiController(IServiceService serviceServices)
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
        public IActionResult Post([FromBody] ServiceDto value)
        {
            var result = _serviceServices.AddService(value);
            return Ok(result);
        }

        // PUT: api/Service/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] ServiceDto value)
        {
            var result = _serviceServices.UpdateService(value);
            return Ok(result);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _serviceServices.RemoveService(id);
            return Ok(result);
        }
    }
}
