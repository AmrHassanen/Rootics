namespace Root.API.Dtos
{
    public class UserTaskRequestDto
    {
        public int PlantId { get; set; }
        public string? TaskDescription { get; set; }
        public DateTime DueDate { get; set; }
        
    }
}
