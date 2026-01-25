using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PA.CompanyManagement.AccountingService.Application.DTOs.Requests.Types;
using PA.CompanyManagement.AccountingService.Application.Repositories.Types;
using PA.CompanyManagement.Core.Exceptions;
using System.Threading.Tasks;

namespace PA.CompanyManagement.AccountingService.Api.Rest.Controllers.Types
{
    [Route("api/expense-type")]
    [ApiController]
    public class ExpenseTypesController : ControllerBase
    {
        private readonly IExpenseTypeRepository _repository;

        public ExpenseTypesController(IExpenseTypeRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Bütün Gider Türlerini getirir.
        /// </summary>
        /// <remarks>
        /// Veritabanında bulunan bütün gider türlerini döner eğer veritabanında bir gider türü yoksa 204 ile boş bir cevap döner.
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType<string>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var response = await _repository.GetAllAsync();

                if (response.Count > 0)
                    return Ok(JsonConvert.SerializeObject(response));

                return NoContent();

            }
            catch(Exception ex) when (ex is PAContextQueryException)
            {
                return Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Server Error",
                    detail: ex.Message,
                    instance: HttpContext.Request.Path);
            }
            catch(Exception ex)
            {
                return Problem(
                   statusCode: StatusCodes.Status500InternalServerError,
                   title: "Unhandled Server Error",
                   detail: ex.Message,
                   instance: HttpContext.Request.Path); 
            }

        }


        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                if(id == Guid.Empty)
                    return Problem(
                        statusCode: StatusCodes.Status400BadRequest,
                        title: "Id değeri boş olaamaz.");


                var response = await _repository.GetAsync(id);

                if (response is null)
                    return Problem(
                        statusCode: StatusCodes.Status400BadRequest,
                        title: "Bu id değeri için veri bulunamadı!");
                    //return NotFound();

                return Ok(response);

            }
            catch (Exception ex) when (ex is PAContextQueryException)
            {
                return Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Server Error",
                    detail: ex.Message,
                    instance: HttpContext.Request.Path);
            }
            catch (Exception ex)
            {
                return Problem(
                   statusCode: StatusCodes.Status500InternalServerError,
                   title: "Unhandled Server Error",
                   detail: ex.Message,
                   instance: HttpContext.Request.Path);
            }
        }


        [HttpGet("detailed/{id:guid}")]
        public async Task<IActionResult> GetDetailed(Guid id)
        {
            try
            {
                if(id == Guid.Empty)
                    return Problem(
                        statusCode: StatusCodes.Status400BadRequest,
                        title: "Id değeri boş olaamaz.");

                var response = await _repository.GetDetailedAsync(id);

                if (response is null)
                    return Problem(
                        statusCode: StatusCodes.Status400BadRequest,
                        title: "Id değeri için veri bulunamadı!");
                        

                return Ok(response);
            }
            catch (Exception ex) when (ex is PAContextQueryException)
            {
                return Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Server Error",
                    detail: ex.Message,
                    instance: HttpContext.Request.Path);
            }
            catch (Exception ex)
            {
                return Problem(
                   statusCode: StatusCodes.Status500InternalServerError,
                   title: "Unhandled Server Error",
                   detail: ex.Message,
                   instance: HttpContext.Request.Path);
            }
        }



        [HttpPost]
        public async Task<IActionResult> Post(ExpenseTypeCreateRequest? request)
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
                    actionName: nameof(Get),
                    routeValues: new { id = response.Id },
                    value: response);

            }
            catch (Exception ex) when (ex is PAContextAddException)
            {
                return Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Server Error",
                    detail: ex.Message);
            }
            catch (Exception ex)
            {
                return Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Unhandled Server Error",
                    detail: ex.Message);
            }

        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, ExpenseTypeUpdateRequest? request)
        {
            try
            {
                if(id == Guid.Empty)
                    return Problem(
                        statusCode: StatusCodes.Status400BadRequest,
                        title: "Geçersiz istek");

                if(request is null)
                    return Problem(
                        statusCode: StatusCodes.Status400BadRequest,
                        title: "İstek geçersiz!");

                if (!ModelState.IsValid)
                    return ValidationProblem(
                        statusCode: StatusCodes.Status400BadRequest,
                        title: "İstek gövdesi geçersiz!",
                        modelStateDictionary: ModelState);

                if(request.Id != id)
                    return Problem(
                        statusCode: StatusCodes.Status400BadRequest,
                        title: "ID uyuşmazlığı");

                await _repository.UpdateAsync(request);

                return NoContent();

            }
            catch (Exception ex) when (ex is PAContextUpdateException)
            {
                return Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Server Error",
                    detail: ex.Message,
                    instance: HttpContext.Request.Path);
            }
            catch (Exception ex)
            {
                return Problem(
                   statusCode: StatusCodes.Status500InternalServerError,
                   title: "Unhandled Server Error",
                   detail: ex.Message,
                   instance: HttpContext.Request.Path);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {

                if (id == Guid.Empty)
                    return Problem(
                        statusCode: StatusCodes.Status400BadRequest,
                        title: "Geçersiz istek");

                await _repository.DeleteAsync(id);

                return NoContent();

            }
            catch (Exception ex) when (ex is PAContextRemoveException)
            {
                return Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Server Error",
                    detail: ex.Message,
                    instance: HttpContext.Request.Path);
            }
            catch (Exception ex)
            {
                return Problem(
                   statusCode: StatusCodes.Status500InternalServerError,
                   title: "Unhandled Server Error",
                   detail: ex.Message,
                   instance: HttpContext.Request.Path);
            }
        }


    }
}
