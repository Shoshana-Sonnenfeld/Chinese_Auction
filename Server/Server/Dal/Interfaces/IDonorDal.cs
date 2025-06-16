using Server.Models;
using Server.Models.DTO;
using Server.Models.DTO;

namespace Server.Dal.Interfaces
{
    public interface IDonorDal
    {
        public Task<List<Donor>> Get();
        public Task<Donor> Get(int id);
        public Task Add(Donor donor);
        public Task Update(int id, DonorDTO donorDto);
        public Task Delete(int id);
        public Task<List<Donor>> Search(string name = null, string email = null, string giftName = null);
    }
}
