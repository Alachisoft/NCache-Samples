(async () => {
    const operations = require('./src/distributed-counter')
    await operations.run(); 
})();