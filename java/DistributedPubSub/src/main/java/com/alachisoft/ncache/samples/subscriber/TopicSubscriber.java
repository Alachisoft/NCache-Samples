// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
package com.alachisoft.ncache.samples.subscriber;

import com.alachisoft.ncache.samples.callbacks.MessageReceivedCallbacks;
import com.alachisoft.ncache.samples.callbacks.TopicDeletedCallback;
import com.alachisoft.ncache.client.Cache;
import com.alachisoft.ncache.client.CacheManager;
import com.alachisoft.ncache.runtime.caching.Topic;

import java.io.Closeable;
import java.util.Scanner;

/**
 * Class that provides the functionality of the publisher
 */
public class TopicSubscriber implements Closeable {

    private Topic _topic;
    private Cache _cache;

    /**
     * This method initializes the specified cache and subscribes to the specified topic for the published messages on it.
     * @param cacheName The name of the cache to initialize.
     * @param topicName The name of the topic to subscribe to.
     * @param messageReceivedCallbacks The callback which will be invoked when a message is published on the topic.
     * @throws Exception
     */
    public void subscribe(String cacheName, String topicName, MessageReceivedCallbacks messageReceivedCallbacks) throws Exception {
        if(cacheName == null){
            throw new Exception("The CacheID cannot be null.");
        }

        if(cacheName.isEmpty()){
            throw new Exception("The CacheID cannot be empty.");
        }

        // Initialize an instance of the cache to begin performing operations:
        _cache = CacheManager.getCache(cacheName);

        // Gets the topic.
        _topic = _cache.getMessagingService().createTopic(cacheName);

        // Creates the topic if it doesn't exist.
        if(_topic == null)
            _topic = _cache.getMessagingService().createTopic(topicName);

        // Subscribes to the topic.
        _topic.createSubscription(messageReceivedCallbacks);
        Thread.sleep(10000);
    }

    /**
     * This property lets the user to subscribe for the topic's deletion event.
     */
    public void deleteTopicCallback()
    {
        TopicDeletedCallback topicDeletedCallback = new TopicDeletedCallback();
        _topic.addTopicDeletedListener(topicDeletedCallback);
    }

    /**
     * This methods releases the resource by the instance.
     */
    @Override
    public void close() {
        try
        {
            if (_topic != null) _topic.close();
            if (_cache != null) _cache.close();
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }
}
