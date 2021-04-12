// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
package com.Alachisoft.NCache.Samples.subscriber;

import com.Alachisoft.NCache.Samples.callbacks.MessageReceivedCallbacks;

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
            Subscriber subscriber = new Subscriber();

            // Subscribes on it, using the provided cache-Id and the topic-name.
            MessageReceivedCallbacks messageReceivedCallbacks = new MessageReceivedCallbacks();
            subscriber.Subscribe(cacheName, topicName, messageReceivedCallbacks);

            System.out.println("Subscriber Started. Press enter to exit.");
            System.console().readLine();



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
