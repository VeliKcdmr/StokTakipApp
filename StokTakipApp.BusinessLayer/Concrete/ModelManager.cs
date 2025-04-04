using StokTakipApp.BusinessLayer.Abstract;
using StokTakipApp.DataAccessLayer.Abstract;
using StokTakipApp.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakipApp.BusinessLayer.Concrete
{
    public class ModelManager : IModelService
    {
        private readonly IModelDal _modelDal;

        public ModelManager(IModelDal modelDal)
        {
            _modelDal = modelDal;
        }

        public void TDelete(Model entity)
        {
            _modelDal.Delete(entity);
        }

        public List<Model> TGetAll()
        {
            return _modelDal.GetAll();
        }

        public Model TGetById(int id)
        {
            return _modelDal.GetById(id);
        }

        public void TInsert(Model entity)
        {
            _modelDal.Insert(entity);
        }

        public void TUpdate(Model entity)
        {
           _modelDal.Update(entity);
        }
    }
}
