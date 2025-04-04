using StokTakipApp.DataAccessLayer.Abstract;
using StokTakipApp.DataAccessLayer.Context;
using StokTakipApp.DataAccessLayer.Repositories;
using StokTakipApp.EntityLayer.Concrete;

namespace StokTakipApp.DataAccessLayer.EntityFramework
{
    public class EfProductDal:GenericRepository<Product>, IProductDal
    {
        public EfProductDal(AppDbContext context) : base(context)
        {
        }
    }
}
