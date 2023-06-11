using System;
using System.Collections.Generic;
using BussinessObject.Models;

namespace Repositories
{
    public interface IOrderRepository
    {
        public List<Order> GetOrdersByCustomer(int customerId);
        public List<Order> GetAllOrders();
        public Order GetOrderById(int id);
        public void DeleteOrder(int id);
        public int AddOrder(Order order);

        public int AddOrder(int? customerId, DateTime? shippedDate, string total, string orderStatus,
            out string message);

        public int UpdateOrder(Order oldOrder, int? customerId, DateTime? shippedDate, string total, string orderStatus,
            out string message);

        public void UpdateOrder(Order order);

        public List<Order> GetDataInRange(DateTime startTime, DateTime endTime, out string message);
    }
}