using System.Text.Json.Serialization;

namespace Root.API.Models
{
    public class FavoritePlant
    {
        public int FavoritePlantId { get; set; }
        // Foreign key for User
        [JsonIgnore]
        public string UserId { get; set; } // Change this to string
        [JsonIgnore]
        public ApplicationUser User { get; set; }
        // Foreign key for Plant
        [JsonIgnore]
        public int PlantId { get; set; }
        public Plant Plant { get; set; }
    }
}
