using System.Collections.Generic;
using BussinessObject.Models;

namespace Repositories
{
    public interface ICategoryRepository
    {
        public List<Category> GetAllCategory();
    }
}