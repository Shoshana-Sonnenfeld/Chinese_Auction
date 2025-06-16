using Server.Bll.Interfaces;
using Server.Dal.Interfaces;
using Server.Models;
using Server.Models.DTO;

namespace Server.Bll
{
    public class DonorService:IDonorService
    {
        private readonly IDonorDal _donorDal;
        public DonorService(IDonorDal donorDal)
        {
            _donorDal = donorDal;
        }
        public async Task<IEnumerable<Donor>> Get()
        {
            return await _donorDal.Get();
        }
        public async Task<Donor> Get(int id)
        {
            return await _donorDal.Get(id);
        }
        public async Task Add(Donor donor)
        {
            await _donorDal.Add(donor);
        }
        public async Task Update(int id, DonorDTO donorDto)
        {
            await _donorDal.Update(id, donorDto);
        }
        public async Task Delete(int id)
        {
            await _donorDal.Delete(id);
        }
        public async Task<IEnumerable<Donor>> Search(string name = null, string email = null, string giftName = null)
        {
            return await _donorDal.Search(name, email, giftName);
        }
    }
}
