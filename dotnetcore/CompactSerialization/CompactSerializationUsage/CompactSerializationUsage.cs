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
using System.Threading;
using System.Collections;
using System.Data;
using System.IO;

using Alachisoft.NCache.Client;
using Alachisoft.NCache.Runtime;
using System.Configuration;

namespace Alachisoft.NCache.Samples
{
	/// <summary>
	///Here is what this program should do:
	///
	///1. Create three classes "NormalObject", "BigObject", and "BiggerObject".
	///They are 4k, 10k, 50k in size respectively. Use C# class for each of the
	///above and within the class you can put a byte-array to take up the desired
	///size if you want but have at least two data members in the class (e.g.
	///String _id being the first data member).
	///
	///2. Write a command line program that goes into a loop (100 times) and for
	///each iteration, it uses the iteration number as the key and then randomly
	///picks one of the objects to add to the cache if GET on this key returns a
	///NULL.
	///
	///3. At the end of this loop, write another loop that goes through a part of
	///the first loop (e.g. 1/10 of the first loop) and does a GET on item (a
	///subset of the first loop). It should find all of these items in the Cache.
	///
	///4. Print out to a log-file (or a standard-output) for each iteration in the
	///loop. Print the following information on ONE LINE:
	///- print key
	///- found or not found
	///- Time it took to do a GET
	///- Time it took to do an INSERT (if insert is done)
	///- a sample line should look like this:
	///
	///1005  Found  GET 20ms
	///1006  Not    GET 50ms  INSERT  200ms
	///
	///5. Calculate the same statistics that you have done in "Program" (best,
	///average, worst, and also different bracket of worst) and print out these
	///statistics at the end.
	///
	///6. Put a 10 second wait after doing Initialize on the cache so when we run
	///it on two machines, the cluster is formed before data is actually added to
	///the cache.
	/// </summary>
	public class CompactSerializationUsage
	{
        /// <summary> Number of iterations </summary>
		const int RunCount = 100;
		/// <summary> Number of checkpoints </summary>
		const int Checkpoint = CompactSerializationUsage.RunCount / 10;
		/// <summary> Part of the main loop for GET operations</summary>
		const int Part = 3; // value should be between 1 to 10

		private static TimeStat 
			_addStats = new TimeStat(60, 120),
			_getStats = new TimeStat(1, 10);

		static FileStream FlStream = new FileStream("cs-log.txt", FileMode.Create);
		static StreamWriter StWriter = new StreamWriter(FlStream);
		static TextWriter TxWriter = Console.Out;
		static int TestID = 1;
        private static ICache _cache;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
        public static void Run()
		{
			try
			{
                // Initialize cache
                InitializeCache();

                //_cache.ExceptionsEnabled = true;

				Console.WriteLine("\n\nSmall objects  (512 bytes)");
                // Perform test of small objects of size 512 bytes
                PerformTest(1);

				Console.WriteLine("\n\nNormal objects (4 KB)");
                // Perform test of normal objects of size 4 KB
                PerformTest(2);

				Console.WriteLine("\n\nBig objects    (10 K)");
                // Perform test of big objects of size 10 KB
                PerformTest(3);

				Console.WriteLine("\n\nBigger objects (50 K)");
                // Perform test of bigger objects of size 50 KB
                PerformTest(4);

				Console.WriteLine("\n\nSample objects ( >5 K)");
                // Perform test of sample objects of size greater than 5 KB
                PerformTest(5);

                Console.WriteLine("\n\nMix all objects randomly");
                // Perform test of objects of different sizes
                PerformTest(6);

                StWriter.Flush();
                // Rest Console writer handle
                Console.SetOut(CompactSerializationUsage.TxWriter);

                _cache.Dispose();
                CompactSerializationUsage.Dispose();
			}
			catch(Exception e)
			{
				Console.SetOut(CompactSerializationUsage.StWriter);
                Console.WriteLine("Exception: " + e.Message);
				Console.SetOut(CompactSerializationUsage.TxWriter);
                Console.WriteLine("Exception: " + e.Message);
			}
		}

