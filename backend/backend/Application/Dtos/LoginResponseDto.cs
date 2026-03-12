namespace backend.Application.Dtos
{
    public class LoginResponseDto(string accessToken, string refreshToken)
    {
        public string accessToken { get; set; } = accessToken;
        public string refreshToken { get; set; } = refreshToken;
    }
}
