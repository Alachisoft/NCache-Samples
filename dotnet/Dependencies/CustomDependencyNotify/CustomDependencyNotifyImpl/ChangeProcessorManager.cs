using Alachisoft.NCache.Runtime.Dependencies;
using Microsoft.Azure.Documents.ChangeFeedProcessor;
using Microsoft.Azure.Documents.ChangeFeedProcessor.Logging;
using Microsoft.Azure.Documents.ChangeFeedProcessor.PartitionManagement;
using Microsoft.Azure.Documents.Client;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Alachisoft.NCache.Samples
{
    public class ChangeProcessorManager
    {
        private string _monitoredUri;
        private string _monitoredSecretKey;
        private string _monitoredDbName;
        private string _monitoredCollectionName;

        private string _leaseUri;
        private string _leaseSecretKey;
        private string _leaseDbName;
        private string _leaseCollectionName;

        private IChangeFeedProcessor _changeFeedProcessor;
        
        public static ChangeProcessorManager Instance { get; private set; } = new ChangeProcessorManager();
        public IChangeFeedProcessor CreateOrGetChangeProcessorBuilder(string monitoredUri, string monitoredSecretKey, string monitoredDbName, string monitoredCollectionName, string leaseUri, string leaseSecretKey, string leaseDbName, string leaseCollectionName)
        {
            if(_changeFeedProcessor == null) 
                _changeFeedProcessor = CreateChangeFeedProcessor(monitoredUri, monitoredSecretKey, monitoredDbName, monitoredCollectionName, leaseUri, leaseSecretKey, leaseDbName, leaseCollectionName);

            return _changeFeedProcessor;
        }

        public IChangeFeedProcessor CreateChangeFeedProcessor(string monitoredUri, string monitoredSecretKey, string monitoredDbName, string monitoredCollectionName, string leaseUri, string leaseSecretKey, string leaseDbName, string leaseCollectionName)
        {
            string hostName = Guid.NewGuid().ToString();

            DocumentCollectionInfo documentCollectionLocation = new DocumentCollectionInfo
            {
                Uri = new Uri(monitoredUri),
                MasterKey = monitoredSecretKey,
                DatabaseName = monitoredDbName,
                CollectionName = monitoredCollectionName
            };

            DocumentCollectionInfo leaseCollectionLocation = new DocumentCollectionInfo
            {
                Uri = new Uri(leaseUri),
                MasterKey = leaseSecretKey,
                DatabaseName = leaseDbName,
                CollectionName = leaseCollectionName
            };

            DocumentClient monitorClient = new DocumentClient(
                serviceEndpoint: new Uri(monitoredUri),
                authKeyOrResourceToken: monitoredSecretKey,
                handler: new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (a, b, c, d) => true
                },
                connectionPolicy: new ConnectionPolicy
                {
                    EnableEndpointDiscovery = false,
                    ConnectionMode = ConnectionMode.Gateway,
                    ConnectionProtocol = Protocol.Tcp
                });

            DocumentClient leaseClient = new DocumentClient(
                serviceEndpoint: new Uri(leaseUri),
                authKeyOrResourceToken: leaseSecretKey,
                handler: new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (a, b, c, d) => true
                },
                connectionPolicy: new ConnectionPolicy
                {
                    EnableEndpointDiscovery = false,
                    ConnectionMode = ConnectionMode.Gateway,
                    ConnectionProtocol = Protocol.Tcp
                });

            var tracelogProvider = new TraceLogProvider();
            using (tracelogProvider.OpenNestedContext(hostName))
            {
                LogProvider.SetCurrentLogProvider(tracelogProvider);
            }

            var builder = new ChangeFeedProcessorBuilder();

            var processor = builder
                .WithHostName(hostName)
                .WithFeedCollection(documentCollectionLocation)
                .WithLeaseCollection(leaseCollectionLocation)
                .WithFeedDocumentClient(monitorClient)
                .WithLeaseDocumentClient(leaseClient)
                .WithObserverFactory(new CosmosDbChangeFeedObserverFactory<Customer>())
                .WithProcessorOptions(new ChangeFeedProcessorOptions
                {
                    LeasePrefix = $"NCache--",
                    StartTime = DateTime.Now
                })
                .BuildAsync()
                .Result;
            processor.StartAsync();

            return processor;
        }

        public bool RegisterDependency(string key, NotifyExtensibleDependency dependency, string monitoredUri, string monitoredSecretKey, string monitoredDbName, string monitoredCollectionName, string leaseUri, string leaseSecretKey, string leaseDbName, string leaseCollectionName)
        {
            lock (this)
            {
                _monitoredUri = monitoredUri;
                _monitoredSecretKey = monitoredSecretKey;
                _monitoredDbName = monitoredDbName;
                _monitoredCollectionName = monitoredCollectionName;
                _leaseUri = leaseUri;
                _leaseSecretKey = leaseSecretKey;
                _leaseDbName = leaseDbName;
                _leaseCollectionName = leaseCollectionName;
                CreateOrGetChangeProcessorBuilder(_monitoredUri, _monitoredSecretKey, _monitoredDbName, _monitoredCollectionName, _leaseUri, _leaseSecretKey, _leaseDbName, _leaseCollectionName);
                DependencyChangeCorrelator.Instance.RegisterDependency(key, dependency);
            }
            return true;
        }

        public bool UnRegisterDependency(string key)
        {
            lock (this)
            {
                if (DependencyChangeCorrelator.Instance.UnRegisterDependency(key))
                {
                    _changeFeedProcessor.StopAsync();
                    _changeFeedProcessor = null;
                }
            }
            return true;
        }
    }
}
