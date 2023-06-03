using System;
using System.ComponentModel;
using BussinessObject.Models;
using System.Windows;
using Repositories;
using Repositories.Implementation;


namespace TranMinhThienWPF
{
    /// <summary>
    /// Interaction logic for CustomerEditor.xaml
    /// </summary>
    public partial class CustomerEditor : Window
    {
        private readonly Customer _updateCustomer;
        private readonly Action<Customer> _onFinish;
        private bool _isUpdate;
        private readonly ICustomerRepository _customerRepository = new CustomerRepository();

        public CustomerEditor()
        {
            InitializeComponent();
        }

        public CustomerEditor(Customer? customer, Action<Customer> onFinish)
        {
            InitializeComponent();
            _onFinish = onFinish;
            _updateCustomer = customer;

        }
        
        private void Awake(object sender, RoutedEventArgs e)
        {
            if (_updateCustomer != null)
            {
                
                Title.Text = "Customer Update";
                _isUpdate = true;
                InitUpdateView();
            }
            else
            {
                InitCreateView();
                Title.Text = "Customer Creator";
            }
        }
        private void InitCreateView()
        {
            OldPassword.Visibility = Visibility.Collapsed;
            OldPasswordText.Visibility = Visibility.Collapsed;
            CreateSms1.Visibility = Visibility.Visible;
            CreateSms2.Visibility = Visibility.Visible;
            CreateSms3.Visibility = Visibility.Visible;
            UpdateSms1.Visibility = Visibility.Collapsed;
            UpdateSms2.Visibility = Visibility.Collapsed;
            UpdateSms3.Visibility = Visibility.Collapsed;
        }
        private void InitUpdateView()
        {
            OldPassword.Visibility = Visibility.Visible;
            OldPasswordText.Visibility = Visibility.Visible;
            CreateSms1.Visibility = Visibility.Collapsed;
            CreateSms2.Visibility = Visibility.Collapsed;
            CreateSms3.Visibility = Visibility.Collapsed;
            UpdateSms1.Visibility = Visibility.Visible;
            UpdateSms2.Visibility = Visibility.Visible;
            UpdateSms3.Visibility = Visibility.Visible;
            
            Email.Text = _updateCustomer.Email;
            Name.Text = _updateCustomer.CustomerName;
            City.Text = _updateCustomer.City;
            Country.Text = _updateCustomer.Country;
        }

        #region Event

        private void OnClickSubmit(object sender, RoutedEventArgs e)
        {
            if (_isUpdate)
            {
                UpdateCustomer();
                return;
            }
            CreateCustomer();
            
        }

        #endregion

        #region Validation information

        private void UpdateCustomer()
        {
            try
            {
                var message = _customerRepository.UpdateCustomer(_updateCustomer,Email.Text, OldPassword.Password,Password.Password, ConfirmPassword.Password,
                    Name.Text, City.Text, Country.Text, null);
                if (!string.IsNullOrEmpty(message)) // ERROR
                {
                    MessageBox.Show(message, "ERROR");
                    return;
                }
            
                MessageBox.Show("Update success fully");
                _onFinish?.Invoke(_customerRepository.GetCustomerById(_updateCustomer.CustomerId));
                Close();
                
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "ERROR");
            }
           
        }
        private void CreateCustomer()
        {
            try
            {
                var message = _customerRepository.CreateCustomer(Email.Text, Password.Password, ConfirmPassword.Password,
                    Name.Text, City.Text, Country.Text, null);
                if (!string.IsNullOrEmpty(message)) // ERROR
                {
                    MessageBox.Show(message, "ERROR");
                    return;
                }
            
                MessageBox.Show("Update success fully");
                _onFinish?.Invoke(null);
                Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "ERROR");
            }
            
        }
        #endregion


        private void CustomerEditor_OnClosing(object sender, CancelEventArgs e)
        {
            _onFinish.Invoke(null);
        }
    }
}