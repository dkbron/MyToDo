using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using MyToDo.Api.Service;
using MyToDo.Common.Models;
using MyToDo.Shared.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Service
{
    public interface IToDoService : IBaseService<ToDoDto>
    {

        public Task<ApiResponse<PagedList<ToDoDto>>> GetAllFilterAsync(ToDoParameter parameter);
    }
}
