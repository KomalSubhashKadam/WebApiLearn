using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiLearning.DTO;
using WebApiLearning.IService;
using WebApiLearning.GenericResponse;
using WebApiLearning.Services;

namespace WebApiLearning.Controllers
{
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
    }
}
