// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// NCache Object Query Language sample
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
package com.alachisoft.ncache.samples;

import com.alachisoft.ncache.runtime.caching.NamedTagsDictionary;
import com.alachisoft.ncache.samples.data.Product;
import com.alachisoft.ncache.web.caching.*;
import java.util.HashMap;
import java.util.Iterator;

public class ObjectQueryLanguage
{

    public static void main(String[] args)
    {
        try
        {
            //Initializing cache
            Cache cache;
            cache = NCache.initializeCache("mycache");
            cache.clear();

            //Querying an item in NCache requires either NamedTags or
            //Indexes defined for objects or both.

            //Querying a simple cache item via namedTags

            NamedTagsDictionary nameTagDict = new NamedTagsDictionary();

            nameTagDict.add("Employee", "DavidBrown");
            nameTagDict.add("Department", "Mathematics");

            cache.add("Employee:DavidBrown", "Department:Mathematics", nameTagDict);

            //Defining Searching criteria
            HashMap criteria = new HashMap();
            criteria.put("Employee", "DavidBrown");

            String query = "SELECT java.lang.String WHERE this.Employee = ?";

            HashMap result = cache.searchEntries(query, criteria);

            if (!result.isEmpty())
            {
                Iterator itor = result.values().iterator();
                while (itor.hasNext())
                {
                    System.out.println(itor.next());
                }
            }

            System.out.println();

            //New product object whose index is definex in NCache Manager
            Product product = new Product(1,
                                      "UninterruptedPowerSupply",
                                      "ClassA",
                                      "QCPassed");

            cache.add("Product:UPS", product);

            //  Query can only be applied to Primitive data types:
            //  java.lang.Integer
            //  java.lang.String
            //  java.lang.Double
            //  java.lang.Float
            //  and
            //  for those non primitive data types whose indexes are defined 
            //  in NCache manager

            query = "Select com.alachisoft.ncache.samples.data.Product Where this.productId = ?";
            criteria = new HashMap();
            criteria.put("productId", 1);

            result = cache.searchEntries(query, criteria);

            if (!result.isEmpty())
            {
                Iterator itor = result.values().iterator();
                while (itor.hasNext())
                {
                    Product productFound = (Product) itor.next();
                    printProductDetails(productFound);
                }
            }

            //Querying indexed object via NamedTags

            nameTagDict = new NamedTagsDictionary();
            nameTagDict.add("Name", "UninterruptedPowerSupply");
            nameTagDict.add("Class", "ClassA");

            cache.insert("Product:UPS", product, nameTagDict);

            query = "Select com.alachisoft.ncache.samples.data.Product Where this.name = ?";

            criteria = new HashMap();
            criteria.put("name", "UninterruptedPowerSupply");

            result = cache.searchEntries(query, criteria);

            if (!result.isEmpty())
            {
                Iterator itor = result.values().iterator();
                while (itor.hasNext())
                {
                    Product productFound = (Product) itor.next();
                    printProductDetails(productFound);
                }
            }

            //Must dispose cache
            cache.dispose();

            System.exit(0);

        }
        catch (Exception ex)
        {
            System.out.println(ex.getMessage());
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
