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
        private const string GuessedNumber = "GuessedNumber";
        private const string History = "History";
        private const string LastValue = "LastValue";
        private const string Victory = "YouWin";
        private const string IsGreater = "IsGreater";

        // GET: /<controller>/
        public IActionResult Index()
        {
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
                    HttpContext.Session.TryGetValue(GuessedNumber, out number);
                    if (number != null)
                    {
                        ViewData[GuessedNumber] = number;
                        int guessedNumber;
                        if (int.TryParse(id, out guessedNumber))
                        {
                            if (((int)number).Equals(guessedNumber))
                            {
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
            HttpContext.Session.TryGetValue(GuessedNumber, out number);
            if (number == null)
            { number = new Random(DateTime.Now.Millisecond).Next(0, 100); }

            ViewData[GuessedNumber] = number;
            HttpContext.Session.Set(GuessedNumber, number);

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
