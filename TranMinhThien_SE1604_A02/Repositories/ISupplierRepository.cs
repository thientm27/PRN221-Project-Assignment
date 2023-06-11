using System.Collections.Generic;
using BussinessObject.Models;

namespace Repositories
{
    public interface ISupplierRepository
    {
        public List<Supplier> GetAllSupplier();
    }
}