using backend.Domain.models;
using backend.Domain.Models;

namespace backend.Domain.Models
{
    public class TypingSession
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid TextId { get; set; }
        public Text Text { get; set; }

        public int CorrectChars { get; set; }

        public DateTime StartedAt { get; set; }
        public DateTime FinishedAt { get; set; }

        public TypingSession() { }

        public TypingSession(Guid userId, Guid textId, int correctChars, DateTime startedAt, DateTime finishedAt)
        {
            UserId = userId;
            TextId = textId;
            CorrectChars = correctChars;
            StartedAt = startedAt;
            FinishedAt = finishedAt;
        }
    }
}