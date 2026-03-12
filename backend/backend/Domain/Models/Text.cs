namespace backend.Domain.Models
{
    public class Text
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Content { get; set; }

        public Guid ChapterId { get; set; }
        public Chapter Chapter { get; set; }

        public Text() { }

        public Text(string content, Guid chapterId)
        {
            Content = content;
            ChapterId = chapterId;
        }
    }
}