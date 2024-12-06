namespace MixBalancer.Application.Services
{
    public class ServiceResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class ServiceResult<T> : ServiceResult
    {
        public T Data { get; set; } // Renomeado para algo genérico
    }
}