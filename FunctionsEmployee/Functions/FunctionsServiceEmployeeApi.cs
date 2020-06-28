
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using DataAccess.Operations;
using AutoMapper;
using FunctionsEmployee.Models;
using System.Collections.Generic;
using DataAccess.Models;
using System.Net.Http;
using System.Text.Json;

namespace FunctionsEmployee.Functions
{
    public class FunctionsServiceEmployeeApi
    {
        private readonly IMapper _mapper;
        private ServiceEmployeeOperations db = new ServiceEmployeeOperations();
        public FunctionsServiceEmployeeApi(IMapper mapper)
        {
            _mapper = mapper;
        }

        [FunctionName("GetServicesEmployee")]
        public IActionResult GetServicesEmployee(
         [HttpTrigger(AuthorizationLevel.Function, "get", Route = "ServiceEmployeeFunctionApi")] HttpRequestMessage req, ExecutionContext context)
        {
            /*Validate JWT*/
            if ((SecurityJwt.ValidateTokenWithRoleAsync(req.Headers.Authorization, context.FunctionAppDirectory, "Admin")) == null)
            {
                return new UnauthorizedResult();
            }
            /*Code*/
            List<ServicesEmployees> servicesEmployees = db.GetAllServicesEmployees();
            List<ServiceEmployeeDto> responseMessage = _mapper.Map<List<ServicesEmployees>, List<ServiceEmployeeDto>>(servicesEmployees);
            return new OkObjectResult(responseMessage);
        }

        [FunctionName("GetServiceEmployee")]
        public IActionResult GetServiceEmployee(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "ServiceEmployeeFunctionApi/{id:int}")] HttpRequestMessage req, int id, ExecutionContext context)
        {
            /*Validate JWT*/
            if ((SecurityJwt.ValidateTokenWithRoleAsync(req.Headers.Authorization, context.FunctionAppDirectory, "Admin")) == null)
            {
                return new UnauthorizedResult();
            }
            /*Code*/
            var result = db.GetByPk(id);
            ServiceEmployeeDto responseMessage = _mapper.Map<ServicesEmployees, ServiceEmployeeDto>(result);
            return new OkObjectResult(responseMessage);
        }

        [FunctionName("AddServiceEmployee")]
        public async Task<IActionResult> AddServiceEmployee(
            [HttpTrigger(AuthorizationLevel.Function, "Post", Route = "ServiceEmployeeFunctionApi")]HttpRequestMessage req, ExecutionContext context)
        {
            /*Validate JWT*/
            if ((SecurityJwt.ValidateTokenWithRoleAsync(req.Headers.Authorization, context.FunctionAppDirectory, "Admin")) == null)
            {
                return new UnauthorizedResult();
            }
            /*Code*/
            var content = await req.Content.ReadAsStringAsync();
            ServiceEmployeeDto serviceEmployeeDto = JsonSerializer.Deserialize<ServiceEmployeeDto>(content);
            ServicesEmployees serviceEmployeeDb = _mapper.Map<ServiceEmployeeDto, ServicesEmployees>(serviceEmployeeDto);
            var result = db.CreateServiceemployee(serviceEmployeeDb);
            ServiceEmployeeDto responseMessage = _mapper.Map<ServicesEmployees, ServiceEmployeeDto>(result);
            return new OkObjectResult(responseMessage);
        }

        [FunctionName("UpdateServiceEmployee")]
        public async Task<IActionResult> UpdateServiceEmployee(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "ServiceEmployeeFunctionApi/{id:int}")] HttpRequestMessage req, int id, ExecutionContext context)
        {
            /*Validate JWT*/
            if ((SecurityJwt.ValidateTokenWithRoleAsync(req.Headers.Authorization, context.FunctionAppDirectory, "Admin")) == null)
            {
                return new UnauthorizedResult();
            }
            /*Code*/
            var content = await req.Content.ReadAsStringAsync();
            ServiceEmployeeDto serviceEmployeeDto = JsonSerializer.Deserialize<ServiceEmployeeDto>(content);
            ServicesEmployees serviceEmployeeDb = _mapper.Map<ServiceEmployeeDto, ServicesEmployees>(serviceEmployeeDto);
            var result = db.UpdateServiceemployee(serviceEmployeeDb);
            ServiceEmployeeDto responseMessage = _mapper.Map<ServicesEmployees, ServiceEmployeeDto>(result);
            return new OkObjectResult(responseMessage);
        }

        [FunctionName("RemoveServiceEmployee")]
        public IActionResult RemoveServiceEmployee(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "ServiceEmployeeFunctionApi/{id:int}")] HttpRequestMessage req, int id, ExecutionContext context)
        {
            /*Validate JWT*/
            if ((SecurityJwt.ValidateTokenWithRoleAsync(req.Headers.Authorization, context.FunctionAppDirectory, "Admin")) == null)
            {
                return new UnauthorizedResult();
            }
            /*Code*/
            var result = db.DeleteServiceemployee(id);
            ServiceEmployeeDto responseMessage = _mapper.Map<ServicesEmployees, ServiceEmployeeDto>(result);
            return new OkObjectResult(responseMessage);
        }
    }
}

