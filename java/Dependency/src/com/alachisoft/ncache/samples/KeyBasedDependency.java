// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// NCache Key Based Dependency Sample.
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================

package com.alachisoft.ncache.samples;

import com.alachisoft.ncache.runtime.CacheItemPriority;
import com.alachisoft.ncache.runtime.dependencies.KeyDependency;
import com.alachisoft.ncache.web.caching.Cache;
import com.alachisoft.ncache.web.caching.NCache;

public class KeyBasedDependency {

    public static void runKeyBasedDependencyDemo() {
        try {
            //Initialize cache
            Cache cache;
            cache = NCache.initializeCache("mycache");
            cache.clear();

            //Key dependency makes an item to get removed when
            //it's accosiated item is removed or has it's value changed.

            cache.add("KeyItem1", "This is item 1");

            cache.add("KeyItem2",
                    "This is item 2",
                    new KeyDependency("KeyItem1"),
                    Cache.NoAbsoluteExpiration,
                    Cache.NoSlidingExpiration,
                    CacheItemPriority.Normal);

            cache.insert("KeyItem1", "This is item 1 updated");

            Object value = cache.get("KeyItem2");

            if (value == null) {
                System.out.println("Dependent item is successfully removed from cache.");
            } else {
                System.out.println("Error while removing the dependent items from cache.");
            }

            //An item can be dependent on multiple keys!
            //All the keys specified for dependecy must exist in cache

            cache.add("KeyItem2", "This is item 2");

            cache.add("KeyItem3",
                    "This is item 3",
                    new KeyDependency(new String[]{
                "KeyItem1",
                "KeyItem2"
            }),
                    Cache.NoAbsoluteExpiration,
                    Cache.NoSlidingExpiration,
                    CacheItemPriority.Normal);

            cache.insert("KeyItem2", "This is item 2 updated");

            value = cache.get("KeyItem3");

            if (value == null) {
                System.out.println("Dependent item is successfully removed from cache.");
            } else {
                System.out.println("Error while removing the dependent items from cache.");
            }

            //Key dependency is cascaded
            //If A is dependent of B and B is dependent on C
            //Altering C will remove B and A from the cache.

            //Must dispose cache
            cache.dispose();


        } catch (Exception ex) {
            System.out.println(ex.getMessage());
        }
    }
}
