using StokTakipApp.BusinessLayer.Abstract;
using StokTakipApp.DataAccessLayer.Abstract;
using System.Collections.Generic;

namespace StokTakipApp.BusinessLayer.Concrete
{
    public class ShelfManager : IShelfService
    {
        private readonly IShelfDal _shelfDal;

        public ShelfManager(IShelfDal shelfDal)
        {
            _shelfDal = shelfDal;
        }

        public void TDelete(Shelf entity)
        {
            _shelfDal.Delete(entity);
        }

        public List<Shelf> TGetAll()
        {
            return _shelfDal.GetAll();
        }

        public Shelf TGetById(int id)
        {
            return _shelfDal.GetById(id);
        }

        public void TInsert(Shelf entity)
        {
            _shelfDal.Insert(entity);
        }

        public void TUpdate(Shelf entity)
        {
           _shelfDal.Update(entity);
        }
    }
}