        /// <summary>
        /// This method performs test and logs statistics
        /// </summary>
        /// <param name="testID"> Test identifier to be performed </param>
        private static void PerformTest(int testID)
        {
            CompactSerializationUsage.TestID = testID;

            // Set stream writer handle
            Console.SetOut(CompactSerializationUsage.StWriter);

            Console.WriteLine("\r\n\n====================================");
            Console.WriteLine(GetTestName(testID));
            Console.WriteLine("====================================");
            Console.WriteLine("\r\nExpected results are:");
            Console.WriteLine("Operation    Best(ms)    Avg(ms)    Worst(ms)");
            Console.WriteLine("---------    --------    -------    ---------");
            Console.WriteLine("Add()        <= {0,-5}   <= {1,-5}  > {1,-5}", _addStats.ExpectedBest, _addStats.ExpectedWorst);
            Console.WriteLine("Get()        <= {0,-5}   <= {1,-5}  > {1,-5}", _getStats.ExpectedBest, _getStats.ExpectedWorst);

            Console.WriteLine("\r\nCompact Serialization Sample Started...\r\n");

            RunSimulation(testID);
            DisplayStatistics();

            // Set console writer handle
            Console.SetOut(CompactSerializationUsage.TxWriter);
        }

        /// <summary>
        /// This method gets test name
        /// </summary>
        /// <param name="testID"> Test identifier </param>
        private static string GetTestName(int testID)
        {
            string testName = string.Empty;

            switch (testID)
            {
                case 1:
                    testName = "Small objects  (512 bytes)";
                    break;
                case 2:
                    testName = "Normal objects (4 KB)";
                    break;
                case 3:
                    testName = "Big objects    (10 K)";
                    break;
                case 4:
                    testName = "Bigger objects (50 K)";
                    break;
                case 5:
                    testName = "Sample objects ( >5 K)";
                    break;
                default:
                    testName = "Mix all objects randomly";
                    break;
            }

            return testName;
        }

        /// <summary>
        /// This method initializes the cache
        /// </summary>
        private static void InitializeCache()
        {
            string cacheId = ConfigurationManager.AppSettings["CacheID"];

            if (String.IsNullOrEmpty(cacheId))
            {
                Console.WriteLine("The CacheID cannot be null or empty.");
                return;
            }

            // Initialize an instance of the cache to begin performing operations:
            _cache = NCache.Client.CacheManager.GetCache(cacheId);

            Console.WriteLine("\nCache initialized successfully...");

        }

		/// <summary>
		/// performs the selected operation
		/// </summary>
		private static void RunSimulation(int testID)
		{
			try 
			{
				long objSize = GetObjectSize(testID);
				Console.WriteLine("Adding " + RunCount + " Items...");

                for (int i = (0 + ((testID - 1) * 100)); i < (RunCount + ((testID - 1) * 100)); i++)
                {
                    PerformOperation(i, ref objSize);
                }

				Console.WriteLine("\r\n-------------------------------------------------------------------------\r\n");
				Console.WriteLine("\r\nPerforming operations in parts...\r\n");

				for(int i = (CompactSerializationUsage.RunCount / 10) * Part ; i < (CompactSerializationUsage.RunCount / 10) * (Part + 1) ; i++)
				{
					PerformOperation(i, ref objSize);
				}
			}
			catch (Exception ex) 
			{
				Console.WriteLine("ERROR: " + ex.Message);
			}
		}

		/// <summary>
		/// Performs the get operation, if unsuccessful, performs an add operation, and log the time taken 
		/// to complete operation
		/// </summary>
		private static void PerformOperation(int i, ref long objSize)
		{
			Console.Write(i.ToString());
			object obj = Get(i.ToString());

			if(obj==null)
			{						
				Console.Write(" not found. Get operation took:" + _getStats.Current.ToString() + " ms\t");

				Console.Write("Inserting "+i);
				objSize = Add(i.ToString());
				Console.Write(", operation took:" + _addStats.Current.ToString() + " ms\t");
			}
			else
			{
				Console.Write(" found. Get operation took:" + _getStats.Current.ToString() + " ms\t");
			}

			if (objSize == 512)
				Console.WriteLine("Object size: 512 bytes");
			else 
				Console.WriteLine("Object size:" + (objSize / 1000) + " KB");
		}

