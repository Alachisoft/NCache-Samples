package com.alachisoft.ncache.samples.Publisher;// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================

import com.alachisoft.ncache.client.Cache;
import com.alachisoft.ncache.client.CacheManager;
import com.alachisoft.ncache.runtime.caching.*;
import com.alachisoft.ncache.runtime.exceptions.CacheException;
import com.alachisoft.ncache.runtime.util.TimeSpan;
import com.alachisoft.ncache.samples.EventListeners.EventListeners;

import java.io.*;
import java.util.*;


public class Publisher {
    private static Cache _cache;
    private Topic _topic;

    public static void run()
    {
        try
        {
            System.out.println("Starting 2 Publishers... Publishers will stop after publishing 30 messages each.");
            new Thread(() -> {
                try {
                    runPublisher("ElectronicsOrders", ElectronicOrder.class);
                } catch (Exception e) {
                    e.printStackTrace();
                }
            }).start();
            new Thread(() -> {
                try {
                    runBulkPublisher("GarmentsOrders", GarmentsOrder.class);
                } catch (Exception e) {
                    e.printStackTrace();
                }
            }).start();
            new Thread(() -> {
                try {
                    runAsyncPublisher("GarmentsOrders", GarmentsOrder.class);
                } catch (Exception e) {
                    e.printStackTrace();
                }
            }).start();

            System.out.println("Publishers Started.");

            System.out.println("Press any key to delete the topics");

            System.in.read();

            deleteTopics("ElectronicsOrders");
            deleteTopics("GarmentsOrders");

        }catch (Exception ex)
        {
            System.out.println(ex.getMessage());
        }
    }

    private static <T extends Orders> void runAsyncPublisher(String topicName, Class<T> t) throws Exception {
        getPropsAndInitCache();

        Publisher publisher = new Publisher();

        publisher.publishAsyncMessages(topicName, t);
    }

    private <T extends Orders> void publishAsyncMessages(String topicName, Class<T> t) throws Exception {
        _topic = _cache.getMessagingService().getTopic(topicName);
        EventListeners listeners = new EventListeners();

        if(_topic == null)
        {
            _topic = _cache.getMessagingService().createTopic(topicName);
        }

        _topic.addMessageDeliveryFailureListener(listeners);

        //Generate 30 messages at the interval of 5 seconds.
        for(int i=0; i<30; i++)
        {
            T order = Orders.generateOrder(t);

            // Publishes the message with expiry.
            Message message = new Message(order, new TimeSpan(0, 0, 15));
            _topic.publishAsync(message, DeliveryOption.All, true);

            System.out.println("Messages for " + t.getName() + " OrderId: " + order.getOrderId() + " generated");

            Thread.sleep(5 * 1000);//Sleep for 5 seconds.
        }
    }

    private static <T extends Orders> void runBulkPublisher(String topicName, Class<T> t) throws Exception {
        getPropsAndInitCache();

        Publisher publisher = new Publisher();

        publisher.publishBulkMesages(topicName, t);
    }

    private <T extends Orders> void publishBulkMesages(String topicName, Class<T> t) throws Exception {
        _topic = _cache.getMessagingService().getTopic(topicName);
        EventListeners listeners = new EventListeners();

        if(_topic == null)
        {
            _topic = _cache.getMessagingService().createTopic(topicName);
        }

        _topic.addMessageDeliveryFailureListener(listeners);

        for(int i=0; i<30; i++)
        {
            T order = Orders.generateOrder(t);

            Map<Message, DeliveryOption> messageList = generateMessages(order, t);

            // publishes the bulk with notifications for delivery failure
            // returns all messages that have encountered an exception
            Map<Message, Exception> messageExceptionMap = _topic.publishBulk(messageList, true);

            // if any exceptions are encountered, throw to client
            if(messageExceptionMap.size() > 0)
                throw new Exception("Publish Bulk topics has encountered " + messageExceptionMap.size() + " exeptions");

            System.out.println(messageList.size() + " Messages for " + t.getName() + " generated");

            Thread.sleep(5 * 1000);//Sleep for 5 seconds.
        }
    }

    private <T extends Orders> Map<Message, DeliveryOption> generateMessages(T order, Class<T> t)
    {
        // create dictionary for storing bulk
        Map<Message, DeliveryOption> messageList = new HashMap<>();

        for(int i=0; i<5; i++)
        {
            // Adds the message with expiry to bulk
            Message message = new Message(order, new TimeSpan(0, 0, 15));
            messageList.put(message, DeliveryOption.Any);
        }
        return messageList;
    }

    /**
     * This method deletes the specified 'Topic' in the argumnet.
     * @param topicName Name of the topic to be deleted.
     * @throws CacheException
     */
    private static void deleteTopics(String topicName) throws CacheException {
        if(topicName == null) throw new IllegalArgumentException("topicName");

        // Deleting the topic.
        _cache.getMessagingService().deleteTopic(topicName);
    }


    private static <T extends Orders> void runPublisher(String topicName, Class<T> t) throws Exception {
        getPropsAndInitCache();
        Publisher publisher = new Publisher();
        publisher.PublishMessages(topicName, t);
    }

    private <T extends Orders> void PublishMessages(String topicName, Class<T> t) throws Exception {
        _topic = _cache.getMessagingService().getTopic(topicName);
        EventListeners listeners = new EventListeners();

        if(_topic == null)
        {
            _topic = _cache.getMessagingService().createTopic(topicName);
        }

        _topic.addMessageDeliveryFailureListener(listeners);

        //Generate 30 messages at the interval of 5 seconds.
        for(int i=0; i<30; i++)
        {
            T order = Orders.generateOrder(t);

            // Publishes the message with expiry.
            Message message = new Message(order, new TimeSpan(0, 0, 15));
            _topic.publish(message, DeliveryOption.All, true);

            System.out.println("Messages for " + t.getName() + " OrderId: " + order.getOrderId() + " generated");

            Thread.sleep(5 * 1000);//Sleep for 5 seconds.
        }
    }


    private static void getPropsAndInitCache() throws Exception {
        // Initialize connection
        Properties _properties = getProperties();
        // Initialize cache
        String cacheName= _properties.getProperty("CacheID");
        //  initialize cache before any operations
        initializeCache(cacheName);
    }

    /**
     * Class that provides the functionality of the sample
     */
    private static void initializeCache(String cacheName) throws Exception {
        if(cacheName == null){
            System.out.println("The CacheID cannot be null.");
            return;
        }

        if(cacheName.isEmpty()){
            System.out.println("The CacheID cannot be empty.");
            return;
        }

        // Initialize an instance of the cache to begin performing operations:
        _cache = CacheManager.getCache(cacheName);

        // Print output on console
        System.out.println();
        System.out.println("Cache " + cacheName + " initialized successfully.");
    }

    /**
     This method returns property file
     */
    private static Properties getProperties() throws IOException
    {
        String path = "config.properties";
        InputStream inputStream = Publisher.class.getClassLoader().getResourceAsStream(path);
        Properties properties=new Properties();
        if (inputStream != null) {
            properties.load(inputStream);
        } else {
            throw new FileNotFoundException("property file '" + path + "' not found in the classpath");
        }
        return properties;
    }
}
