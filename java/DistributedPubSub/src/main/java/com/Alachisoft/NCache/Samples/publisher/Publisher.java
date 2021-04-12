// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
package com.Alachisoft.NCache.Samples.publisher;

import com.Alachisoft.NCache.Samples.callbacks.MessageFailedListeners;
import com.alachisoft.ncache.client.Cache;
import com.alachisoft.ncache.client.CacheManager;
import com.alachisoft.ncache.runtime.caching.DeliveryOption;
import com.alachisoft.ncache.runtime.caching.Message;
import com.alachisoft.ncache.runtime.caching.Topic;
import com.alachisoft.ncache.runtime.exceptions.CacheException;
import com.alachisoft.ncache.runtime.util.TimeSpan;

import java.io.Closeable;

/**
 * Class that provides the functionality of the publisher
 */
public class Publisher implements Closeable {

    private Topic _topic;
    private Cache _cache;
    private boolean _isStarted;

    /**
     * Event subscriber for the event of message delivery failure.
     * @throws Exception
     */
    public void subscribe() throws Exception {
        if(!_isStarted) throw new Exception("Publisher is not started.");
        MessageFailedListeners eventListeners = new MessageFailedListeners();
        _topic.addMessageDeliveryFailureListener(eventListeners);
    }

    /**
     * Event unsubscriber for the event of message delivery failure.
     * @throws Exception
     */
    public void unSubscribe() throws Exception {
        if(!_isStarted) throw new Exception("Publisher is not started.");
        _topic.removeMessageDeliveryFailureListener();
    }

    /**
     * This methods starts the specified cache and creates the specified topic on it.
     * @param cacheName The cache's name.
     * @param topicName The topic's name.
     * @throws Exception
     */
    public void start(String cacheName, String topicName) throws Exception {
        if(cacheName == null){
            throw new Exception("The CacheID cannot be null.");
        }

        if(cacheName.isEmpty()){
            throw new Exception("The CacheID cannot be empty.");
        }

        // Initialize an instance of the cache to begin performing operations:
        _cache = CacheManager.getCache(cacheName);

        // Create messaging topic.
        _topic = _cache.getMessagingService().createTopic(topicName);

        // Marking it as started.
        _isStarted = true;
    }

    /**
     * This method publishes a message, which is created by the provided arguments.
     * @param payLoad The payload for the message.
     * @param timeSpan Optional and nullable message expiry time.
     * @throws Exception
     */
    public void publish(Object payLoad, TimeSpan timeSpan) throws Exception {
        if(timeSpan == null)
            timeSpan = new TimeSpan();

        // Creating new message instance with the specified expiration TimeSpan.
        Message message = new Message(payLoad, timeSpan);

        // Publishing the above created message.
        _topic.publish(message, DeliveryOption.All, true);
    }

    /**
     * This method publishes a message, which is created by the provided arguments.
     * @param payLoad The payload for the message.
     * @throws Exception
     */
    public void publish(Object payLoad) throws Exception {

        Message message = new Message(payLoad);

        _topic.publish(message, DeliveryOption.All, true);
    }

    /**
     * This method deletes the specified 'Topic' in the argumnet.
     * @param topicName Name of the topic to be deleted.
     * @throws CacheException
     */
    public void deleteTopic(String topicName) throws CacheException {
        if(topicName == null) throw new IllegalArgumentException();

        // Deleting the topic.
        _cache.getMessagingService().deleteTopic(topicName);
    }

    /**
     * This methods releases the resource by the instance.
     */
    @Override
    public void close() {

        _isStarted = false;

        //Disposing the acquired resources.
        try {
            if (_topic != null) _topic.close();
            if (_cache != null) _cache.close();
        }catch (Exception ex){
            ex.printStackTrace();
        }
    }
}
