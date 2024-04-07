using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace Root.API.Models
{
    public class ApplicationUser:IdentityUser
    {
        [JsonIgnore]
        public ICollection<FavoritePlant> FavoritePlants { get; set; }
        [JsonIgnore]
        public ICollection<UserPlantActivity> PlantsInCare { get; set; }

    }
}
