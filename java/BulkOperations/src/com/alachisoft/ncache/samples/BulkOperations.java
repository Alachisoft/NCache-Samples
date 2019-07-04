// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// NCache Bulk Operations sample
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
package com.alachisoft.ncache.samples;

import com.alachisoft.ncache.samples.data.Product;
import com.alachisoft.ncache.web.caching.*;
import java.util.HashMap;
import java.util.Iterator;

public class BulkOperations
{

    public static void main(String[] args)
    {

        try
        {
            Cache cache;
            cache = NCache.initializeCache("mycache");
            cache.clear();

            String[] keysToAdd = new String[]
            {
                "Product:Cheese", "Product:Butter",
                "Product:Cream", "Product:Youghart"
            };

            Product[] products = new Product[4];

            products[0] = new Product(1, "Dairy Milk Cheese", "ClassA", "Edibles");
            products[1] =  new Product(2, "American Butter", "ClassA", "Edibles");
            products[2] = new Product(3, "Walmart Delicious Cream", "ClassA", "Edibles");
            products[3] = new Product(4, "Nestle Youghart", "ClassA", "Edibles");

            //Adding Bulk Items
            //Bulk operation returns 
            HashMap result = cache.addBulk(keysToAdd,
                                           new CacheItem[]
            {
                new CacheItem(products[0]),
                new CacheItem(products[1]),
                new CacheItem(products[2]),
                new CacheItem(products[3])
            },
                                           DSWriteOption.None,
                                           null);

            if (result.isEmpty())
            {
                System.out.println("All items are successfully added to cache.");
            }
            else
            {
                System.out.println("One or more items could not be added to the cache. Iterate hashmap for details.");
                //This is how to iterate the hashmap
                for (Iterator iter = result.values().iterator(); iter.hasNext();)
                {
                    Product product = (Product) iter.next();
                    printProductDetails(product);
                }
            }

            //Getting bulk items
            HashMap items = cache.getBulk(keysToAdd, DSReadOption.None);
            if (!items.isEmpty())
            {
                for (Iterator iter = items.values().iterator(); iter.hasNext();)
                {
                    Product product = (Product) iter.next();
                    printProductDetails(product);
                }
            }

            //Updating Bulk Items
            //Previous products have their product id, class and category changed.
            products[0].setClassName("ClassB");
            products[1].setClassName("ClassC");
            products[2].setClassName("ClassA");
            products[3].setClassName("ClassD");

            result = cache.insertBulk(keysToAdd,
                                      new CacheItem[]
            {
                new CacheItem(products[0]),
                new CacheItem(products[1]),
                new CacheItem(products[2]),
                new CacheItem(products[3])
            },
                                      DSWriteOption.None,
                                      null);

            if (result.isEmpty())
            {
                System.out.println("All items successfully added/updated in cache.");
            }
            else
            {
                System.out.println("One or more items could not be added to the cache. Iterate hashmap for details.");
                //This is how to iterate the hashmap
                for (Iterator iter = result.values().iterator(); iter.hasNext();)
                {
                    Product product = (Product) iter.next();
                    printProductDetails(product);
                }
            }

            items = cache.getBulk(keysToAdd, DSReadOption.None);
            if (!items.isEmpty())
            {
                for (Iterator iter = items.values().iterator(); iter.hasNext();)
                {
                    Product product = (Product) iter.next();
                    printProductDetails(product);
                }
            }

            //Remove Bulk operation
            //Remove returns all the items removed from the cache in them form of Hashmap
            result = cache.removeBulk(keysToAdd,
                                      DSWriteOption.None,
                                      null);

            if (result.isEmpty())
            {
                System.out.println("No items removed from the cache against the provided keys.");
            }
            else
            {
                System.out.println("Following products have been removed from the cache:");
                //This is how to iterate the hashmap
                for (Iterator iter = result.values().iterator(); iter.hasNext();)
                {
                    Product product = (Product) iter.next();
                    printProductDetails(product);
                }
            }

            //Delete Bulk from Cache is same as Remove
            //Delete bulk does not return anything 

            cache.deleteBulk(keysToAdd, DSWriteOption.None, null);
            System.out.println("Cache contains " + cache.getCount() + " items.");

            //Must dispose cache 
            cache.dispose();

            System.exit(0);

        }
        catch (Exception e)
        {
            System.out.println(e.getMessage());
            System.exit(0);
        }
    }
    
    static void printProductDetails(Product product)
    {        
        System.out.println("Id:       " + product.getId());            
        System.out.println("Name:     " + product.getName());            
        System.out.println("Class:    " + product.getClassName());            
        System.out.println("Category: " + product.getCategory());       
        System.out.println();
    }    
}
