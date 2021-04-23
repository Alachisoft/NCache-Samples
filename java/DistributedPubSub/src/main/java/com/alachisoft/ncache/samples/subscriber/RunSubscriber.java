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
import java.io.*;
import java.util.Properties;

/**
 * A sample program that demonstrates how to use the messaging api in NCache.
 */
public class RunSubscriber {
    public static void main(String[] args) {
        try
        {
            // Initialize connection
            Properties _properties = getProperties();
            // Initialize cache
            String cacheName= _properties.getProperty("CacheID");

            String topicName = "myTopic";
            // Creates a subscriber's instance.
            TopicSubscriber subscriber = new TopicSubscriber();

            // Subscribes on it, using the provided cache-Id and the topic-name.
            MessageReceivedCallbacks messageReceivedCallbacks = new MessageReceivedCallbacks();
            subscriber.subscribe(cacheName, topicName, messageReceivedCallbacks);

            Thread.sleep(10000);


        }catch (Exception e)
        {
            e.printStackTrace();
        }
    }

    /**
     This method returns property file
     */
    private static Properties getProperties() throws IOException
    {
        String path = "config.properties";
        InputStream inputStream = RunSubscriber.class.getClassLoader().getResourceAsStream(path);
        Properties properties=new Properties();
        if (inputStream != null) {
            properties.load(inputStream);
        } else {
            throw new FileNotFoundException("property file '" + path + "' not found in the classpath");
        }
        return properties;
    }
}
