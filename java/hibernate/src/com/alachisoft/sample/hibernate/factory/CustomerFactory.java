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
package com.alachisoft.sample.hibernate.factory;

import hibernator.BLL.Customers;
import java.util.ArrayList;
import java.util.List;
import org.hibernate.Session;
import org.hibernate.SessionFactory;
import org.hibernate.Transaction;
import org.hibernate.cfg.Configuration;

public class CustomerFactory
{
    Session session;
    SessionFactory factory;
    /// <summary>
    /// Get All Customers, should rarely be used...
    /// </summary>
    /// <returns>Complete list of customers</returns>

    public List<Customers> GetCustomers() throws Exception
    {
        List customers = new ArrayList();
        Transaction tx = null;
        String id = "";
        try
        {
            if (!session.isConnected())
            {
                session = factory.openSession();
            }
            tx = session.beginTransaction();

            List customerEnumerator = session.createQuery("from Customers c").setCacheable(true).list(); //Retrieves the list of Customers from 2nd level cache

            for (Customers cust : (List<Customers>) customerEnumerator)
            {
                customers.add(cust);
            }

            tx.commit();
            //customers = session.CreateCriteria(typeof(Customer)).List();  //List() loads  data directly from database without accessing 2nd level cache
        }
        catch (Exception ex)
        {

            tx.rollback();
            session.clear();
            session.close();
            throw ex;
        }
        return customers;
    }

    /// <summary>
    /// Gets a Customer
    /// </summary>
    /// <param name="CustomerID">string representing customer id</param>
    /// <returns>Object representing customer of "Customer" type. </returns>
    public Customers GetCustomer(String CustomerID) throws Exception
    {
        Customers customer = null;
        Transaction tx = null;

        //int ordercount =0;
        try
        {
            if (!session.isConnected())
            {
                session = factory.openSession();
            }
            tx = session.beginTransaction();
            customer=(Customers)session.get( Customers.class, CustomerID);
            tx.commit();
        }
        catch (Exception ex)
        {
            tx.rollback();
//            session.clear();
//            session.disconnect();
            throw ex;
        }

        return customer;
    }
    //customer = (Customer)session.Load(typeof(Customer), CustomerID);    //Load customer object from 2nd level cache

    public Customers GetCustomerOrders(String CustomerID) throws Exception
    {
        Customers customer = null;
        Transaction tx = null;

        try
        {
            if (!session.isConnected())
            {
                session = factory.openSession();
            }
            tx = session.beginTransaction();
            customer = (Customers) session.get(Customers.class, CustomerID);
            tx.commit();
        }
        catch (Exception ex)
        {
            tx.rollback();
            throw ex;
        }
        return customer;
    }

    /// <summary>
    /// Add current customer in database.
    /// </summary>
    /// <param name="cust">Customer to save.</param>
    public void SaveCustomer(Customers customer) throws Exception
    {
        Transaction tx = null;
        try
        {
            if (!session.isConnected())
            {
                session = factory.openSession();
            }
            tx = session.beginTransaction();
            session.save(customer);
            session.flush();
            System.out.println("\nCustomer with ID: " + customer.getCustomerID() + " succefully added into database");
            tx.commit();
        }
        catch (Exception ex)
        {
            tx.rollback();
//            session.clear();
//            session.disconnect();
            throw ex;
            // handle exception
        }
    }

    /// <summary>
    /// Insert/Update current customer in database.
    /// </summary>
    /// <param name="cust">Customer to update.</param>
    public void UpdateCustomer(Customers customer) throws Exception
    {
        Transaction tx = null;
        try
        {

            if (!session.isConnected())
            {
                session = factory.openSession();
            }
            tx = session.beginTransaction();
            session.merge(customer);
            session.flush();
            System.out.println("\nCustomer with ID: " + customer.getCustomerID() + " succefully updated into database");
            tx.commit();
        }
        catch (Exception ex)
        {
            tx.rollback();
//            session.clear();
//            session.disconnect();
            throw ex;
            // handle exception
        }
    }

    /// <summary>
    /// Removes the customer with customerID
    /// </summary>
    /// <param name="CustomerID"></param>
    public void RemoveCustomer(String CustomerID) throws Exception
    {
        Transaction tx = null;
        Customers customer;
        try
        {

            if (!session.isConnected())
            {
                session = factory.openSession();
            }
            tx = session.beginTransaction();
            List enumerator = session.createQuery("select cust " + "from Customer cust where " + "cust.CustomerID = '" + CustomerID + "'").list();

            if (!enumerator.isEmpty())
            {
                customer = (Customers) enumerator.get(0);
                if (customer != null)
                {
                    session.delete(customer);
                    session.flush();
                }
                else
                {
                    System.out.println("No such customer exist.");
                }
            }
            else
            {
                System.out.println("No such customer exist.");
            }


            tx.commit();
        }
        catch (Exception ex)
        {
            tx.rollback();
//            session.clear();
//            session.disconnect();
            throw ex;
            // handle exception
        }
    }

    /// <summary>
    /// Clears and dissconnects the session
    /// </summary>
    public void SessionDisconnect()
    {
        session.clear();
       session.close();
    }

    /// <summary>
    /// Create a customer factory based on the configuration given in the configuration file
    /// </summary>
    public CustomerFactory()
    {
        Object obj = new Configuration();
        Object obj2 = ((Configuration) obj).configure();
//        Object obj3 = ((org.hibernate.cfg.Configuration) obj2).configure().buildSessionFactory();
        factory = new Configuration().configure().buildSessionFactory();
        session = factory.openSession();
    }

    /// <summary>
    /// Make sure we clean up session etc.
    /// </summary>
    public void Dispose()
    {
        session.close();
        factory.close();
    }
}
