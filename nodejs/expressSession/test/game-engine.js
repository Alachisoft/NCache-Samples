const os = require('os')
function GetStatus(storedNumber,userGuessedNumber) {
    return isNaN(userGuessedNumber) ? "Pick a number and guess": userGuessedNumber === storedNumber ? "won": 
        userGuessedNumber < storedNumber ? "The number that you have tried is lesser than the guess.":
        "The number that you have tried is greater than the guess.";
}
function GetMinMax(historyList,storedNumber) {
    let min =0;
    let max = 101;
    for (let i = historyList.length - 1; i >= 0; i--)
    {
        if (historyList[i] < storedNumber && historyList[i] > min)
        {
            min = historyList[i];
        }
        if (historyList[i] > storedNumber && historyList[i] < max)
        {
            max = historyList[i];
        }
    }
    return [min,max]
}
function GetData(req) {
    let hasWon = false;
    let [storedNumber,userGuessedNumber,lastValue,status,historyList,min,max] = UpdateGameStatus(req);
    if (status == "won") {
        status = `Congratulations, you guessed the right number. ${storedNumber} was the selected number.`
        hasWon = true;
        req.session.destroy();
    }
    return {
        status: status,
        lastValue:lastValue,
        history:historyList,
        min: min,
        max: max,
        hasWon: hasWon,
        user: os.userInfo().username || ""
    };
}
function UpdateHistoryList (req,userGuessedNumber) {
    let historyList =  req.session.history ? req.session.history : [];
    if (!isNaN(userGuessedNumber))
        historyList.push(userGuessedNumber);   
    req.session.history = historyList
    return historyList;
}
function UpdateGameStatus(req) {
    const storedNumber = req.session.secretGeneratedNumber;
    const userGuessedNumber = req.session.userGuessedValue ? parseInt(req.session.userGuessedValue,10) : NaN;
    req.session.userGuessedValue = null;
    const lastValue = !isNaN(userGuessedNumber) ? userGuessedNumber : "None"; 
    status = GetStatus(storedNumber,userGuessedNumber);
    req.session.lastValue = lastValue
    const historyList =  UpdateHistoryList(req,userGuessedNumber);
    const [min, max] = GetMinMax(historyList,storedNumber);
    return [storedNumber,userGuessedNumber,lastValue,status,historyList,min,max];
}
module.exports = {
    UpdateGameStatus,
    UpdateHistoryList,
    GetMinMax,
    GetStatus,
    GetData
}