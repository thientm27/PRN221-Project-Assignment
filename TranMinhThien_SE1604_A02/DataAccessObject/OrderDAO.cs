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
            var existingOrder = _context.Orders.Find(order.OrderId);
            if (existingOrder != null)
            {
                _context.Entry(existingOrder).State = EntityState.Detached;
                existingOrder = order;
                _context.Update(existingOrder);
                _context.SaveChanges();
            }

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
            var order = _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefault(o => o.OrderId == id);

            if (order == null)
            {
                // Handle the case when the order does not exist
                return;
            }

            if (order.OrderStatus.ToUpper().Trim() == "CANCEL" || order.OrderStatus.Trim().ToUpper() == "DONE")
            {
                // Handle the case when the order is already canceled or done
                return;
            }

            foreach (var orderDetail in order.OrderDetails)
            {
                var flower = _context.FlowerBouquets.Find(orderDetail.FlowerBouquetId);
                if (flower != null)
                {
                    flower.UnitsInStock += orderDetail.Quantity;
                    _context.Entry(flower).State = EntityState.Modified;
                }
            }

            order.OrderStatus = "CANCEL";
            _context.SaveChanges();
        }

    }
}