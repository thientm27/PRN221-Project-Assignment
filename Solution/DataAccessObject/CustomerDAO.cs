using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace DataAccessObject
{
    public class CustomerDao
    {
        private static CustomerDao instance = null;
        private static object instanceLook = new object();

        public static CustomerDao Instance
        {
            get
            {
                lock (instanceLook)
                {
                    if (instance == null)
                    {
                        instance = new CustomerDao();
                    }

                    return instance;
                }
            }
        }

        readonly FUFlowerBouquetManagementContext _context = new FUFlowerBouquetManagementContext();

        public List<Customer?> GetAllCustomer()
        {
            return _context.Customers.ToList();
        }

        public void AddCustomer(Customer customer)
        {
            var maxId = _context.Customers.Max(c => c.CustomerId);
            customer.CustomerId = maxId + 1;

            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        public bool AddCustomer2(Customer customer)
        {
            using (StreamReader r = new StreamReader("appsettings.json"))
            {
                string json = r.ReadToEnd();
                IConfiguration config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", true, true)
                    .Build();
                string name = config["account:defaultAccount:Email"];

                if (customer.Email.ToUpper().Equals(name.ToUpper()) )
                {
                    return false; // not valid email
                }
            }
            
            var maxId = _context.Customers.Max(c => c.CustomerId);
            customer.CustomerId = maxId + 1;

            _context.Customers.Add(customer);
            _context.SaveChanges();
            return true;
        }

        public void DeleteCustomer(int id)
        {
            var customer = _context.Customers.Where(c => c.CustomerId == id).ToList()[0];
            _context.Customers.Remove(customer);
            _context.SaveChanges();
        }

        public Customer GetCustomerById(int id)
        {
            var tmp = _context.Customers.Where(c => c.CustomerId == id).ToList();
            if (tmp.Count > 0)
            {
                return tmp[0];
            }

            return null;
        }

        public void UpdateCustomer(Customer customer)
        {
            var existingCustomer = _context.Customers.Find(customer.CustomerId);
            if (existingCustomer != null)
            {
                _context.Entry(existingCustomer).State = EntityState.Detached;
            }

            _context.Update(customer);
            _context.SaveChanges();
        }

        public bool CheckAdminLogin(string email, string pass)
        {
            using (StreamReader r = new StreamReader("appsettings.json"))
            {
                string json = r.ReadToEnd();
                IConfiguration config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", true, true)
                    .Build();
                string name = config["account:defaultAccount:Email"];
                string password = config["account:defaultAccount:Password"];

                if (email.ToUpper().Equals(name.ToUpper()) && password.Equals(pass))
                {
                    return true;
                }

                return false;
            }
        }

        public List<Customer?> GetCustomerByEmail(string email)
        {
            return _context.Customers.Where(cus => cus.Email.ToUpper().Contains(email.ToUpper())).ToList();
        }

        public List<Customer?> GetCustomerByCity(string email)
        {
            return _context.Customers.Where(cus => cus.City.ToUpper().Contains(email.ToUpper())).ToList();
        }

        public List<Customer?> GetCustomerByCountry(string email)
        {
            return _context.Customers.Where(cus => cus.Country.ToUpper().Contains(email.ToUpper())).ToList();
        }

        public List<Customer?> GetCustomerByName(string name)
        {
            return _context.Customers.Where(cus => cus.CustomerName.ToUpper().Contains(name.ToUpper())).ToList();
        }
    }
}