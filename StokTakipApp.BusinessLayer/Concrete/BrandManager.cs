using StokTakipApp.BusinessLayer.Abstract;
using StokTakipApp.DataAccessLayer.Abstract;
using StokTakipApp.DataAccessLayer.Context;
using StokTakipApp.EntityLayer.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Runtime.Remoting.Contexts;

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
            entity.Name = entity.Name?.Trim();
            _brandDal.Insert(entity);
        }

        public void TUpdate(Brand entity)
        {
            entity.Name = entity.Name?.Trim();
            _brandDal.Update(entity);
        }
    }
}
