using Microsoft.AspNetCore.Mvc;
using MyToDo.Api.Service;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;
using System.Threading.Tasks;

namespace MyToDo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LoginController:ControllerBase
    {
        private readonly ILoginService service;
        public LoginController(ILoginService userService)
        {
            this.service = userService;
        }
         
        [HttpGet]
        public async Task<ApiResponse> Login(string account, string passWord) => await service.Login(account, passWord);

        [HttpPost]
        public async Task<ApiResponse> Register([FromBody] UserDto model) => await service.Register(model); 
    }
}
