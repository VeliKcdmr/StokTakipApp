using StokTakipApp.BusinessLayer.Abstract;
using StokTakipApp.DataAccessLayer.Abstract;
using StokTakipApp.EntityLayer.Concrete;
using System;
using System.Collections.Generic;

namespace StokTakipApp.BusinessLayer.Concrete
{
    public class CategoryManager : ICategoryService
    {
        private readonly ICategoryDal _categoryDal;

        public CategoryManager(ICategoryDal categoryDal)
        {
            _categoryDal = categoryDal;
        }

        public void TDelete(Category entity)
        {
           _categoryDal.Delete(entity);
        }

        public List<Category> TGetAll()
        {
          return _categoryDal.GetAll();
        }

        public Category TGetById(int id)
        {
           return _categoryDal.GetById(id);
        }

        public void TInsert(Category entity)
        {
            _categoryDal.Insert(entity);
        }

        public void TUpdate(Category entity)
        {
            _categoryDal.Update(entity);
        }
    }
}
