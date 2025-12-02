// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ResponseCaching.Models;

namespace ResponseCaching.Controllers
{
    /// <summary>
    /// Controller responsible for handling product-related operations such as
    /// displaying, editing, and updating product details.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private string _connectionString;
        private readonly IProductsRepository _productRepository;

        /// -------------------------------------------------------------------------------
        /// <summary>
        /// Initializes the HomeController with logging, configuration, 
        /// and product repository dependencies.
        /// </summary>

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, IProductsRepository productsRepository)
        {
            _productRepository = productsRepository;
            _connectionString = configuration.GetValue<string>("ConnectionStrings:DBServer1");
            _logger = logger;
        }

        /// -------------------------------------------------------------------------------
        /// <summary>
        /// Displays the list of products, filtered by price.
        /// </summary>
        /// <param name="price">Price filter for products.</param>
        /// <returns>Returns the Index view with the list of products.</returns>
        public IActionResult Index(int? price)
        {
            var products = _productRepository.GetAllProductsFromDatabase(price);
            return View("Index", products);
        }

        /// -------------------------------------------------------------------------------
        /// <summary>
        /// Displays details of a specific product based on its ProductID.
        /// </summary>
        /// <param name="ProductID">The product ProductID.</param>
        /// <returns>Returns the ViewItem view with product details.</returns>
        public IActionResult Details(int ProductID)
        {
            var products = _productRepository.GetProductById(ProductID);
            return View("viewItem", products);
        }

        /// -------------------------------------------------------------------------------
        /// <summary>
        /// Displays the edit form for a specific product.
        /// </summary>
        /// <param name="ProductID">The product ProductID.</param>
        /// <returns>Returns the EditItem view with the product data.</returns>
        public IActionResult Edit(int ProductID)
        {
            var product = _productRepository.GetProductById(ProductID);
            return View("EditItem", product);
        }

        /// -------------------------------------------------------------------------------
        /// <summary>
        /// Displays the success page after a successful update.
        /// </summary>
        /// <param name="ProductID">The updated product ProductID.</param>
        /// <returns>Returns the Success view with the product details.</returns>
        public IActionResult Success(int ProductID)
        {
            var product = _productRepository.GetProductById(ProductID);
            return View("Success", product);
        }

        /// -------------------------------------------------------------------------------
        /// <summary>
        /// Saves updated product details to the database and redirects accordingly.
        /// </summary>
        /// <param name="ProductID">Product ProductID to update.</param>
        /// <param name="name">Updated product name.</param>
        /// <param name="price">Updated product price.</param>
        /// <param name="quantityPerUnit">Updated quantity per unit.</param>
        /// <returns>Redirects to Success view if updated, otherwise Edit view.</returns>
        [HttpPost]
        public IActionResult Save(int ProductID, string name, string price, string quantityPerUnit)
        {
            // validate inputs first
            if (!decimal.TryParse(price, out var parsedPrice))
            {
                ModelState.AddModelError("Price", "Invalid price value.");
                return RedirectToAction("Edit", new { ProductID });
            }

            var product = new Product
            {
                ProductId = ProductID,
                ProductName = name,
                UnitPrice = parsedPrice,
                QuantityPerUnit = quantityPerUnit
            };

            bool success = _productRepository.UpdateProductInDatabase(product);

            // delay for chanings to take place 
            Thread.Sleep(5000);
            return success
                ? RedirectToAction("Success", new { ProductID = product.ProductId })
                : RedirectToAction("Edit", new { ProductID = product.ProductId });
        }

        /// -------------------------------------------------------------------------------
        /// <summary>
        /// Displays the Privacy page.
        /// </summary>
        /// <returns>Returns the Privacy view.</returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// -------------------------------------------------------------------------------
        /// <summary>
        /// Displays error information with request tracking ProductID.
        /// </summary>
        /// <returns>Returns the Error view.</returns>
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
