using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Server.Bll.Interfaces;
using Server.Dal.Interfaces;
using Server.Models;
using Server.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiftsController : ControllerBase
    {
        private readonly IGiftService _giftService;
        private readonly ITicketService _ticketService;
        private readonly IMapper _mapper;
        private readonly ILogger<GiftsController> _logger;

        public GiftsController(IGiftService giftService, ITicketService ticketService, IMapper mapper, ILogger<GiftsController> logger)
        {
            _giftService = giftService;
            _ticketService = ticketService;
            _mapper = mapper;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Getting all gifts");
            try
            {
                var gifts = await _giftService.Get();
                _logger.LogInformation("Successfully retrieved gifts");
                return Ok(gifts);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "No gifts found");
                return NotFound(ex.Message); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting gifts");
                return StatusCode(500, "server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation($"Getting gift with id {id}");
            try
            {
                var gift = await _giftService.Get(id);
                _logger.LogInformation($"Successfully retrieved gift with id {id}");
                return Ok(gift);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Gift with id {id} not found");
                return NotFound(ex.Message); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting gift with id {id}");
                return StatusCode(500, "server error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Add([FromBody] GiftDTO giftDto)
        {
            _logger.LogInformation("Adding a new gift");
            try
            {
                if (giftDto == null || giftDto.CategoryId == 0 || giftDto.DonorId == 0 || giftDto.GiftName == null)
                {
                    return BadRequest("Gift data cannot be null.");
                }
                if (giftDto.Price < 10 || giftDto.Price > 100)
                {
                    return BadRequest("Price must be between 10 and 100.");
                }
               
                var existingGift = await _giftService.TitleExists(giftDto.GiftName);
                if (existingGift)
                {
                    return Conflict("Gift with this name already exists.");
                }

                var gift = _mapper.Map<Gift>(giftDto);
                await _giftService.Add(gift);
                _logger.LogInformation($"Successfully added gift with id {gift.Id}");
                return CreatedAtAction(nameof(Get), new { id = gift.Id }, gift);
            }
            catch(InvalidDataException ex)
            {
                _logger.LogWarning(ex, "Invalid data for gift addition");
                return BadRequest(ex.Message); 
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Conflict occurred while adding gift");
                return Conflict(ex.Message); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding a new gift");
                return StatusCode(500, "server error");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Update(int id, [FromBody] GiftDTO giftDto)
        {
            _logger.LogInformation($"Updating gift with id {id}");
            try
            {
                if (giftDto == null || giftDto.CategoryId == 0 || giftDto.DonorId == 0 || giftDto.GiftName == null )
                {
                    return BadRequest("Gift data cannot be null.");
                }
                if(giftDto.Price < 10 || giftDto.Price>100)
                {
                    return BadRequest("Price must be between 10 and 100.");
                }

                var existingGift = await _giftService.Get(id);
                if (existingGift == null)
                {
                    return NotFound($"Gift with ID {id} not found.");
                }

                var existingGiftWithSameName = await _giftService.TitleExists(giftDto.GiftName);
                if (existingGiftWithSameName && existingGift.GiftName != giftDto.GiftName)
                {
                    return Conflict("Gift with this name already exists.");
                }

                await _giftService.Update(id, giftDto);
                _logger.LogInformation($"Successfully updated gift with id {id}");
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Gift with id {id} not found for update");
                return NotFound(ex.Message); 
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Conflict occurred while updating gift");
                return Conflict(ex.Message); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating gift with id {id}");
                return StatusCode(500, "server error");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation($"Deleting gift with id {id}");
            try
            {
                var hasTickets = await _ticketService.GetByGiftId(id);
                if (hasTickets.Any())
                {
                    // מחזיר תשובה מובנית ולא זורק חריגה
                    return BadRequest("Cannot delete gift because it has associated tickets.");
                }

                var deleted = await _giftService.Delete(id);
                if (!deleted)
                    return NotFound($"Gift with ID {id} not found.");

                _logger.LogInformation($"Successfully deleted gift with id {id}");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting gift with id {id}");
                // טיפול כללי בשגיאות
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet("search")]
        public async Task<IActionResult> Search(string? giftName = null, string? donorName = null, int? buyerCount = null)
        {
            _logger.LogInformation($"Searching gifts with giftName: {giftName}, donorName: {donorName}, buyerCount: {buyerCount}");
            try
            {
                var gifts = await _giftService.Search(giftName ?? string.Empty, donorName ?? string.Empty, buyerCount);
                _logger.LogInformation("Successfully searched gifts");
                return Ok(gifts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching gifts");
                return StatusCode(500, "server error");
            }
        }

        [HttpGet("donor/{giftId}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetDonor(int giftId)
        {
            _logger.LogInformation($"Getting donor for gift with id {giftId}");
            try
            {
                var donor = await _giftService.GetDonor(giftId);
                _logger.LogInformation($"Successfully retrieved donor for gift with id {giftId}");
                return Ok(donor);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Donor for gift with id {giftId} not found");
                return NotFound(ex.Message); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting donor for gift with id {giftId}");
                return StatusCode(500, "server error");
            }
        }

        [HttpGet("sort/price")]
        public async Task<IActionResult> SortByPrice()
        {
            _logger.LogInformation("Sorting gifts by price");
            try
            {
                var gifts = await _giftService.SortByPrice();
                _logger.LogInformation("Successfully sorted gifts by price");
                return Ok(gifts);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "No gifts found to sort by price");
                return NotFound(ex.Message); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sorting gifts by price");
                return StatusCode(500, "server error");
            }
        }

        [HttpGet("sort/category")]
        public async Task<IActionResult> SortByCategory()
        {
            _logger.LogInformation("Sorting gifts by category");
            try
            {
                var gifts = await _giftService.SortByCategory();
                _logger.LogInformation("Successfully sorted gifts by category");
                return Ok(gifts);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "No gifts found to sort by category");
                return NotFound(ex.Message); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sorting gifts by category");
                return StatusCode(500, "server error");
            }
        }

        [HttpPut("raffle/{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Raffle(int id)
        {
            _logger.LogInformation($"Raffling gift with id {id}");
            try
            {
                await _giftService.raffle(id);
                _logger.LogInformation($"Successfully completed raffle for gift with id {id}");
                return Ok("Raffle completed successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Gift with id {id} not found for raffle");
                return NotFound(ex.Message); 
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation for the raffle");
                return BadRequest(ex.Message); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while conducting raffle for gift with id {id}");
                return StatusCode(500, "server error"); 
            }
        }

        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage([FromForm] FileUploadDto dto)
        {
            var file = dto.File;
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var imageUrl = $"/images/{fileName}";
            return Ok(new { imageUrl });
        }
    }
}
