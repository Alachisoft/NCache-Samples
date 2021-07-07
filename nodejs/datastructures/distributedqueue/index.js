(async () => {
    const operations = require('./src/distributed-queue')
    await operations.run(); 
})();