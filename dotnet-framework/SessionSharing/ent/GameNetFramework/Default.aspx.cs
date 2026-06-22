// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;


namespace GameNetFramework
{
    /// <summary>
    /// Code-behind class for the default game page.
    /// Method to handle the Guess Game logic including initializing sessions,
    /// processing guesses, and managing UI updates.
    /// </summary>
    public partial class _Default : Page
    {
        /// <summary>
        /// Method to start a new guessing game by clearing previous session data,
        /// generating a new secret number, and resetting all UI elements to default values.
        /// </summary>
        private void StartNewGame()
        {
            Session.Clear();
            Random rndObject = new Random();
            Session["SecretNumber"] = rndObject.Next(1, 100);
            Session["History"] = new List<int>();
            Session["StartNewGame"] = "false";


            cmdGuess.Text = "Guess!";
            txtGuess.Enabled = true;
            lblHint.Text = "Hint: The number is between 0 and 101.";
            lblMessage.Text = @"You haven't made any attempt yet.";
            lblLastGuess.Text = "None";
            lblHistory.Text = "None";
        }

        /// <summary>
        /// Method to handle page load event. Starts a new game if no active game session exists.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event arguments.</param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.lblSystemName.Text = "Page generated at: " + Environment.MachineName;

            if (!IsPostBack) // Only check session and initialize once per load
            {
                if (Session["StartNewGame"] == null || Session["StartNewGame"].Equals("true"))
                {
                    StartNewGame();
                }
                else
                {
                    // If a game is already in progress, restore history
                    if (Session["History"] != null && Session["SecretNumber"] != null)
                    {
                        int secretNumber = Convert.ToInt32(Session["SecretNumber"]);
                        List<int> history = Session["History"] as List<int>;

                        if (history != null && history.Count > 0)
                        {
                            lblLastGuess.Text = history.Last().ToString();
                            lblMessage.Text = "Previous game resumed. Continue guessing!";
                            PopulateHistory(secretNumber);
                        }
                        else
                        {
                            lblLastGuess.Text = "None";
                            lblMessage.Text = "You haven't made any attempt yet.";
                        }
                    }
                    else
                    {
                        StartNewGame();
                    }
                }
            }
        }


        /// <summary>
        /// Method to handle the Guess button click event. Validates user input, compares it
        /// with the secret number, updates the session and UI, and starts a new game if guessed correctly.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event arguments.</param>
        protected void OnClickGuess(object sender, System.EventArgs e)
        {
            if (cmdGuess.Text == "New Game")
            {
                StartNewGame();
                return;
            }

            int guess = 0;
            try
            {
                guess = Convert.ToInt32(txtGuess.Text);
                txtGuess.Text = string.Empty;
            }
            catch (Exception)
            {
                lblMessage.Text = "Please specify a valid number.";
                return;
            }

            int secretNumber = Convert.ToInt32(Session["SecretNumber"]);
            List<int> history = Session["History"] as List<int>;

            lblLastGuess.Text = guess.ToString();
            lblMessage.Text = "#" + history.Count + ": ";
            if (guess > secretNumber)
            {
                lblMessage.Text += "The number that you have tried is greater than the guess.";
            }
            else if (guess < secretNumber)
            {
                lblMessage.Text += "The number that you have tried is lesser than the guess.";
            }
            else
            {
                lblMessage.Text += "You have found the Secret Number";
                txtGuess.Enabled = false;
                cmdGuess.Text = "New Game";
                Session["StartNewGame"] = "true";
            }
            history.Add(guess);
            Session["History"] = history;

            PopulateHistory(secretNumber);
        }


        /// <summary>
        /// Method to populate list box with history
        /// It is to verify the working of distributed session
        /// </summary>
        private void PopulateHistory(int secret)
        {
            string attempts = string.Empty;
            List<int> history = Session["History"] as List<int>;

            int min = 0, max = 101;
            for (int i = 0; i < history.Count; i++)
            {
                if (i >= 1) attempts += ", ";
                int val = history[i];

                if (val < secret && val > min) min = val;
                if (val > secret && val < max) max = val;

                attempts += val.ToString();
            }
            lblHistory.Text = attempts;
            lblHint.Text = "Hint: The number is between " +
                min + " and " + max + ".";
        }
    }
}