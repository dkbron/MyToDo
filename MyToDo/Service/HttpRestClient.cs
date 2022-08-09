using MyToDo.Api.Service;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Service
{
    public class HttpRestClient
    {
        private readonly string apiUrl;

        protected readonly RestClient client;

        public HttpRestClient(string apiUrl)
        {
            this.apiUrl = apiUrl;
            client = new RestClient();
        }

        public async Task<ApiResponse> ExcuteAsync(BaseRequest baseRequest)
        {
            var request = new RestRequest(baseRequest.Method);
            request.AddHeader("Content-Type", baseRequest.ContentType);
            if(baseRequest.Parameter!=null)
                request.AddParameter("param", JsonConvert.SerializeObject(baseRequest.Parameter),ParameterType.RequestBody);

            client.BaseUrl = new Uri(apiUrl + baseRequest.Route);
            var response = await client.ExecuteAsync(request); 
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<ApiResponse>(response.Content);
            else
            {
                return new ApiResponse()
                {
                    Status = false,
                    Message = response.ErrorMessage
                };
            }

        }

        public async Task<ApiResponse<T>> ExcuteAsync<T>(BaseRequest baseRequest)
        {
            try
            {
                var request = new RestRequest(baseRequest.Method);
                request.AddHeader("Content-Type", baseRequest.ContentType);
                if (baseRequest.Parameter != null)
                    request.AddParameter("param", JsonConvert.SerializeObject(baseRequest.Parameter), ParameterType.RequestBody); 
                client.BaseUrl = new Uri(apiUrl + baseRequest.Route);
                var response = await client.ExecuteAsync(request);

                var content = JsonConvert.DeserializeObject(response.Content);
                if (response.StatusCode==System.Net.HttpStatusCode.OK) 
                    return JsonConvert.DeserializeObject<ApiResponse<T>>(response.Content);
                else
                {
                    return new ApiResponse<T>()
                    {
                        Status = false,
                        Message = response.ErrorMessage
                    };
                } 
            }
            catch (Exception ex)
            {
                return new ApiResponse<T>("");
            }
        }

    }
}
