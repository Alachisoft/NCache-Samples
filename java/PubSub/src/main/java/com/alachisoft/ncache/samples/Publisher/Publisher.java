/*
 * ===============================================================================
 * Alachisoft (R) NCache Sample Code.
 * NCache Basic Operations sample
 * ===============================================================================
 * Copyright © Alachisoft.  All rights reserved.
 * THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
 * OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
 * LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
 * FITNESS FOR A PARTICULAR PURPOSE.
 * ===============================================================================
 */

package com.alachisoft.ncache.samples.Publisher;
import com.alachisoft.ncache.client.Cache;
import com.alachisoft.ncache.client.CacheManager;
import com.alachisoft.ncache.runtime.caching.*;
import com.alachisoft.ncache.runtime.exceptions.CacheException;
import com.alachisoft.ncache.runtime.util.TimeSpan;
import com.alachisoft.ncache.samples.DurableSubscriber.DurableSubscriber;
import com.alachisoft.ncache.samples.EventListeners.MessageDeliveryFailureListener;
import com.alachisoft.ncache.samples.Utilities.TopicUtil;
import com.alachisoft.ncache.samples.data.Order;
import java.io.IOException;
import java.io.InputStream;
import java.util.HashMap;
import java.util.Map;
import java.util.Properties;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

/*
 * ===============================================================================
 * Publish/Subscribe (Pub/Sub) is a messaging feature provided by NCache.
 * It allows applications to send and receive messages through topics without
 * being directly connected to each other. Some possible use cases of Pub/Sub
 * in NCache include:
 *   1. Sending real-time updates, such as order status, inventory changes, or
 *      quote alerts.
 *   2. Broadcasting notifications to multiple systems, microservices, or users.
 *   3. Updating dashboards or live data feeds automatically without polling.
 *   4. Facilitating communication between different components of a distributed
 *      system.
 *   5. Implementing event-driven architectures for decoupled and scalable systems.
 *
 * This sample demonstrates publishing messages synchronously, asynchronously,
 * and in bulk to topics. Corresponding Durable and Non-Durable Subscriber samples
 * can receive these messages.
 * ===============================================================================
 */

public class Publisher {
    private static Cache _cache;
    private Topic topic;
    private static int messagesToBePublished = 30;

    public static void main(String[] args) throws Exception {

        // Connect to a running cache and get cache handle for it
        _cache = getCache();

        if (_cache == null) {
            System.out.println("Cache could not be initialized.");
            return;
        }

        System.out.println("Starting 3 Publishers... Publishers will stop after publishing 30 messages each.");

        // Using Executor Service to manage multiple publishers
        ExecutorService executor = Executors.newFixedThreadPool(3);

        // Submitting publisher tasks
        executor.submit(() -> {
            try {
                runPublisher("ElectronicsOrders", ElectronicsOrder.class);
            } catch (Exception e) {
                e.printStackTrace();
            }
        });
        executor.submit(() -> {
            try {
                runBulkPublisher("GarmentsOrders", GarmentsOrder.class);
            } catch (Exception e) {
                e.printStackTrace();
            }
        });
        executor.submit(() -> {
            try {
                runAsyncPublisher("GarmentsOrders", GarmentsOrder.class);
            } catch (Exception e) {
                e.printStackTrace();
            }
        });


        System.out.println("Publishers Started successfully.");

        System.out.println("Press any key to delete the topics");

        System.in.read();

        // Shutting down the executor service
        executor.shutdown();

        // Deleting the topics
        deleteTopics("ElectronicsOrders");
        deleteTopics("GarmentsOrders");

        // Closing the cache handle
       _cache.close();

    }

    // ------------------------------------------------------------------------------
    /**
     * Method to connect to a running cache and return cache handle for it
     *
     * @return Cache handle for the connected cache
     */
    private static Cache getCache() throws Exception {
        // Getting cache name from configuration file
        String cacheName = getConfigValue("CacheName");

        // Validating cache name
        if (cacheName == null || cacheName.isEmpty()) {
            System.out.println("The CacheName cannot be null or empty.");
            return null;
        }

        // Trying to connect to the cache
        try {
            // Connecting to a running cache and return a cache handle for it
            Cache cache = CacheManager.getCache(cacheName);

            // Printing output on console
            System.out.printf("Cache '%s' is connected.%n", cacheName);

            // Returning cache handle
            return cache;
        } catch(Exception e) {
            System.out.println("Unable to connect to cache: " + e.getMessage());
            return null;
        }
    }


