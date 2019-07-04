using System;

namespace List
{
    class Program
    {
        static void Main (string[] args)
        {
            try
            {
                Alachisoft.NCache.Samples.DistributedList.Run();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}
