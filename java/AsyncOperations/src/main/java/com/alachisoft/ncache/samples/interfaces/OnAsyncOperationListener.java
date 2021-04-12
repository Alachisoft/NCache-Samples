package com.alachisoft.ncache.samples.interfaces;

import com.alachisoft.ncache.client.CacheItemVersion;
import com.alachisoft.ncache.runtime.exceptions.CacheException;

import java.util.concurrent.Future;

public interface OnAsyncOperationListener {
    void onItemAdded(String key, Future<CacheItemVersion> itemVersionFuture) throws CacheException;
    void onItemRemoved(String key, Future<CacheItemVersion> itemVersionFuture) throws CacheException;
    void onItemUpdated(String key, Future<CacheItemVersion> itemVersionFuture) throws CacheException;
}
