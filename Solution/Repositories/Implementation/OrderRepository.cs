using System;
using System.Collections.Generic;
using BussinessObject.Models;
using DataAccessObject;

namespace Repositories.Implementation
{
    public class OrderRepository : IOrderRepository
    {
        public List<Order> GetOrdersByCustomer(int customerId)
        {
            return OrderDAO.Instance.GetOrdersByCustomer(customerId);
        }

        public List<Order> GetAllOrders()
        {
            return OrderDAO.Instance.GetAllOrder();
        }

        public Order GetOrderById(int id)
        {
            return OrderDAO.Instance.GetOrderById(id);
        }

        public void DeleteOrder(int id)
        {
            OrderDAO.Instance.DeleteOrder(id);
        }

        public int AddOrder(Order order)
        {
            return OrderDAO.Instance.AddOrder(order);
        }

        public int AddOrder(int? customerId, DateTime? shippedDate, string total, string orderStatus,
            out string message)
        {
            message = "";
            if (string.IsNullOrEmpty(orderStatus))
            {
                message = "Status cannot empty";
                return -1;
            }

            if (string.IsNullOrEmpty(total))
            {
                message = "Total cannot empty";
                return -1;
            }

            decimal totalTmp = 0;
            try
            {
                totalTmp = decimal.Parse(total);
            }
            catch (Exception e)
            {
                message = "Input number not valid";
                return -1;
            }

            return OrderDAO.Instance.AddOrder(new Order()
            {
                CustomerId = customerId,
                OrderDate = DateTime.Now,
                OrderStatus = orderStatus,
                ShippedDate = shippedDate,
                Total = totalTmp,
            });
        }

        public int UpdateOrder(Order oldOrder, int? customerId, DateTime? shippedDate, string total,
            string orderStatus,
            out string message)
        {
            var orderUpdate = oldOrder;

            message = "";
            if (string.IsNullOrEmpty(orderStatus))
            {
                message = "Status cannot empty";
                return -1;
            }

            if (string.IsNullOrEmpty(total))
            {
                message = "Total cannot empty";
                return -1;
            }

            decimal totalTmp;
            try
            {
                totalTmp = decimal.Parse(total);
            }
            catch (Exception e)
            {
                message = "Input number not valid";
                return -1;
            }

            orderUpdate.CustomerId = customerId;
            orderUpdate.OrderDate = DateTime.Now;
            orderUpdate.OrderStatus = orderStatus;
            orderUpdate.ShippedDate = shippedDate;
            orderUpdate.Total = totalTmp;
            OrderDAO.Instance.Update(orderUpdate);
            return orderUpdate.OrderId;
        }

        public void UpdateOrder(Order order)
        {
            OrderDAO.Instance.Update(order);

        }

        public List<Order> GetDataInRange(DateTime startTime, DateTime endTime, out string message)
        {
            message = "";
            List<Order> listResult = new List<Order>();
            if (string.IsNullOrEmpty(startTime.ToString()) || string.IsNullOrEmpty(endTime.ToString()))
            {
                message = "Please input date time";
                return listResult;
            }

            if (startTime > endTime)
            {
                message = "Start Time must before end time";
                return listResult;
            }

            try
            {
                listResult = OrderDAO.Instance.GetOrderInRange(startTime, endTime);
            }
            catch (Exception e)
            {
                message = e.Message;
            }

            return listResult;
        }
    }
}