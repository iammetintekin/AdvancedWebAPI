using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Entity.DTOs.Identity;
using Project.Services.ServiceManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBAPIFramework.ActionFilters;

namespace WEBAPIFramework.Controllers
{
    [ApiController]
    [Route("api/authentication")]
    public class AuthenticationController: ControllerBase
    {
        private readonly IServiceManager _service; 
        public AuthenticationController(IServiceManager service)
        {
            _service = service;
        }
        /// <summary>
        /// Kullanıcı oluşturma yetkili admin.
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        [HttpPost] 
        [Authorize(Roles = "Admin")]
        [ServiceFilter(typeof(ValidationFilterAttribute))] // dto doğrulaması yapıyor
        public async Task<IActionResult> RegisterUser([FromBody]UserForRegistrationDto Model)
        {
            var result = await _service.AuthenticationService.RegisterUser(Model);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.TryAddModelError(item.Code, item.Description);
                }
                return BadRequest(ModelState);
            }
            return Ok();
        }
        [HttpPost("Login")]
        [ServiceFilter(typeof(ValidationFilterAttribute))] // dto doğrulaması yapıyor
        public async Task<IActionResult> Login([FromBody] UserForAuthenticationDto Model)
        {
            if (await _service.AuthenticationService.ValidateUser(Model) == false)
                return Unauthorized();
            var created_token = await _service.AuthenticationService.CreateToken(populateExp: true); 
            return Ok(new
            {
                RefreshToken = created_token.RefreshToken,
                AccessToken = created_token.AccessToken
            });

        }
        [HttpPost("Refresh")]
   //     [ServiceFilter(typeof(ValidationFilterAttribute))] // dto doğrulaması yapıyor
        public async Task<IActionResult> Refresh([FromBody] TokenDto Model)
        {
            var tokenDtoResult = await _service.AuthenticationService.RefreshToken(Model);
            return Ok(new
            {
                RefreshToken = tokenDtoResult.RefreshToken,
                AccessToken = tokenDtoResult.AccessToken
            });
        }
    }
}
