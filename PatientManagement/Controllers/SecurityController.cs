using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PatientManagement.Models;
using PatientManagement.Repository;

namespace PatientManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly ISecurityRepository _securityRepository;

        public SecurityController(ISecurityRepository securityRepository)
        {
            _securityRepository = securityRepository;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUp signUp)
        {
            var result =await _securityRepository.SignUpModelAsync(signUp);

            if(result.Succeeded)
            {
                return Ok(result.Succeeded);
            }

            return Unauthorized();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] SignIn signIn)
        {
            var result = await _securityRepository.LoginAsync(signIn);

            if(string.IsNullOrEmpty(result))
            {
                return Unauthorized(result);
            }

            return Ok(result);
        }
    }
}
