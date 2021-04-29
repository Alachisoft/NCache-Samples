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

import hibernator.BLL.Orders;
import java.util.ArrayList;
import java.util.List;
import org.hibernate.Session;
import org.hibernate.SessionFactory;
import org.hibernate.Transaction;
import org.hibernate.cfg.Configuration;

public class OrdersFactory
{
    Session session;
    SessionFactory factory;
    
     public OrdersFactory()
    {
        Object obj = new Configuration();
        Object obj2 = ((Configuration) obj).configure();
        
//        Object obj3 = ((org.hibernate.cfg.Configuration) obj2).configure().buildSessionFactory();
        factory = new Configuration().configure().buildSessionFactory();
        
        session = factory.openSession();
    }
    /// <summary>
    /// Get All Customers, should rarely be used...
    /// </summary>
    /// <returns>Complete list of customers</returns>

    public List<Orders> GetOrders() throws Exception
    {
        List orders = new ArrayList();
        Transaction tx = null;
        String id = "";
        try
        {
            if (!session.isConnected())
            {
                session = factory.openSession();
            }
            tx = session.beginTransaction();

            List customerEnumerator = session.createQuery("from Orders o").setCacheable(true)                    
                    .list(); //Retrieves the list of Customers from 2nd level cache

            for (Orders cust : (List<Orders>) customerEnumerator)
            {
                orders.add(cust);
            }

            tx.commit();
            //customers = session.CreateCriteria(typeof(Customer)).List();  //List() loads  data directly from database without accessing 2nd level cache
        }
        catch (Exception ex)
        {
            tx.rollback();
            session.clear();
            session.disconnect();
            throw ex;
        }
        return orders;
    }

}
