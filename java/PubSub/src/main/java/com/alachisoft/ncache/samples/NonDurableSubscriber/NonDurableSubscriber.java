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

package com.alachisoft.ncache.samples.NonDurableSubscriber;
import com.alachisoft.ncache.client.Cache;
import com.alachisoft.ncache.client.CacheManager;
import com.alachisoft.ncache.runtime.caching.SubscriptionPolicy;
import com.alachisoft.ncache.runtime.caching.Topic;
import com.alachisoft.ncache.runtime.caching.TopicSubscription;
import com.alachisoft.ncache.runtime.caching.messaging.TopicSearchOptions;
import com.alachisoft.ncache.samples.DurableSubscriber.DurableSubscriber;
import com.alachisoft.ncache.samples.EventListeners.MessageReceivedListenerImpl;
import com.alachisoft.ncache.samples.EventListeners.PatternBasedMessageReceivedListener;
import com.alachisoft.ncache.samples.Utilities.TopicUtil;
import java.io.IOException;
import java.io.InputStream;
import java.util.Properties;

/*
 * ===============================================================================
 * Non-Durable Subscribers receive messages only while they are active and
 * connected. If a subscriber disconnects or the application stops, it will miss
 * any messages published during its downtime.
 * Use non-durable subscriptions for real-time, transient data streams such as
 * live dashboards or chat notifications.
 * ===============================================================================
 */

public class NonDurableSubscriber {
    private static Cache _cache;


    public static void main(String[] args) throws Exception {
        // Connect to a running cache and get cache handle for it
        _cache = getCache();

        if (_cache == null) {
            System.out.println("Cache could not be initialized.");
            return;
        }

        // Create Non Durable Subscriptions
        TopicSubscription electronicsSharedSubs = CreateNonDurableSubscription("ElectronicsOrders",
                "ElectronicsSubscription", SubscriptionPolicy.Shared);
        TopicSubscription garmentsExclusiveSubs = CreateNonDurableSubscription("GarmentsOrders",
                "GarmentsSubscription", SubscriptionPolicy.Exclusive);

        //Following patterns can be used;
        //* -	 represents zero or more characters. For example, ‘bl*’ finds TOPICs ‘black’, ‘blue’, and ‘blob’ etc.
        //? -	 represents one character. For example, ‘h?t’ finds ‘hot’, ‘hat’, and ‘hit’ etc.
        //[]-	 represents any single character within the brackets. For example, ‘h[oa]t’ finds hot and hat, but not hit.

        TopicSubscription allOrdersSubscription = CreatePatternSubscription("*Orders");

        System.out.println("Press any key to stop subscribers...");
        System.in.read();

        // Unsubscribe Non Durable Subscriptions
        electronicsSharedSubs.unSubscribe();
        garmentsExclusiveSubs.unSubscribe();
        assert allOrdersSubscription != null;
        allOrdersSubscription.unSubscribe();

        // Closing the cache handle
        _cache.close();
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to connect to a running cache and return a cache handle for it.
     *
     * @return Cache handle for the connected cache.
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
     * Method to create NonDurable Subscription on a topic.
     *
     * @param topicName        Name of the topic.
     * @param subscriptionName Name of the subscription.
     * @param policy           Subscription policy.
     * @return NonDurable Topic Subscription.
     */
    private static TopicSubscription CreateNonDurableSubscription(String topicName, String subscriptionName,
                                                                  SubscriptionPolicy policy) throws Exception {

        // Create Subscription on the specified topic.
        Topic topic = TopicUtil.getOrCreateTopic(_cache, topicName);

        // Create message listener
        MessageReceivedListenerImpl messageReceivedListener = new MessageReceivedListenerImpl();

        // Subscribes to the topic.
        return topic.createSubscription(messageReceivedListener);
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to create NonDurable Subscription on topics matching a pattern.
     *
     * @param pattern Pattern to match topic names.
     * @return NonDurable Topic Subscription.
     */
    private static TopicSubscription CreatePatternSubscription(String pattern) throws Exception {

        //Create Subscription on all the topics that matches the pattern.
        Topic patternTopic = _cache.getMessagingService().getTopic(pattern, TopicSearchOptions.ByPattern);

        // Create pattern based message listener
        PatternBasedMessageReceivedListener patternBasedMessageReceivedListener =
                new PatternBasedMessageReceivedListener();

        // Subscribes to the topic.
        if(patternTopic != null)
            return patternTopic.createSubscription(patternBasedMessageReceivedListener);

        return null;
    }


    // ------------------------------------------------------------------------------
    /**
     * Reads value from config.properties
     */
    private static String getConfigValue(String key) {
        try {
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
