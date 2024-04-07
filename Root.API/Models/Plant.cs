using System.Text.Json.Serialization;

namespace Root.API.Models
{
    public class Plant
    {
        public int PlantId { get; set; }
        public string? Name { get; set; }
        public string?image { get; set; }
        public string? Description { get; set; }
        [JsonIgnore]
        public ICollection<FavoritePlant> FavoritePlants { get; set; }
        [JsonIgnore]
        public ICollection<UserPlantActivity> UsersCaring { get; set; }
    }
}
