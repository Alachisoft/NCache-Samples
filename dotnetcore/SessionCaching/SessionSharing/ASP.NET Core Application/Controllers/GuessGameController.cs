using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Alachisoft.NCache.Web.SessionState;
// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Alachisoft.Samples.GuessGameCore.Controllers
{
    public class GuessGameController : Controller
    {
        private const string SecretNumber = "SecretNumber";
        private const string History = "History";
        private const string LastValue = "LastValue";
        private const string Victory = "YouWin";
        private const string IsGreater = "IsGreater";
        private const string StartNewGameKey = "StartNewGame";

        // GET: /<controller>/
        public IActionResult Index()
        {
            object newGame;
            HttpContext.Session.TryGetValue(StartNewGameKey, out newGame);
            if(newGame == null || newGame.Equals("true"))
            {
                HttpContext.Session.Set(StartNewGameKey,"false");
            }

            StartNewGame();
            return View();
        }

        public IActionResult Guess(string id)
        {
            ViewData[Victory] = false;
            if (id != null)
            {
                object history = null;
                if (HttpContext.Session.TryGetValue(History, out history))
                {
                    ViewData[History] = history;

                    object number;
                    HttpContext.Session.TryGetValue(SecretNumber, out number);
                    if (number != null)
                    {
                        ViewData[SecretNumber] = number;
                        int guessedNumber;
                        if (int.TryParse(id, out guessedNumber))
                        {
                            if (((int)number).Equals(guessedNumber))
                            {
                                HttpContext.Session.Set(StartNewGameKey, "true");
                                ViewData[Victory] = true;
                            }
                            else
                            {
                                if (guessedNumber > ((int)number))
                                {
                                    ViewData[IsGreater] = true;
                                }
                                else
                                {
                                    ViewData[IsGreater] = false;
                                }
                            }
                            var list = history as List<int>;
                            list?.Add(guessedNumber);

                            ViewData[LastValue] = guessedNumber;
                        }
                    }
                    HttpContext.Session.Set(History, history);
                }
                else
                {
                    return NewGame();
                }
            }
            return View("Index");
        }

        public IActionResult NewGame()
        {
            HttpContext.Session.Clear();
            StartNewGame();
            return View("Index");
        }

        private void StartNewGame()
        {
            object number;
            HttpContext.Session.TryGetValue(SecretNumber, out number);
            if (number == null)
            { number = new Random(DateTime.Now.Millisecond).Next(0, 100); }

            ViewData[SecretNumber] = number;
            HttpContext.Session.Set(SecretNumber, number);

            object history;
            if (HttpContext.Session.TryGetValue(History, out history))
            {
                var list = history as List<int>;
                if (list.Count > 0)
                {
                    ViewData[History] = list;
                    ViewData[LastValue] = list[list.Count - 1];
                }
            }
            else
            {
                history = new List<int>();
                HttpContext.Session.Set(History, history);
            }
        }
    }
}
