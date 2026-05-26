using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiLearning.DTO;
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
                    return NotFound(result.Item2);
                }
                if(result.Item1==1)
                {
                    return BadRequest(result.Item2);
                }

                return Ok(result.Item2);
            }
            catch(Exception)
            {
                throw;
            }
        }

    }
}
