using System.Collections.Generic;
using BussinessObject.Models;

namespace Repositories
{
    public interface IFlowerBouquetRepository
    {
        public List<FlowerBouquet> GetAllFlower();
        public string GetFlowerName(int id);

        public void DeleteFlower(int idFlower);

        public string UpdateFlower(FlowerBouquet oldFlower, int categoryId, string flowerName, string description,
            string unitPrice, string unitsInStock, int? supplierId);

        public string CreateFlower(int categoryId, string flowerName, string description, string unitPrice,
            string unitsInStock, int? supplierId);

        public List<FlowerBouquet> FindFlower(int findCase, string value);
    }
}