		/// <summary>
		/// Adds an object to the cache if it is not already present.
		/// </summary>
		private static long Add(string key)
		{
			long objSize = 0;
			if (!_cache.Contains(key))
			{
				try
				{
					int operation = TestID;
					if (CompactSerializationUsage.TestID == 6) 
					{
						Random rand = new Random();
						operation = (int)rand.Next(1,5);
					}

					object Obj;
					switch(operation)
					{
						case 1:
							Obj = new SmallObject();
							objSize = 512;
							break;
						case 2:
							Obj = new NormalObject();
							objSize = 4000;
							break;
						case 3:
							Obj = new BigObject();
							objSize = 10000;
							break;
						case 4:
							Obj = new BiggerObject();
							objSize = 50000;
							break;
						default:
							Obj = new SampleObject();
							objSize = 5000;
							break;
					}

					_addStats.BeginSample();
					_cache.Add(
						key,
						Obj
                        );
					_addStats.EndSample();
				}
				catch(Exception ex)
				{
					Console.WriteLine("ERROR: " + ex.Message);
				}
			}
			return objSize;
		}

        /// <summary>
        /// Returns object size
        /// </summary>
        /// <returns>Size of the object in bytes.</returns>
        private static long GetObjectSize(int testID)
        {
            long objSize = 0;
            switch (testID)
            {
                case 1:
                    objSize = 512;
                    break;
                case 2:
                    objSize = 4000;
                    break;
                case 3:
                    objSize = 10000;
                    break;
                case 4:
                    objSize = 50000;
                    break;
                default:
                    objSize = 5000;
                    break;
            }
            return objSize;
        }

		/// <summary>
		/// gets an object from the cache if present.
		/// </summary>
		private static Object Get(string key)
		{
			try
			{
				_getStats.BeginSample();
                Object obj = new object(); //NCache.Cache.Get(key);
                obj = _cache.Get<object>(key);
				_getStats.EndSample();
				
				return obj;
			}
			catch(Exception)
			{
				return null;
			}
		}

        /// <summary>
        /// This method displays statistics
        /// </summary>
		private static void DisplayStatistics()
		{
			Console.WriteLine("\r\nStatistics at: {0}", DateTime.Now.ToString());			
			Console.WriteLine("Operation    # Runs     Best(ms)    Avg(ms)    Worst(ms)");
			Console.WriteLine("---------    ------     --------    -------    ---------");
			Console.WriteLine("Add()        {0,-6}     {1,-8}    {2,-8:F2}   {3,-8}", _addStats.Runs, _addStats.Best, _addStats.Avg, _addStats.Worst);
			Console.WriteLine("Get()        {0,-6}     {1,-8}    {2,-8:F2}   {3,-8}", _getStats.Runs, _getStats.Best, _getStats.Avg, _getStats.Worst);
			

			Console.WriteLine("\r\n            Performance distribution");
			Console.WriteLine("            ------------------------");

			if(_addStats.Runs > 0)
				Console.WriteLine("Add()       {0,5:F1}% Best {1,5:F1}% Avg. {2,5:F1}% Worst -> ({3}, {4}, {5})", 
					new object[] {_addStats.PctBestCases, _addStats.PctAvgCases, _addStats.PctWorstCases, _addStats.BestCases, _addStats.AvgCases, _addStats.WorstCases });
			else
				Console.WriteLine("Add()        -");
			
			if(_getStats.Runs > 0)
				Console.WriteLine("Get()       {0,5:F1}% Best {1,5:F1}% Avg. {2,5:F1}% Worst -> ({3}, {4}, {5})",
					new object[] {	_getStats.PctBestCases, _getStats.PctAvgCases, _getStats.PctWorstCases, _getStats.BestCases, _getStats.AvgCases, _getStats.WorstCases });
			else
				Console.WriteLine("Get()        -");
			
			_addStats.Reset();
			_getStats.Reset();
		}

		/// <summary>
		/// Performing application specific clean-up, and disposal of allocated resources.
		/// </summary>
		private static void Dispose()
		{
			TxWriter.Close();
			StWriter.Close();
			FlStream.Close();
			_cache.Clear();
			_cache.Dispose();
		}		
	}
}

			

