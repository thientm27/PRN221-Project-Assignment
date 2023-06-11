using System.Collections.Generic;
using System.Linq;
using BussinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessObject
{
    public class FlowerBouquetDAO
    {
        private static FlowerBouquetDAO instance = null;
        private static object instanceLook = new object();

        public static FlowerBouquetDAO Instance
        {
            get
            {
                lock (instanceLook)
                {
                    if (instance == null)
                    {
                        instance = new FlowerBouquetDAO();
                    }

                    return instance;
                }
            }
        }

        private readonly FUFlowerBouquetManagementContext _context = new FUFlowerBouquetManagementContext();

        public List<FlowerBouquet> GetAllFlower()
        {
            return _context.FlowerBouquets
                .Include(f => f.Category)
                .Include(f => f.Supplier)
                .Where(o => o.FlowerBouquetStatus == 1)
                .ToList();
            // return _context.FlowerBouquets.Where(f => f.FlowerBouquetStatus != 0).ToList();
        }

        public void AddFlower(FlowerBouquet flower)
        {
            var maxId = _context.FlowerBouquets.Max(c => c.FlowerBouquetId);
            flower.FlowerBouquetId = maxId + 1;
            
            _context.FlowerBouquets.Add(flower);
            _context.SaveChanges();
        }
        public FlowerBouquet GetFlowerById(int ?id)
        {
            return _context.FlowerBouquets.FirstOrDefault(item => item.FlowerBouquetId == id);
        }
        public void DeleteFlower(int id)
        {
            var flower = _context.FlowerBouquets.Where(c => c.FlowerBouquetId == id).ToList()[0];
            var isFlowerInOrder = _context.OrderDetails.Where(c => c.FlowerBouquetId == id).ToList().Count > 0;
            if (isFlowerInOrder) // have in order --> Update status
            {
                flower.FlowerBouquetStatus = 0;
                flower.Category = null;
                flower.Supplier = null;

                _context.FlowerBouquets.Update(flower);
                _context.SaveChanges();
            }
            else // Not have in order --> Remove
            {
                _context.FlowerBouquets.Remove(flower);
                _context.SaveChanges();
            }
        }

        public List<FlowerBouquet> GetCustomerByName(string name)
        {
            return _context.FlowerBouquets.Where(cus => cus.FlowerBouquetName.ToUpper().Contains(name.ToUpper())).ToList();
        }
        public List<FlowerBouquet> GetCustomerDes(string description)
        {
            return _context.FlowerBouquets.Where(cus => cus.Description.ToUpper().Contains(description.ToUpper())).ToList();
        }

        public void UpdateFlower(FlowerBouquet flower)
        {
            var existingFlower = _context.FlowerBouquets.Find(flower.FlowerBouquetId);
            if (existingFlower != null)
            {
                existingFlower.FlowerBouquetName = flower.FlowerBouquetName;
                existingFlower.FlowerBouquetStatus = 1;
                existingFlower.UnitPrice = flower.UnitPrice;
                existingFlower.UnitsInStock = flower.UnitsInStock;
                existingFlower.Description = flower.Description;
                existingFlower.CategoryId = flower.CategoryId;
                existingFlower.SupplierId = flower.SupplierId;

                
             
                _context.SaveChanges();
            }
        }

        public string GetFlowerName(int id)
        {
            return _context.FlowerBouquets.Where(p => p.FlowerBouquetId == id).ToList()[0].FlowerBouquetName;
        }
    }
}