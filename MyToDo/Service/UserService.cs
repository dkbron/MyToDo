using MyToDo.Api.Service;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Service
{
    public class UserService : BaseService<UserDto>, IUserService
    {
        private readonly HttpRestClient client;
        public UserService(HttpRestClient client) : base(client, "User")
        {
            this.client = client;
        }

        public async Task<ApiResponse<UserDto>> LoginAsync(string account, string password)
        {
            BaseRequest baseRequest = new BaseRequest();
             

            baseRequest.Method = RestSharp.Method.GET;
            baseRequest.Route = $"api/Login/Login?account={account}&password={password}";
            
            return await client.ExcuteAsync<UserDto>(baseRequest);
        }
    }
}
