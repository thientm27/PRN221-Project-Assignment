using System.Collections.Generic;
using BussinessObject.Models;
using DataAccessObject;

namespace Repositories.Implementation
{
    public class SupplierRepository : ISupplierRepository
    {
        public List<Supplier> GetAllSupplier()
        {
            return SupplierDAO.Instance.GetAllSupplier();
        }
    }
}