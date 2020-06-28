using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using DataAccess.Operations;
using AutoMapper;
using DataAccess.Models;
using FunctionsEmployee.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;

namespace FunctionsEmployee.Functions
{
    public class FunctionsEmployeeShiftApi
    {
        private readonly IMapper _mapper;
        private EmployeeShiftOperations db = new EmployeeShiftOperations();
        public FunctionsEmployeeShiftApi(IMapper mapper)
        {
            _mapper = mapper;
        }

        [FunctionName("GetEmployeesShifts")]
        public IActionResult GetEmployeesShifts(
           [HttpTrigger(AuthorizationLevel.Function, "get", Route = "EmployeeShiftFunctionApi")] HttpRequestMessage req, ExecutionContext context)
        {
            /*Validate JWT*/
            if ((SecurityJwt.ValidateTokenWithRoleAsync(req.Headers.Authorization, context.FunctionAppDirectory, "Admin")) == null)
            {
                return new UnauthorizedResult();
            }
            /*Code*/
            var result = db.GetAllEmployeesShifts();
            List<EmployeeShiftDto> responseMessage = _mapper.Map<List<EmployeesShifts>, List<EmployeeShiftDto>>(result);
            return new OkObjectResult(responseMessage);
        }

        [FunctionName("GetEmployeeShift")]
        public IActionResult GetEmployeeShift(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "EmployeeShiftFunctionApi/{id:int}")] HttpRequestMessage req, int id, ExecutionContext context)
        {
            /*Validate JWT*/
            if ((SecurityJwt.ValidateTokenWithRoleAsync(req.Headers.Authorization, context.FunctionAppDirectory, "Admin")) == null)
            {
                return new UnauthorizedResult();
            }
            /*Code*/
            var result = db.GetByPk(id);
            EmployeeShiftDto responseMessage = _mapper.Map<EmployeesShifts, EmployeeShiftDto>(result);
            return new OkObjectResult(responseMessage);
        }

        [FunctionName("AddEmployeeShift")]
        public async Task<IActionResult> AddEmployeeShift(
            [HttpTrigger(AuthorizationLevel.Function, "Post", Route = "EmployeeShiftFunctionApi")]HttpRequestMessage req, ExecutionContext context)
        {
            /*Validate JWT*/
            if ((SecurityJwt.ValidateTokenWithRoleAsync(req.Headers.Authorization, context.FunctionAppDirectory, "Admin")) == null)
            {
                return new UnauthorizedResult();
            }
            /*Code*/
            var content = await req.Content.ReadAsStringAsync();
            EmployeeShiftDto employeeShiftDto = JsonSerializer.Deserialize<EmployeeShiftDto>(content);
            EmployeesShifts employeesShiftDb = _mapper.Map<EmployeeShiftDto, EmployeesShifts>(employeeShiftDto);
            var result = db.CreateEmployeeShift(employeesShiftDb);
            EmployeeShiftDto responseMessage = _mapper.Map<EmployeesShifts, EmployeeShiftDto>(result);
            return new OkObjectResult(responseMessage);
        }

        [FunctionName("UpdateEmployeeShift")]
        public async Task<IActionResult> UpdateEmployeeShift(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "EmployeeShiftFunctionApi/{id:int}")] HttpRequestMessage req, int id, ExecutionContext context)
        {
            /*Validate JWT*/
            if ((SecurityJwt.ValidateTokenWithRoleAsync(req.Headers.Authorization, context.FunctionAppDirectory, "Admin")) == null)
            {
                return new UnauthorizedResult();
            }
            /*Code*/
            var content = await req.Content.ReadAsStringAsync();
            EmployeeShiftDto employeeShiftDto = JsonSerializer.Deserialize<EmployeeShiftDto>(content);
            EmployeesShifts employeesShiftDb = _mapper.Map<EmployeeShiftDto, EmployeesShifts>(employeeShiftDto);
            var result = db.UpdateEmployeeShift(employeesShiftDb);
            EmployeeShiftDto responseMessage = _mapper.Map<EmployeesShifts, EmployeeShiftDto>(result);
            return new OkObjectResult(responseMessage);
        }

        [FunctionName("RemoveEmployeeShift")]
        public IActionResult RemoveEmployeeShift(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "EmployeeShiftFunctionApi/{id:int}")] HttpRequestMessage req, int id, ExecutionContext context)
        {
            /*Validate JWT*/
            if ((SecurityJwt.ValidateTokenWithRoleAsync(req.Headers.Authorization, context.FunctionAppDirectory, "Admin")) == null)
            {
                return new UnauthorizedResult();
            }
            /*Code*/
            var result = db.DeleteEmployeeShift(id);
            EmployeeShiftDto responseMessage = _mapper.Map<EmployeesShifts, EmployeeShiftDto>(result);
            return new OkObjectResult(responseMessage);
        }
    }
}

