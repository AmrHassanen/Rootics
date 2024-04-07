using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Root.API.Data;
using Root.API.Dtos;
using Root.API.Interface;
using Root.API.Models;

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Root.API.Repositories
{
    public class AdvertisementRepository : IAdvertisement
    {
        private readonly ApplicationDbContext _context;
        private readonly IPhotoService _photoService;

        public AdvertisementRepository(ApplicationDbContext context, IPhotoService photoService)
        {
            _context = context;
            _photoService = photoService;
        }

        public async Task<IEnumerable<Advertisement>> GetAdvertisementsAsync()
        {
            return await _context.Advertisements.ToListAsync();
        }

        public async Task<Advertisement> GetAdvertisementByIdAsync(int id)
        {
            return await _context.Advertisements.FindAsync(id);
        }

        public async Task CreateAdvertisementAsync(AdvertisementDto advertisementDto)
        {
            if (advertisementDto.TreatmentImage == null || advertisementDto.TreatmentImage.Length == 0)
            {
                throw new ArgumentException("Treatment image is required.", nameof(advertisementDto.TreatmentImage));
            }

            // Upload image to Cloudinary using the PhotoService
            var uploadResult = await _photoService.AddPhotoAsync(advertisementDto.TreatmentImage);

            var advertisement = new Advertisement
            {
                PlantDiseaseName = advertisementDto.PlantDiseaseName,
                MedicineName = advertisementDto.MedicineName,
                CompanyName = advertisementDto.CompanyName,
                TreatmentImage = uploadResult.SecureUrl.AbsoluteUri // Store Cloudinary image URL directly
            };

            _context.Advertisements.Add(advertisement);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAdvertisementAsync(int id, AdvertisementDto advertisementDto)
        {
            var advertisement = await _context.Advertisements.FindAsync(id);

            if (advertisement == null)
            {
                throw new ArgumentException($"Advertisement with ID {id} not found.", nameof(id));
            }

            advertisement.PlantDiseaseName = advertisementDto.PlantDiseaseName;
            advertisement.MedicineName = advertisementDto.MedicineName;
            advertisement.CompanyName = advertisementDto.CompanyName;

            _context.Update(advertisement);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAdvertisementAsync(int id)
        {
            var advertisement = await _context.Advertisements.FindAsync(id);
            if (advertisement != null)
            {
                _context.Advertisements.Remove(advertisement);
                await _context.SaveChangesAsync();
            }
        }
    }
}
