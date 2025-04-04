using StokTakipApp.BusinessLayer.Abstract;
using StokTakipApp.DataAccessLayer.Abstract;
using StokTakipApp.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

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
            entity.Name = entity.Name?.Trim();
            _categoryDal.Insert(entity);
        }

        public void TUpdate(Category entity)
        {
            entity.Name = entity.Name?.Trim();
            _categoryDal.Update(entity);
        }
    }
}
