using System;
using System.Collections.Generic;
using System.Linq;
using BussinessObject.Models;

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
            return _context.Orders.ToList();
        }

        public void DeleteOrder(int id)
        {
            var customer = _context.Orders.Where(c => c.OrderId == id).ToList()[0];
            _context.Orders.Remove(customer);
            _context.SaveChanges();
        }
    }
}