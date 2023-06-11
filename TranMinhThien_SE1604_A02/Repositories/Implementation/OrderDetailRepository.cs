using System.Collections.Generic;
using BussinessObject.Models;
using DataAccessObject;

namespace Repositories.Implementation
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        public List<OrderDetail> GetOrderDetailById(int orderId)
        {
            return OderDetailDao.Instance.GetOrderDetailByOrderId(orderId);
        }

        public void AddOrderDetails(List<OrderDetail> orderDetails)
        {
            OderDetailDao.Instance.AddOrderDetails(orderDetails);
        }

        public void UpdateOrderDetails(List<OrderDetail> orderDetails)
        {
            OderDetailDao.Instance.UpdateOrderDetails(orderDetails);
        }

        public List<OrderDetail> GetOrderDetailByOrderId(int orderId)
        {
           return  OderDetailDao.Instance.GetOrderDetailByOrderId(orderId);
        }
    }
}