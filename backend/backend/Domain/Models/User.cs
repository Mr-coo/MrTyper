namespace backend.Domain.models
{
    public class User
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public DateTime createdAt { get; set; }
        public decimal balance { get; set; }

        public User(string name, string username, string password)
        {
            this.name = name;
            this.username = username;
            this.password = password;
            this.createdAt = DateTime.UtcNow;
            this.balance = 0;
            this.id = Guid.NewGuid();
        }
    }
}
