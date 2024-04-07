using Root.API.Dtos;
using Root.API.Models;

namespace Root.API.Interface
{
    public interface IAdvertisement
    {
        Task<IEnumerable<Advertisement>> GetAdvertisementsAsync();
        Task<Advertisement> GetAdvertisementByIdAsync(int id);
        Task CreateAdvertisementAsync(AdvertisementDto advertisement);
        Task UpdateAdvertisementAsync(int id, AdvertisementDto advertisementDto);
        Task DeleteAdvertisementAsync(int id);
    }
}
