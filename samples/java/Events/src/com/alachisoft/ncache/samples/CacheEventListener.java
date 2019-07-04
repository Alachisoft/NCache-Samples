// ===============================================================================
// Alachisoft (R) NCache Sample Code
// NCache Events sample
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================

package com.alachisoft.ncache.samples;

import com.alachisoft.ncache.event.CacheEvent;
import com.alachisoft.ncache.event.CacheListener;

public class CacheEventListener implements CacheListener
{

    //These methods can be modified to suite the needs.
    //Can be customzid to include full cache and item details.
    
    @Override
    public void cacheCleared() {
        //Define your implementation here
        //Examples includes writing to log files
        
        System.out.println("Cache cleared!");
    }

    @Override
    public void cacheItemAdded(CacheEvent ce) {
        System.out.println("Cache: An item is added");
    }

    @Override
    public void cacheItemRemoved(CacheEvent ce) {
        System.out.println("Cache: An item is removed");
    }

    @Override
    public void cacheItemUpdated(CacheEvent ce) {
        System.out.println("Cache: An item is updated");
    }
    
}
