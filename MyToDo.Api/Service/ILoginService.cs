using MyToDo.Shared.Dtos;
using System.Threading.Tasks;

namespace MyToDo.Api.Service
{
    public interface ILoginService
    {
        Task<ApiResponse> Register(UserDto user);

        Task<ApiResponse> Login(string account, string password);

    }
}
