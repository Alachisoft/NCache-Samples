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

import com.alachisoft.ncache.web.caching.CacheDataModificationListener;
import com.alachisoft.ncache.web.events.CacheEventArg;

public class CacheDataModificationImpl implements CacheDataModificationListener {

    @Override
    public void cacheDataModified(String string, CacheEventArg cea) {
        System.out.println("Cache event received for the key: " + string);
    }

    @Override
    public void cacheCleared() {
        System.out.println("Cache cleared event received."); 
    }
    
}
