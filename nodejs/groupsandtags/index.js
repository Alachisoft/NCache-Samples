(async () => {
    const groupsOperations = require('./src/groups');
    await groupsOperations.Run(); 
    const tagsOperations = require('./src/tags');
    await tagsOperations.Run(); 
    const namedTagsOperations = require('./src/namedtags');
    await namedTagsOperations.Run(); 
    console.log("done");
})();