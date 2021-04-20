// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
package com.alachisoft.ncache.samples.publisher;
import com.alachisoft.ncache.runtime.util.TimeSpan;

import java.io.*;
import java.util.Properties;
import java.util.Scanner;

/**
 * A sample program that demonstrates how to use the messaging api in NCache.
 * @requirements
 * 1. A running NCache cache
 * 2. Connection attributes in app.config
 */
public class RunPublisher {
    public static void main(String[] args) {
        try{
            // Initialize connection
            Properties _properties = getProperties();
            // Initialize cache
            String cacheName= _properties.getProperty("CacheID");

            String topicName = "myTopic";
            int messageCount = 100;

            // Creates a publisher's instance.
            Publisher publisher = new Publisher();
            // Starts the publisher on the topic.
            publisher.start(cacheName, topicName);
            publisher.subscribe();

            System.out.println("Publisher Started...");

            for(int i=1; i<=messageCount; i++)
            {
                String payLoad = String.format("Message %s", i);

                if(i % 2 == 0)
                {
                    // Publishes the message with expiry.
                    publisher.publish(payLoad, TimeSpan.FromMinutes(5));
                    System.out.println("Message published with the expiry of 5 minutes.");
                    Thread.sleep(1000);
                }
                else
                {
                    // Publishes the message.
                    publisher.publish(payLoad);
                    System.out.println("Message published without expiry.");
                    Thread.sleep(1000);
                }
            }

            System.out.println("Press enter to continue...");
            Scanner s = new Scanner(System.in);
            String u = s.nextLine();

            //Deletes the topic.
            publisher.deleteTopic(topicName);
            System.out.println("Topic Deleted.");

            publisher.unSubscribe();
        }catch (Exception ex){
            ex.printStackTrace();
        }
    }

    /**
     This method returns property file
     */
    private static Properties getProperties() throws IOException
    {
        String path = "config.properties";
        InputStream inputStream = RunPublisher.class.getClassLoader().getResourceAsStream(path);
        Properties properties=new Properties();
        if (inputStream != null) {
            properties.load(inputStream);
        } else {
            throw new FileNotFoundException("property file '" + path + "' not found in the classpath");
        }
        return properties;
    }
}
