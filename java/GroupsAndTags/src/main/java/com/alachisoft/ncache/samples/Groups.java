// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// NCache Groups sample class
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================

package com.alachisoft.ncache.samples;

import com.alachisoft.ncache.client.Cache;
import com.alachisoft.ncache.client.CacheItem;
import com.alachisoft.ncache.client.CacheManager;
import com.alachisoft.ncache.samples.data.Product;

import java.io.*;
import java.util.Map;
import java.util.Properties;

public class Groups {
    
    public static void runGroupsDemo()
    {
        try
        {
            System.out.println();

            Properties properties =  getProperties();
            String cacheName= properties.getProperty("CacheID");
            Cache cache = CacheManager.getCache(cacheName);
            cache.clear();
            
            //Adding item in same group

            Product product1 = new Product("Product:CellularPhoneHTC", "HTCPhone", "Electronics", "Mobiles");
            Product product2 = new Product("Product:CellularPhoneNokia", "NokiaPhone", "Electronics", "Mobiles");
            Product product3 = new Product("Product:CellularPhoneSamsung", "SamsungPhone", "Electronics", "Mobiles");
            Product product4 = new Product("Product:ProductLaptopAcer", "AcerLaptop", "Electronics", "Laptops");
            Product product5 = new Product("Product:ProductLaptopHP", "HPLaptop", "Electronics", "Laptops");
            Product product6 = new Product("Product:ProductLaptopDell", "DellLaptop", "Electronics", "Laptops");
            Product product7 = new Product("Product:ElectronicsHairDryer", "HairDryer", "Electronics", "SmallElectronics");
            Product product8 = new Product("Product:ElectronicsVaccumCleaner", "VaccumCleaner", "Electronics", "SmallElectronics");
            Product product9 = new Product("Product:ElectronicsIron", "Iron", "Electronics", "SmallElectronics");

            CacheItem cacheItem1 = new CacheItem(product1);
            cacheItem1.setGroup(product1.getCategory());
            cache.add(product1.getId(), cacheItem1);

            CacheItem cacheItem2 = new CacheItem(product2);
            cacheItem2.setGroup(product2.getCategory());
            cache.add(product2.getId(), cacheItem2);

            CacheItem cacheItem3 = new CacheItem(product3);
            cacheItem3.setGroup(product3.getCategory());
            cache.add(product3.getId(), cacheItem3);

            CacheItem cacheItem4 = new CacheItem(product4);
            cacheItem4.setGroup(product4.getCategory());
            cache.add(product4.getId(), cacheItem4);

            CacheItem cacheItem5 = new CacheItem(product5);
            cacheItem5.setGroup(product5.getCategory());
            cache.add(product5.getId(), cacheItem4);

            CacheItem cacheItem6 = new CacheItem(product6);
            cacheItem6.setGroup(product6.getCategory());
            cache.add(product6.getId(), cacheItem6);

            CacheItem cacheItem7 = new CacheItem(product7);
            cacheItem7.setGroup(product7.getCategory());
            cache.add(product7.getId(), cacheItem7);

            CacheItem cacheItem8 = new CacheItem(product8);
            cacheItem8.setGroup(product8.getCategory());
            cache.add(product8.getId(), cacheItem8);

            CacheItem cacheItem9 = new CacheItem(product9);
            cacheItem9.setGroup(product9.getCategory());
            cache.add(product9.getId(), cacheItem9);

            // Getting group data
            Map<String, Object> items = cache.getSearchService().getGroupData("Electronics"); // Will return nine items
            if ( !items.isEmpty() ) 
            {
                System.out.println("Item count: " + items.size());
                System.out.println("Following Products are found in group 'Electronics'");
                for (Object o : items.values()) {
                    Product product = (Product) o;
                    System.out.println(product.toString());
                }
                System.out.println();
            }
            
            //getGroupKeys is yet another function to retrive group data.
            //It however requires multiple iterations to retrive actual data
            //1) To get List of Keys and 2) TO get items for the return List of Keys
            
            //Updating items in groups
            product9.setClassName("SmallElectronics");
            CacheItem cacheItem10 = new CacheItem(product9);
            cacheItem10.setGroup(product9.getCategory());
            cache.add(product9.getId(), cacheItem9);
            cache.insert("Product:ElectronicsIron", cacheItem10); //Item is updated at the specified group
            
            
            //Removing group data
            System.out.println("Item count: " + cache.getCount()); // Itemcount = 9
            cache.getSearchService().removeGroupData("Electronics");  // Will remove all group data
            System.out.println();
            
            //Must dispose cache
            cache.close();

        }
        catch(Exception ex)
        {
            System.out.println(ex.getMessage());
        }
    }

    /**
     This method returns property file
     */
    private static Properties getProperties() throws IOException
    {
        String path = "config.properties";
        InputStream inputStream = Groups.class.getClassLoader().getResourceAsStream(path);
        Properties properties=new Properties();
        if (inputStream != null) {
            properties.load(inputStream);
        } else {
            throw new FileNotFoundException("property file '" + path + "' not found in the classpath");
        }
        return properties;
    }

}
