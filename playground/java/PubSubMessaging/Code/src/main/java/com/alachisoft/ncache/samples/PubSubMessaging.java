package com.alachisoft.ncache.samples;

import java.io.Serializable;
import java.time.LocalDate;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import com.alachisoft.ncache.client.*;
import com.alachisoft.ncache.client.services.MessagingService;
import com.alachisoft.ncache.runtime.caching.*;
import com.alachisoft.ncache.runtime.caching.messaging.MessageReceivedListener;

// Use NCache Pub/Sub Messaging in this sample. In NCache, you can create multiple Topics
// and against each topic create multiple publishers and subscribers. In this sample,
// 2 parallel Async publisher tasks publish on the same topic and 2 subscribers receive
// messages from this topic. All messages use DeliveryOption. Any so only one subscriber
// gets each message. DeliveryOption.All is available but not used.

public class PubSubMessaging {
    static String topicName = "ORDERS";
    static final Object lock = new Object();

    public static void main(String[] args) throws Exception {

        System.out.println("Sample showing NCache Pub/Sub Messaging for reliable real-time messaging between publishers and subscribers.\n");

        String cacheName = args[0];

        System.out.println("Connecting to cache: " + cacheName);
        // Connect to the cache and return a cache handle
        Cache cache = CacheManager.getCache(cacheName);
        System.out.println("Connected to Cache successfully: " + cacheName + "\n");

        System.out.println("Create a Topic " + topicName + " ...\n");

        // Create a Topic in NCache.
        MessagingService messagingService = cache.getMessagingService();
        Topic topic = messagingService.createTopic(topicName);

        System.out.println("Register 2 subscribers for Topic " + topicName + "\n");

        // Register subscribers to this Topic
        MessageReceivedListener subscriptionListener1 = new MessageReceivedListener() {
            @Override
            public void onMessageReceived(Object o, MessageEventArgs messageEventArgs) {
                messageReceivedSubscription1(messageEventArgs.getMessage());
            }
        };
        MessageReceivedListener subscriptionListener2 = new MessageReceivedListener() {
            @Override
            public void onMessageReceived(Object o, MessageEventArgs messageEventArgs) {
                messageReceivedSubscription2(messageEventArgs.getMessage());
            }
        };
        TopicSubscription subscription1 = topic.createSubscription(subscriptionListener1);
        TopicSubscription subscription2 = topic.createSubscription(subscriptionListener2);

        // Create a thread pool for publishers
        ExecutorService publisherThreadPool = Executors.newFixedThreadPool(2);

        // Create first asynchronous publisher task to publish 2 messages
        publisherThreadPool.submit(() -> {
            try {
                Message message1 = new Message(
                        new Order(17348, "Vins et alcools Chevalier", LocalDate.parse("1997-07-04"), "Federal Shipping"));

                Message message2 = new Message(
                        new Order(17350, "Hanari Carnes", LocalDate.parse("1997-07-07"), "United Package"));

                printMessage(message1, "Publisher 1", null);
                // "Any" means that only one subscriber will receive this message
                // Another option is "All" where all subscribers receive the message
                topic.publish(message1, DeliveryOption.Any);

                // Wait for 1 second before publishing again
                Thread.sleep(1000);
                printMessage(message2, "Publisher 1", null);

                topic.publish(message2, DeliveryOption.Any);
            } catch (Exception ex) {
            }
        });

        // Create second asynchronous publisher task to publish 2 messages
        publisherThreadPool.submit(() -> {
            try {
                Message message1 = new Message(
                        new Order(17349, "Toms Spezialitäten", LocalDate.parse("1997-07-05"), "Speedy Express"));

                Message message2 = new Message(
                        new Order(17351, "Victuailles en stock", LocalDate.parse("1997-07-08"), "Speedy Express"));

                printMessage(message1, "Publisher 2", null);

                // "Any" means that only one subscriber will receive this message
                // Another option is "All" where all subscribers receive the message
                topic.publish(message1, DeliveryOption.Any);

                // Wait for 1 second before publishing again
                Thread.sleep(1000);
                printMessage(message2, "Publisher 2", null);

                topic.publish(message2, DeliveryOption.Any);
            } catch (Exception ex) {
            }
        });

        // Wait for 5 seconds
        Thread.sleep(5000);

        // Unsubscribe subscribers
        subscription1.unSubscribe();
        subscription2.unSubscribe();

        // Shut down the thread pool
        publisherThreadPool.shutdown();

        topic.close();
        System.out.println("Sample completed successfully.");
        cache.close();
    }

    private static void messageReceivedSubscription1(Message message) {
        printMessage(message, null, "Subscriber 1");
    }

    private static void messageReceivedSubscription2(Message message) {
        printMessage(message, null, "Subscriber 2");
    }

    static void printMessage(Message message, String sender, String receiver) {
        Order order = (Order) message.getPayload();

        synchronized (lock) {
            if (sender != null && !sender.isEmpty()) {
                System.out.println(sender + " publishes a message for Topic " + topicName + " ...");
            } else if (receiver != null && !receiver.isEmpty()) {
                System.out.println(receiver + " receives message ...");
            }

            System.out.println("OrderID, Customer, Order Date, Ship Via");
            System.out.println("- " + order.getOrderID() + ", " + order.getCustomer() + ", "
                    + order.getOrderDate().toString() + ", " + order.getShipVia() + " \n");
        }
    }
}
// Sample class for Order
class Order implements Serializable {
    private int orderID;
    private String customer;
    private LocalDate orderDate;
    private String shipVia;

    public Order(int orderID, String customer, LocalDate orderDate, String shipVia) {
        this.orderID = orderID;
        this.customer = customer;
        this.orderDate = orderDate;
        this.shipVia = shipVia;
    }

    public int getOrderID() { return orderID; }
    public String getCustomer() { return customer; }
    public LocalDate getOrderDate() { return orderDate; }
    public String getShipVia() { return shipVia; }
}