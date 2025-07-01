using Server.Bll.Interfaces;
using Server.Dal.Interfaces;
using Server.Models;
using Server.Models.DTO;
using Microsoft.Extensions.Logging;

namespace Server.Bll
{
    public class TicketSevice : ITicketService
    {
        private readonly ITicketDal  _ticketDal;
        private readonly IGiftDal _giftDal;
        private readonly ILogger<TicketSevice> _logger;

        public TicketSevice(ITicketDal ticketDal, IGiftDal giftDal, ILogger<TicketSevice> logger)
        {
            _ticketDal = ticketDal;
            _giftDal = giftDal;
            _logger = logger;
        }
        public async Task<IEnumerable<Ticket>> Get()
        {
            _logger.LogInformation("Getting all tickets");
            var result = await _ticketDal.Get();
            _logger.LogInformation($"Returned {result.Count()} tickets");
            return result;
        }
        public async Task<IEnumerable<Ticket>> GetByUserPaid()
        {
            _logger.LogInformation("Getting paid tickets for user");
            var result = await _ticketDal.GetByUserPaid();
            _logger.LogInformation($"Returned {result.Count()} paid tickets");
            return result;
        }
        public async Task<IEnumerable<Ticket>> GetByUserPending()
        {
            _logger.LogInformation("Getting pending tickets for user");
            var result = await _ticketDal.GetByUserPending();
            _logger.LogInformation($"Returned {result.Count()} pending tickets");
            return result;
        }
        public async Task<Ticket> Get(int id)
        {
            _logger.LogInformation($"Getting ticket with id {id}");
            var result = await _ticketDal.Get(id);
            _logger.LogInformation($"Returned ticket with id {id}");
            return result;
        }
        public async Task<IEnumerable<Ticket>> GetByGiftId(int id)
        {
            _logger.LogInformation($"Getting tickets for gift id {id}");
            var result = await _ticketDal.GetByGiftId(id);
            _logger.LogInformation($"Returned {result.Count()} tickets for gift id {id}");
            return result;
        }
        public async Task Add(Ticket ticket)
        {
            _logger.LogInformation($"Adding new ticket");
            await _ticketDal.Add(ticket);
            _logger.LogInformation("Ticket added successfully");
        }
        public async Task pay(int id)
        {
            _logger.LogInformation($"Paying for ticket with id {id}");
            await _ticketDal.pay(id);
            _logger.LogInformation($"Ticket with id {id} paid successfully");
        }
        public async Task Delete(int id)
        {
            _logger.LogInformation($"Deleting ticket with id {id}");
            await _ticketDal.Delete(id);
            _logger.LogInformation($"Ticket with id {id} deleted successfully");
        }
        
    }
}
