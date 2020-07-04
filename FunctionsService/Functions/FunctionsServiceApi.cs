using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using DataAccess.Operations;
using AutoMapper;
using FunctionsService.Models;
using System.Collections.Generic;
using DataAccess.Models;
using System.Text.Json;
using System.Net.Http;

namespace FunctionsService.Functions
{
    public  class FunctionsServiceApi
    {
        private ServiceOperations db = new ServiceOperations();
        private readonly IMapper _mapper;
        public FunctionsServiceApi(IMapper mapper)
        {
            _mapper = mapper;
        }
     
        [FunctionName("GetServices")]
        public IActionResult GetServices(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "ServiceFunctionApi")] HttpRequestMessage req,  ExecutionContext context)
        {
            /*Validate JWT*/
            if ((SecurityJwt.ValidateTokenWithRoleAsync(req.Headers.Authorization, context.FunctionAppDirectory, "Admin")) == null)
            {
                return new UnauthorizedResult();
            }
            /*Code*/
            var result = db.GetAllServices();
            List<ServiceDto> responseMessage = _mapper.Map<List<Services>, List<ServiceDto>>(result);
            return new OkObjectResult(responseMessage);
        }

        [FunctionName("GetService")]
        public IActionResult GetService(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "ServiceFunctionApi/{id:int}")] HttpRequestMessage req, int id, ExecutionContext context)
        {
            /*Validate JWT*/
            if ((SecurityJwt.ValidateTokenWithRoleAsync(req.Headers.Authorization, context.FunctionAppDirectory, "Admin")) == null)
            {
                return new UnauthorizedResult();
            }
            /*Code*/
            var result = db.GetByPk(id);
            ServiceDto responseMessage = _mapper.Map<Services, ServiceDto>(result);
            return new OkObjectResult(responseMessage);
        }

        [FunctionName("AddService")]
        public async Task<IActionResult> AddService(
            [HttpTrigger(AuthorizationLevel.Function, "Post", Route = "ServiceFunctionApi")]HttpRequestMessage req, ExecutionContext context)
        {
            /*Validate JWT*/
            if ((SecurityJwt.ValidateTokenWithRoleAsync(req.Headers.Authorization, context.FunctionAppDirectory, "Admin")) == null)
            {
                return new UnauthorizedResult();
            }
            /*Code*/
            var content = await req.Content.ReadAsStringAsync();
            ServiceDto servicesDto = JsonSerializer.Deserialize<ServiceDto>(content);
            Services servicesDb = _mapper.Map<ServiceDto, Services>(servicesDto);
            var result = db.CreateService(servicesDb);
            ServiceDto responseMessage = _mapper.Map<Services, ServiceDto>(result);
            return new OkObjectResult(responseMessage);
        }

        [FunctionName("UpdateService")]
        public async Task<IActionResult> UpdateService(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "ServiceFunctionApi/{id:int}")] HttpRequestMessage req, int id, ExecutionContext context)
        {
            /*Validate JWT*/
            if ((SecurityJwt.ValidateTokenWithRoleAsync(req.Headers.Authorization, context.FunctionAppDirectory, "Admin")) == null)
            {
                return new UnauthorizedResult();
            }
            /*Code*/
            var content = await req.Content.ReadAsStringAsync();
            ServiceDto servicesDto = JsonSerializer.Deserialize<ServiceDto>(content);
            Services servicesDb = _mapper.Map<ServiceDto, Services>(servicesDto);
            var result = db.UpdateService(servicesDb);
            ServiceDto responseMessage = _mapper.Map<Services, ServiceDto>(result);
            return new OkObjectResult(responseMessage);
        }

        [FunctionName("RemoveService")]
        public IActionResult RemoveService(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "ServiceFunctionApi/{id:int}")] HttpRequestMessage req, int id, ExecutionContext context)
        {
            /*Validate JWT*/
            if (( SecurityJwt.ValidateTokenWithRoleAsync(req.Headers.Authorization,context.FunctionAppDirectory,"Admin")) == null)
            {
                return new UnauthorizedResult();
            }
            /*Code*/
            var result = db.DeleteService(id);
            ServiceDto responseMessage = _mapper.Map<Services, ServiceDto>(result);
            return new OkObjectResult(responseMessage);
        }
    }
}
