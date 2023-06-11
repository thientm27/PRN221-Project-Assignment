using System;
using System.Collections.Generic;
using System.Linq;
using BussinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessObject
{
    public class OrderDAO
    {
        private static OrderDAO instance = null;
        private static object instanceLook = new object();

        public static OrderDAO Instance
        {
            get
            {
                lock (instanceLook)
                {
                    if (instance == null)
                    {
                        instance = new OrderDAO();
                    }

                    return instance;
                }
            }
        }

        private FUFlowerBouquetManagementContext _context = new FUFlowerBouquetManagementContext();

        public List<Order> GetOrderInRange(DateTime startDay, DateTime endDay)
        {
            return _context.Orders.Where(od => od.OrderDate >= startDay && od.OrderDate <= endDay).ToList();
        }

        public int AddOrder(Order order)
        {
            var maxId = _context.Orders.Max(c => c.OrderId);
            maxId++;
            order.OrderId = maxId;

            _context.Orders.Add(order);
            _context.SaveChanges();
            return maxId;
        }

        public void Update(Order order)
        {
            _context.Orders.Update(order);
            _context.SaveChanges();
        }

        public List<Order> GetOrdersByCustomer(int customerId)
        {
            return _context.Orders.Where(o => o.CustomerId == customerId).ToList();
        }

        public List<Order> GetAllOrder()
        {
            return _context.Orders
                .Include(f => f.Customer)
                .ToList();

        }

        public Order GetOrderById(int id)
        {
            var order = _context.Orders.Where(c => c.OrderId == id).ToList();
            if (order.Count == 0)
            {
                return null;
            }
            return order[0];
        }

        public void DeleteOrder(int id)
        {
            var order = _context.Orders.Where(o => o.OrderId == id).ToList()[0];
            if (order.OrderStatus.ToUpper().Trim().Equals("CANCEL") || order.OrderStatus.Trim().ToUpper().Equals("DONE"))
            {
                return;
            }
            var orderDetails = _context.OrderDetails.Where(o => o.OrderId == id).ToList();
            // Not delete, return flower to stock
            foreach (var orderDetail in orderDetails)
            {
                var flower = FlowerBouquetDAO.Instance.GetFlowerById(orderDetail.FlowerBouquetId);
                flower.UnitsInStock += orderDetail.Quantity;
                _context.FlowerBouquets.Update(flower);
                _context.SaveChanges();
            }
            order.OrderStatus = "Cancel";
           
            _context.Orders.Update(order);
            _context.SaveChanges();
        }
    }
}