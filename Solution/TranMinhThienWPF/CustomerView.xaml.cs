using System;
using System.Collections.Generic;

using System.Windows;
using BussinessObject.Models;
using Repositories;
using Repositories.Implementation;

namespace TranMinhThienWPF
{
    /// <summary>
    /// Interaction logic for CustomerView.xaml
    /// </summary>
    public partial class CustomerView : Window
    {
        #region Attributes

        private Customer _user;
        private List<Order> _orders = new();
        private List<OrderDetail> _orderDetails = new();
        private readonly IOrderRepository _orderRepository = new OrderRepository();
        private readonly IOrderDetailRepository _orderDetailRepository = new OrderDetailRepository();
        private readonly IFlowerBouquetRepository _flowerBouquetRepository = new FlowerBouquetRepository();
        private ICustomerRepository _customerRepository = new CustomerRepository();
        private int _orderSelectIndex = -1;
        #endregion
        public CustomerView()
        {
            InitializeComponent();
        }
        public CustomerView(Customer user)
        {
            InitializeComponent();
            _user = user;
        }
        // Validation
        private void Awake(object sender, RoutedEventArgs e)
        {
            if (_user == null || _user.CustomerId == -1)
            {
                LogOut();
            }
            else
            {
                UserDisplayName.Text = _customerRepository.GetCustomerById(_user.CustomerId).CustomerName;
                LoadDataOrder();
                UpdateOrderListView();
            }
        }
        // Order
        private void LoadDataOrder()
        {
            try
            {
                _orders = _orderRepository.GetOrdersByCustomer(_user.CustomerId);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "ERROR");
            }
        }
        private void UpdateOrderListView()
        {
            if (_orders != null)
            {
                OrderView.ItemsSource = _orders;
            }
            else
            {
                MessageBox.Show("You do not have any order", "Message");
            }

        }
        // Order Details
        private void LoadDataOrderDetail(int id)
        {
            try
            {
                _orderDetails = _orderDetailRepository.GetOrderDetailById(id);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "ERROR");
            }
        }
        private void UpdateOrderDetailListView()
        {
            var viewModel = new List<OrderDetailViewModel>();
            foreach (OrderDetail detail in _orderDetails)
            {
                viewModel.Add(new(detail.OrderId, GetFlowerName(detail.FlowerBouquetId), detail.UnitPrice, detail.Quantity, detail.Discount));
            }

            OrderDetailView.ItemsSource = viewModel;
        }
        
        #region Repositories interaction

      
        private string GetFlowerName(int id)
        {
            try
            {
                var name = _flowerBouquetRepository.GetFlowerName(id);
                return name ?? "";
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "ERROR");
                return "";
            }
            
        }

        private void LogOut()
        {
            Login login = new Login();
            login.Show();
            _user = null;
            this.Close();
        }

        #endregion

        #region Event

        private void OnFinishUpdate(Customer? newCustomer)
        {
            if (newCustomer == null )
            {
                Show();
                return;
            }
            
            _user = newCustomer;
            if (_user.CustomerId == -1)
            {
                LogOut();
            }
            else
            {
                UserDisplayName.Text = _customerRepository.GetCustomerById(_user.CustomerId).CustomerName;
                LoadDataOrder();
                UpdateOrderListView();
            }
        }
        private void OnClickUpdate(object sender, RoutedEventArgs e)
        {
            CustomerEditor customerEditor = new CustomerEditor(_user, OnFinishUpdate);
            customerEditor.Show();
            Hide();
        }
        private void OnClickLogOut(object sender, RoutedEventArgs e)
        {
            LogOut();
        }
        private void OnChangeSelectedOrder(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if(_orderSelectIndex != OrderView.SelectedIndex)
            {
                _orderSelectIndex = OrderView.SelectedIndex;
                LoadDataOrderDetail(_orders[_orderSelectIndex].OrderId);
                UpdateOrderDetailListView();
            }
         
        }

        #endregion

        
    }


    public class OrderDetailViewModel{
        public int OrderId { get; set; }
        public string FlowerBouquetName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public double Discount { get; set; }

        public OrderDetailViewModel(int orderId, string flowerBouquetName, decimal unitPrice, int quantity, double discount)
        {
            OrderId = orderId;
            FlowerBouquetName = flowerBouquetName;
            UnitPrice = unitPrice;
            Quantity = quantity;
            Discount = discount;
        }
    }

}