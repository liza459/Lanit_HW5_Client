using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebModelDto.Request;
using WebModelDto.Response;
using WebWorkersLogic;

namespace Lanit_HW5.Controllers
{
    [Route("[controller]")]
    public class WorkerController : ControllerBase
    {

        [HttpPost("Create")]
        public async Task<CreateWorkersResponse> Create([FromServices] ICreateWorkersCommand command,
          [FromBody] CreateWorkersRequest request)
        {
            CreateWorkersResponse response = new CreateWorkersResponse();

            if (request is null)
            {
                HttpContext.Response.StatusCode = 400;

                response.IsSuccess = false;
                response.Errors = new List<string> { "invalid request" };

                return response;
            }
            else
            {
                response = await command.ExecuteAsync(request);

                if (response.IsSuccess)
                {
                    HttpContext.Response.StatusCode = 201;
                    return response;
                }
                else
                {
                    HttpContext.Response.StatusCode = 404;
                    return response;
                }
            }
        }

        [HttpGet ("GetAll")]
        public async Task<GetWorkersResponse> GetAll([FromServices] IGetWorkersCommand command,
            [FromQuery] GetWorkersRequest request)
        {
            GetWorkersResponse response = await command.ExecuteAsync(request);
            return response;
        }

        [HttpPut("Update")]
        public async Task<UpdateWorkersResponse> Update([FromServices] IUpdateWorkersCommand command,
            [FromBody] UpdateWorkersRequest request)
        {
            if (request is null)
            {
                HttpContext.Response.StatusCode = 400;

                UpdateWorkersResponse response = new UpdateWorkersResponse();
                response.IsSuccess = false;
                response.Errors = new List<string> { "invalid request" };

                return response;
            }
            else
            {
                HttpContext.Response.StatusCode = 202;
                return await command.ExecuteAsync(request);
            }
        }

        [HttpDelete("Delete")]
        public async Task<DeleteWorkersResponse> Delete([FromServices] IDeleteWorkersCommand command,
            [FromBody] DeleteWorkersRequest request)
        { 
            DeleteWorkersResponse response = new DeleteWorkersResponse();

            if (request is null)
            {
                HttpContext.Response.StatusCode = 400;

                response.IsSuccess = false;
                response.Errors = new List<string> { "invalid request" };
            }
            else
            {

                response = await command.ExecuteAsync(request);

                if (response.IsSuccess)
                {
                    HttpContext.Response.StatusCode = 204;
                }
                else
                {
                    HttpContext.Response.StatusCode = 404;
                }
            }

            return response;
        }
    }
}
