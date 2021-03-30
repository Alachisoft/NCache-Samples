using NHibernate.Cache;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NHibernate.Engine;

namespace NHibernate.Caches.NCache
{
    public class NCacheClient : NCacheClientBase,
#pragma warning disable 618
        ICache
#pragma warning restore 618
    {
        public NCacheClient(
                    CacheHandle cacheHandle,
                    string regionPrefix,
                    RegionConfig regionConfig,
                    IDictionary<string, string> properties)
            :base(cacheHandle,regionPrefix,regionConfig,properties)
        { }

        public new int Timeout => 
            base.Timeout;

        public new string RegionName => 
            base.RegionName;

        public new bool PreferMultipleGet => 
            base.PreferMultipleGet;

        public new void Clear() =>
            base.Clear();

        public new Task ClearAsync(
                CancellationToken cancellationToken) => 
            base.ClearAsync(cancellationToken);

        public new void Destroy() =>
            base.Destroy();

        public new object Get(
                object key) =>
            base.Get(key);

        public new Task<object> GetAsync(
                object key,
                CancellationToken cancellationToken) =>
            base.GetAsync(
                        key,
                        cancellationToken);

        public new object[] GetMany(
                object[] keys) =>
            base.GetMany(keys);

        public new Task<object[]> GetManyAsync(
                object[] keys,
                CancellationToken cancellationToken) => 
            base.GetManyAsync(
                            keys,
                            cancellationToken);

        public new object Lock(
                object key) =>
            base.Lock(key);

        public new Task<object> LockAsync(
                object key,
                CancellationToken cancellationToken) =>
            base.LockAsync(
                         key,
                         cancellationToken);

        public new object LockMany(
                object[] keys) => 
            base.LockMany(keys);

        public new Task<object> LockManyAsync(
                object[] keys,
                CancellationToken cancellationToken) =>
            base.LockManyAsync(
                            keys,
                            cancellationToken);

        public new long NextTimestamp() =>
            base.NextTimestamp();

        public new void Put(
                object key,
                object value) =>
            base.Put(
                   key,
                   value);

        public new Task PutAsync(
                object key,
                object value,
                CancellationToken cancellationToken) =>
            base.PutAsync(
                        key,
                        value,
                        cancellationToken);

        public new void PutMany(
                object[] keys,
                object[] values) =>
            base.PutMany(
                        keys,
                        values);

        public new Task PutManyAsync(
                object[] keys,
                object[] values,
                CancellationToken cancellationToken) => 
            base.PutManyAsync(
                            keys,
                            values,
                            cancellationToken);

        public new void Remove(
                object key) =>
            base.Remove(key);
        
        public new Task RemoveAsync(
                object key,
                CancellationToken cancellationToken) => 
            base.RemoveAsync(
                            key, 
                            cancellationToken);

        public new void Unlock(
                object key,
                object lockValue) =>
            base.Unlock(
                      key,
                      lockValue);

        public new Task UnlockAsync(
                object key,
                object lockValue,
                CancellationToken cancellationToken) => 
            base.UnlockAsync(
                            key,
                            lockValue,
                            cancellationToken);

        public new void UnlockMany(
                object[] keys,
                object lockValue) =>
            base.UnlockMany(
                          keys,
                          lockValue);

        public new Task UnlockManyAsync(
                object[] keys,
                object lockValue,
                CancellationToken cancellationToken) =>
            base.UnlockManyAsync(
                                keys,
                                lockValue, 
                                cancellationToken);


        internal new void Put(
                            object key,
                            object value,
                            ISessionImplementor session,
                            List<QueryDependencyConfiguration> queryDependencies) =>
            base.Put(
                   key,
                   value,
                   session,
                   queryDependencies);

        internal new Task PutAsync(
                        object key,
                        object value,
                        ISessionImplementor session,
                        List<QueryDependencyConfiguration> queryDependencies,
                        CancellationToken cancellationToken) =>
            base.PutAsync(
                        key,
                        value,
                        session,
                        queryDependencies,
                        cancellationToken);

        internal new void PutMany(
                            object[] keys,
                            object[] values,
                            ISessionImplementor session,
                            List<QueryDependencyConfiguration> queryDependencies) =>
            base.PutMany(
                       keys,
                       values,
                       session,
                       queryDependencies);

        internal new Task PutManyAsync(
                                    object[] keys,
                                    object[] values,
                                    ISessionImplementor session,
                                    List<QueryDependencyConfiguration> queryDependencies,
                                    CancellationToken cancellationToken) =>
            base.PutManyAsync(
                        keys,
                        values,
                        session,
                        queryDependencies,
                        cancellationToken);

    }
}
