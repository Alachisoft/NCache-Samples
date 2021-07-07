async function delay(ms) {
    // return await for better async stack trace support in case of errors.
    return await new Promise(resolve => setTimeout(resolve, ms));
}
const catchHandler = error => ({payload:error, resolved :false});
const successHandler = result =>({payload:result, resolved :true});
const promiseHandler = promise => (promise instanceof Promise) ? 
                       promise.then(result => ({payload:result, resolved :true}),error =>({payload:error, resolved :false})) :
                       {payload:promise, resolved :true};
module.exports = {
    delay,
    catchHandler,
    successHandler,
    promiseHandler
}