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

import com.alachisoft.ncache.client.CacheDataModificationListener;
import com.alachisoft.ncache.client.CacheEventArg;

public class CacheDataModificationImpl implements CacheDataModificationListener {

    @Override
    public void onCacheDataModified(String s, CacheEventArg cacheEventArg) {
        System.out.println("Cache event received for the key: " + s);
    }

    @Override
    public void onCacheCleared(String s) {
        System.out.println("Cache cleared event received.");
    }
}
