using System;

namespace NHibernate.Caches.NCache
{
    public class RegionConfig
    {
        public string CacheId { get; set; }
        public string RegionName { get; set; }
        public TimeSpan? Expiration { get; set; } = null;
        public bool? UseSlidingExpiration { get; set; } = null;
        public string CacheKeyPrefix { get; set; } =
                                        "NHibernate-NCache-";

        public TimeSpan LockKeyTimeout { get; set; } =
                            TimeSpan.FromSeconds(5);
        public TimeSpan LockAcquireTimeout { get; set; } =
                            TimeSpan.FromSeconds(5);
        public int LockRetryTimes { get; set; } = 5;

        public TimeSpan LockMaxRetryDelay { get; set; } =
                            TimeSpan.FromMilliseconds(500);
        public TimeSpan LockMinRetryDelay { get; set; } =
                            TimeSpan.FromMilliseconds(20);

        public int CircuitBreakerExceptionsAllowed { get; set; } =
                                                1;
        public int CircuitBreakerDurationOfBreak { get; set; } =
                            30;
    }
}
