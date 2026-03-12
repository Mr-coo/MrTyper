namespace backend.Application.Dtos
{
    public class LoginResponseDto(string accessToken, string refreshToken, string message)
    {
        public string accessToken { get; set; } = accessToken;
        public string refreshToken { get; set; } = refreshToken;
        public string message { get; set;  } = message;
    }
}
