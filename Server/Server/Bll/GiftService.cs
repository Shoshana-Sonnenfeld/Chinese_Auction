using Server.Bll.Interfaces;
using Server.Dal.Interfaces;
using Server.Models;
using Server.Models.DTO;
using Microsoft.Extensions.Logging;

namespace Server.Bll
{
    public class GiftService : IGiftService
    {
        private readonly IGiftDal _giftDal;
        private readonly ITicketDal _ticketDal;
        private readonly ILogger<GiftService> _logger;

        public GiftService(IGiftDal giftRepository, ITicketDal ticketDal, ILogger<GiftService> logger)
        {
            _giftDal = giftRepository;
            _ticketDal = ticketDal;
            _logger = logger;
        }
        public async Task<IEnumerable<Gift>> Get()
        {
            _logger.LogInformation("Getting all gifts from DAL");
            var result = await _giftDal.Get();
            _logger.LogInformation($"Returned {result.Count()} gifts");
            return result;
        }
        public async Task<Gift> Get(int id)
        {
            _logger.LogInformation($"Getting gift with id {id} from DAL");
            var result = await _giftDal.Get(id);
            _logger.LogInformation($"Returned gift with id {id}");
            return result;
        }
        public async Task Add(Gift gift)
        {
            _logger.LogInformation($"Adding new gift: {gift?.GiftName}");
            await _giftDal.Add(gift);
            _logger.LogInformation("Gift added successfully");
        }
        public async Task Update(int id, GiftDTO gift)
        {
            _logger.LogInformation($"Updating gift with id {id}");
            await _giftDal.Update(id, gift);
            _logger.LogInformation($"Gift with id {id} updated successfully");
        }
        public async Task<bool> Delete(int id)
        {
            _logger.LogInformation($"Deleting gift with id {id}");
            var result = await _giftDal.Delete(id);
            _logger.LogInformation($"Gift with id {id} deleted: {result}");
            return result;
        }
        public async Task<IEnumerable<Gift>> Search(string? giftName = null, string? donorName = null, int? buyerCount = null)
        {
            _logger.LogInformation($"Searching gifts with giftName: {giftName}, donorName: {donorName}, buyerCount: {buyerCount}");
            var result = await _giftDal.Search(giftName ?? string.Empty, donorName ?? string.Empty, buyerCount);
            _logger.LogInformation($"Found {result.Count()} gifts");
            return result;
        }
        public async Task<Donor> GetDonor(int giftId)
        {
            _logger.LogInformation($"Getting donor for giftId {giftId}");
            var result = await _giftDal.GetDonor(giftId);
            _logger.LogInformation($"Returned donor for giftId {giftId}");
            return result;
        }
        public async Task<bool> TitleExists(string title)
        {
            _logger.LogInformation($"Checking if title exists: {title}");
            var result = await _giftDal.TitleExists(title);
            _logger.LogInformation($"Title exists: {result}");
            return result;
        }
        public async Task<IEnumerable<Gift>> SortByPrice()
        {
            _logger.LogInformation("Sorting gifts by price");
            var result = await _giftDal.SortByPrice();
            _logger.LogInformation($"Returned {result.Count()} gifts sorted by price");
            return result;
        }
        public async Task<IEnumerable<Gift>> SortByCategory()
        {
            _logger.LogInformation("Sorting gifts by category");
            var result = await _giftDal.SortByCategory();
            _logger.LogInformation($"Returned {result.Count()} gifts sorted by category");
            return result;
        }

        public async Task raffle(int id)
        {
            var gift = await _giftDal.Get(id);
            if (gift == null)
            {
                throw new InvalidOperationException("Gift not found");
            }
            if (gift.Winner != null)
            {
                throw new InvalidOperationException("Gift already won");
            }
            var numOfTickets = gift.Tickets.Count;
            if (numOfTickets == 0)
            {
                throw new InvalidOperationException("No tickets sold");
            }
            var random = new Random();
            var winnerIndex = random.Next(0, numOfTickets);
            var winnerTicket = gift.Tickets[winnerIndex];

            await _ticketDal.Win(winnerTicket.Id);
            await _giftDal.UpdateWinnerId(id, winnerTicket.UserId);
        }

    }
}
