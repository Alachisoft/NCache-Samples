package com.alachisoft.ncache.samples;

import com.alachisoft.ncache.client.CacheItemVersion;
import com.alachisoft.ncache.runtime.exceptions.CacheException;
import com.alachisoft.ncache.samples.interfaces.OnAsyncOperationListener;

import java.util.concurrent.Future;

public class AsyncController {
    private OnAsyncOperationListener listener;

    public void registerCacheCallback(OnAsyncOperationListener listener){
        this.listener = listener;
    }

    public void doAddOperation(String key, Future<CacheItemVersion> itemVersionFuture)
    {
        new Thread(new Runnable() {
            @Override
            public void run() {
                if(listener != null){
                    try {
                        listener.onItemAdded(key, itemVersionFuture);
                    } catch (CacheException e) {
                        System.out.println(e.getMessage());
                    }
                }
            }
        }).start();
    }

    public void doUpdateOperation(String key, Future<CacheItemVersion> itemVersionFuture)
    {
        new Thread(new Runnable() {
            @Override
            public void run() {
                if(listener != null){
                    try {
                        listener.onItemUpdated(key, itemVersionFuture);
                    } catch (CacheException e) {
                        System.out.println(e.getMessage());
                    }
                }
            }
        }).start();
    }

    public void doRemoveOperation(String key, Future<CacheItemVersion> itemVersionFuture)
    {
        new Thread(
                new Runnable() {
                    @Override
                    public void run() {
                        if(listener != null){
                            try {
                                listener.onItemRemoved(key, itemVersionFuture);
                            } catch (CacheException e) {
                                System.out.println(e.getMessage());
                            }
                        }
                    }
                }
        ).start();
    }
}
