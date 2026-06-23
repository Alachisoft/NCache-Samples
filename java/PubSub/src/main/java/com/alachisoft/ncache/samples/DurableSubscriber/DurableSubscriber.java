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

package com.alachisoft.ncache.samples.DurableSubscriber;
import com.alachisoft.ncache.client.Cache;
import com.alachisoft.ncache.client.CacheManager;
import com.alachisoft.ncache.runtime.caching.SubscriptionPolicy;
import com.alachisoft.ncache.runtime.caching.Topic;
import com.alachisoft.ncache.runtime.caching.messaging.DurableTopicSubscription;
import com.alachisoft.ncache.runtime.caching.messaging.TopicSearchOptions;
import com.alachisoft.ncache.runtime.util.TimeSpan;
import com.alachisoft.ncache.samples.EventListeners.MessageReceivedListenerImpl;
import com.alachisoft.ncache.samples.EventListeners.PatternBasedMessageReceivedListener;
import com.alachisoft.ncache.samples.Utilities.TopicUtil;
import java.io.IOException;
import java.io.InputStream;
import java.util.Properties;

/*
 * ===============================================================================
 * Durable Subscribers are persistent and can recover messages published while
 * they were offline. Once reconnected, they receive all pending messages that
 * match their subscription.
 * Use durable subscriptions for critical or time-sensitive systems such as
 * order processing and payment workflows.
 * ===============================================================================
 */

public class DurableSubscriber {

    private static Cache _cache;
    public static void main(String[] args) throws Exception {

        // Connect to a running cache and get cache handle for it
        _cache = getCache();

        if (_cache == null) {
            System.out.println("Cache could not be initialized.");
            return;
        }

        // Create Durable Subscriptions
        DurableTopicSubscription electronicsSharedSubs = CreateDurableSubscription("ElectronicsOrders",
                "ElectronicsSubscription", SubscriptionPolicy.Shared);
        DurableTopicSubscription garmentsExclusiveSubs = CreateDurableSubscription("GarmentsOrders",
                "GarmentsSubscription", SubscriptionPolicy.Exclusive);

        //Following patterns can be used;
        //* -	 represents zero or more characters. For example, ‘bl*’ finds TOPICs ‘black’, ‘blue’, and ‘blob’ etc.
        //? -	 represents one character. For example, ‘h?t’ finds ‘hot’, ‘hat’, and ‘hit’ etc.
        //[]-	 represents any single character within the brackets. For example, ‘h[oa]t’ finds hot and hat, but not hit.

        DurableTopicSubscription allOrdersSubscription = CreatePatternSubscription("*Orders", "AllOrdersSubscription");

        System.out.println("Press any key to stop subscribers...");
        System.in.read();

        // Unsubscribe Durable Subscriptions
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
     * Method to create a Durable Topic Subscription.
     *
     * @param topicName        Name of the topic.
     * @param subscriptionName Name of the subscription.
     * @param policy           Subscription policy.
     * @return Durable Topic Subscription.
     */
    private static DurableTopicSubscription CreateDurableSubscription(String topicName, String subscriptionName,
                                                                      SubscriptionPolicy policy) throws Exception {
        Topic topic = TopicUtil.getOrCreateTopic(_cache, topicName);
        MessageReceivedListenerImpl messageReceivedListener = new MessageReceivedListenerImpl();

        return topic.createDurableSubscription(subscriptionName, policy, messageReceivedListener,
                TimeSpan.FromHours(1));
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to create Durable Subscription on topics matching a pattern.
     *
     * @param pattern Pattern to match topic names.
     * @return Durable Topic Subscription.
     */
    private static DurableTopicSubscription CreatePatternSubscription(String pattern, String subscriptionName) throws Exception {
        //Create Subscription on all the topics that matches the pattern.
        Topic patternTopic = _cache.getMessagingService().getTopic(pattern, TopicSearchOptions.ByPattern);
        PatternBasedMessageReceivedListener patternBasedMessageReceivedListener =
                new PatternBasedMessageReceivedListener();

        // Subscribes to the topic.
        if(patternTopic != null)
            return patternTopic.createDurableSubscription(subscriptionName, SubscriptionPolicy.Shared
                    , patternBasedMessageReceivedListener, new TimeSpan(1, 0, 0));

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
