using Sample.CustomerService.Domain;
using SampleApp.DatabaseSeed;
using SampleApp.NHibernateHelpers;
using SampleApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SampleApp
{
    public class NHibernate
    {
        private readonly CustomerRepository _customerRepository;
        private readonly ProductRepository _productRepository;
        private readonly EmployeeRepository _employeeRepository;

        public NHibernate(
            NHibernateHelper nhibernateHelper)
        {
            if (nhibernateHelper == null)
            {
                throw new ArgumentNullException(nameof(nhibernateHelper));
            }

            _customerRepository = new CustomerRepository(nhibernateHelper);
            _productRepository = new ProductRepository(nhibernateHelper);
            _employeeRepository = new EmployeeRepository(nhibernateHelper);
        }

        public void Run()
        {
            var done = false;
            while (!done)
            {
                var userChoice = GetUserChoice();

                switch (userChoice)
                {
                    case 1:
                        {
                            var customers = _customerRepository.GetCustomers();

                            if (customers.Count() == 0)
                            {
                                Console.WriteLine("No customers in database");
                            }
                            else
                            {
                                PrintCustomerIDs(customers);
                            }

                            Console.WriteLine("\n\n");
                        }
                        break;
                    case 2:
                        {
                            var customerID = GetCustomerID();
                            if (!string.IsNullOrWhiteSpace(customerID))
                            {
                                var customer = _customerRepository.GetCustomer(customerID);
                                PrintCustomerDetails(customer);
                            }

                            Console.WriteLine("\n\n");
                        }
                        break;
                    case 3:
                        {
                            var customerID = GetCustomerID();
                            if (!string.IsNullOrWhiteSpace(customerID))
                            {
                                var orders =
                                    _customerRepository.GetCustomerOrders(customerID);

                                if (orders.Count() == 0)
                                {
                                    Console.WriteLine($"No orders exist for {customerID}\n");
                                }
                                else
                                {
                                    Console.WriteLine($"{customerID}\'s orders:\n");
                                    PrintOrderDetails(orders);
                                }
                            }

                            Console.WriteLine("\n\n");
                        }
                        break;
                    case 4:
                        {
                            var customerID = GetCustomerID();
                            if (!string.IsNullOrWhiteSpace(customerID))
                            {
                                var customer = _customerRepository.GetCustomer(customerID);
                                var deleted = _customerRepository.RemoveCustomer(customer);

                                if (!deleted)
                                {
                                    Console.WriteLine(
                                        $"{customerID} was not deleted because " +
                                        $"it doesn't exist in database");
                                }
                                else
                                {
                                    Console.WriteLine(
                                        $"{customerID} deleted from database");
                                }
                            }
                        }
                        break;
                    case 5:
                        {
                            var customer = SeedData.SeedCustomers()
                                .Generate();

                            Console.WriteLine("New customer:\n");
                            PrintCustomerDetails(customer);

                            _customerRepository.SaveUpdateCustomer(customer);

                            Console.WriteLine($"Customer {customer.Id} saved");
                        }
                        break;
                    case 6:
                        {
                            var country = GetCustomerCountry();

                            if (!string.IsNullOrWhiteSpace(country))
                            {
                                var customers =
                                    _customerRepository.GetCustomerByCountry(country);

                                if (customers.Count() == 0)
                                {
                                    Console.WriteLine(
                                        $"No customers in database with country {country}");
                                }
                                else
                                {
                                    PrintCustomerIDs(customers.Select(x => x.Id));
                                }
                            }

                            Console.WriteLine("\n\n");
                        }
                        break;
                    case 7:
                        {
                            var products = _productRepository.GetProducts();

                            if (products.Count() == 0)
                            {
                                Console.WriteLine("No products in database");
                            }
                            else
                            {
                                PrintProductIDs(products);
                            }

                            Console.WriteLine("\n\n");
                        }
                        break;
                    case 8:
                        {
                            var employees = _employeeRepository.GetEmployees();

                            if (employees.Count() == 0)
                            {
                                Console.WriteLine("No employees in database");
                            }
                            else
                            {
                                PrintEmployeeIDs(employees);
                            }

                            Console.WriteLine("\n\n");
                        }
                        break;
                    case 9:
                        {
                            var employee = SeedData.SeedEmployees()
                                .Generate();

                            Console.WriteLine("New Employee:\n");
                            PrintEmployeeDetails(employee);

                            _employeeRepository.SaveUpdateEmployee(employee);

                            Console.WriteLine($"Employee {employee.Id} saved");
                        }
                        break;
                    case 10:
                        {
                            done = true;
                        }
                        break;

                }
            }
        }

        private static int GetUserChoice()
        {
            Console.WriteLine("");
            Console.WriteLine(" 1- View customers list");
            Console.WriteLine(" 2- View customer details");
            Console.WriteLine(" 3- View customer orders");
            Console.WriteLine(" 4- Delete customer");
            Console.WriteLine(" 5- Add customer");
            Console.WriteLine(" 6- View customers by country");
            Console.WriteLine(" 7- View Products list");
            Console.WriteLine(" 8- View Employees list");
            Console.WriteLine(" 9- Add employee");
            Console.WriteLine("10- Exit");
            Console.WriteLine("");

            Console.Write("Enter your choice (1 - 10): ");
            try
            {
                int choice = Convert.ToInt32(Console.ReadLine());
                if (choice >= 1 && choice <= 10)
                    return choice;
            }
            catch (Exception)
            {
            }
            Console.WriteLine("Please enter a valid choice (1 - 10)");
            return GetUserChoice();
        }

        private static string GetCustomerID()
        {
            Console.Write("Please enter the customer Id: ");
            string result = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(result))
            {
                Console.WriteLine("customer id can not be empty.");
                return null;
            }

            return result.ToUpperInvariant();
        }

        private static string GetCustomerCountry()
        {
            Console.Write("Please enter the country: ");
            string result = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(result))
            {
                Console.WriteLine("customer country can not be empty.");
                return null;
            }

            return result;
        }

        private static void PrintCustomerIDs(IEnumerable<string> customers)
        {
            int i = 1;
            foreach (var customer in customers)
            {
                Console.WriteLine($"{i++}-{customer}");
            }

        }

        private static void PrintProductIDs(IEnumerable<Products> products)
        {
            int i = 1;
            foreach (var product in products)
            {
                Console.WriteLine($"{i++}-{product.Id}");
            }
        }

        private static void PrintEmployeeIDs(IEnumerable<Employees> employees)
        {
            int i = 1;
            foreach (var employee in employees)
            {
                Console.WriteLine($"{i++}-{employee.Id}");
            }
        }

        private static void PrintCustomerDetails(Customers customer)
        {
            if (customer != null)
            {
                Console.WriteLine("Customer's Details");
                Console.WriteLine("------------------");

                Console.WriteLine("Customer ID : " + customer.Id);
                Console.WriteLine("Name        : " + customer.ContactName);
                Console.WriteLine("Company     : " + customer.CompanyName);
                Console.WriteLine("Address     : " + customer.Address);
                Console.WriteLine("Country     : " + customer.Country);
            }
            else
            {
                Console.WriteLine("No such customer exists.");
            }
        }

        private static void PrintEmployeeDetails(Employees employee)
        {
            if (employee != null)
            {
                Console.WriteLine("Employee's Details");
                Console.WriteLine("------------------");

                Console.WriteLine("Employee ID : " + employee.Id);
                Console.WriteLine("First Name  : " + employee.FirstName);
                Console.WriteLine("Last Name   : " + employee.LastName);
                Console.WriteLine("Address     : " + employee.Address);
                Console.WriteLine("Country     : " + employee.Country);
            }
            else
            {
                Console.WriteLine("No such employee exists.");
            }
        }


        private static void PrintOrderDetails(IEnumerable<Orders> orders)
        {
            foreach (var order in orders)
            {
                Console.WriteLine(
                    " {0,-10} {1,-25} {2,-20}",
                    order.Id,
                    order.OrderDate.ToString(),
                    order.ShipName);
            }
        }



    }
}
