using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NCacheResponseCaching.Data;
using NCacheResponseCaching.Models;
using ResponseCaching.Models;

namespace NCacheResponseCaching.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private string connectionString;
        private readonly IProductsRepository _productRepository;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration,IProductsRepository productsRepository)
        {
            _productRepository = productsRepository;
           connectionString = configuration.GetValue<string>("ConnectionStrings:DBServer1");
            _logger = logger;
        }

        public IActionResult Index(int? price)
        {
            var products= _productRepository.GetAllProductsFromDatabase(price);
            return View("index", products);
        }

        public IActionResult Details( int id)
        {
           var products= _productRepository.GetProductById(id);
            return View("viewItem", products);
        }
        public IActionResult Edit(int id)
        {
           
            var products= _productRepository.GetProductById(id);
            return View("editItem", products);
        }
        public IActionResult Success(int id)
        {
           
            var products= _productRepository.GetProductById(id);
            return View("success", products);
        }
        public IActionResult Save(int id, string name, string price, string quantityPerUnit)
        {
            
            Product product = new Product(id, name,Convert.ToDecimal(price), quantityPerUnit);
            bool success= _productRepository.UpdateProductInDatabase(product);
            Thread.Sleep(5000);
            if (success)
            {
                return RedirectToAction("Success", new { id = product.ProductID });
            }
            else 
            { 
                return RedirectToAction("Edit", new { id = product.ProductID });
            }
        }
      

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
