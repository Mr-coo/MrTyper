namespace backend.Application.Dtos
{
    public class CommonResponseDto<T>
    {
        public string message { get; set; } = "";
        public T? result { get; set; }
    }
}
