namespace MixBalancer.Domain.Entities
{
    public class AuthResult
    {
        public bool IsSuccess { get; set; }
        public string Token { get; set; }
        public string ErrorMessage { get; set; }

        public static AuthResult Success(string token) => new AuthResult
        {
            IsSuccess = true,
            Token = token
        };

        public static AuthResult Failed(string errorMessage) => new AuthResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}