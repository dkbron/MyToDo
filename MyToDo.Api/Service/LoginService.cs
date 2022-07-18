using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using MyToDo.Api.Context;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MyToDo.Api.Service
{
    public class LoginService : ILoginService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public LoginService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<ApiResponse> Login(string account, string password)
        {
            try
            { 
                var user = await unitOfWork.GetRepository<User>().GetFirstOrDefaultAsync(predicate: a => a.Account.Equals(account) && a.Password.Equals(password));

                if (user == null)
                    return new ApiResponse("账号密码错误！");

                return new ApiResponse(true, user);
            }
            catch(Exception ex)
            {
                return new ApiResponse(false, ex.Message);
            }
  
        }


        public async Task<ApiResponse> Register(UserDto model)
        {
            try
            {
                var user = mapper.Map<User>(model);
                var repository = unitOfWork.GetRepository<User>();

                var userModel = await unitOfWork.GetRepository<User>().GetFirstOrDefaultAsync(predicate: a => a.Account.Equals(model.Account));

                if (userModel != null)
                    return new ApiResponse($"该账户 {model.Account} 已存在！");

                await repository.InsertAsync(user);

                if (await unitOfWork.SaveChangesAsync() > 0)
                    return new ApiResponse(true, model);
                return new ApiResponse("注册账号失败");
            }
            catch
            {
                return new ApiResponse("注册账号失败", false);
            }
        } 
    }
}
