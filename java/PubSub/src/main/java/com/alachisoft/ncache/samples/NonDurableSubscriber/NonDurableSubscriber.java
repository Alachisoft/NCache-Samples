package java.com.alachisoft.ncache.samples.NonDurableSubscriber;
// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================


import java.com.alachisoft.ncache.samples.EventListeners.OnMessageReceivedListener;
import java.com.alachisoft.ncache.samples.EventListeners.PatternBasedMessageReceivedListener;
import com.alachisoft.ncache.client.Cache;
import com.alachisoft.ncache.client.CacheManager;
import com.alachisoft.ncache.runtime.caching.Topic;
import com.alachisoft.ncache.runtime.caching.TopicSubscription;
import com.alachisoft.ncache.runtime.caching.messaging.TopicSearchOptions;

import java.io.*;
import java.util.Properties;

public class NonDurableSubscriber {
    private static Cache _cache;

    public static void run()
    {
        try
        {
            getPropsAndInitCache();

            TopicSubscription electronicsSubscription = runSubscriber("ElectronicsOrders");
            TopicSubscription garmentsSubscription = runSubscriber("GarmentsOrders");

            //Following patterns can be used;
            //* -	 represents zero or more characters. For example, ‘bl*’ finds TOPICs ‘black’, ‘blue’, and ‘blob’ etc.
            //? -	 represents one character. For example, ‘h?t’ finds ‘hot’, ‘hat’, and ‘hit’ etc.
            //[]-	 represents any single character within the brackets. For example, ‘h[oa]t’ finds hot and hat, but not hit.

            TopicSubscription allOrdersSubscription = runPatternBasedSubscriber("*Orders");

            System.out.println("Press any key to stop subscribers...");
            System.in.read();

            electronicsSubscription.unSubscribe();
            garmentsSubscription.unSubscribe();
            allOrdersSubscription.unSubscribe();

        }catch (Exception ex)
        {
            ex.printStackTrace();
        }
    }

    private static TopicSubscription runPatternBasedSubscriber(String pattern) throws Exception {
        //Create Subscription on all the topics that matches the pattern.
        Topic patternTopic = _cache.getMessagingService().getTopic(pattern, TopicSearchOptions.ByPattern);
        PatternBasedMessageReceivedListener subscriberCallbacks = new PatternBasedMessageReceivedListener();

        // Subscribes to the topic.
        if(patternTopic != null)
            return patternTopic.createSubscription(subscriberCallbacks);

        return null;
    }

    private static TopicSubscription runSubscriber(String topicName) throws Exception {
        // Initialize cache
        getPropsAndInitCache();

        Topic topic = _cache.getMessagingService().getTopic(topicName);

        OnMessageReceivedListener messageReceivedListener = new OnMessageReceivedListener();

        if(topic == null)
            topic = _cache.getMessagingService().createTopic(topicName);

        // Subscribes to the topic.
        return topic.createSubscription(messageReceivedListener);
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
        System.out.println("Cache initialized succesfully.");
    }

    /**
     This method returns property file
     */
    private static Properties getProperties() throws IOException
    {
        String path = "java/resources/config.properties";
        InputStream inputStream = NonDurableSubscriber.class.getClassLoader().getResourceAsStream(path);
        Properties properties=new Properties();
        if (inputStream != null) {
            properties.load(inputStream);
        } else {
            throw new FileNotFoundException("property file '" + path + "' not found in the classpath");
        }
        return properties;
    }
}
