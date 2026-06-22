// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using SampleApp.ViewModels;

namespace SampleApp.Controllers
{
    /// <summary>
    /// Controller to demonstrate caching using IDistributedCache (e.g., NCache)
    /// Provides actions to display the cached time and reset it
    /// </summary>
    public class HomeController : Controller
    {
        const string CACHED_TIME = "cachedTime";

        // Setting expiry time for item
        const int EXPIRY_TIME = 20;

        /// <summary>
        /// The distributed cache instance
        /// </summary>
        private readonly IDistributedCache _cache;

        /// -------------------------------------------------------------------------------
        /// <summary>
        /// Constructor: injects IDistributedCache instance
        /// </summary>
        /// <param name="cache">The distributed cache service</param>
        public HomeController(IDistributedCache cache)
        {
            _cache = cache;
        }

        /// -------------------------------------------------------------------------------
        /// <summary>
        /// Method to Display the current time and the cached time
        /// If the cached value has expired or does not exist, displays "Cached time not set or expired"
        /// </summary>
        /// <returns>Index view with HomeViewModel</returns>
        public async Task<IActionResult> Index()
        {
            // Trying to get cached value
            // We can also use _cache.Get() as well for synchronous operation
            var cachedBytes = await _cache.GetAsync(CACHED_TIME);
            string cachedTime;

            // Checking if cache exists
            if (cachedBytes != null)
            {
                // If cache exists, decode the bytes to string
                cachedTime = Encoding.UTF8.GetString(cachedBytes);
            }
            else
            {
                // If cache does not exist (expired or never set), show expired message
                cachedTime = "Cached time not set or expired";
            }

            // Creating the ViewModel and pass the cached time
            var model = new HomeViewModel
            {
                CachedTime = cachedTime
            };

            // Saving data to view for display
            ViewData["expiry_time"] = EXPIRY_TIME;
            ViewData["time_key"] = CACHED_TIME;

            // Returning the Index view with the model
            return View(model);
        }

        /// -------------------------------------------------------------------------------
        /// <summary>
        /// Method to Reset the cached time to the current time and sets a 20-second absolute expiration
        /// </summary>
        /// <returns>Redirected to index action</returns>
        [HttpGet]
        public async Task<IActionResult> SetCachedTime()
        {
            // Getting the current time as string
            var timeString = DateTime.Now.ToString("hh:mm:ss tt");

            // Converting to byte array and storing in distributed cache with absolute expiration
            // We can also use _cache.Set() as well for synchronous operation
            await _cache.SetAsync(CACHED_TIME, Encoding.UTF8.GetBytes(timeString),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(20) // Cache expires in 20 seconds
                });

            // Creating ViewModel to pass updated cached time to the view
            var model = new HomeViewModel
            {
                CachedTime = timeString
            };

            // Returning Index view with updated cache
            return RedirectToAction("Index");
        }

        /// -------------------------------------------------------------------------------
        /// <summary>
        /// Method to remove cached item
        /// </summary>
        /// <returns>Redirectedto index action</returns>
        [HttpGet]
        public async Task<IActionResult> RemoveCachedTime()
        {
            // Removing item from cache asynchronously
            // We can also use _cache.RemoveAsync() as well for synchronous operation
            await _cache.RemoveAsync(CACHED_TIME);

            return RedirectToAction("Index");
        }
    }
}

