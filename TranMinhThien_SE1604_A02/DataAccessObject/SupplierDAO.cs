using System.Collections.Generic;
using System.Linq;
using BussinessObject.Models;

namespace DataAccessObject
{
    public class SupplierDAO
    {
        private static SupplierDAO instance = null;
        private static object instanceLook = new object();

        public static SupplierDAO Instance
        {
            get
            {
                lock (instanceLook)
                {
                    if (instance == null)
                    {
                        instance = new SupplierDAO();
                    }
                    return instance;
                }
            }
        }
        private readonly FUFlowerBouquetManagementContext _context = new FUFlowerBouquetManagementContext();
        
        public List<Supplier> GetAllSupplier()
        {
            return _context.Suppliers.ToList();
        } 
    }
}