const {CacheManager} = require('ncache-client');
let cache = null;
const config = require('./app.config.json');
const {delay} = require('./helper/util');
const MessageReceivedListener = require('ncache-client/src/runtime/caching/messagereceivedlistener');
async function Run() {
    await InitializeCache();
    const subscription1 = await RunSubscriber("ElectronicsOrders");
    const subscription2 = await RunSubscriber("GarmentsOrders");
    await delay(60000);
    console.log("unsubsribed subscription")
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
async function RunSubscriber(topicName) {
     // Initialize cache 
    await InitializeCache();
    const messagingService = await cache.getMessagingService();
    let topic = await messagingService.getTopic(topicName);
    if (!topic)
        topic = await messagingService.createTopic(topicName);
    // Subscribes to the topic.
    return topic.createSubscription(new MessageReceivedListener(MessageReceivedCallback));
}
function MessageReceivedCallback(sender, args) {
    console.log(`Message recived for topic ${args.getTopicName()}`)
}
module.exports = {
    Run
}