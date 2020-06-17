using System.Collections.Generic;
using Api.IServicesApi;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ServiceEmployeeApiController : ControllerBase
    {
        IServiceEmployeeService _serviceEmployeeService;
        public ServiceEmployeeApiController(IServiceEmployeeService serviceEmployeeService)
        {
            this._serviceEmployeeService = serviceEmployeeService;
        }
        // GET: api/ServiceEmployeeApiController
        [HttpGet]
        public IActionResult GetServicesEmployees()
        {
            List<ServiceEmployeeDto> reservations = this._serviceEmployeeService.GetServicesEmployees();
            return Ok(reservations);
        }

        // GET: api/ServiceEmployeeApiController/5
        [HttpGet("{id}", Name = "GetServiceEmployee")]
        public IActionResult GetServiceEmployee(int id)
        {
            var result = this._serviceEmployeeService.GetServiceEmployeeById(id);
            return Ok(result);
        }

        // POST: api/ServiceEmployeeApiController
        [HttpPost]
        public IActionResult Post([FromBody] ServiceEmployeeDto value)
        {
            var result = this._serviceEmployeeService.AddServiceEmployee(value);
            return Ok(result);
        }

        // PUT: api/ServiceEmployeeApiController/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] ServiceEmployeeDto value)
        {
            var result = this._serviceEmployeeService.UpdateServiceEmployee(value);
            return Ok(result);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = this._serviceEmployeeService.RemoveServiceEmployee(id);
            return Ok(result);
        }
    }
}
