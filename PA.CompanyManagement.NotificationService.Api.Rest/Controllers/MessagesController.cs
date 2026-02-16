using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PA.CompanyManagement.Core.Exceptions;
using PA.CompanyManagement.NotificationService.Application.DTOs.Requests;
using PA.CompanyManagement.NotificationService.Application.Repositories;

namespace PA.CompanyManagement.NotificationService.Api.Rest.Controllers
{
    [Route("api/message")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageRepository _repository;

        public MessagesController(IMessageRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var data = await _repository.GetAllAsync();

                if (data == null || data.Count <= 0)
                    return NoContent();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: ex.Message);
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            try
            {
                var data = await _repository.GetAsync(id);

                if (data is null)
                    return NotFound();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: ex.Message);
            }
        }

        [HttpGet("minimal/{id:guid}")]
        public async Task<IActionResult> GetMinimalAsync(Guid id)
        {
            try
            {
                var data = await _repository.GetMinimalAsync(id);

                if (data is null)
                    return NotFound();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: ex.Message);
            }
        }

        [HttpGet("detailed/{id:guid}")]
        public async Task<IActionResult> GetDetailedAsync(Guid id)
        {
            try
            {
                var data = await _repository.GetDetailedAsync(id);

                if (data is null)
                    return NotFound();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: ex.Message);
            }
        }

        [HttpPost("{sendMail:bool}")]
        public async Task<IActionResult> CreateAsync(MessageCreateRequest request, [FromRoute] bool sendMail = false)
        {
            try
            {
                if (!ModelState.IsValid)
                    return ValidationProblem(
                        statusCode: StatusCodes.Status400BadRequest,
                        title: "Model geçersiz",
                        modelStateDictionary: ModelState);

                var response = await _repository.CreateAsync(request, sendMail);

                if (response is null)
                    throw new PAContextAddException();

                //return CreatedAtAction(nameof(GetAsync), new { id = response.Id }, response);
                return Created(new Uri($"https://localhost:7074/api/message/{response.Id}"), response);

            }
            catch (Exception ex)
            {
                return Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: ex.Message);
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            try
            {
                await _repository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: ex.Message);
            }
        }
    }
}
