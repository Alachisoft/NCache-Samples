// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================

using System;
using System.Configuration;
using Alachisoft.NCache.Client;
using Alachisoft.NCache.Client.DataTypes;
using Alachisoft.NCache.Client.DataTypes.Collections;
using Alachisoft.NCache.Runtime.Caching;
using Alachisoft.NCache.Sample.Data;

// ===============================================================================
// Distributed List is a distributed collection that allows storing objects in a
// list format and provides various methods to manipulate the list such as adding,
// removing, updating, and fetching objects. Distributed List maintains the order
// of objects based on the index at which they are added. It also allows
// enumerating over the objects in the list. Some possible use cases of
// Distributed List include:
//   1. Storing a list of recent activities or events performed by users
//   2. Maintaining a list of top N trending or frequently accessed items
//   3. Keeping track of messages or chat history in a messaging application
//   4. Managing task queues or ordered job lists across distributed services
//   5. Recording time-ordered logs or audit trails for real-time monitoring
// ===============================================================================

// Connect to a running cache and get cache handle for it
ICache cache = GetCache();

// Validating cache handle
if (cache == null)
{
    Console.Write("Cache connection could not be established. Exiting the sample.");
    return;
}

// Names of distributed lists to be used in sample
string priorityCustomers = "DList:PriorityCustomers";
string regularCustomers = "DList:RegularCustomers";

// Creating or getting both distributed lists by name
IList<Customer> priorityList = GetOrCreateList(priorityCustomers);
IList<Customer> regularList = GetOrCreateList(regularCustomers);

// Adding items to both distributed lists
AddPriorityCustomers(priorityList);
AddRegularCustomers(regularList);

// Displaying customers from both distributed lists
DisplayList(priorityList);
DisplayList(regularList);

// Fetching the first customer from distributed list by index
Customer customer = GetCustomerFromList(0, priorityList);

// Modifying the customer and updating in distributed list
UpdateObjectsInList(0, customer, priorityList);

// Removing the existing customer from distributed list by index
RemoveViaIndex(2, regularList);

// Removing the existing customer from distributed list by instance
RemoveViaInstance(customer, priorityList);

// Removing both distributed lists from cache
DeleteList(priorityCustomers);
DeleteList(regularCustomers);

