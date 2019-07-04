// ===============================================================================
// Alachisoft (R) NCache Sample Code
// NCache Hibernate sample
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
package hibernator.BLL;

import com.alachisoft.sample.hibernate.factory.CustomerFactory;
import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.util.Iterator;
import java.util.List;

public class HibernateTest
{


    public static CustomerFactory cf = null;
    static BufferedReader reader = new BufferedReader(new InputStreamReader(System.in));

    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) throws Exception
    {
        try
        {
            // Initialize customer factory
            cf = new CustomerFactory();
            System.out.println("Hibernate test started");

            System.out.println("Loading customers into the cache...");

            cf.GetCustomers();
            System.out.println("Customers information loaded in the cache");


            int choice = 0;
            while (choice != 7)
            {
                choice = GetUserChoice();

                String id;
                try
                {
                    switch (choice)
                    {
                        case 1:
                            PrintCustomerList(cf.GetCustomers());
                            cf.SessionDisconnect();
                            break;
                        case 2:
                            id = GetCustomerId();
                            PrintCustomerDetail(cf.GetCustomer(id));
                            cf.SessionDisconnect();
                            break;
                        case 3:
                            id = GetCustomerId();
                            Customer customer = cf.GetCustomerOrders(id);
                            PrintCustomerOrders(customer);
                            cf.SessionDisconnect();
                            break;
                        case 4:
                            id = GetCustomerId();
                            cf.RemoveCustomer(id);
                            cf.SessionDisconnect();
                            break;
                        case 5:
                            cf.SaveCustomer(AddCustomer(true));
                            cf.SessionDisconnect();
                            break;
                        case 6:
                            Customer tempCust = AddCustomer(false);
                            if (tempCust != null)
                            {
                                cf.UpdateCustomer(tempCust);
                                cf.SessionDisconnect();
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    System.out.println("\n" + ex + "\n");
                }
            }
            cf.Dispose();
            //System.exit(0);
        }
        catch (Exception e)
        {
            System.out.println("Error:" + e.getMessage());            
            System.exit(0);
        }
    }

    private static int GetUserChoice()
    {
        System.out.println("");
            System.out.println(" 1- View customers list");
            System.out.println(" 2- View customer details");
            System.out.println(" 3- View customer orders");
            System.out.println(" 4- Delete customer");
            System.out.println(" 5- Add customer");
            System.out.println(" 6- Update customer");
            System.out.println(" 7- Exit");
            System.out.println("");

            System.out.print("Enter your choice (1 - 7): ");
            try
            {
                String read = reader.readLine();
                int choice = Integer.parseInt(read);
                if (choice >= 1 && choice <= 7)
                    return choice;
            }
            catch (Exception ex)
            {
                System.out.print("");
            }
            System.out.println("Please enter a valid choice (1 - 7)");
            return GetUserChoice();
    }

    private static void PrintCustomerList(List<Customer> GetCustomers)
    {
        System.out.println("Customer ID    Customer Name");
        System.out.println("-----------    -------------");

        if (GetCustomers != null)
        {
            for(Customer customer : GetCustomers)
            {
                System.out.println(String.format("%-15s%s", customer.getCustomerID(),customer.getContactName() ));
                //System.out.println("{0,-13}  {1,-30}", customer.getCustomerID(), customer.getContactName());
            }
        }
    }

    private static String GetCustomerId() throws IOException
    {
        System.out.print("Enter customer ID: ");
        BufferedReader reader = new BufferedReader(new InputStreamReader(System.in));
        return reader.readLine();
    }

    private static void PrintCustomerDetail(Customer GetCustomer)
    {
        if (GetCustomer != null)
        {
            System.out.println("Customer's Detail");
            System.out.println("-----------------");

            System.out.println("Customer ID : " + GetCustomer.getCustomerID());
            System.out.println("Name        : " + GetCustomer.getContactName());
            System.out.println("Company     : " + GetCustomer.getCompanyName());
            System.out.println("Address     : " + GetCustomer.getAddress());
        }
        else
        {
            System.out.println("No such customer exist.");
        }
    }

    private static void PrintCustomerOrders(Customer customer)
    {
        if (customer != null)
            {
                System.out.println(customer.getContactName() + "'s Orders");
                System.out.println("------------------------");

                if (customer.getOrders() != null)
                {
                    System.out.println(String.format("%-15s%-30s%s", "Order ID", "Order Date", "Shipped Date"));
                    System.out.println(String.format("%-15s%-30s%s", "--------", "----------", "---------"));
                    Iterator orderList = customer.getOrders().iterator();
                    while(orderList.hasNext())
                    {
                        Orders order = (Orders)orderList.next();
                        PrintOrderDetail(order);
                    }
                }
            }
            else
            {
                System.out.println("No such customer exist.");
            }
    }

    private static Customer AddCustomer(boolean b) throws IOException, Exception
    {
            boolean validID = false;
            //bool validCustomer = true;
            String userInput = "";
            Customer customer = new Customer();

            while (!validID)
            {
                System.out.print("\nEnter Customer ID (maximum length = 5): ");
                customer.setCustomerID(reader.readLine());
                if (customer.getCustomerID().length() > 5 || customer.getCustomerID().equalsIgnoreCase("") /*|| customer.getCustomerID().indexOf(ch)*/)
                {
                    System.out.println("Exception: CustomerID cannot accept string of length > 5");
                }
                else
                    validID = true;
            }
            if (!b)
            {
                if (cf.GetCustomer(customer.getCustomerID()) == null)
                {
                    System.out.println("No such customer exist.");
                    return null;
                }
            }

            System.out.print("\nEnter Customer Name: ");
            userInput = reader.readLine();
            customer.setContactName((userInput == "") ? " " : userInput);

            System.out.print("\nEnter Customer Company Name: ");
            userInput = reader.readLine();
            customer.setCompanyName((userInput == "") ? " " : userInput);

            System.out.print("\nEnter Country Name: ");
            userInput = reader.readLine();
            customer.setCountry((userInput == "") ? " " : userInput);

            System.out.print("\nEnter City: ");
            userInput = reader.readLine();
            customer.setCity((userInput == "") ? " " : userInput);

            System.out.print("\nEnter Address: ");
            userInput = reader.readLine();
            customer.setAddress((userInput == "") ? " " : userInput);


            return customer;
    }

    private static void PrintOrderDetail(Orders order)
    {
        if (order != null)
        {
            System.out.println(String.format("%-15s%-30s%s", order.getOrderID(), order.getOrderDate().toString(), order.getShippedDate()));
        }
    }
}
