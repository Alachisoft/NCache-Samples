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
using System.Configuration;


namespace Alachisoft.NCache.Samples
{
	/// <summary>
	/// The application
	/// </summary>
	public class Program
	{		
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main() 
		{
			try
			{
                // running main app starts the functioning of sample
                MainApp main = new MainApp();
                main.Run();
            }
			catch(Exception ex)
			{
                // display exception in case of any error
                Console.WriteLine("Error: " + ex.Message);
            }
		}
	}
}
