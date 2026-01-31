using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PA.CompanyManagement.AccountingService.Application.DTOs.Requests.Metas;
using PA.CompanyManagement.AccountingService.Application.Repositories.Metas;
using PA.CompanyManagement.Core.Exceptions;

namespace PA.CompanyManagement.AccountingService.Api.Rest.Controllers.Metas
{
    [Route("api/expense")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private IExpensRepository _repository;


        public ExpenseController(IExpensRepository repository)
        {
            _repository = repository;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            try
            {
                var response = await _repository.GetAllAsync();

                if (response.Count <= 0)
                    return NoContent();

                return Ok(response);
            }
            catch (PAContextSaveException ex)
            {
                return Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Server Error",
                    detail: ex.Message);
            }
            catch (PAContextUncatchedException ex)
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
                    title: "Server Error",
                    detail: ex.Message);
            }

        }


        [HttpGet("type/{typeId:guid}")]
        public async Task<IActionResult> GetAll(Guid typeId)
        {
            try
            {
                var response = await _repository.GetAllAsync(typeId);

                if (response.Count <= 0)
                    return NoContent();

                return Ok(response);
            }
            catch (PAContextSaveException ex)
            {
                return Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Server Error",
                    detail: ex.Message);
            }
            catch (PAContextUncatchedException ex)
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
                    title: "Server Error",
                    detail: ex.Message);
            }

        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var reponse = await _repository.GetAsync(id);
                
                if(reponse == null)
                    return NotFound();

                return Ok(reponse);
            }
            catch (PAContextQueryException ex)
            {
                return Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Server Error",
                    detail: ex.Message);
            }
            catch(Exception ex)
            {
                return Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Server Error",
                    detail: ex.Message);
            }
        }


        [HttpGet("detailed/{id:guid}")]
        public async Task<IActionResult> GetDetailed(Guid id)
        {
            try
            {
                var reponse = await _repository.GetDetailedAsync(id);

                if (reponse == null)
                    return NotFound();

                return Ok(reponse);
            }
            catch (PAContextQueryException ex)
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
                    title: "Server Error",
                    detail: ex.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Post(ExpenseCreateRequest request)
        {
            try
            {
                if(request is null)
                    return Problem(
                        statusCode: StatusCodes.Status400BadRequest,
                        title: "Request is null");

                if (!ModelState.IsValid)
                    return ValidationProblem(
                        statusCode: StatusCodes.Status400BadRequest,
                        title: "Model Invalid",
                        modelStateDictionary: ModelState);

                var response = await _repository.CreateAsync(request);

                return CreatedAtAction(
                    actionName: nameof(Get),
                    routeValues: new { id = response.Id },
                    value: response);

            }
            catch(Exception ex) when (
                ex is PAContextSaveException || 
                ex is PAContextUncatchedException)
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
                    title: "Server Error",
                    detail: ex.Message);
            }

        }


        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put(Guid id, ExpenseUpdateRequest request)
        {

            try
            {
                if (request is null)
                    return Problem(
                        statusCode: StatusCodes.Status400BadRequest,
                        title: "İstek gövdesi boş olamaz!");

                if (request.Id != id)
                    return Problem(
                        statusCode: StatusCodes.Status400BadRequest,
                        title: "Hatalı ID");

                if (!ModelState.IsValid)
                    return ValidationProblem(
                        statusCode: StatusCodes.Status400BadRequest,
                        title: "Model Geçersiz",
                        modelStateDictionary: ModelState);

                await _repository.UpdateAsync(request);

                return NoContent();

            }
            catch (Exception ex) when (
                ex is PAContextSaveException ||
                ex is PAContextUncatchedException)
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
                    title: "Server Error",
                    detail: ex.Message);
            }
        }


        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> Patch(Guid id, ExpensePatchRequest request)
        {
            try
            {
                if (request is null)
                    return Problem(
                        statusCode: StatusCodes.Status400BadRequest,
                        title: "İstek gövdesi boş olamaz!");

                if (request.Id != id)
                    return Problem(
                        statusCode: StatusCodes.Status400BadRequest,
                        title: "Hatalı ID");

                if (!ModelState.IsValid)
                    return ValidationProblem(
                        statusCode: StatusCodes.Status400BadRequest,
                        title: "Model Geçersiz",
                        modelStateDictionary: ModelState);

                await _repository.PatchAsync(request);

                return NoContent();

            }
            catch (Exception ex) when (
                ex is PAContextSaveException ||
                ex is PAContextUncatchedException)
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
                    title: "Server Error",
                    detail: ex.Message);
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _repository.DeleteAsync(id);
                return NoContent();
            }
            catch (PAContextUncatchedException ex)
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
                    title: "Server Error",
                    detail: ex.Message);


            }

        }



    }



}
