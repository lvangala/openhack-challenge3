using System;

namespace Openhack_Challenge3.Models
{
    public class Feedback
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string ProductId { get; set; }
        public DateTime Timestamp { get; set; }
        public string LocationName { get; set; }
        public int Rating { get; set; }
        public string UserNotes { get; set; }
        public string Message { get; set; }

        public Feedback()
        {
            Id = Guid.NewGuid().ToString();
            Timestamp = DateTime.UtcNow;
            Message = "Staging testing"
        }
    }
}