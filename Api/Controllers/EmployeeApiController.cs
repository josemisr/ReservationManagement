using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.IServicesApi;
using Api.Models;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeApiController : ControllerBase
    {
        IEmployeeService _employeeService;
        public EmployeeApiController(IEmployeeService employeeService)
        {
            this._employeeService = employeeService;
        }
        // GET: api/EmployeeApi
        [HttpGet]
        public IActionResult Get()
        {
            List<EmployeeDto> employees = this._employeeService.GetEmployees();
            return Ok(employees);
        }

        // GET: api/EmployeeApi/5
        [HttpGet("{id}", Name = "GetEmployees")]
        public IActionResult Get(int id)
        {
            var result = this._employeeService.GetEmployeeById(id);
            return Ok(result);
        }

        // POST: api/EmployeeApi
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Post([FromBody] EmployeeDto value)
        {
            var result = this._employeeService.AddEmployee(value);
            return Ok(result);
        }

        // PUT: api/EmployeeApi/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Put(int id, [FromBody] EmployeeDto value)
        {
            var result = this._employeeService.UpdateEmployee(value);
            return Ok(result);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var result = this._employeeService.RemoveEmployee(id);
            return Ok(result);
        }
    }
}
