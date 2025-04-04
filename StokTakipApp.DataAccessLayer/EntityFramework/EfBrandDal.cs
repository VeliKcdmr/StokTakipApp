using StokTakipApp.DataAccessLayer.Abstract;
using StokTakipApp.DataAccessLayer.Context;
using StokTakipApp.DataAccessLayer.Repositories;
using StokTakipApp.EntityLayer.Concrete;

namespace StokTakipApp.DataAccessLayer.EntityFramework
{
    public class EfBrandDal : GenericRepository<Brand>, IBrandDal
    {
        public EfBrandDal(AppDbContext context) : base(context)
        {
        }
    }

}