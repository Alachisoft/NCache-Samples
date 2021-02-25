(async () => {
    const operations = require('./src/sql-dependency')
    await operations.run(); 
})();