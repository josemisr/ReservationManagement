using System.Collections.Generic;
using EmployeeServiceApi.IServicesApi;
using EmployeeServiceApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeServiceApi.Controllers
{
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = "TestKey")]
    [ApiController]
    public class EmployeeShiftServiceController : ControllerBase
    {
        IEmployeeShiftService _employeeShiftService;
        public EmployeeShiftServiceController(IEmployeeShiftService employeeShiftService)
        {
            this._employeeShiftService = employeeShiftService;
        }

        // GET: api/EmployeeShift
        [HttpGet]
        public IActionResult Get()
        {
            List<EmployeeShiftDto> employeeShifts = this._employeeShiftService.GetEmployeesShifts();
            return Ok(employeeShifts);
        }

        // GET: api/EmployeeShift/5
        [HttpGet("{id}", Name = "GetEmployeeShift")]
        public IActionResult Get(int id)
        {
            var result = this._employeeShiftService.GetEmployeeShiftById(id);
            return Ok(result);
        }

        // POST: api/EmployeeShift
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Post([FromBody] EmployeeShiftDto value)
        {
            var result = this._employeeShiftService.AddEmployeeShift(value);
            return Ok(result);
        }

        // PUT: api/EmployeeShift/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Put(int id, [FromBody] EmployeeShiftDto value)
        {
            var result = this._employeeShiftService.UpdateEmployeeShift(value);
            return Ok(result);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var result = this._employeeShiftService.RemoveEmployeeShift(id);
            return Ok(result);
        }
    }
}
