using System;
using System.Windows;
using System.Windows.Input;
using Repositories;
using Repositories.Implementation;

namespace TranMinhThienWPF
{
    public partial class Login : Window
    {
        private ICustomerRepository _customerRepository = new CustomerRepository();
        
        public Login()
        {
            InitializeComponent();
        }

        private void OnClickLogin(object sender, RoutedEventArgs e)
        {
            try
            {
                var user = _customerRepository.Login(Email.Text, Password.Password);
                if (user != null)
                {
                    if (user.CustomerId == -1)
                    {
                        
                        AdminView adminView = new AdminView();
                        adminView.Show();
                        Close();
                    }
                    else
                    {
                        CustomerView customerView = new CustomerView(user);
                        customerView.Show();
                        Close();
                    }
                }
                else
                {
                    MessageBox.Show("Wrong email or password", "Alert");
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "ERROR");
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // click to drag form to anywhere
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}