// Dispose the cache once done
cache.Dispose();

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to connect to a running cache and return cache handle for it 
/// </summary>
/// <returns>Cache handle for the connected cache</returns>
ICache GetCache()
{
    // Getting cache name from configuration file
    string? cacheName = ConfigurationManager.AppSettings["CacheName"];

    // Validating cache name
    if (String.IsNullOrEmpty(cacheName))
    {
        Console.WriteLine("The CacheName cannot be null or empty.");
        return null;
    }

    // Trying to connect to cache
    try
    {
        // Connecting to a running cache and return a cache handle for it
        ICache cache = CacheManager.GetCache(cacheName);

        // Printing output on console
        Console.WriteLine(string.Format("Cache '{0}' is connected.", cacheName));

        // Returning cache handle
        return cache;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error connecting to cache '{cacheName}': {ex.Message}");
        return null;
    }
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to create a distributed list to which instances of customers are to be added
/// </summary>
/// <param name="listName">Name of distributed list</param>
/// <returns>Distributed List containing customers</returns>
IList<Customer> GetOrCreateList(string listName)
{
    // Trying to get the distributed list from cache
    IDistributedList<Customer> distributedList = cache.DataTypeManager.GetList<Customer>(listName);

    // If distributed list does not exist, create it
    if (distributedList == null)
    {
        DataTypeAttributes attributes = new DataTypeAttributes
        {
            Expiration = new Expiration(ExpirationType.Absolute, TimeSpan.FromMinutes(1))
        };

        // Creating distributed list with absolute expiration of 1 minute
        distributedList = cache.DataTypeManager.CreateList<Customer>(listName, attributes);
    }

    return distributedList;
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to add customers to priority list
/// </summary>
/// <param name="distributedList">Distributed list to which customers are to be added</param>
void AddPriorityCustomers(IList<Customer> distributedList)
{
    Console.WriteLine("Adding customers to distributed list.");

    distributedList.Add(new Customer
    {
        ContactName = "David Johnes",
        CompanyName = "Lonesome Pine Restaurant",
        ContactNo = "(1) 354-9768",
        Address = "Silicon Valley, Santa Clara, California",
    });
    distributedList.Add(new Customer
    {
        ContactName = "Carlos Gonzalez",
        CompanyName = "LILA-Supermercado",
        ContactNo = "(9) 331-6954",
        Address = "Carrera 52 con Ave. Bolivar #65-98 Llano Largo",
    });
    distributedList.Add(new Customer
    {
        ContactName = "Carlos Hernandez",
        CompanyName = "HILARION-Abastos",
        ContactNo = "(5) 555-1340",
        Address = "Carrera 22 con Ave. Carlos Soublette #8-35",
    });
    distributedList.Add(new Customer
    {
        ContactName = "Elizabeth Brown",
        CompanyName = "Consolidated Holdings",
        ContactNo = "(171) 555-2282",
        Address = "Berkeley Gardens 12 Brewery",
    });
    distributedList.Add(new Customer
    {
        ContactName = "Felipe Izquierdo",
        CompanyName = "LINO-Delicateses",
        ContactNo = "(8) 34-56-12",
        Address = "Ave. 5 de Mayo Porlamar",
    });

    // Printing output on console.
    Console.WriteLine("Customers are added to distributed list.");
    Console.WriteLine();
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to add customers to regular list
/// </summary>
/// /// <param name="distributedList">Distributed list to which customers are to be added</param>
void AddRegularCustomers(IList<Customer> distributedList)
{
    Console.WriteLine("Adding customers to distributed list.");
    distributedList.Add(new Customer
    {
        ContactName = "Maria Anders",
        CompanyName = "Alfreds Futterkiste",
        ContactNo = "(5) 555-4729",
        Address = "Obere Str. 57, Berlin",
    });
    distributedList.Add(new Customer
    {
        ContactName = "Ana Trujillo",
        CompanyName = "Ana Trujillo Emparedados y helados",
        ContactNo = "(5) 555-4729",
        Address = "Avda. de la Constitución 2222, México D.F.",
    });
    distributedList.Add(new Customer
    {
        ContactName = "Antonio Moreno",
        CompanyName = "Antonio Moreno Taquería",
        ContactNo = "(5) 555-3932",
        Address = "Mataderos 2312, México D.F.",
    });
    // Printing output on console.
    Console.WriteLine("Customers are added to distributed list.");
    Console.WriteLine();
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to get number of objects in distributed list
/// </summary>
/// <param name="distributedList">Distributed list from which objects are to be fetched</param>
void DisplayList(IList<Customer> distributedList)
{
    Console.WriteLine("Fetching all customers from distributed list.");

    Customer[] cachedCustomers = new Customer[distributedList.Count];

    // Enumerating over all customer instances and assigning them at respective indices
    var counter = 0;
    foreach (Customer customer in distributedList)
    {
        cachedCustomers[counter++] = customer;
    }

    Console.WriteLine("{0} Customers are fetched from distributed list", counter);
    Console.WriteLine();
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to get an object from distributed list specified by index
/// </summary>
/// <param name="index">Index of object to be fetched from distributed list</param>
/// <param name="distributedList">Distributed list from which object is to be fetched</param>
/// <returns>Customer retrieved from distributed list by index</returns>
Customer GetCustomerFromList(int index, IList<Customer> distributedList)
{
    Customer cachedCustomer = distributedList[index];
    // Printing output on console
    Console.WriteLine("Customer is fetched from distributed list");

    // Printing customer details
    PrintCustomerDetails(cachedCustomer);
    Console.WriteLine();
    return cachedCustomer;
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to update objects in distributed list
/// </summary>
/// <param name="index">Index of object to be updated at in distributed list</param>
/// <param name="customer">Instance of Customer that will be updated in the distributed list</param>
/// <param name="distributedList">Distributed list in which object is to be updated</param>
void UpdateObjectsInList(int index, Customer customer, IList<Customer> distributedList)
{
    Console.WriteLine("Updating customer in distributed list.");

    // Modifying company name of customer
    customer.CompanyName = "Gourmet Lanchonetes";

    // Updating object via indexer
    distributedList[index] = customer;

    // Printing output on console
    Console.WriteLine("Customer is updated in distributed list.");
    Console.WriteLine();
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to remove an object from distributed list by index
/// </summary>
/// <param name="index">Index of object to be deleted from distributed list</param>
/// <param name="distributedList">Distributed list from which object is to be deleted</param>
void RemoveViaIndex(int index, IList<Customer> distributedList)
{
    Console.WriteLine("Removing customer from distributed list via index.");

    // Removing the existing customer
    distributedList.RemoveAt(index);

    // Printing output on console
    Console.WriteLine("Customer is removed from distributed list.");
    Console.WriteLine();
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to remove an object in distributed list by instance of object
/// </summary>
/// <param name="customer">Instance of object to be deleted from distributed list</param>
/// <param name="distributedList">Distributed list from which object is to be deleted</param>
void RemoveViaInstance(Customer customer, IList<Customer> distributedList)
{
    Console.WriteLine("Removing customer from distributed list via instance.");

    // Removing the existing customer (expensive operation use carefully)
    bool removed = distributedList.Remove(customer);

    // Printing on console
    Console.WriteLine("Customer is" + (removed ? " " : " not ") + "removed from distributed List.");
    Console.WriteLine();
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to remove distributed list from cache
/// </summary>
/// <param name="listName">Name of distributed list to be removed from cache</param>
void DeleteList(string listName)
{
    Console.WriteLine("Removing distributed list from cache.");

    // Removing list
    cache?.DataTypeManager.Remove(listName);

    // Printing detail on output
    Console.WriteLine("Distributed list successfully removed from cache.");
    Console.WriteLine();
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to print details of customer type
/// </summary>
/// <param name="customer">Customer instance whose attributes are to be printed</param>
void PrintCustomerDetails(Customer customer)
{
    if (customer == null) return;

    Console.WriteLine();
    Console.WriteLine("Customer details are as follows: ");
    Console.WriteLine("ContactName: " + customer.ContactName);
    Console.WriteLine("CompanyName: " + customer.CompanyName);
    Console.WriteLine("Contact No: " + customer.ContactNo);
    Console.WriteLine("Address: " + customer.Address);
    Console.WriteLine();
}