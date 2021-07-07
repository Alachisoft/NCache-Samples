const {CacheManager,DeliveryMode,TimeSpan} = require('ncache-client');
let cache = null;
const config = require('./app.config.json');
const {delay} = require('./helper/util');
const SubscriptionPolicy = require('ncache-client/src/runtime/caching/subscriptionpolicy');
const MessageReceivedListener = require('ncache-client/src/runtime/caching/messagereceivedlistener');
async function Run() {
    await InitializeCache();
    const subscription1 = await RunSubscriber("ElectronicsOrders","ElectronicsSubscription", SubscriptionPolicy.Shared);
    const subscription2 = await RunSubscriber("GarmentsOrders","GarmentsSubscription",SubscriptionPolicy.Exclusive);
    await delay(60000);
    console.log("unsubsribed durable subscription")
    await subscription1.unSubscribe();
    await subscription2.unSubscribe();
    await cache.close();
}
async function InitializeCache() {
    // Get Cache Name
    const cacheName = config.cacheProperties.cacheName
    if (!cacheName) {
        console.log("The CacheID cannot be null or empty.");
        return;
    }
    // Initialize an instance of the cache to begin performing operations:
    cache = await CacheManager.getCache(cacheName);

    // Print output on console
    console.log(`Cache ${cacheName} is initialized.`);
}
async function RunSubscriber(topicName,subscriptionName,policy) {
     // Initialize cache 
    await InitializeCache();
    const messagingService = await cache.getMessagingService();
    let topic = await messagingService.getTopic(topicName);
    if (!topic)
        topic = await messagingService.createTopic(topicName);
    // Subscribes to the topic.
    const listener = new MessageReceivedListener(MessageReceivedCallback);
    return await topic.createDurableSubscription(subscriptionName,policy,listener,new TimeSpan(1,0,0), DeliveryMode.Sync);
}
function MessageReceivedCallback(sender, args) {
    console.log(`Message recived for topic ${args.getTopicName()}`)
}
module.exports = {
    Run
}