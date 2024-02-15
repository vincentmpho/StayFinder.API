using StayFinder.API.Data;
using StayFinder.API.Models;
using StayFinder.API.Repository.Interface;

namespace StayFinder.API.Repository
{
    public class HotelsRepository : GenericRepository<Hotel>, IHotelsRepository
    {
        public HotelsRepository(StayFinderDbContext Context) : base(Context)
        {
        }
    }
}
