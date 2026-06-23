// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================

using Alachisoft.NCache.Web.SessionState;
using GameNetCore_Companion.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GameNetCore_Companion.Controllers
{
    /// <summary>
    /// Code-behind class for the default game page.
    /// Handles the Guess Game logic including initializing sessions,
    /// processing guesses, and managing UI updates.
    /// </summary>
    public class HomeController : Controller
    {
        private const string SecretNumber = "SecretNumber";
        private const string History = "History";
        private const string LastValue = "LastValue";
        private const string Victory = "YouWin";
        private const string IsGreater = "IsGreater";
        private const string StartNewGameKey = "StartNewGame";

        /// -------------------------------------------------------------------------------
        /// <summary>
        /// Method to display the main game view and initializes a new game session if required.
        /// Checks if a new game needs to be started based on the session value.
        /// </summary>
        /// <returns>Returns the Index view with the current game state.</returns>
        public IActionResult Index()
        {
            object newGame;
            HttpContext.Session.TryGetValue(StartNewGameKey, out newGame);
            if (newGame == null || newGame.Equals("true"))
            {
                HttpContext.Session.Set(StartNewGameKey, "false");
                StartNewGame();
            }
            else
            {
                // Retrieve previous guesses and secret number from session if available
                object historyObj;
                if (HttpContext.Session.TryGetValue(History, out historyObj))
                {
                    var historyList = historyObj as List<int>;
                    if (historyList != null && historyList.Count > 0)
                    {
                        ViewData[History] = historyList;
                        ViewData[LastValue] = historyList.Last();
                    }
                }

                object numberObj;
                if (HttpContext.Session.TryGetValue(SecretNumber, out numberObj))
                {
                    ViewData[SecretNumber] = numberObj;
                }
            }

            return View();
        }

        /// -------------------------------------------------------------------------------
        /// <summary>
        /// Method to handle the guess attempt by the player and updates the session accordingly.
        /// Determines if the guessed number is higher, lower, or equal to the secret number.
        /// </summary>
        /// <param name="id">The guessed number provided by the player as a string.</param>
        /// <returns>Returns the updated Index view showing the result of the guess.</returns>
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
                            if (((Int64)number).Equals(guessedNumber))
                            {
                                HttpContext.Session.Set(StartNewGameKey, "true");
                                ViewData[Victory] = true;
                            }
                            else
                            {
                                if (guessedNumber > ((Int64)number))
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

        /// -------------------------------------------------------------------------------
        /// <summary>
        /// Method to start a completely new game session by clearing existing session data
        /// and generating a new secret number.
        /// </summary>
        /// <returns>Returns the Index view after starting a new game.</returns>
        public IActionResult NewGame()
        {
            HttpContext.Session.Clear();
            StartNewGame();
            return View("Index");
        }

        /// -------------------------------------------------------------------------------
        /// <summary>
        /// Method to initialize a new game by generating a secret number and setting up
        /// the player's guess history in session.
        /// </summary>
        private void StartNewGame()
        {
            object numberObj;
            HttpContext.Session.TryGetValue(SecretNumber, out numberObj);

            // If no secret number exists, create one
            if (numberObj == null)
            {
                numberObj = new Random(DateTime.Now.Millisecond).Next(0, 100);
            }

            ViewData[SecretNumber] = numberObj;
            HttpContext.Session.Set(SecretNumber, numberObj);

            object historyObj;
            if (HttpContext.Session.TryGetValue(History, out historyObj))
            {
                var list = historyObj as List<int>;
                if (list != null && list.Count > 0)
                {
                    ViewData[History] = list;
                    ViewData[LastValue] = list.Last();
                }
                else
                {
                    ViewData[History] = new List<int>();
                    ViewData[LastValue] = "None";
                }
            }
            else
            {
                // Initialize empty history
                var emptyHistory = new List<int>();
                HttpContext.Session.Set(History, emptyHistory);
                ViewData[History] = emptyHistory;
                ViewData[LastValue] = "None";
            }
        }
    }
}
