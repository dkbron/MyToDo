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
    public class MemoService : IMemoService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public MemoService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<ApiResponse> AddAsync(MemoDto model)
        {
            try
            {
                var meMo = mapper.Map<Memo>(model);

                var repository = unitOfWork.GetRepository<Memo>();
                await repository.InsertAsync(meMo);

                if (await unitOfWork.SaveChangesAsync() > 0)
                    return new ApiResponse(true, meMo);
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
                var repository = unitOfWork.GetRepository<Memo>();
                var meMo = await repository.GetFirstOrDefaultAsync(predicate: a => a.Id.Equals(id));

                unitOfWork.GetRepository<Memo>().Delete(meMo);

                if (await unitOfWork.SaveChangesAsync() > 0)
                    return new ApiResponse(true, meMo);
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
                var respository = unitOfWork.GetRepository<Memo>();
                var meMos = await respository.GetPagedListAsync(predicate:
                    x=>string.IsNullOrWhiteSpace(parameter.Search)?true:x.Title.Equals(parameter.Search),
                    pageIndex:parameter.PageIndex,
                    pageSize:parameter.PageSize,
                    orderBy:source=>source.OrderByDescending(t=>t.CreateDate));

                return new ApiResponse(true, meMos);

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
                var repository = unitOfWork.GetRepository<Memo>();
                var meMo = await repository.GetFirstOrDefaultAsync(predicate: a => a.Id.Equals(id));

                return new ApiResponse(true, meMo);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }

        }

        public async Task<ApiResponse> UpdateAsync(MemoDto model)
        {
            try
            {
                var dbMeMo = mapper.Map<Memo>(model);

                var repository = unitOfWork.GetRepository<Memo>();

                var meMo = repository.GetFirstOrDefault(predicate: a => a.Id.Equals(dbMeMo.Id));

                meMo.Title = dbMeMo.Title;
                meMo.Content = dbMeMo.Content;
                meMo.Status = dbMeMo.Status;
                meMo.UpdateDate = dbMeMo.UpdateDate;

                repository.Update(meMo);
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
