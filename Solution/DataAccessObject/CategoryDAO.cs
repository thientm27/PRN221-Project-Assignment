using System.Collections.Generic;
using System.Linq;
using BussinessObject.Models;

namespace DataAccessObject
{
    public class CategoryDao
    {
        private static CategoryDao instance = null;
        private static object instanceLook = new object();

        public static CategoryDao Instance
        {
            get
            {
                lock (instanceLook)
                {
                    if (instance == null)
                    {
                        instance = new CategoryDao();
                    }
                    return instance;
                }
            }
        }
        private readonly FUFlowerBouquetManagementContext _context = new FUFlowerBouquetManagementContext();
        
        public List<Category> GetAllCategory()
        {

            return _context.Categories.ToList();
        } 
    }
}