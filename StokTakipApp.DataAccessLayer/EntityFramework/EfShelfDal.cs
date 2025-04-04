using StokTakipApp.DataAccessLayer.Abstract;
using StokTakipApp.DataAccessLayer.Context;
using StokTakipApp.DataAccessLayer.Repositories;

namespace StokTakipApp.DataAccessLayer.EntityFramework
{
    public class EfShelfDal : GenericRepository<Shelf>, IShelfDal
    {
        public EfShelfDal(AppDbContext context) : base(context)
        {
        }
    }
}
