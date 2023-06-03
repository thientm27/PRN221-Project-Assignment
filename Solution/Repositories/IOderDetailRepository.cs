using System.Collections.Generic;
using BussinessObject.Models;

namespace Repositories
{
    public interface IOrderDetailRepository
    {
        public List<OrderDetail> GetOrderDetailById(int orderId);
        public void AddOrderDetails(List<OrderDetail> orderDetails);
        public void UpdateOrderDetails(List<OrderDetail> orderDetails);
        public List<OrderDetail> GetOrderDetailByOrderId(int orderId);
    }
}