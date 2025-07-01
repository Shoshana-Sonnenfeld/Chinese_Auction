using AutoMapper;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Server.Dal.Interfaces;
using Server.Models;
using Server.Models.DTO;
using Microsoft.Extensions.Logging;

namespace Server.Dal
{
    public class GiftDal : IGiftDal
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<GiftDal> _logger;

        public GiftDal(ApplicationDbContext context, IMapper mapper, ILogger<GiftDal> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<List<Gift>> Get()
        {
            _logger.LogInformation("Getting all gifts from database");
            var gifts = await _context.Gifts
                .Include(g => g.Category)
                .Include(g => g.Donor)
                .Include(g => g.Tickets.Where(t => t.Status != TicketStatus.Pending))
                .Include(g => g.Winner)
                .ToListAsync();

            if (gifts == null || !gifts.Any())
            {
                _logger.LogWarning("No gifts found in database");
                throw new InvalidOperationException("No gifts found.");
            }

            _logger.LogInformation($"Returned {gifts.Count} gifts from database");
            return _mapper.Map<List<Gift>>(gifts);
        }
        public async Task<Gift> Get(int id)
        {
            _logger.LogInformation($"Getting gift with id {id} from database");
            var gift = await _context.Gifts
                .Include(g => g.Category)
                .Include(g => g.Donor)
                .Include(g => g.Tickets.Where(t => t.Status != TicketStatus.Pending))
                .Include(g => g.Winner)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (gift == null)
            {
                _logger.LogWarning($"Gift with ID {id} not found in database");
                throw new KeyNotFoundException($"Gift with ID {id} not found.");
            }

            _logger.LogInformation($"Returned gift with id {id} from database");
            return gift;
        }
        public async Task Add(Gift gift)
        {
            _logger.LogInformation($"Adding gift to database: {gift?.GiftName}");
            var existCategory = await _context.Categories.FindAsync(gift.CategoryId);
            if (existCategory == null)
            {
                _logger.LogWarning($"Category with ID {gift.CategoryId} not found");
                throw new InvalidDataException($"Category with ID {gift.CategoryId} not found.");
            }
            var existDonor = await _context.Donors.FindAsync(gift.DonorId);
            if (existDonor == null)
            {
                _logger.LogWarning($"Donor with ID {gift.DonorId} not found");
                throw new InvalidDataException($"Donor with ID {gift.DonorId} not found.");
            }
            if (gift.WinnerId != null)
            {
                var existWinner = await _context.Users.FindAsync(gift.WinnerId);
                if (existWinner == null)
                {
                    _logger.LogWarning($"Winner with ID {gift.WinnerId} not found");
                    throw new InvalidDataException($"Winner with ID {gift.WinnerId} not found.");
                }
            }
            var existingGift = await TitleExists(gift.GiftName);
            if (existingGift)
            {
                _logger.LogWarning("Gift with this name already exists.");
                throw new InvalidOperationException("Gift with this name already exists.");
            }

            _context.Gifts.Add(gift);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Gift added to database successfully");
        }
        public async Task Update(int id, GiftDTO gift)
        {
            _logger.LogInformation($"Updating gift with id {id}");
            var existingGift = await _context.Gifts.FindAsync(id);
            if (existingGift == null)
            {
                _logger.LogWarning($"Gift with ID {id} not found.");
                throw new KeyNotFoundException($"Gift with ID {id} not found.");
            }
            var duplicate = await _context.Gifts.AnyAsync(g => g.GiftName == gift.GiftName && g.Id != id);
            if (duplicate)
            {
                _logger.LogWarning($"Gift with name {gift.GiftName} already exists.");
                throw new InvalidOperationException($"Gift with name {gift.GiftName} already exists.");
            }
            var existCategory = await _context.Categories.FindAsync(gift.CategoryId);
            if (existCategory == null)
            {
                _logger.LogWarning($"Category with ID {gift.CategoryId} not found");
                throw new InvalidDataException($"Category with ID {gift.CategoryId} not found.");
            }
            var existDonor = await _context.Donors.FindAsync(gift.DonorId);
            if (existDonor == null)
            {
                _logger.LogWarning($"Donor with ID {gift.DonorId} not found");
                throw new InvalidDataException($"Donor with ID {gift.DonorId} not found.");
            }
            if (gift.WinnerId != null)
            {
                var existWinner = await _context.Users.FindAsync(gift.WinnerId);
                if (existWinner == null)
                {
                    _logger.LogWarning($"Winner with ID {gift.WinnerId} not found");
                    throw new InvalidDataException($"Winner with ID {gift.WinnerId} not found.");
                }
            }
            existingGift.GiftName = gift.GiftName;
            existingGift.Details = gift.Details;
            existingGift.Price = gift.Price;
            existingGift.CategoryId = gift.CategoryId;
            existingGift.DonorId = gift.DonorId;
            existingGift.WinnerId = gift.WinnerId;
            existingGift.ImageUrl = gift.ImageUrl;
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Gift with id {id} updated successfully");
        }
        public async Task<bool> Delete(int id)
        {
            _logger.LogInformation($"Deleting gift with id {id}");
            var gift = await _context.Gifts.FindAsync(id);
            if (gift == null)
            {
                _logger.LogWarning($"Gift with ID {id} not found.");
                throw new KeyNotFoundException($"Gift with ID {id} not found.");
            }

            _context.Gifts.Remove(gift);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Gift with id {id} deleted successfully");
            return true;
        }
        public async Task<List<Gift>> Search(string? giftName = null, string? donorName = null, int? buyerCount = null)
        {
            _logger.LogInformation("Searching gifts in database");
            var query = _context.Gifts
                .Include(g => g.Category)
                .Include(g => g.Donor)
                .Include(g => g.Tickets.Where(t => t.Status != TicketStatus.Pending))
                .Include(g => g.Winner)
                .AsQueryable();

            if (!string.IsNullOrEmpty(giftName))
            {
                query = query.Where(g => g.GiftName.Contains(giftName));
            }

            if (!string.IsNullOrEmpty(donorName))
            {
                query = query.Where(g => g.Donor.Name.Contains(donorName));
            }

            if (buyerCount.HasValue)
            {
                query = query.Where(g => g.Tickets.Count == buyerCount);
            }
            var gifts = await query.ToListAsync();
            _logger.LogInformation($"Found {gifts.Count} gifts matching search criteria");
            return gifts;
        }
        public async Task<Donor> GetDonor(int giftId)
        {
            _logger.LogInformation($"Getting donor for gift with id {giftId}");
            var gift = await _context.Gifts
                .Include(g => g.Donor)
                .FirstOrDefaultAsync(g => g.Id == giftId);
            if (gift == null)
            {
                _logger.LogWarning($"Gift with ID {giftId} not found.");
                throw new KeyNotFoundException($"Gift with ID {giftId} not found.");
            }

            _logger.LogInformation($"Returned donor for gift with id {giftId}");
            return gift.Donor;
        }
        public async Task<bool> TitleExists(string title)
        {
            return await _context.Gifts.AnyAsync(g => g.GiftName == title);
        }
        public async Task<List<Gift>> SortByPrice()
        {
            _logger.LogInformation("Sorting gifts by price");
            var gifts = await _context.Gifts
                .Include(g => g.Category)
                .Include(g => g.Donor)
                .Include(g => g.Tickets.Where(t => t.Status != TicketStatus.Pending))
                .Include(g => g.Winner)
                .OrderBy(g => g.Price)
                .ToListAsync();

            if (gifts == null || !gifts.Any())
            {
                _logger.LogWarning("No gifts found to sort by price.");
                throw new InvalidOperationException("No gifts found to sort by price.");
            }

            _logger.LogInformation($"Sorted {gifts.Count} gifts by price");
            return gifts;
        }
        public async Task<List<Gift>> SortByCategory()
        {
            _logger.LogInformation("Sorting gifts by category");
            var gifts = await _context.Gifts
                .Include(g => g.Category)
                .Include(g => g.Donor)
                .Include(g => g.Tickets.Where(t => t.Status != TicketStatus.Pending))
                .Include(g => g.Winner)
                .OrderBy(g => g.Category.Name)
                .ToListAsync();

            if (gifts == null || !gifts.Any())
            {
                _logger.LogWarning("No gifts found to sort by category.");
                throw new InvalidOperationException("No gifts found to sort by category.");
            }
            _logger.LogInformation($"Sorted {gifts.Count} gifts by category");
            return gifts;
        }

        public async Task UpdateWinnerId(int id, int winnerId)
        {
            _logger.LogInformation($"Updating winner ID for gift with id {id}");
            var gift = await _context.Gifts.FindAsync(id);
            if (gift == null)
            {
                _logger.LogWarning($"Gift with ID {id} not found.");
                throw new KeyNotFoundException($"Gift with ID {id} not found.");
            }
            var winner = await _context.Users.FindAsync(winnerId);
            if (winner == null)
            {
                _logger.LogWarning($"Winner with ID {winnerId} not found.");
                throw new InvalidDataException($"Winner with ID {winnerId} not found.");
            }
            gift.WinnerId = winnerId;
            _context.Gifts.Update(gift);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Winner ID for gift with id {id} updated to {winnerId}");
        }
    }
}
