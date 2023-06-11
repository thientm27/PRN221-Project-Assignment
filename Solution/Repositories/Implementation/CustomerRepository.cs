using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using BussinessObject.Models;
using DataAccessObject;

namespace Repositories.Implementation
{
    public class CustomerRepository : ICustomerRepository
    {
        public void AddNewCustomer(Customer newCustomer)
        {
            CustomerDao.Instance.AddCustomer(newCustomer);
        }

        public bool AddNewCustomer2(Customer newCustomer)
        {
             return   CustomerDao.Instance.AddCustomer2(newCustomer);
        }

        public void UpdateCustomer(Customer newCustomer)
        {
            CustomerDao.Instance.UpdateCustomer(newCustomer);
        }

        public void DeleteCustomer(int idCustomer)
        {
            CustomerDao.Instance.DeleteCustomer(idCustomer);
        }

        public List<Customer?> GetAllCustomer()
        {
            return CustomerDao.Instance.GetAllCustomer();
        }

        public Customer GetCustomerById(int id)
        {
            return CustomerDao.Instance.GetCustomerById(id);
        }


        public Customer? Login(string email, string password)
        {
            // Check for admin
            if (CustomerDao.Instance.CheckAdminLogin(email, password))
            {
                return new Customer()
                {
                    CustomerId = -1
                };
            }

            // Customer login
            var customerList = CustomerDao.Instance.GetAllCustomer();
            foreach (var cus in customerList)
            {
                // Check for email if not -> next customer
                if (cus.Email != email)
                {
                    continue;
                }

                // Check password, if valid --> Return cus
                if (cus.Password == password)
                {
                    return cus;
                }
            }

            return null;
        }

        public string UpdateCustomer(Customer oldCustomer, string email, string oldPassword, string password,
            string confirmPass, string name, string city,
            string country, DateTime? birthday)
        {
            // Fill all the * information
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(name) ||
                string.IsNullOrEmpty(city) || string.IsNullOrEmpty(country))
            {
                return "Please fill all required information!";
            }

            if (!oldCustomer.Email.Equals(email))
            {
                // Email check
                // check for @
                if (!email.Contains("@"))
                {
                    return "Email not valid, must contain '@'";
                }

                // check length
                if (email.Length > 30)
                {
                    return "Email too long, below 30 character";
                }

                // check space
                if (email.Contains(" "))
                {
                    return "Email contain special character";
                }

                // check for special character and tail email
                string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                Regex regex = new Regex(pattern);
                if (!regex.IsMatch(email))
                {
                    return "Email tail not valid or email contain special character";
                }
            }

            // Check other filed 
            if (name.Length > 30)
            {
                return "Name too long, below 30 character";
            }

            if (city.Length > 30)
            {
                return "City too long, below 30 character";
            }

            if (country.Length > 30)
            {
                return "Country too long, below 30 character";
            }

            var isEditPass = false;
            if (!string.IsNullOrEmpty(oldPassword) || !string.IsNullOrEmpty(password) ||
                !string.IsNullOrEmpty(confirmPass))
            {
                isEditPass = true;

                if (!oldCustomer.Password.Equals(oldPassword)) return "Old password is not correct";

                // Passwords don't match
                if (!password.Equals(confirmPass)) return "Password does not match";

                // Check password criteria
                if (password.Length < 8)
                {
                    // Password is too short
                    return "Password is too short, at least 8 character";
                }

                if (!Regex.IsMatch(password, @"\d"))
                {
                    // Password doesn't contain a digit
                    return "Password must have at least one digit";
                }

                if (!Regex.IsMatch(password, @"[a-zA-Z]"))
                {
                    // Password doesn't contain a letter
                    return "Password must have at least one letter";
                }
            }

            oldCustomer.Email = email;
            oldCustomer.CustomerName = name;
            oldCustomer.Password = isEditPass ? password : oldCustomer.Password;
            oldCustomer.City = city;
            oldCustomer.Country = country;
            oldCustomer.Birthday = birthday;

            CustomerDao.Instance.UpdateCustomer(oldCustomer);
            // Additional criteria can be added as needed
            return "";
        }

        public string CreateCustomer(string email, string password, string confirmPass, string name, string city,
            string country,
            DateTime? birthday)
        {
            // Fill all the * information
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(confirmPass) || string.IsNullOrEmpty(name) ||
                string.IsNullOrEmpty(city) || string.IsNullOrEmpty(country))
            {
                return "Please fill all required information!";
            }

            // Email check
            // check for @
            if (!email.Contains("@"))
            {
                return "Email not valid, must contain '@'";
            }

            // check length
            if (email.Length > 30)
            {
                return "Email too long, below 30 character";
            }

            // check space
            if (email.Contains(" "))
            {
                return "Email contain special character";
            }

            // check for special character and tail email
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex regex = new Regex(pattern);
            if (!regex.IsMatch(email))
            {
                return "Email tail not valid or email contain special character";
            }

            // Check other filed 
            if (name.Length > 30)
            {
                return "Name too long, below 30 character";
            }

            if (city.Length > 30)
            {
                return "City too long, below 30 character";
            }

            if (country.Length > 30)
            {
                return "Country too long, below 30 character";
            }

            // Passwords don't match
            if (!password.Equals(confirmPass)) return "Password does not match";

            // Check password criteria
            if (password.Length < 8)
            {
                // Password is too short
                return "Password is too short, at least 8 character";
            }

            if (!Regex.IsMatch(password, @"\d"))
            {
                // Password doesn't contain a digit
                return "Password must have at least one digit";
            }

            if (!Regex.IsMatch(password, @"[a-zA-Z]"))
            {
                // Password doesn't contain a letter
                return "Password must have at least one letter";
            }

            // Additional criteria can be added as needed

            CustomerDao.Instance.AddCustomer(new Customer()
            {
                Birthday = birthday,
                City = city,
                Country = country,
                CustomerName = name,
                Email = email,
                Password = password
            });

            return "";
        }

        public List<Customer?> FindCustomer(int findCase, string value)
        {
            switch (findCase)
            {
                case 0:
                {
                    return CustomerDao.Instance.GetCustomerByName(value);
                }
                case 1:
                {
                    return CustomerDao.Instance.GetCustomerByEmail(value);
                }
                case 2:
                {
                    return CustomerDao.Instance.GetCustomerByCity(value);
                }
                case 3:
                {
                    return CustomerDao.Instance.GetCustomerByCountry(value);
                }
                case 4:
                {
                    var newList = new List<Customer?>();
                    var customer = CustomerDao.Instance.GetCustomerById(int.Parse(value));
                    if (customer != null)
                    {
                        newList.Add(customer);
                    }

                    return newList;
                }
            }

            return null;
        }
    }
}