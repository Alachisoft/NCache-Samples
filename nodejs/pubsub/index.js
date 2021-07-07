(async () => {
    const {promiseHandler} = require('./src/helper/util');
    const publisher = require('./src/pub-sub');
    const subscriber = require('./src/subscriber');
    const durableSubscriber = require('./src/durable-subscribe');
    await Promise
        .all([publisher.Run(),subscriber.Run(),durableSubscriber.Run()]
        .map(promiseHandler))
        .then(results => {
            console.log(`Publisher ${formatter(results[0])}`);
            console.log(`Subscriber ${formatter(results[1])}`);
            console.log(`Durable Subscriber ${formatter(results[2])}`);
        }); 
    function formatter(result) {
        return `has ${result.resolved ? "no error":`error ${result.payload}`}`;
    }
})();