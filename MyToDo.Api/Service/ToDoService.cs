using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyToDo.Api.Context;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace MyToDo.Api.Service
{
    public class ToDoService : IToDoService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public ToDoService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<ApiResponse> AddAsync(ToDoDto model)
        {
            try
            {
                var toDo = mapper.Map<ToDo>(model);

                var repository = unitOfWork.GetRepository<ToDo>();
                await repository.InsertAsync(toDo);

                if (await unitOfWork.SaveChangesAsync() > 0)
                    return new ApiResponse(true, toDo);
                return new ApiResponse("添加数据失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }

        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            try
            {
                var repository = unitOfWork.GetRepository<ToDo>();
                var toDo = await repository.GetFirstOrDefaultAsync(predicate: a => a.Id.Equals(id));

                unitOfWork.GetRepository<ToDo>().Delete(toDo);

                if (await unitOfWork.SaveChangesAsync() > 0)
                    return new ApiResponse(true, toDo);
                return new ApiResponse("删除数据失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }

        }

        public async Task<ApiResponse> GetAllAsync(QueryParameter parameter)
        {
            try
            {
                var respository = unitOfWork.GetRepository<ToDo>();
                var toDos = await respository.GetPagedListAsync(predicate:
                     x => string.IsNullOrWhiteSpace(parameter.Search) ? true : x.Title.Contains(parameter.Search),
                     pageIndex: parameter.PageIndex,
                     pageSize: parameter.PageSize,
                     orderBy: source => source.OrderByDescending(t => t.CreateDate));

                return new ApiResponse(true, toDos);

            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> GetAllAsync(ToDoParameter parameter)
        {
            try
            {
                var respository = unitOfWork.GetRepository<ToDo>();
                var toDos = await respository.GetPagedListAsync(predicate:
                     x => (string.IsNullOrWhiteSpace(parameter.Search) ? true : x.Title.Contains(parameter.Search)) &&
                     (parameter.StatusIndex==null?true:x.Status.Equals(parameter.StatusIndex)),
                     pageIndex: parameter.PageIndex,
                     pageSize: parameter.PageSize,
                     orderBy: source => source.OrderByDescending(t => t.CreateDate));

                return new ApiResponse(true, toDos);

            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> GetSingleAsync(int id)
        {
            try
            {
                var repository = unitOfWork.GetRepository<ToDo>();
                var toDo = await repository.GetFirstOrDefaultAsync(predicate: a => a.Id.Equals(id));

                return new ApiResponse(true, toDo);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }

        }

        public async Task<ApiResponse> Summary()
        {
            try
            {
                var toDoRepository = unitOfWork.GetRepository<ToDo>();
                var toDos = await toDoRepository.GetAllAsync(
                    orderBy: source => source.OrderByDescending(t => t.CreateDate));
                  

                var memoRepository = unitOfWork.GetRepository<Memo>();
                var memos = await memoRepository.GetAllAsync(
                    orderBy:source => source.OrderByDescending(t => t.CreateDate));

                SummaryDto summary = new SummaryDto()
                {
                    Summary = toDos.Count,
                    MemoCount = memos.Count, 
                };

                summary.ToDoDtos = new ObservableCollection<ToDoDto>(list: mapper.Map<List<ToDoDto>>(toDos.Where(t=>t.Status==0)));
                summary.MemoDtos = new ObservableCollection<MemoDto>(list: mapper.Map<List<MemoDto>>(memos));

                summary.CompletedCount = toDos.Count - summary.ToDoDtos.Count;
                summary.CompletedRatio = ((toDos.Count - summary.ToDoDtos.Count) / (double)toDos.Count).ToString("0%");

                return new ApiResponse(true, summary);
            }
            catch(Exception ex)
            {
                return new ApiResponse(false, ex.Message);
            }
        }

        public async Task<ApiResponse> UpdateAsync(ToDoDto model)
        {
            try
            {
                var dbToDo = mapper.Map<ToDo>(model);

                var repository = unitOfWork.GetRepository<ToDo>();

                var toDo = repository.GetFirstOrDefault(predicate: a => a.Id.Equals(dbToDo.Id));

                toDo.Title = dbToDo.Title;
                toDo.Content = dbToDo.Content;
                toDo.Status = dbToDo.Status;
                toDo.UpdateDate = dbToDo.UpdateDate;

                repository.Update(toDo);
                if (await unitOfWork.SaveChangesAsync() > 0)
                    return new ApiResponse(true, model);
                return new ApiResponse("更新数据失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }
    }
}
