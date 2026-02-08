using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PA.CompanyManagement.AccountingService.Application.DTOs.Requests.Types;
using PA.CompanyManagement.AccountingService.Application.DTOs.Responses.Types;
using PA.CompanyManagement.AccountingService.Application.Repositories.Types;
using PA.CompanyManagement.Core.Exceptions;
using System.Net;
using System.Threading.Tasks;

namespace PA.CompanyManagement.AccountingService.Api.Rest.Controllers.Types
{
    [Route("api/income-type")]
    [ApiController]
    public class IncomeTypesController : ControllerBase
    {
        private readonly IIncomeTypeRepository _repository;

        public IncomeTypesController(IIncomeTypeRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Bütün Gider Türlerini döner.
        /// </summary>
        /// <remarks>
        /// Veritabaınında bulunan bütün gider türlerini döner eğer veritabanında bir gider türü yoksa 204 ile boş bir cevap döner.
        /// </remarks>
        [HttpGet]
        [ProducesResponseType<List<IncomeTypeResponse>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var response = await _repository.GetAllAsync();

                if (response.Count > 0)
                    //return Ok(JsonConvert.SerializeObject(response));
                    return Ok(response);

                return NoContent();
            }
            catch (Exception ex) when (ex is PAContextQueryException)
            {
                //return StatusCode(StatusCodes.Status500InternalServerError);

                return Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Server Error!",
                    detail: ex.Message,
                    instance: HttpContext.Request.Path);
            }
            catch (Exception ex)
            {
                return Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Unhandled Server Error!",
                    detail: ex.Message,
                    instance: HttpContext.Request.Path);
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return Problem(
                        statusCode: StatusCodes.Status400BadRequest,
                        title: "Id değeri boş olamaz!");

                var response = await _repository.GetAsync(id);

                if (response is null)
                    //return NotFound();
                    return Problem(
                        statusCode: StatusCodes.Status404NotFound,
                        title: "Bu id değeri için veri bulunamdı!");

                return Ok(response);
            }
            catch (Exception ex) when (ex is PAContextQueryException)
            {
                return Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Server Error!",
                    detail: ex.Message,
                    instance: HttpContext.Request.Path);
            }
            catch (Exception ex)
            {
                return Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Unhandled Server Error!",
                    detail: ex.Message,
                    instance: HttpContext.Request.Path);
            }
        }


        [HttpGet("detailed/{id:guid}")]
        public async Task<IActionResult> GetDetailed(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return Problem(
                        statusCode: StatusCodes.Status400BadRequest,
                        title: "Id değeri boş olamaz!");

                var response = await _repository.GetDetailedAsync(id);

                if (response is null)
                    return Problem(
                        statusCode: StatusCodes.Status404NotFound,
                        title: "Bu id değeri için veri bulunamdı!");

                return Ok(response);
            }
            catch (Exception ex) when (ex is PAContextQueryException)
            {
                return Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Server Error!",
                    detail: ex.Message,
                    instance: HttpContext.Request.Path);
            }
            catch (Exception ex)
            {
                return Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Unhandled Server Error!",
                    detail: ex.Message,
                    instance: HttpContext.Request.Path);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(IncomeTypeCreateRequest? request)
        {
            try
            {
                if (request is null)
                    return Problem(
                        statusCode: StatusCodes.Status400BadRequest,
                        title: "İstek geçersiz!");

                if (!ModelState.IsValid)
                    return ValidationProblem(
                        statusCode: StatusCodes.Status400BadRequest,
                        title: "İstek gövdesi geçersiz!",
                        modelStateDictionary: ModelState);

                var response = await _repository.CreateAsync(request);

                return CreatedAtAction(
                    nameof(Get),
                    new { id = response.Id },
                     response);

            }
            catch (Exception ex) when (ex is PAContextAddException)
            {
                return Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Server Error!",
                    detail: ex.Message);
            }
            catch (Exception ex)
            {
                return Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Unhandled Server Error!",
                    detail: ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, IncomeTypeUpdateRequest? request)
        {
            try
            {
                if(id == Guid.Empty)
                    return Problem(
                        statusCode: StatusCodes.Status400BadRequest,
                        title: "Geçersiz istek!");

                if (request is null)
                    return Problem(
                        statusCode: StatusCodes.Status400BadRequest,
                        title: "Geçersiz istek!");

                if (!ModelState.IsValid)
                    return ValidationProblem(
                        statusCode: StatusCodes.Status400BadRequest,
                        title: "İstek gövdesi geçersiz!",
                        modelStateDictionary: ModelState);

                if(request.Id != id)
                    return Problem(
                        statusCode: StatusCodes.Status400BadRequest,
                        title: "Id uyuşmazlığı!");

                await _repository.UpdateAsync(request);

                return NoContent();
            }
            catch (Exception ex) when (ex is PAContextUpdateException)
            {
                return Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Server Error!",
                    detail: ex.Message);
            }
            catch (Exception ex)
            {
                return Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Unhandled Server Error!",
                    detail: ex.Message);
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return Problem(
                        statusCode: StatusCodes.Status400BadRequest,
                        title: "Geçersiz istek!");

                await _repository.DeleteAsync(id);

                return NoContent();
            }
            catch (Exception ex) when (ex is PAContextRemoveException)
            {
                return Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Server Error!",
                    detail: ex.Message);
            }
            catch (Exception ex)
            {
                return Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Unhandled Server Error!",
                    detail: ex.Message);
            }
        }
    }
}
