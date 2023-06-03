using System;
using System.Collections.Generic;
using System.Linq;
using BussinessObject.Models;

namespace DataAccessObject
{
    public class OderDetailDao
    {
        private static OderDetailDao instance = null;
        private static object instanceLook = new object();

        public static OderDetailDao Instance
        {
            get
            {
                lock (instanceLook)
                {
                    if (instance == null)
                    {
                        instance = new OderDetailDao();
                    }

                    return instance;
                }
            }
        }

        private FUFlowerBouquetManagementContext _context = new FUFlowerBouquetManagementContext();

        public List<OrderDetail> GetOrderDetailByOrderId(int orderId)
        {
            return _context.OrderDetails.Where(o => o.OrderId == orderId).ToList();
        }

        public void AddOrderDetails(List<OrderDetail> orderDetails)
        {
            foreach (var orderDetail in orderDetails)
            {
                var flower = FlowerBouquetDAO.Instance.GetFlowerById(orderDetail.FlowerBouquetId);
         
                if (flower.UnitsInStock < orderDetail.Quantity)
                {
                    throw new Exception("Not enough flower " + flower.FlowerBouquetName + " id: " + flower.FlowerBouquetId);
                }
                flower.UnitsInStock -= orderDetail.Quantity;
                _context.FlowerBouquets.Update(flower);
                _context.OrderDetails.Add(orderDetail);
            }

            _context.SaveChanges();
        }

        public void UpdateOrderDetails(List<OrderDetail> orderDetails)
        {
            if (orderDetails.Count <= 0)
            {
                return;
            }

            var listOrder = _context.OrderDetails.Where(or => or.OrderId == orderDetails[0].OrderId).ToList();
            foreach (var orderDetail in listOrder)
            {
                var flower = FlowerBouquetDAO.Instance.GetFlowerById(orderDetail.FlowerBouquetId);
                flower.UnitsInStock += orderDetail.Quantity;
                _context.FlowerBouquets.Update(flower);
                _context.Remove(orderDetail);
            }

            _context.SaveChanges();
            foreach (var orderDetail in orderDetails)
            {
                var flower = FlowerBouquetDAO.Instance.GetFlowerById(orderDetail.FlowerBouquetId);
                if (flower.UnitsInStock < orderDetail.Quantity)
                {
                    throw new Exception("Not enough flower " + flower.FlowerBouquetName + " id: " + flower.FlowerBouquetId);
                }
                flower.UnitsInStock -= orderDetail.Quantity;

                _context.FlowerBouquets.Update(flower);
                _context.OrderDetails.Add(orderDetail);
            }

            _context.SaveChanges();
        }
    }
}