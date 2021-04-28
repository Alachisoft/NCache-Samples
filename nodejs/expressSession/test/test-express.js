(async () => {
    try {
        const path = require('path')
        const config = require(path.join(__dirname,'config','config'));
        const session = require('express-session');
        const ncacheStore = require('ncache-sessions')(session);
        const bodyParser = require('body-parser');
        const gameEngine = require('./game-engine');
        const express = require('express');
        const app = express();
        const store = await ncacheStore.createStore(config.ncacheStore);
        app.use(session({
            secret: 'keyboard cat',
            resave: false,
            saveUninitialized: true,
            store: store
        }));
        const views = path.join(__dirname,'views');
        app.set('views',views);
        app.set('view engine', 'ejs');
        var urlencodedParser = bodyParser.urlencoded({ extended: false })
        app.get('/',(req,res) => {
            if (!req.session.secretGeneratedNumber) {             
                req.session.secretGeneratedNumber = generateRandomNumber(101);
            }
            res.status(200).render('base',{data:gameEngine.GetData(req) });
         })     
         app.post('/GuessNumber',urlencodedParser,(req,res) =>{
            req.session.userGuessedValue = req.body?.userGuessedValue;
            res.redirect('/');
         });
         function generateRandomNumber(max)
         {
             return Math.floor(Math.random() * Math.floor(max));
         } 
         app.get('*',(req,res) => {
             res.redirect('/');
         });
         app.listen(3000, () => {
             console.log(`Session Example app listening at http://localhost:${3000}`)
         });
    }
    catch(err) {
        console.log(err);
    }
})();