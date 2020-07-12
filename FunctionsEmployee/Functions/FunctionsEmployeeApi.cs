using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using AutoMapper;
using DataAccess.Operations;
using System.Net.Http;
using DataAccess.Models;
using System.Collections.Generic;
using FunctionsEmployee.Models;
using System.Text.Json;

namespace FunctionsEmployee.Functions
{
    public class FunctionsEmployeeApi
    {
        private readonly IMapper _mapper;
        private EmployeeOperations db = new EmployeeOperations();
        public FunctionsEmployeeApi(IMapper mapper)
        {
            _mapper = mapper;
        }

        [FunctionName("GetEmployees")]
        public IActionResult GetEmployees(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "EmployeeFunctionApi")] HttpRequestMessage req, ExecutionContext context)
        {
            /*Validate JWT*/
            if ((SecurityJwt.ValidateToken(req.Headers.Authorization, context.FunctionAppDirectory)) == null)
            {
                return new UnauthorizedResult();
            }
            /*Code*/
            List<Employees> employees = db.GetAllEmployees();
            List<EmployeeDto> responseMessage = _mapper.Map<List<Employees>, List<EmployeeDto>>(employees);
            return new OkObjectResult(responseMessage);
        }

        [FunctionName("GetEmployee")]
        public IActionResult GetEmployee(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "EmployeeFunctionApi/{id:int}")] HttpRequestMessage req, int id, ExecutionContext context)
        {
            /*Validate JWT*/
            if ((SecurityJwt.ValidateToken(req.Headers.Authorization, context.FunctionAppDirectory)) == null)
            {
                return new UnauthorizedResult();
            }
            /*Code*/
            var result = db.GetByPk(id);
            EmployeeDto responseMessage = _mapper.Map<Employees, EmployeeDto>(result);
            return new OkObjectResult(responseMessage);
        }

        [FunctionName("AddEmployee")]
        public async Task<IActionResult> AddEmployee(
            [HttpTrigger(AuthorizationLevel.Function, "Post", Route = "EmployeeFunctionApi")]HttpRequestMessage req, ExecutionContext context)
        {
            /*Validate JWT*/
            if ((SecurityJwt.ValidateTokenWithRoleAsync(req.Headers.Authorization, context.FunctionAppDirectory, "Admin")) == null)
            {
                return new UnauthorizedResult();
            }
            /*Code*/
            var content = await req.Content.ReadAsStringAsync();
            EmployeeDto employeeDto = JsonSerializer.Deserialize<EmployeeDto>(content);
            Employees employeeDb = _mapper.Map<EmployeeDto, Employees>(employeeDto);
            var result = db.CreateEmployee(employeeDb);
            EmployeeDto responseMessage = _mapper.Map<Employees, EmployeeDto>(result);
            return new OkObjectResult(responseMessage);
        }

        [FunctionName("UpdateEmployee")]
        public async Task<IActionResult> UpdateEmployee(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "EmployeeFunctionApi/{id:int}")] HttpRequestMessage req, int id, ExecutionContext context)
        {
            /*Validate JWT*/
            if ((SecurityJwt.ValidateTokenWithRoleAsync(req.Headers.Authorization, context.FunctionAppDirectory, "Admin")) == null)
            {
                return new UnauthorizedResult();
            }
            /*Code*/
            var content = await req.Content.ReadAsStringAsync();
            EmployeeDto employeeDto = JsonSerializer.Deserialize<EmployeeDto>(content);
            Employees employeeDb = _mapper.Map<EmployeeDto, Employees>(employeeDto);
            var result = db.UpdateEmployee(employeeDb);
            EmployeeDto responseMessage = _mapper.Map<Employees, EmployeeDto>(result);
            return new OkObjectResult(responseMessage);
        }

        [FunctionName("RemoveEmployee")]
        public IActionResult RemoveEmployee(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "EmployeeFunctionApi/{id:int}")] HttpRequestMessage req, int id, ExecutionContext context)
        {
            /*Validate JWT*/
            if ((SecurityJwt.ValidateTokenWithRoleAsync(req.Headers.Authorization, context.FunctionAppDirectory, "Admin")) == null)
            {
                return new UnauthorizedResult();
            }
            /*Code*/
            var result = db.DeleteEmployee(id);
            EmployeeDto responseMessage = _mapper.Map<Employees, EmployeeDto>(result);
            return new OkObjectResult(responseMessage);
        }
    }
}
