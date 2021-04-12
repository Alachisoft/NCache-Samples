package java.com.alachisoft.ncache.samples.DurableSubscriber;// ===============================================================================
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
import com.alachisoft.ncache.runtime.caching.SubscriptionPolicy;
import com.alachisoft.ncache.runtime.caching.Topic;
import com.alachisoft.ncache.runtime.caching.messaging.DurableTopicSubscription;
import com.alachisoft.ncache.runtime.caching.messaging.TopicSearchOptions;
import com.alachisoft.ncache.runtime.exceptions.CacheException;
import com.alachisoft.ncache.runtime.util.TimeSpan;
import java.com.alachisoft.ncache.samples.EventListeners.OnMessageReceivedListener;
import java.com.alachisoft.ncache.samples.EventListeners.PatternBasedMessageReceivedListener;

import java.io.*;
import java.util.Properties;

public class DurableSubscriber {
    private static Cache _cache;

    public static void run() {
        try
        {
            getPropsAndInitCache();

            DurableTopicSubscription electronicsSharedSubs = runSubscriber("ElectronicsOrders",
                    "ElectronicsSubscription", SubscriptionPolicy.Shared);
            DurableTopicSubscription garmentsExclusiveSubs = runSubscriber("GarmentsOrders",
                    "GarmentsSubscription", SubscriptionPolicy.Exclusive);

            //Following patterns can be used;
            //* -	 represents zero or more characters. For example, ‘bl*’ finds TOPICs ‘black’, ‘blue’, and ‘blob’ etc.
            //? -	 represents one character. For example, ‘h?t’ finds ‘hot’, ‘hat’, and ‘hit’ etc.
            //[]-	 represents any single character within the brackets. For example, ‘h[oa]t’ finds hot and hat, but not hit.

            DurableTopicSubscription allOrdersSubscription = runPatternBasedSubscriber("*Orders", "AllOrdersSubscription");

            System.out.println("Press any key to stop subscribers...");
            System.in.read();

            electronicsSharedSubs.unSubscribe();
            garmentsExclusiveSubs.unSubscribe();
            assert allOrdersSubscription != null;
            allOrdersSubscription.unSubscribe();
        }catch (Exception ex)
        {
            ex.printStackTrace();
        }
    }

    private static DurableTopicSubscription runPatternBasedSubscriber(String pattern, String subscriptionName) throws Exception {
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

    private static DurableTopicSubscription runSubscriber(String topicName, String subscriptionName,
                                                          SubscriptionPolicy policy) throws CacheException {
        Topic topic = _cache.getMessagingService().getTopic(topicName);
        OnMessageReceivedListener messageReceivedListener = new OnMessageReceivedListener();

        if (topic == null)
            topic = _cache.getMessagingService().createTopic(topicName);

        //Creates durable subscription
        //subscriptionName: User defined name for subscription
        //Subscription Policy: If subscription policy is shared , it means subscription can hold multiple active subscribers
        //If subscription policy is exclusive , it means subscription can hold one active client only.

        return topic.createDurableSubscription(subscriptionName, policy, messageReceivedListener,
                new TimeSpan(1, 0, 0));
    }

    private static void getPropsAndInitCache() throws Exception {
        // Initialize connection
        Properties _properties = getProperties();
        // Initialize cache
        String cacheName = _properties.getProperty("CacheID");
        //  initialize cache before any operations
        initializeCache(cacheName);
    }

    /**
     * Class that provides the functionality of the sample
     */
    private static void initializeCache(String cacheName) throws Exception {
        if (cacheName == null) {
            System.out.println("The CacheID cannot be null.");
            return;
        }

        if (cacheName.isEmpty()) {
            System.out.println("The CacheID cannot be empty.");
            return;
        }

        // Initialize an instance of the cache to begin performing operations:
        _cache = CacheManager.getCache(cacheName);

        // Print output on console
        System.out.println();
        System.out.println("Cache initialized succesfully.");
    }

    /**
     This method returns property file
     */
    private static Properties getProperties() throws IOException
    {
        String path = "java/resources/config.properties";
        InputStream inputStream = DurableSubscriber.class.getClassLoader().getResourceAsStream(path);
        Properties properties=new Properties();
        if (inputStream != null) {
            properties.load(inputStream);
        } else {
            throw new FileNotFoundException("property file '" + path + "' not found in the classpath");
        }
        return properties;
    }
}
