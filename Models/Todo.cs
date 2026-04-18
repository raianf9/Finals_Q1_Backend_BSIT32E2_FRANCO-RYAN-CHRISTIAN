namespace TodoApi.Models
{
    public class Todo
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool Completed { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string PreviousHash { get; set; } = "GENESIS";
        public string Hash { get; set; } = string.Empty;

        public int Nonce { get; set; }
        public string Proof { get; set; } = string.Empty;
    }
}