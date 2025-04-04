using StokTakipApp.BusinessLayer.Abstract;
using StokTakipApp.DataAccessLayer.Abstract;
using StokTakipApp.EntityLayer.Concrete;
using System.Collections.Generic;

namespace StokTakipApp.BusinessLayer.Concrete
{
    public class BrandManager : IBrandService
    {
        private readonly IBrandDal _brandDal;

        public BrandManager(IBrandDal brandDal)
        {
            _brandDal = brandDal;
        }

        public void TDelete(Brand entity)
        {
          _brandDal.Delete(entity);
        }

        public List<Brand> TGetAll()
        {
           return _brandDal.GetAll();
        }

        public Brand TGetById(int id)
        {
            return _brandDal.GetById(id);
        }

        public void TInsert(Brand entity)
        {
            _brandDal.Insert(entity);
        }

        public void TUpdate(Brand entity)
        {
            _brandDal.Update(entity);
        }
    }
}
