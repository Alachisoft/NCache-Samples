using RefresherSample;
using System;
using System.Collections.Generic;

namespace TestingApp
{
    class Program
    {
        static void Main(string[] args)
        {
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("ConnectionString", "Server = 20.200.20.36; Database = northwind; User Id = usman; Password = 4Islamabad;");
            Refresher refresher = new Refresher();
            refresher.Init( parameters, "newcahe");
            refresher.LoadDatasetOnStartup("products");
            Console.WriteLine("Hello World!");
        }
    }
}
