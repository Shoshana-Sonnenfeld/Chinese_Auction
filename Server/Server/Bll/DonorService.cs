using Server.Bll.Interfaces;
using Server.Dal.Interfaces;
using Server.Models;
using Server.Models.DTO;
using Microsoft.Extensions.Logging;

namespace Server.Bll
{
    public class DonorService:IDonorService
    {
        private readonly IDonorDal _donorDal;
        private readonly ILogger<DonorService> _logger;
        public DonorService(IDonorDal donorDal, ILogger<DonorService> logger)
        {
            _donorDal = donorDal;
            _logger = logger;
        }
        public async Task<IEnumerable<Donor>> Get()
        {
            _logger.LogInformation("Getting all donors");
            var result = await _donorDal.Get();
            _logger.LogInformation($"Returned {result.Count()} donors");
            return result;
        }
        public async Task<Donor> Get(int id)
        {
            _logger.LogInformation($"Getting donor with id {id}");
            var result = await _donorDal.Get(id);
            _logger.LogInformation($"Returned donor with id {id}");
            return result;
        }
        public async Task Add(Donor donor)
        {
            _logger.LogInformation($"Adding new donor");
            await _donorDal.Add(donor);
            _logger.LogInformation("Donor added successfully");
        }
        public async Task Update(int id, DonorDTO donorDto)
        {
            _logger.LogInformation($"Updating donor with id {id}");
            await _donorDal.Update(id, donorDto);
            _logger.LogInformation($"Donor with id {id} updated successfully");
        }
        public async Task Delete(int id)
        {
            _logger.LogInformation($"Deleting donor with id {id}");
            await _donorDal.Delete(id);
            _logger.LogInformation($"Donor with id {id} deleted successfully");
        }
        public async Task<IEnumerable<Donor>> Search(string? name = null, string? email = null, string? giftName = null)
        {
            _logger.LogInformation($"Searching donors with name: {name}, email: {email}, giftName: {giftName}");
            var result = await _donorDal.Search(name ?? string.Empty, email ?? string.Empty, giftName ?? string.Empty);
            _logger.LogInformation($"Found {result.Count()} donors");
            return result;
        }
    }
}
