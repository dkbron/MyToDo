namespace MyToDo.Api.Service
{
    public class ApiResponse
    {
        public ApiResponse()
        {

        }
        public ApiResponse(string message, bool status = false)
        {
            this.Message = message;
        }

        public ApiResponse(bool status, object Result)
        {
            this.Status = status;
            this.Result = Result;
        }

        public string Message { get; set; }
        public bool Status{get; set; }
        public object Result { get; set; }

    }

    public class ApiResponse<T>
    {
        public ApiResponse()
        {

        }

        public ApiResponse(string message, bool status = false)
        {
            this.Message = message;
        }

        public ApiResponse(bool status, T Result)
        {
            this.Status = status;
            this.Result = Result;
        }
        public string Message { get; set; }
        public bool Status { get; set; }
        public T Result { get; set; }

    }
}
