using Server.Bll.Interfaces;
using Server.Dal.Interfaces;
using Server.Models;
using Server.Models.DTO;

namespace Server.Bll
{
    public class TicketSevice : ITicketService
    {
        private readonly ITicketDal  _ticketDal;
        private readonly IGiftDal _giftDal;

        public TicketSevice(ITicketDal ticketDal, IGiftDal giftDal)
        {
            _ticketDal = ticketDal;
            _giftDal = giftDal;
        }
        public async Task<IEnumerable<Ticket>> Get()
        {
            return await _ticketDal.Get();
        }

        public async Task<IEnumerable<Ticket>> GetByUserPaid()
        {
            return await _ticketDal.GetByUserPaid();
        }

        public async Task<IEnumerable<Ticket>> GetByUserPending()
        {
            return await _ticketDal.GetByUserPending();
        }

        public async Task<Ticket> Get(int id)
        {
            return await _ticketDal.Get(id);
        }

        public async Task Add(Ticket ticket)
        {
            await _ticketDal.Add(ticket);
        }

        public async Task pay(int id)
        {
            await _ticketDal.pay(id);
        }

        public async Task Delete(int id)
        {
            await _ticketDal.Delete(id);
        }
        
    }
}