    // ------------------------------------------------------------------------------
    /**
     * Method to call publisher to start publishing messages to a specific topic
     *
     * @param topicName Name of the topic to publish messages to.
     * @param t Type of Order to be published.
     */
    private static <T extends Order> void runPublisher(String topicName, Class<T> t) throws Exception {

        // Initialize publisher and call publish method
        Publisher publisher = new Publisher();
        publisher.PublishMessages(topicName,t);
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to start publishing messages synchronously to a specific topic
     * Each message is published individually with confirmation
     * @param topicName Name of the topic to publish messages to.
     * @param t Type of Order to be published.
     */
    private <T extends Order> void PublishMessages(String topicName, Class<T> t) throws Exception {

        // Get or create topic
        topic = TopicUtil.getOrCreateTopic(_cache, topicName);

        // Add message delivery failure listener
        MessageDeliveryFailureListener listeners = new MessageDeliveryFailureListener();

        // Attach the listener to the topic
        topic.addMessageDeliveryFailureListener(listeners);

        //Generate 30 messages at the interval of 5 seconds.
        for(int i=0; i<messagesToBePublished; i++)
        {
            T order = Order.generateOrder(t);

            // Publishes the message with expiry.
            Message message = new Message(order, new TimeSpan(0, 0, 15));
            topic.publish(message, DeliveryOption.All, true);
            System.out.println("Messages for ElectronicsOrder. OrderId: " + order.getOrderID()+ " generated");
        }

        Thread.sleep(5000);//Sleep for 5 seconds.
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to call publisher to start publishing messages in bulk to a specific topic
     *
     * @param topicName Name of the topic to publish messages to.
     * @param t Type of Order to be published.
     */
     private static <T extends Order> void runBulkPublisher(String topicName, Class<T> t) throws Exception {

         // Initialize publisher and call publish bulk method
        Publisher publisher = new Publisher();
        publisher.publishBulkMesages(topicName, t);
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to start publishing bulk messages synchronously to a specific topic
     *
     * @param topicName Name of the topic to publish messages to.
     * @param t Type of Order to be published.
     */
    private <T extends Order> void publishBulkMesages(String topicName, Class<T> t) throws Exception {

        // Get or create topic
        topic = TopicUtil.getOrCreateTopic(_cache, topicName);

        // Add message delivery failure listener
        MessageDeliveryFailureListener listeners = new MessageDeliveryFailureListener();

        // Attach the listener to the topic
        topic.addMessageDeliveryFailureListener(listeners);

        // Generate 30 messages at the interval of 5 seconds.
        for(int i=0; i<messagesToBePublished; i++)
        {
            // Generate order
            T order = Order.generateOrder(t);

            // Generate messages for bulk publishing
            Map<Message, DeliveryOption> messageList = generateMessages(order, t);

            // publishes the bulk with notifications for delivery failure
            // returns all messages that have encountered an exception
            Map<Message, Exception> messageExceptionMap = topic.publishBulk(messageList, true);

            // if any exceptions are encountered, throw to client
            if(!messageExceptionMap.isEmpty())
                throw new Exception("Publist Bulk topics has encountered " + messageExceptionMap.size() + " exeptions");

            System.out.println(messageList.size() + " Messages for " + t.getName() + " generated");

            Thread.sleep(5000);//Sleep for 5 seconds.
        }
    }


    // ------------------------------------------------------------------------------
    /**
     * Method to call publisher to start publishing messages asynchronously to a specific topic
     *
     * @param topicName Name of the topic to publish messages to.
     * @param t Type of Order to be published.
     */
    private static <T extends Order> void runAsyncPublisher(String topicName, Class<T> t) throws Exception {

        // Initialize publisher and call publish async method
        Publisher publisher = new Publisher();
        publisher.publishAsyncMessages(topicName, t);
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to start publishing messages asynchronously to a specific topic
     * Each message is published individually with confirmation
     * @param topicName Name of the topic to publish messages to.
     * @param t Type of Order to be published.
     */
    private <T extends Order> void publishAsyncMessages(String topicName, Class<T> t) throws Exception {

        // Get or create topic
        topic = TopicUtil.getOrCreateTopic(_cache, topicName);

        // Add message delivery failure listener
        MessageDeliveryFailureListener listeners = new MessageDeliveryFailureListener();

        // Attach the listener to the topic
        topic.addMessageDeliveryFailureListener(listeners);

        //Generate 30 messages at the interval of 5 seconds.
        for(int i=0; i<messagesToBePublished; i++)
        {
            T order = Order.generateOrder(t);

            // Publishes the message with expiry.
            Message message = new Message(order, new TimeSpan(0, 0, 15));
            topic.publishAsync(message, DeliveryOption.All, true);
            System.out.println("Messages for " + t.getName() + " OrderId: " + order.getOrderID() + " generated");
        }

        Thread.sleep(5000);//Sleep for 5 seconds.
    }

    /**
     * This method deletes the specified 'Topic' in the argumnet.
     * @param topicName Name of the topic to be deleted.
     */
    private static void deleteTopics(String topicName) throws CacheException {

        // Validate topic name
        if(topicName == null) throw new IllegalArgumentException();

        // Deleting the topic.
        _cache.getMessagingService().deleteTopic(topicName);
    }


    // ------------------------------------------------------------------------------
    /**
     * Method to call publisher to start publishing messages in bulk to a specific topic
     *
     * @param order Order object to wrap in each message.
     * @param t Type of Order to be published.
     */
    private <T extends Order> Map<Message, DeliveryOption> generateMessages(T order, Class<T> t)
    {
        // create dictionary for storing bulk
        Map<Message, DeliveryOption> messageList = new HashMap<>();

        // Generate 5 messages for bulk
        for(int i=0; i<5; i++)
        {
            // Adds the message with expiry to bulk
            Message message = new Message(order, new TimeSpan(0, 0, 15));
            messageList.put(message, DeliveryOption.All);
        }
        return messageList;
    }

    // ------------------------------------------------------------------------------
    /**
     * Reads value from config.properties
     */
    private static String getConfigValue(String key) {
        try{
            // Getting input stream to read properties file
            InputStream stream = DurableSubscriber.class
                    .getClassLoader()
                    .getResourceAsStream("config.properties");

            if (stream == null) {
                System.out.println("config.properties not found.");
                return null;
            }

            // Loading properties from the input stream
            Properties props = new Properties();
            props.load(stream);

            // Returning value for the provided key
            return props.getProperty(key);

        } catch (IOException e) {
            return null;
        }
    }
    

}

