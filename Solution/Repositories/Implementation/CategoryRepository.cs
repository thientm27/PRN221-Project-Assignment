using System.Collections.Generic;
using BussinessObject.Models;
using DataAccessObject;

namespace Repositories.Implementation
{
    public class CategoryRepository : ICategoryRepository

    {
        public List<Category> GetAllCategory()
        {
            return CategoryDao.Instance.GetAllCategory();
        }
    }
}