using Alachisoft.NCache.Samples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alachisoft.NCache.Samples
{
    public class CosmosDbChangeFeedObserverFactory<T> : Microsoft.Azure.Documents.ChangeFeedProcessor.FeedProcessing.IChangeFeedObserverFactory
    {
        private string _key;
        private DependencyChangeCorrelator Correlator { get; set; } = DependencyChangeCorrelator.Instance;

        public static CosmosDbChangeFeedObserverFactory<T> Instance { get; private set; } = new CosmosDbChangeFeedObserverFactory<T>();

        public Microsoft.Azure.Documents.ChangeFeedProcessor.FeedProcessing.IChangeFeedObserver CreateObserver()
        {
            return new NCacheChangeFeedObserver<T>(Correlator);
        }
    }
}
