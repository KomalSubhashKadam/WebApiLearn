using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiLearning.DTO;
using WebApiLearning.GenericResponse;
using WebApiLearning.IService;

namespace WebApiLearning.Controllers
{
    
    [Route("api/[controller]")] //api/auth
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authservice;

        public AuthController(IAuthService authService)
        {
            _authservice = authService;
        }
		
		[HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody]UserDTO dto)
        {
            try
            {
                var result = await _authservice.LoginUser(dto);
                if(result.Item1 == 0)
                {
                    //return NotFound(result.Item2);
                    //instead traditional return, we'll return the data using generic common response model
                    return NotFound(ResponseResult<TokenDTO>.Failed(result.Item2, result.Item2.Message));
                }
                if(result.Item1==1)
                {
                    // return BadRequest(result.Item2);
                    return Ok(ResponseResult<TokenDTO>.Success(result.Item2, result.Item2.Message));
                }

                //return Ok(result.Item2);
                return Ok(ResponseResult<TokenDTO>.Success(result.Item2, result.Item2.Message));
            }
            catch(Exception)
            {
                throw;
            }
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]UserDTO dto)
        {
            try
            {
                var result = await _authservice.RegisterUser(dto);
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
