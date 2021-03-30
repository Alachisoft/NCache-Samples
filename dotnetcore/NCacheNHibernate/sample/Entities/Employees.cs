using System;
using System.Collections.Generic;

namespace Sample.CustomerService.Domain 
{
    public class Employees :Entity<int>
    {
        public Employees() { }
        public virtual Employees EmployeesVal { get; set; }
        public virtual string LastName { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string Title { get; set; }
        public virtual string TitleOfCourtesy { get; set; }
        public virtual DateTime? BirthDate { get; set; }
        public virtual DateTime? HireDate { get; set; }
        public virtual string Address { get; set; }
        public virtual string City { get; set; }
        public virtual string Region { get; set; }
        public virtual string Postalcode { get; set; }
        public virtual string Country { get; set; }
        public virtual string Homephone { get; set; }
        public virtual string Extension { get; set; }
        public virtual byte[] Photo { get; set; }
        public virtual string Notes { get; set; }
        public virtual string Photopath { get; set; }

        public virtual ISet<Employeeterritories> Employeeterritories { get; set; }
        public virtual ISet<Orders> Orders { get; set; }
        public virtual void AddEmployeeTerritory(Employeeterritories employeeterritories)
        {
            Employeeterritories.Add(employeeterritories);
            employeeterritories.Employees = this;
        }
        public virtual void AddOrder(Orders order)
        {
            Orders.Add(order);
            order.Employees = this;
        }
        public virtual void AddSupervisor(Employees employees)
        {
            EmployeesVal = employees;
        }
    }
}
