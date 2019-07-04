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

import com.alachisoft.ncache.web.caching.Cache;
import com.alachisoft.ncache.web.caching.NCache;
import java.util.HashMap;
import java.util.Iterator;

public class Groups {
    
    public static void runGroupsDemo()
    {
        try
        {
            System.out.println();
            
            Cache cache = NCache.initializeCache("mycache");
            cache.clear();
            
            //Adding item in same group
            //Group can be done at two levels 
            //Groups and Subgroups.          
            
            cache.add("Product:CellularPhoneHTC", "HTCPhone", "Electronics", "Mobiles");
            cache.add("Product:CellularPhoneNokia", "NokiaPhone", "Electronics", "Mobiles");
            cache.add("Product:CellularPhoneSamsung", "SamsungPhone", "Electronics", "Mobiles");
            cache.add("Product:ProductLaptopAcer", "AcerLaptop", "Electronics", "Laptops");
            cache.add("Product:ProductLaptopHP", "HPLaptop", "Electronics", "Laptops");
            cache.add("Product:ProductLaptopDell", "DellLaptop", "Electronics", "Laptops");
            cache.add("Product:ElectronicsHairDryer", "HairDryer", "Electronics", "SmallElectronics");
            cache.add("Product:ElectronicsVaccumCleaner", "VaccumCleaner", "Electronics", "SmallElectronics");
            cache.add("Product:ElectronicsIron", "Iron", "Electronics", "SmallElectronics");
                        
            // Getting group data
            HashMap items = cache.getGroupData("Electronics", null); // Will return nine items since no subgroup is defined;
            if ( !items.isEmpty() ) 
            {
                System.out.println("Item count: " + items.size());
                System.out.println("Following Products are found in group 'Electronics'");
                Iterator itor = items.values().iterator();
                while( itor.hasNext() )
                {                    
                    System.out.println(itor.next().toString());
                }
                System.out.println();
            }
                        
            items = cache.getGroupData("Electronics", "Mobiles"); // Will return thre items under the subgroup Mobiles
            if ( !items.isEmpty() ) 
            {
                System.out.println("Item count: " + items.size());
                System.out.println("Following Products are found in group 'Electronics' and Subgroup 'Mobiles'");
                Iterator itor = items.values().iterator();
                while( itor.hasNext() )
                {
                    
                    System.out.println(itor.next().toString());
                }
                System.out.println();
            }                
            
            //getGroupKeys is yet another function to retrive group data.
            //It however requires multiple iterations to retrive actual data
            //1) To get List of Keys and 2) TO get items for the return List of Keys
            
            //Updating items in groups
            cache.insert("Product:ElectronicsIron", "PanaSonicIron", "Electronics", "SmallElectronics"); //Item is updated at the specified group
            
            
            //Removing group data
            System.out.println("Item count: " + cache.getCount()); // Itemcount = 9
            cache.removeGroupData("Electronics", "Mobiles");  // Will remove 3 items from cache based on subgroup Mobiles     
            
            System.out.println("Item count: " + cache.getCount()); // Itemcount = 6
            cache.removeGroupData("Electronics", null); // Will remove all items from cache based on group Electronics
            
            System.out.println("Item count: " + cache.getCount()); // Itemcount = 0
            
            System.out.println();
            
            //Must dispose cache
            cache.dispose();

        }
        catch(Exception ex)
        {
            System.out.println(ex.getMessage());
        }
    }        
}
