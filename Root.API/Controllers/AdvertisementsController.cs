using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Root.API.Dtos;
using Root.API.Interface;
using Root.API.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Root.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdvertisementsController : ControllerBase
    {
        private readonly IAdvertisement _advertisementRepository;

        public AdvertisementsController(IAdvertisement advertisementRepository)
        {
            _advertisementRepository = advertisementRepository ?? throw new ArgumentNullException(nameof(advertisementRepository));
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Advertisement>>> GetAdvertisements()
        {
            var advertisements = await _advertisementRepository.GetAdvertisementsAsync();
            return Ok(advertisements);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Advertisement>> GetAdvertisementById(int id)
        {
            var advertisement = await _advertisementRepository.GetAdvertisementByIdAsync(id);

            if (advertisement == null)
            {
                return NotFound();
            }

            return Ok(advertisement);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Advertisement>> CreateAdvertisement([FromForm] AdvertisementDto advertisementDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _advertisementRepository.CreateAdvertisementAsync(advertisementDto);
                return Ok("Advertisement created successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAdvertisement(int id, [FromForm] AdvertisementDto advertisementDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _advertisementRepository.UpdateAdvertisementAsync(id, advertisementDto);
                return Ok("Advertisement updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAdvertisement(int id)
        {
            try
            {
                await _advertisementRepository.DeleteAdvertisementAsync(id);
                return Ok("Advertisement deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }
    }
}
