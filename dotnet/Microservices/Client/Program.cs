// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// ===============================================================================
// Copyright © Alachisoft.
// All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================

using System.Net.Http;
using System.Net.Http.Json;
using Alachisoft.NCache.Sample.Data;
using Microsoft.AspNetCore.Mvc;

// ===============================================================================
// The Client Application interactions with Order and Inventory Services through
// RestAPI. It sends multiple sample orders to the Order Service and queries the
// Inventory Service to check for stock.
// ===============================================================================

// Defining the base URLs for the Order Service and Inventory Service
const string ORDER_SERVICE_URL = "https://localhost:54341";
const string INVENTORY_SERVICE_URL = "https://localhost:54342"; 

// Creating a list of 5 sample orders
var orders = CreateSampleOrders();

// Iterating through each order to send to the Order Service
foreach (var order in orders)
{
    // Sending order to Order Service
    bool isSent = await SendOrder(order);

    // Checking if the order was sent successfully
    if (isSent)
    {
        // Adding a delay to allow services to process the order
        await Task.Delay(TimeSpan.FromSeconds(1));

        // Querying the Inventory Service to check current stock
        await QueryInventory();
    }
    else
    {
        // Breaking the loop if sending the order failed
        break;
    }
}

// Cleaning up sample data from cache added by services
Console.WriteLine("Performing cleanup...");

// Removing Orders List
await ClearOrdersData();

// Removing Stock Counter
await ClearStockData();

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to send a single order to the Order Service using an HTTP POST request.
/// </summary>
/// <param name="order">The order object to be sent to the service.</param>
/// <returns>A boolean value indicating isSent (true) or failure (false) of the request.</returns>
async Task<bool> SendOrder(Order order)
{
    // Declaring the response variable
    HttpResponseMessage response;

    // Creating a new HttpClient instance to send the request
    using (var httpClient = new HttpClient())
    {
        // Sending POST request to Order Service with order data and fetching the response
        response = await httpClient.PostAsJsonAsync($"{ORDER_SERVICE_URL}/orders", order);
    }

    // Processing the HTTP response to determine success or failure
    bool isResponseSuccessful = await ProcessHTTPResponse(response);

    // Returning the result of the response processing
    return isResponseSuccessful;
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to query the Inventory Service to retrieve the current stock count.
/// Prints the stock value if successful; otherwise, prints an error content.
/// </summary>
async Task QueryInventory()
{
    // Declaring the response variable
    HttpResponseMessage response;

    // Creating a new HttpClient instance to send the request
    using (var httpClient = new HttpClient())
    {
        // Sending GET request to Inventory Service to fetch current stock
        response = await httpClient.GetAsync($"{INVENTORY_SERVICE_URL}/stock");
    }

    // Checking if the response indicates isSent
    if (response.IsSuccessStatusCode)
    {
        var stockJson = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Current Stock: {stockJson}\n");
    }
    else
    {
        Console.WriteLine($"Failed to fetch stock. Status: {response.StatusCode}");
    }
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to send a DELETE request to the Order Service to clean up any cached or persisted order data.
/// Prints the response content indicating isSent or failure.
/// </summary>
async Task ClearOrdersData()
{
    // Declaring the response variable
    HttpResponseMessage response;

    // Creating a new HttpClient instance to send the request
    using (var httpClient = new HttpClient())
    {
        // Sending DELETE request to Order Service to clean up orders
        response = await httpClient.DeleteAsync($"{ORDER_SERVICE_URL}/cleanup");
    }

    // Processing the HTTP response
    await ProcessHTTPResponse(response);
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to send a DELETE request to the Inventory Service to reset or remove inventory counter data.
/// Prints the response content indicating isSent or failure.
/// </summary>
async Task ClearStockData()
{
    // Declaring the response variable
    HttpResponseMessage response;

    // Creating a new HttpClient instance to send the request
    using (var httpClient = new HttpClient())
    {
        // Sending DELETE request to Inventory Service to clean up inventory counter
        response = await httpClient.DeleteAsync($"{INVENTORY_SERVICE_URL}/cleanup");
    }

    // Processing the HTTP response
    await ProcessHTTPResponse(response);
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to create a list of 5 sample orders with random order IDs and mock shipping details.
/// This data is used to simulate sending multiple orders to the Order Service.
/// </summary>
/// <returns>A list of <see cref="Order"/> objects representing sample orders.</returns>
List<Order> CreateSampleOrders()
{
    // Initializing random number generator and orders list
    var random = new Random();
    var orders = new List<Order>();

    // Generating 5 sample orders with unique details
    for (int i = 1; i <= 5; i++)
    {
        // Creating and adding a new order to the list
        orders.Add(new Order
        {
            OrderID = random.Next(1000, 9999),
            OrderDate = DateTime.UtcNow,
            ShipName = $"Customer {i}",
            ShipAddress = $"{i} Main St",
            ShipCity = "Lakeside",
            ShipCountry = "Brookley"
        });
    }

    // Returning the list of sample orders
    return orders;
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to process the HTTP response from the API calls.
/// </summary>
/// <param name="response">The HTTP response message to process.</param>
/// <returns>A task representing the asynchronous operation, with a boolean result indicating success or failure.</returns>
async Task<bool> ProcessHTTPResponse(HttpResponseMessage response)
{
    // Checking if the response indicates isSent
    if (response.IsSuccessStatusCode)
    {
        // Reading the response content as a string
        var content = await response.Content.ReadAsStringAsync();

        // Printing the response content
        Console.WriteLine($"Response: {content}");

        // Returning true to indicate success
        return true;
    }
    else
    {
        // Reading the error details from the response
        ProblemDetails problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        // Printing the error detail
        Console.WriteLine($"Error: {problemDetails.Detail}");

        // Returning false to indicate failure
        return false;
    }
}