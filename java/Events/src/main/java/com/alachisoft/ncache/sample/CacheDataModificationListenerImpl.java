package com.alachisoft.ncache.sample;

import com.alachisoft.ncache.client.CacheDataModificationListener;
import com.alachisoft.ncache.client.CacheEventArg;
import com.alachisoft.ncache.samples.data.Product;

public class CacheDataModificationListenerImpl implements CacheDataModificationListener {

    @Override
    public void onCacheDataModified(String key, CacheEventArg cacheEventArg) {
        String eventText = "";
        switch (cacheEventArg.getEventType()) {
            case ItemAdded:
                System.out.println("Cache event received for the key: " + key + " Event Type: added\n");
                break;
            case ItemRemoved:
                if (cacheEventArg.getItem() != null) {
                    System.out.println("Item with Key '" + key + "' is removed from cache\n");
                }else{
                    System.out.println("Cache event received for the key: " + key + " Event Type: removed\n");
                }
                break;
            case ItemUpdated:
                if (cacheEventArg.getItem() != null) {
                    System.out.println("Item with Key '" + key + "' is updated in cache");
                    Product updatedProduct = cacheEventArg.getItem().getValue(Product.class); //Ignore error code works
                    System.out.println("Updated Item: " + updatedProduct);
                }else{
                    System.out.println("Cache event received for the key: " + key + " Event Type: updated\n");
                }
                break;
        }

    }

    @Override
    public void onCacheCleared(String s) {
        System.out.println("Cache cleared event received.");
    }
}

