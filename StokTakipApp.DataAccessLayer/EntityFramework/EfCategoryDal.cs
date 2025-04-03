using StokTakipApp.DataAccessLayer.Abstract;
using StokTakipApp.DataAccessLayer.Context;
using StokTakipApp.DataAccessLayer.Repositories;
using StokTakipApp.EntityLayer.Concrete;

namespace StokTakipApp.DataAccessLayer.EntityFramework
{
    public class EfCategoryDal : GenericRepository<Category>, ICategoryDal
    {
        public EfCategoryDal(AppDbContext context) : base(context)
        {
        }
    }
}
