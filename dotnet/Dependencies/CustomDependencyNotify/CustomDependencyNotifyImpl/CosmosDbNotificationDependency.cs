

using Alachisoft.NCache.Runtime.Dependencies;
using Microsoft.Azure.Documents.ChangeFeedProcessor;
using Microsoft.Azure.Documents.ChangeFeedProcessor.Logging;
using Microsoft.Azure.Documents.ChangeFeedProcessor.PartitionManagement;
using Microsoft.Azure.Documents.Client;
using Models;
using System;
using System.Net.Http;

namespace Alachisoft.NCache.Samples
{
    [Serializable]
    public class CosmosDbNotificationDependency<T> : NotifyExtensibleDependency
    {
        private string _key = "";
        private string _monitoredUri;
        private string _monitoredSecretKey;
        private string _monitoredDbName;
        private string _monitoredCollectionName;

        private string _leaseUri;
        private string _leaseSecretKey;
        private string _leaseDbName;
        private string _leaseCollectionName;
        public CosmosDbNotificationDependency(string key, 
            string monitoredUri, 
            string monitoredPrimaryKey, 
            string monitoredDbName, 
            string monitoredCollectionName, 
            string leaseUri, 
            string leasePrimaryKey, 
            string leaseDbName, 
            string leaseCollectionName)
        {
            _key = key;
            _monitoredUri = monitoredUri;
            _monitoredSecretKey = monitoredPrimaryKey;
            _monitoredDbName = monitoredDbName;
            _monitoredCollectionName = monitoredCollectionName;
            _leaseUri = leaseUri;
            _leaseSecretKey = leasePrimaryKey;
            _leaseDbName = leaseDbName;
            _leaseCollectionName = leaseCollectionName;
        }

        public override bool Initialize() 
        {
            ChangeProcessorManager.Instance.RegisterDependency(_key, this, 
                _monitoredUri, 
                _monitoredSecretKey, 
                _monitoredDbName, 
                _monitoredCollectionName, 
                _leaseUri, 
                _leaseSecretKey, 
                _leaseDbName, 
                _leaseCollectionName);
            return true;
        }


        protected override void DependencyDispose()
        {
            ChangeProcessorManager.Instance.UnRegisterDependency(_key);
        }

    }
}
