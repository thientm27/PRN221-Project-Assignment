using System;
using System.Collections.Generic;
using BussinessObject.Models;
using DataAccessObject;

namespace Repositories.Implementation
{
    public class FlowerBouquetRepository : IFlowerBouquetRepository
    {
        public List<FlowerBouquet> GetAllFlower()
        {
            return FlowerBouquetDAO.Instance.GetAllFlower();
        }

        public string GetFlowerName(int id)
        {
            return FlowerBouquetDAO.Instance.GetFlowerName(id);
        }

        public void DeleteFlower(int id)
        {
            FlowerBouquetDAO.Instance.DeleteFlower(id);
        }
        
        public string UpdateFlower(FlowerBouquet oldFlower, int categoryId, string flowerName, string description,
            string unitPrice,
            string unitsInStock, int? supplierId)
        {
            // Fill all the * information
           
            if ( string.IsNullOrEmpty(flowerName) ||
                string.IsNullOrEmpty(description) || string.IsNullOrEmpty(unitPrice) ||
                string.IsNullOrEmpty(unitsInStock))
            {
                return "Please fill all required information!";
            }
            
            if (categoryId == -1)
            {
                return "Please choose category!";
            }

            if (flowerName.Length > 30)
            {
                return "Name too long, below 30 character";
            }

            if (description.Length > 200)
            {
                return "Description too long, below 50 character";
            }

            // check number
            decimal unitPriceTmp;
            int unitsInStockTmp;
            try
            {
                unitPriceTmp = decimal.Parse(unitPrice);
                unitsInStockTmp = int.Parse(unitsInStock);
            }
            catch (Exception)
            {
                return "Unit price or unit in stock not valid";
            }

            var updateFlower = oldFlower;
            updateFlower.CategoryId = categoryId;
            updateFlower.FlowerBouquetName = flowerName;
            updateFlower.Description = description;
            updateFlower.UnitsInStock = unitsInStockTmp;
            updateFlower.UnitPrice = unitPriceTmp;
            updateFlower.SupplierId = supplierId != -1 ? supplierId : null;
            
            FlowerBouquetDAO.Instance.UpdateFlower(updateFlower);
            return "";
        }

        public string CreateFlower(int categoryId, string flowerName, string description, string unitPrice,
            string unitsInStock, int? supplierId)
        {
            // Fill all the * information
           
            if ( string.IsNullOrEmpty(flowerName) ||
                 string.IsNullOrEmpty(description) || string.IsNullOrEmpty(unitPrice) ||
                 string.IsNullOrEmpty(unitsInStock))
            {
                return "Please fill all required information!";
            }
            
            if (categoryId == -1)
            {
                return "Please choose category!";
            }

            if (flowerName.Length > 30)
            {
                return "Name too long, below 30 character";
            }

            if (description.Length > 200)
            {
                return "Description too long, below 50 character";
            }

            // check number
            decimal unitPriceTmp;
            int unitsInStockTmp;
            try
            {
                unitPriceTmp = decimal.Parse(unitPrice);
                unitsInStockTmp = int.Parse(unitsInStock);
            }
            catch (Exception)
            {
                return "Unit price or unit in stock not valid";
            }

            var updateFlower = new FlowerBouquet();
            updateFlower.CategoryId = categoryId;
            updateFlower.FlowerBouquetName = flowerName;
            updateFlower.Description = description;
            updateFlower.UnitsInStock = unitsInStockTmp;
            updateFlower.UnitPrice = unitPriceTmp;
            updateFlower.FlowerBouquetStatus = 1;
            updateFlower.SupplierId = supplierId != -1 ? supplierId : null;
            
            FlowerBouquetDAO.Instance.AddFlower(updateFlower);
            return "";
        }

        public List<FlowerBouquet> FindFlower(int findCase, string value)
        {
            switch (findCase)
            {
                case 0:
                {
                    return FlowerBouquetDAO.Instance.GetCustomerByName(value);
                }
                case 1:
                {
                    return FlowerBouquetDAO.Instance.GetCustomerDes(value);
                }
             
            }

            return null;
        }


        public FlowerBouquet GetFlowerById(int? id)
        {
            return FlowerBouquetDAO.Instance.GetFlowerById(id);
        }

        public void UpdateFlower(FlowerBouquet newFlower)
        {

            FlowerBouquetDAO.Instance.UpdateFlower(newFlower);
        }

        public void CreateFlower(FlowerBouquet newFlower)
        {
            newFlower.FlowerBouquetStatus = 1;
            FlowerBouquetDAO.Instance.AddFlower(newFlower);
        }
    }
}