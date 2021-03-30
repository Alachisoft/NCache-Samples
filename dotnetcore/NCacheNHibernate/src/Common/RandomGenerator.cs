using System;

namespace NHibernate.Caches.NCache
{
    internal static class RandomGenerator
    {
        private static Random _global = new Random();

        [ThreadStatic]
        private static Random _local;

        internal static Random Instance()
        {
            if (_local == null)
            {
                int seed;

                lock (_global)
                {
                    seed = _global.Next();
                }

                _local = new Random(seed);
            }

            return _local;
        }
    }
}
