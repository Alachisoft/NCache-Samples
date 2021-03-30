using Alachisoft.NCache.Runtime.Dependencies;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.ChangeFeedProcessor;
using Microsoft.Azure.Documents.ChangeFeedProcessor.FeedProcessing;
using Microsoft.Azure.Documents.ChangeFeedProcessor.Logging;
using Microsoft.Azure.Documents.ChangeFeedProcessor.PartitionManagement;
using Microsoft.Azure.Documents.Client;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Alachisoft.NCache.Samples
{
    [Serializable]
    public class NCacheChangeFeedObserver<T> : Microsoft.Azure.Documents.ChangeFeedProcessor.FeedProcessing.IChangeFeedObserver
    {
        private DependencyChangeCorrelator _correlator;

        public NCacheChangeFeedObserver(DependencyChangeCorrelator correlator)
        {
            _correlator = correlator;
        }

        public Task CloseAsync(Microsoft.Azure.Documents.ChangeFeedProcessor.FeedProcessing.IChangeFeedObserverContext context, Microsoft.Azure.Documents.ChangeFeedProcessor.FeedProcessing.ChangeFeedObserverCloseReason reason)
        {
            try
            {
                return Task.CompletedTask;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task OpenAsync(Microsoft.Azure.Documents.ChangeFeedProcessor.FeedProcessing.IChangeFeedObserverContext context)
        {
            try
            {
                return Task.CompletedTask;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task ProcessChangesAsync(IChangeFeedObserverContext context, IReadOnlyList<Document> docs, CancellationToken cancellationToken)
        {
            try
            {
                var deletedDocsKeys = new List<string>();

                var updatedInsertedDocsKeys = new List<string>();

                IDictionary<string, T> cacheItems = new Dictionary<string, T>();
                
                string key = "";

                foreach (Document doc in docs)
                {
                    key = doc.Id;
                    var isDeleted = doc.GetPropertyValue<string>("deleted");

                    if (!string.IsNullOrEmpty(isDeleted))
                    {
                        deletedDocsKeys.Add(key);
                    }
                    else
                    {
                        updatedInsertedDocsKeys.Add(key);
                    }
                }

                if (deletedDocsKeys.Count > 0)
                {
                    foreach (var deletedDocKey in deletedDocsKeys)
                    {
                        _correlator.OnFeedChange(deletedDocKey);
                    }
                }

                if (updatedInsertedDocsKeys.Count > 0)
                {
                    foreach (var updatedInsertedDocsKey in updatedInsertedDocsKeys)
                    {
                        _correlator.OnFeedChange(updatedInsertedDocsKey);
                    }
                }
                return Task.CompletedTask;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
