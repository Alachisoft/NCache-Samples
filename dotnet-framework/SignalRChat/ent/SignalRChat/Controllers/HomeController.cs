// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// ===============================================================================
// Copyright © Alachisoft.
// All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SignalRChat.Controllers
{
    /// <summary>
    /// HomeController to manage the main views of the application
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Method to display the index view with existing messages
        /// </summary>
        /// <returns>Index view</returns>
        public ActionResult Index()
        {
            return View();
        }

       
    }
}