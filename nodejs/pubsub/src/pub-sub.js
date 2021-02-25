const {CacheManager,DeliveryOption, Message,TopicListener,TimeSpan} = require('ncache-client');
const config = require('./app.config.json');
const {delay, promiseHandler} = require('./helper/util')
async function Run() {
    console.log("Starting 2 Publishers... Publishers will stop after publishing 5 messages each");
    console.log("Publishers Started.");
    await Promise.all([RunPublisher("ElectronicsOrders"),RunBulkPublisher("GarmentsOrders")]
    .map(promiseHandler))
    .then(async (result) => {
        await delay(6000);
        if (result[0].resolved) {
            await DeleteTopic(result[0].payload,"ElectronicsOrders");
            console.log("Electronics TOPIC Deleted.");
            await result[0].payload.close();
        }
        if (result[1].resolved) {
            await DeleteTopic(result[1].payload,"GarmentsOrders");
            console.log("Garments TOPIC Deleted.");
            await result[1].payload.close();
        }
    });
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
    return cache;
            
}
async function RunPublisher(topicName) {
    const cache = await InitializeCache();
    const messagingService = await cache.getMessagingService();
    let topic = await  messagingService.getTopic(topicName);
    if (!topic)
        topic = await messagingService.createTopic(topicName);
    await topic.addMessageDeliveryFailureListener(new TopicListener(TopicDeleted,MessageDeliveryFailure)); 
    for (let i=0 ; i< 5 ; i++) {
        const order = CreateNewOrder(topicName);
        console.log(`order with id ${order.OrderID} generated`);
        const message = new Message(order, new TimeSpan(0,0,15));
        await topic.publish(message,DeliveryOption.All,null,true);
        await delay(5000);
    }
    console.log(`all orders generated for ${topicName}`)
    return cache;
}
async function RunBulkPublisher(topicName) {
    const cache = await InitializeCache();
    const messagingService = await cache.getMessagingService();
    let topic = await  messagingService.getTopic(topicName);
    if (!topic)
        topic = await messagingService.createTopic(topicName);
    await topic.addMessageDeliveryFailureListener(new TopicListener(TopicDeleted,MessageDeliveryFailure));
    for (let i=0 ; i< 5 ; i++) {
        const order = CreateNewOrder(topicName);
        console.log(`order with id ${order.OrderID} generated`);
        const messageList = GenerateMessages(order);
        await topic.publishBulk(messageList,true);
        await delay(5000);
    }
    console.log(`all orders generated for ${topicName}`)
    return cache;
}
async function DeleteTopic(cache,topicName) {
    const messagingService = await cache.getMessagingService();
    await  messagingService.deleteTopic(topicName)
}
function MessageDeliveryFailure(sender,args) {
    console.log(`Message not delivered. Reason: ${args.getMessgeFailureReason()}`);
}
function TopicDeleted(sender,args) {
    console.log("topic deleted");
}
function GenerateMessages(order) {
    let messages = new Map();
    for (let i=0 ; i<5 ; i++) {
        const message = new Message(order,new TimeSpan(0,0,15));
        messages.set(message,DeliveryOption.All);
    }
    return messages;
}

function CreateNewOrder(type) {
    return({
        OrderID : `Order-${(new Date()).toISOString()}`,
        OrderDate : (new Date()).toISOString(),
        Type : type
    });
}
module.exports = {
    Run
}