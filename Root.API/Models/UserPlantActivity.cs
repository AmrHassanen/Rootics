using System.Text.Json.Serialization;

namespace Root.API.Models
{
    public class UserPlantActivity
    {
        public int UserPlantActivityId { get; set; }
        // Foreign key for User
        [JsonIgnore]
        public string UserId { get; set; } // Change this to string
        [JsonIgnore]
        public ApplicationUser User { get; set; }
        // Foreign key for Plant
        [JsonIgnore]
        public int PlantId { get; set; }
        public Plant Plant { get; set; }
        // Additional properties related to plant care
        // Task details
        public string TaskDescription { get; set; }
        public DateTime DueDate { get; set; }
        // Other properties specific to tasks
        // Image property
    }

}
