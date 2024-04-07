namespace Root.API.Dtos
{
    public class FavoritePlantDto
    {
        public int FavoritePlantId { get; set; }
        public int PlantId { get; set; }
        public PlantDto Plant { get; set; }
    }
}
