namespace Root.API.Models
{
    public class Response
    {
        public string? Status { get; set; }
        public string? Message { get; set; }
        public string ?Token { get; set; }  // Add this property
    }
}
