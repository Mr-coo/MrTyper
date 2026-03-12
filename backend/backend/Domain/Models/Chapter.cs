using backend.Domain.models;

namespace backend.Domain.Models
{
    public class Chapter
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public bool IsPrivate { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public Chapter() { }

        public Chapter(string name, Guid userId, bool isPrivate)
        {
            Name = name;
            UserId = userId;
            IsPrivate = isPrivate;
        }
    }
}