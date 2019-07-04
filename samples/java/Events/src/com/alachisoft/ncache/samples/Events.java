// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// NCache Events sample
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
package com.alachisoft.ncache.samples;

import com.alachisoft.ncache.runtime.events.EventDataFilter;
import com.alachisoft.ncache.runtime.events.EventType;
import com.alachisoft.ncache.web.caching.Cache;
import com.alachisoft.ncache.web.caching.NCache;
import java.util.EnumSet;

public class Events
{

    public static void main(String[] args)
    {

        try
        {

            //Initialize cache
            Cache cache;
            cache = NCache.initializeCache("mycache");
            cache.clear();

            //Event notifications must be enabled in NCache manager->Options for events to work

            CacheEventListener cacheListener = new CacheEventListener();
            EnumSet<EventType> e = EnumSet.allOf(EventType.class);
            cache.addCacheDataModificationListener(new CacheDataModificationImpl(), e, EventDataFilter.Metadata);

            //See output

            cache.add("Item:1", "Value:1");

            //Key based notifications allows item notifications for particular
            //items in cache
            //Registering key based notifications involves
            //Implementing CacheItemRemovedCallback Interface
            //Implementing CacheItemUpdatedCallback Interface
            //Implementations are included in sample

            cache.addCacheDataModificationListener("Item:1", new CacheDataModificationImpl(), e, EventDataFilter.Metadata);
            //See output

            cache.insert("Item:1", "Value:1-------Changed for notifications");

            //See output

            cache.delete("Item:1");

            CustomEventListener customEvents = new CustomEventListener();
            cache.addCustomEventListener(customEvents);

            cache.add("CustomEventObjectKey", "CustomEventObject");

            cache.raiseCustomEvent("CustomEventObjectKey", "CustomEventObject");

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
}
