using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Core;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Lucene.Net.Util;
using System;
using System.Configuration;
using System.Collections.Generic;
using Lucene.Net.Search;
using Lucene.Net.QueryParsers.Classic;
using DistributedLucene.LuceneData;
using Lucene.Net.Analysis.Standard;

namespace Alachisoft.NCache.Samples
{
    public class LuceneService
    {
        // Note there are many different types of Analyzer that may be used with Lucene, 
        // the exact one you use will depend on your requirements
        private Analyzer _analyzer = new StandardAnalyzer(LuceneVersion.LUCENE_48);
        private NCacheDirectory _luceneIndexDirectory;
        private IndexWriter _writer;

        private string _indexName = default(string);
        private string _cacheID = default(string);
        
        /// <summary>
        /// Executing this method will perform all the operations of the sample
        /// </summary>
        public void Run()
        {
            Console.WriteLine($"\nSample application for Distributed Lucene started\n");
            
            // Initializes Lucene Index on cache.
            InitializeLucene();

            // Populate Index with documents
            BuildIndex();

            // Perform Searches
            ExecuteQueries();

            // Releasing all instances
            DisposeInstances();

            Console.WriteLine($"\nSample application for Distributed Lucene ended");
            Console.ReadKey();
        }
        
        /// <summary>
        /// This method creates the NCacheDirectory and IndexWriter
        /// </summary>
        private void InitializeLucene()
        {
            _indexName = ConfigurationManager.AppSettings["IndexName"];
            _cacheID = ConfigurationManager.AppSettings["CacheID"];
            _luceneIndexDirectory = NCacheDirectory.Open(_cacheID, _indexName);
            _writer = new IndexWriter(_luceneIndexDirectory, new IndexWriterConfig(LuceneVersion.LUCENE_48, _analyzer));

            Console.WriteLine($"NCacheDirectory and IndexWriter opened with cache name: [{_cacheID}], and index name: [{_indexName}]");
        }

        /// <summary>
        /// This method popluates the Index created with documents containing Product informations. 
        /// </summary>
        private void BuildIndex()
        {
            IEnumerable<Product> dataToIndex = DataBuilder.FetchProducts(50) ;

            foreach (var product in dataToIndex)
            {
                Document doc = new Document()
                {
                    new TextField("Name", product.Name, Field.Store.YES),
                    new TextField("Category", product.Category, Field.Store.YES),
                    new TextField("Description", product.Description, Field.Store.YES)
                };

                // Adds the created documents with Product information on the opened writer.
                _writer.AddDocument(doc);
            }

            // Commits the current state of Index. 
            _writer.Commit();

            Console.WriteLine($"50 documents added in cache.");
        }

        /// <summary>
        /// This method performs searches for Products on the created index.
        /// </summary>
        private void ExecuteQueries()
        {
            // Initializing Reader, Searcher and Analyzer
            using (IndexReader reader = _writer.GetReader(true))
            {
                IndexSearcher searcher = new IndexSearcher(reader);

                // Performing search using TermQuery which gives result for the documents containing the term 'Tofu'.
                Query nameQuery = new TermQuery(new Term("Name", "tofu"));
                TopDocs nameDocsFound = searcher.Search(nameQuery, 50);
                Console.WriteLine($"{nameDocsFound.TotalHits} Documents returned on Product Name Search Result.");

                // Performing search using FuzzyQuery which takes a misspelled term and gives result for the documents containing the actual term 'Beverages'.
                Query categoryQuery = new FuzzyQuery(new Term("Category", "beeverages"));
                TopDocs categoryDocsFound = searcher.Search(categoryQuery, 50);
                Console.WriteLine($"{categoryDocsFound.TotalHits} Documents returned on Category Search Result.");

                // Performing search for documents containing Description field which has the term 'Soft Drinks'.
                QueryParser parser = new QueryParser(LuceneVersion.LUCENE_48, "Description", _analyzer);
                Query descriptionQuery = parser.Parse("Soft Drinks");
                TopDocs descriptionDocsFound = searcher.Search(descriptionQuery, 50);
                Console.WriteLine($"{descriptionDocsFound.TotalHits} Documents returned on Description Search Result.");
            }
        }

        /// <summary>
        /// This method disposes the instances. 
        /// </summary>
        private void DisposeInstances()
        {
            _writer.Dispose();
            _luceneIndexDirectory.Dispose();

            Console.WriteLine($"Instances Disposed.");
        }
    }
}
