﻿using Arch.EntityFrameworkCore.UnitOfWork.Collections;
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
    public class ToDoService : BaseService<ToDoDto>, IToDoService
    {
        private readonly HttpRestClient client;

        public ToDoService(HttpRestClient client) : base(client, "ToDo")
        {
            this.client = client;
        }

        public async Task<ApiResponse<PagedList<ToDoDto>>> GetAllFilterAsync(ToDoParameter parameter)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.GET;
            request.Route = $"api/ToDo/getAll?pageIndex={parameter.PageIndex}" +
                $"&pageSize={parameter.PageSize}" +
                $"&search={parameter.Search}"+
                $"&StatusIndex={parameter.StatusIndex}";
            request.Parameter = parameter;
            return await client.ExcuteAsync<PagedList<ToDoDto>>(request);
        }

        public async Task<ApiResponse<SummaryDto>> GetSummaryAsync()
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.GET;
            request.Route = $"api/ToDo/getSummary";
            return await client.ExcuteAsync<SummaryDto>(request);
        }
    }
}
