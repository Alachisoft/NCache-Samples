//===============================================================================
// Copyrightę 2014 Alachisoft.  All rights reserved.
//===============================================================================

using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Alachisoft.NCache.Web.SessionState;
using System.Collections.Generic;

namespace guessgame
{
	/// <summary>
	/// Summary description for WebForm1.
	/// </summary>
	public partial class MainForm : System.Web.UI.Page
	{
		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion

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

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.lblSystemName.Text = "Page generated at: " + Environment.MachineName;

			if (Session["StartNewGame"] == null || Session["StartNewGame"].Equals("true"))
			{
				StartNewGame();
			}
		}

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
			catch(Exception)
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
		/// Populate list box with history
		/// It is to verify the working of distributed session
		/// </summary>
		private void PopulateHistory(int secret)
		{
			string attempts = string.Empty;
			List<int> history = Session["History"] as List<int>;

			int min = 0, max = 101;
			for (int i=0; i< history.Count; i++)
			{
				if(i >= 1) attempts += ", ";
				int val = history[i];

				if (val < secret && val > min) min = val;
				if(val > secret && val < max) max = val;

				attempts += val.ToString();
			}
			lblHistory.Text = attempts;
			lblHint.Text = "Hint: The number is between " + 
				min + " and " + max + ".";
		}
	}
}
