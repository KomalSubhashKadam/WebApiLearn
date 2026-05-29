using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiLearning.DTO;
using WebApiLearning.GenericResponse;
using WebApiLearning.IService;
using WebApiLearning.Services;

namespace WebApiLearning.Controllers
{

    [Authorize]
	[Route("api/[controller]")]
    [ApiController]
    public class EmployeeController(IEmployeeService _employeeservice) : ControllerBase
    {
		[HttpGet("GetAllEmployees")]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                var result = await _employeeservice.GetAllEmployeeAsync();
                if(!result.Item2.Any())
                {
                    return Ok(ResponseResult<List<EmployeeDTO>>.Failed(null, "No employee found"));
                }
                return Ok(ResponseResult<List<EmployeeDTO>>.Success(result.Item2, "Employees found"));
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        [HttpGet("GetEmployeeByID/{id}")]
        public async Task<IActionResult> GetEmpById([FromRoute]int id)
        {
            try
            {
                var result = await _employeeservice.GetEmpByID(id);
                if(result.Item1 == 0)
                {
                    return Ok(ResponseResult<EmployeeDTO>.Failed(null, "Employeenot found"));
                }
                return Ok(ResponseResult<EmployeeDTO>.Success(result.Item2, "Employee found"));

            }
            catch(Exception ex)
            {
                throw;
            }
        }

		
		[HttpPost("CreateEmployee")]
        public async Task<IActionResult> CreateEmployee([FromBody]EmployeeDTO empdto)
        {
            try
            {
                var result = await _employeeservice.CreateEmployee(empdto);
                if(result.Item1 == 0 || result.Item1 == null)
                {

                    return Ok(ResponseResult<string>.Failed(null, result.Item2));
                }
                return Ok(ResponseResult<string>.Success(null,result.Item2));
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        [HttpPut("UpdateEmployee")]
        public async Task<IActionResult> UpdateEmployee([FromBody] EmployeeDTO empdto)
        {
            try
            {
                
                var result = await _employeeservice.UpdateEmployee(empdto);
                if(result.Item1 == 0 || result.Item1 == null)
                {
                    return Ok(ResponseResult<string>.Failed(null, result.Item2));
                }
                return Ok(ResponseResult<string>.Success(null, result.Item2));
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        [HttpDelete("DeleteEmployee")]
        public async Task<IActionResult> DeleteEmployee([FromBody] EmployeeDTO empdto)
        {
            try
            {
                var result = await _employeeservice.DeleteEmployee(empdto);
                if(result.Item1 == 0)
                {
                    return Ok(ResponseResult<string>.Failed(null, result.Item2));
                }
                return Ok(ResponseResult<string>.Success(null, result.Item2));
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}
