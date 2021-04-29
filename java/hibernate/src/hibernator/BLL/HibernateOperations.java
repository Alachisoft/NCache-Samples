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
import com.alachisoft.sample.hibernate.factory.OrdersFactory;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.util.List;

public class HibernateOperations {


    public static CustomerFactory customerFactory = null;
    public static OrdersFactory orderFactory = null;

    static BufferedReader reader = new BufferedReader(new InputStreamReader(System.in));


    public static void run() throws Exception {
        // Initialize customer factory
        customerFactory = new CustomerFactory();
        orderFactory = new OrdersFactory();

        System.out.println("Hibernate test started");

        System.out.println("Loading customers into the cache...");

        //Loading to Cache
        customerFactory.GetCustomers();

        orderFactory.GetOrders();

        System.out.println("Customers information loaded in the cache");

        int choice = 0;
        while (choice != 8) {
            choice = GetUserChoice();

            String id;
            try {
                switch (choice) {
                    case 1:
                        PrintCustomerList(customerFactory.GetCustomers());
//                            customerFactory.SessionDisconnect();
                        break;
                    case 2:
                        id = GetCustomerId();
                        PrintCustomerDetail(customerFactory.GetCustomer(id));
//                            customerFactory.SessionDisconnect();
                        break;
                    case 3:
                        id = GetCustomerId();
                        Customers customer = customerFactory.GetCustomerOrders(id);
                        PrintCustomerOrders(customer);
//                            customerFactory.SessionDisconnect();
                        break;
                    case 4:
                        id = GetCustomerId();
                        customerFactory.RemoveCustomer(id);
//                            customerFactory.SessionDisconnect();
                        break;
                    case 5:
                        customerFactory.SaveCustomer(AddCustomer());
//                            customerFactory.SessionDisconnect();
                        break;
                    case 7:

                        PrintOrderList(orderFactory.GetOrders());
//                            customerFactory.SessionDisconnect();
                        break;
                }
            } catch (Exception ex) {
                System.out.println("\n" + ex + "\n");
            }
        }
        customerFactory.Dispose();
        //System.exit(0);
    }

    private static int GetUserChoice() {
        System.out.println();
        System.out.println(" 1- View customers list");
        System.out.println(" 2- View customer details");
        System.out.println(" 3- View customer orders");
        System.out.println(" 4- Delete customer");
        System.out.println(" 5- Add customer");
        System.out.println(" 6- Update customer");
        System.out.println(" 7- Exit");
        System.out.println();

        System.out.print("Enter your choice (1 - 7): ");
        try {
            String read = reader.readLine();
            int choice = Integer.parseInt(read);
            if (choice >= 1 && choice <= 7)
                return choice;
        } catch (Exception ex) {
            System.out.print("");
        }
        System.out.println("Please enter a valid choice (1 - 7)");
        return GetUserChoice();
    }

    private static void PrintCustomerList(List<Customers> GetCustomers) {
        System.out.println("Customer ID    Customer Name");
        System.out.println("-----------    -------------");

        if (GetCustomers != null) {
            for (Customers customer : GetCustomers) {
                System.out.println(String.format("%-15s%s", customer.getCustomerID(), customer.getContactName()));
                //System.out.println("{0,-13}  {1,-30}", customer.getCustomerID(), customer.getContactName());
            }
        }
    }

    private static void PrintOrderList(List<Orders> ordersList) {
        System.out.println("Order ID    ShippedName");
        System.out.println("-----------    -------------");

        if (ordersList != null) {
            for (Orders customer : ordersList) {
                System.out.println(String.format("%-15s%s", customer.getCustomerID(), customer.getShippedName()));
                //System.out.println("{0,-13}  {1,-30}", customer.getCustomerID(), customer.getContactName());
            }
        }
    }

    private static String GetCustomerId() throws IOException {
        System.out.print("Enter customer ID: ");
        BufferedReader reader = new BufferedReader(new InputStreamReader(System.in));
        return reader.readLine();
    }

    private static void PrintCustomerDetail(Customers GetCustomer) {
        if (GetCustomer != null) {
            System.out.println("Customer's Detail");
            System.out.println("-----------------");

            System.out.println("Customer ID : " + GetCustomer.getCustomerID());
            System.out.println("Name        : " + GetCustomer.getContactName());
            System.out.println("Company     : " + GetCustomer.getCompanyName());
            System.out.println("Address     : " + GetCustomer.getAddress());
        } else {
            System.out.println("No such customer exist.");
        }
    }

    private static void PrintCustomerOrders(Customers customer) {
        if (customer != null) {
            System.out.println(customer.getContactName() + "'s Orders");
            System.out.println("------------------------");

            if (customer.getOrders() != null) {
                System.out.println(String.format("%-15s%-30s%s", "Order ID", "Shipped Address", "Shipped City"));
                System.out.println(String.format("%-15s%-30s%s", "--------", "----------", "---------"));
                for (Object o : customer.getOrders()) {
                    Orders order = (Orders) o;
                    PrintOrderDetail(order);
                }
            }
        } else {
            System.out.println("No such customer exist.");
        }
    }

    private static Customers AddCustomer() throws Exception {
        boolean validID = false;
        //bool validCustomer = true;
        String userInput;
        Customers customer = new Customers();

        while (!validID) {
            System.out.print("\nEnter Customer ID (maximum length = 5): ");
            customer.setCustomerID(reader.readLine());
            if (customer.getCustomerID().length() > 5 || customer.getCustomerID().equalsIgnoreCase("") /*|| customer.getCustomerID().indexOf(ch)*/) {
                System.out.println("Exception: CustomerID cannot accept string of length > 5");
            } else
                validID = true;
        }

        System.out.print("\nEnter Customer Name: ");
        userInput = reader.readLine();
        customer.setContactName((userInput.equals("")) ? " " : userInput);

        System.out.print("\nEnter Customer Company Name: ");
        userInput = reader.readLine();
        customer.setCompanyName((userInput.equals("")) ? " " : userInput);

        System.out.print("\nEnter Country Name: ");
        userInput = reader.readLine();
        customer.setCountry((userInput.equals("")) ? " " : userInput);

        System.out.print("\nEnter City: ");
        userInput = reader.readLine();
        customer.setCity((userInput.equals("")) ? " " : userInput);

        System.out.print("\nEnter Address: ");
        userInput = reader.readLine();
        customer.setAddress((userInput.equals("")) ? " " : userInput);


        return customer;
    }

    private static void PrintOrderDetail(Orders order) {
        if (order != null) {
//            System.out.println(String.format("%-15s%-30s%s", order.getOrderID(), order.getOrderDate().toString(), order.getShippedDate()));
            System.out.println(String.format("%-15s%-30s%s", order.getOrderID(), order.getShipAddress(), order.getShipCity()));
        }
    }
}
