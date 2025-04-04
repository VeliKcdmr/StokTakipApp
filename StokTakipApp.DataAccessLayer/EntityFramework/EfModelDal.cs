
using StokTakipApp.DataAccessLayer.Abstract;
using StokTakipApp.DataAccessLayer.Context;
using StokTakipApp.DataAccessLayer.Repositories;
using StokTakipApp.EntityLayer.Concrete;

namespace StokTakipApp.DataAccessLayer.EntityFramework
{
    public class EfModelDal : GenericRepository<Model>, IModelDal
    {
        public EfModelDal(AppDbContext context) : base(context)
        {
        }
    }
}
