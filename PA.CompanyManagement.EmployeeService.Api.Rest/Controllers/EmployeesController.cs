using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PA.CompanyManagement.EmployeeService.Application.DTOs.Requests;
using PA.CompanyManagement.EmployeeService.Application.Repositories;

namespace PA.CompanyManagement.EmployeeService.Api.Rest.Controllers
{
    [Route("api/employee")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _repository;

        public EmployeesController(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var response = await _repository.GetAllAsync();

                if (response.Count > 0)
                    return Ok(response);

                return NoContent();
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
                var response = await _repository.GetAsync(id);

                if (response is null)
                    return NotFound();

                return Ok(response);
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
                var response = await _repository.GetDetailedAsync(id);

                if (response is null)
                    return NotFound();

                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(EmployeeCreateRequest request)
        {
            try
            {
                if (request is null)
                    return Problem(
                        statusCode: StatusCodes.Status400BadRequest);

                if (!ModelState.IsValid)
                    return ValidationProblem(
                        statusCode: StatusCodes.Status400BadRequest,
                        modelStateDictionary: ModelState);

                var response = await _repository.CreateAsync(request);

                return CreatedAtAction(
                    nameof(GetAllAsync), 
                    new { id = response.Id }, 
                    response);
            }
            catch (Exception ex)
            {
                return Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: ex.Message);
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> PutAsync(Guid id, EmployeeUpdateRequest request)
        {
            try
            {
                if (request is null)
                    return Problem(
                        statusCode: StatusCodes.Status400BadRequest);

                if (!ModelState.IsValid)
                    return ValidationProblem(
                        statusCode: StatusCodes.Status400BadRequest,
                        modelStateDictionary: ModelState);

                await _repository.UpdateAsync(request);

                return NoContent();
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
