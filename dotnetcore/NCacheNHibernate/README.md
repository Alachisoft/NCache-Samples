# NCache NHibernate Second-Level Cache Provider

## [Table of contents](#table-of-contents)

* [Introduction](#introduction)
* [Considerations When Using NCache NHibernate Provider](#considerations-when-using-ncache-nhibernate-provider)
* [NCache NHibernate Second-Level Cache Main Classes](#ncache-nhibernate-second-level-cache-main-classes)
    
    * [NCacheProvider Class](#ncacheprovider-class)

    * [ApplicationConfig Class](#applicationconfig-class)

    * [CacheConfig Class](#cacheconfig-class)

      * [NCacheServerInfo Class](#ncacheserverinfo-class)

    * [RegionConfig Class](#regionconfig-class)

    * [ConfigurationSettings Class](#configurationsettings-class)
    
* [NCache NHibernate Second-Level Cache Database Dependencies](#ncache-nhibernate-second-level-cache-database-dependencies)
    * [Database Dependencies on NHibernate Entities and Query Results](#database-dependencies-on-nhibernate-entities-and-query-results)

      * [Entity and Query Result Dependencies Go Hand-in-Hand](#entity-and-query-result-dependencies-go-hand-in-hand)

      * [Handle Polling-Based Dependencies With **Care**](#handle-polling-based-dependencies-with-care)

      * [Use NCacheQueryCache When Working with Database Dependencies](#use-ncachequerycache-when-working-with-database-dependencies)

    * [Entity Database Dependencies Classes](#entity-database-dependencies-classes)

      * [DependencyConfig](#dependencyconfig)

      * [CommandDependencyConfig](#commanddependencyconfig)

      * [SQLDependencyConfig](#sqldependencyconfig)

      * [OracleDependencyConfig](#oracledependencyconfig)

      * [OleDbDependencyConfig](#oledbdependencyconfig)

    * [Query Result Set Database Dependencies Classes](#query-result-set-database-dependencies-classes)

      * [QueryDependencyConfiguration](#querydependencyconfiguration)

      * [QueryCommandDependencyConfiguration](#querycommanddependencyconfiguration)

      * [QueryTableDependencyConfiguration](#querytabledependencyconfiguration)

* [Sample Demonstration](#sample-demonstration)

  * [Overview](#overview)

  * [Prerequisites](#prerequisites)

  * [Build and Run the Sample](#build-and-run-the-sample)

* [Additional Resources](#additional-resources)

* [Technical Support](#technical-support)

* [Copyrights](#copyrights)

### Introduction

The NCache provider has been upgraded in light of the changes introduced in NHibernate 5.x and it is now possible to use the NCache caching provider in **both .NET Framework as well as .NET Core applications**.

The NCache NHibernate caching provider is unique in that it leaverages the power of NCache as a fast, distributed and scalable caching solution together with the [database dependency](#ncache-nhibernate-second-level-cache-database-dependencies) and [expiration](https://www.alachisoft.com/resources/docs/ncache/prog-guide/expirations.html) strategy features of NCache to mitigate cache staleness issues. No other caching provider, as far as we know, provides all these desirable qualities in one package.

Furthermore, it also supports all NHibernate cache-concurrency strategies, whether they be [READ-WRITE](https://nhibernate.info/doc/nhibernate-reference/performance.html#performance-cache-readwrite), [READ-ONLY](https://nhibernate.info/doc/nhibernate-reference/performance.html#performance-cache-readonly) or [NON-STRICT READ-WRITE](https://nhibernate.info/doc/nhibernate-reference/performance.html#performance-cache-nonstrict).

[Back to top](#table-of-contents)

### Considerations When Using NCache NHibernate Provider

When using NCache as an NHibernate caching provider, the following considerations from [here](https://nhibernate.info/doc/nhibernate-reference/caches.html#NHibernate.Caches-howto) as well specific to the NCache provider should be kept in mind which are as follows:

- The NCache NHibernate 2nd-level [region cache configurations](#ncacheprovider-class) are shared between multiple NHibernate **ISessionFactory** instances if the application is accessing multiple databases. 

  To allow the cache results of these **ISessionFactory** instances to work independently and not have their operations interfere with one another, set a default *regions prefix* with each of the instances as explained in the below quoted text taken from [here](https://nhibernate.info/doc/nhibernate-reference/caches.html):
  
  > "Depending on the chosen cache provider, the second level cache may be actually shared between different session factories. If you need to avoid this for some session factories, configure each of them with a different cache.region_prefix."
  
   Therefore, in case where multiple databases are being accessed in an application, set a different ***region prefix*** for each of them and then set the same value of the ***regions prefix*** when specifying the [**CacheConfig**](#cacheconfig-class), [**RegionConfig**](#regionconfig-class) instances as well as when specifying the [query and entity dependencies](#ncache-nhibernate-second-level-cache-database-dependencies). Please see the [*App.config*](./sample/App.config) and [*appsettings.json*](./sample/appsettings.json) files in the accompanying sample for demonstration where the region prefix is set to the value given in the [SQL Server](./sample/NHibernateHelpers/NHibernateHelperSQL.cs) and [Oracle](./sample/NHibernateHelpers/NHibernateHelperOracle.cs) ISession factories.

- You can explicitly specify different cache region configuration for the different cache regions and override some or all of the values using the properties of the [**RegionConfig**](#regionconfig-class) class. 

  However, if you don't provide a configuration for a specific cache region, the default values will be used. This can be seen in the accompanying [sample](./sample/) where the *UpdateTimestampsCache* and the *NHibernate.Cache.StandardQueryCache* are not explicitly configured in the [*App.config*](./sample/App.config) and [*appsettings.json*](./sample/appsettings.json) files.

- Different NHibernate entities may have different requirements when it comes to cache storage. These requirements include reliability, scalability etc. For maximum reliability, the NCache [Replicated](https://www.alachisoft.com/resources/docs/ncache/admin-guide/cache-topologies.html#replicated-cache) topology would be most appropriate whereas maximum scalability is achieved using the [Partitioned](https://www.alachisoft.com/resources/docs/ncache/admin-guide/cache-topologies.html#partitioned-cache) topology. The [Partition-Replica](https://www.alachisoft.com/resources/docs/ncache/admin-guide/cache-topologies.html#partitioned-replica-cache) provides a middle ground between these two often-conflicting requirements.

  In light of this, you can map the different cache regions to separate cache clusters with different topologies using the [**RegionConfig**](#regionconfig-class) *CacheId* property which specifies the cache cluster the entities in the cache region will be stored.

  In case multiple clusters are used, one, ***and only one***, of the [**CacheConfig**](#cacheconfig-class) instances registered with the [**NCacheProvider**](#ncacheprovider-class) should have the *IsDefault* property set to *true*. The *CacheId* property of this default instance will then be used by regions for which no explicit cache region configurations have been assigned. This is the case for the **UpdateTimestampsCache* and the *NHibernate.Cache.StandardQueryCache** in the [sample](./sample/).

- In the absence of [database dependencies](#ncache-nhibernate-second-level-cache-database-dependencies), NHibernate is not made aware of changes to cache by an external process which may result in stale results with partial mitigation provided by introducing cache item expiration strategies. 

- The second level cache requires the use of transactions for it to function. Transactions are **strongly** encouraged regardless but in the case of a second-level cache, they are a necessity.

- In the case of composite ids or custom [IUserType](https://nhibernate.info/doc/nhibernate-reference/mapping.html#mapping-types-custom) identifiers, it is **necessary** with the NCache NHibernate provider to override the **ToString()** method of the entity class as that is used to create unique cache keys of type **System.String** against which the items are inserted in an NCache cache. Such is the case with the [**Customercustomerdemo**](.\sample\Entities\Customercustomerdemo.cs), [**Employeeterritories**](.\sample\Entities\Employeeterritories.cs) and [**Orderdetails**](.\sample\Entities\Orderdetails.cs) entity classes in the sample.

- As mentioned before, the NCache NHibernate provider allows the flexibility to have different NHibernate cache regions to be mapped to physically separate cache clusters.  

  However, if polling-based database dependencies are set on query results and entities with the results and entities being stored in separate cache clusters, then invalid results may be found due to the dependencies on the query results and the related entitities triggering at different times. 
  
  Therefore, for cache regions spread out over multiple NCache clusters, notification-based dependencies are recommended.

- The NCache NHibernate provider has a built-in [circuit breaker](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/implement-circuit-breaker-pattern) feature that would ensure that, even with Networking issues causing the cache cluster to be become temporarily inaccessible, the application won't crash and it would still be able to get valid up-to-date data from the database. 

  An exception to this is [READ-WRITE](https://nhibernate.info/doc/nhibernate-reference/performance.html#performance-cache-readwrite) is used that requires locks on cache items during updates, not being able to access the cache for locking or unlocking will cause an NHibernate [**CacheException**](https://github.com/nhibernate/nhibernate-core/blob/5.3.x/src/NHibernate/Cache/CacheException.cs) to be thrown as per NHibernate specifications. The application, then, is responsible for catching the exception and take steps to resolve it.


[Back to top](#table-of-contents)  

### NCache NHibernate Second-Level Cache Main Classes

The following are the main classes used during intial NHibernate configuration and set-up of the NHibernate session factory.

- #### NCacheProvider Class

  The entry point of NCache as an NHibernate caching provider is the [**NCacheProvider**](./src/NCacheProvider.cs) class. When NCache is configured for use an an NHibernate caching provider, as shown in the next section [**NCache as NHibernate Second-Level Cache**](#ncache-as-nhibernate-second-level-cache), the NHibernate [**ISessionFactory**](https://nhibernate.info/doc/nhibernate-reference/architecture.html#architecture-overview) uses an instance of **NCacheProvider** to instantiate entity caches for the different regions specified by the entity mapping data supplied when setting up the NHibernate configuration. 

  This is done by calling the **BuildCache** method on the **NCacheProvider** instance for each region registered with NHibernate configuration which returns instances of [**NCacheClient**](.\src\NCacheClient.cs) class. Furthermore, if query result caching is enabled, The **ISessionFactory** creates regions named *NHibernate.Cache.StandardQueryCache* and *UpdateTimestampsCache* as explained [here](https://nhibernate.info/doc/nhibernate-reference/performance.html#performance-querycache) and prepares **NCacheClient** instances for them as well.

  The **NCacheProvider** class has a static field *ApplicationConfiguration* which is an instance of [**ApplicationConfig**](.\src\Configurations\ApplicationConfig.cs) class. This encapsulates all the information used to instantiate the different **NCacheClient** instances for the different NHibernate cache regions. Please see section [*ApplicationConfig Class*](#applicationconfig-class) for more information.

  Another important static field of the **NCacheProvider** class is *cacheHandles* which is a dictionary whose values are instances of the [**CacheHandle**](.\src\Common\CacheHandle.cs) class. These instances are used to carry out the actual CRUD operations when calling the different methods of the **NCacheClient** class during NHibernate caching operations through the use of its *Cache* NCache handle property that is instantiated in the instance constructor. 

  Each **CacheHandle** instance in the dictionary corresponds to a single [**CacheConfig**](.\src\Configurations\CacheConfig.cs) instance by setting the keys of the instances equal to the **CacheConfigId** property of the corresponding **CacheConfig** instance. Please see section [*CacheConfig Class*](#cacheconfig-class) for more information on the **CacheConfig** class.

  The **NCacheProvider** class has a static constructor that looks for any *AppConfig* xml file in the root directory of the project and if there is a configuration section *ncache* of type [**NHibernate.Caches.NCache.NCacheSectionHandler, NHibernate.Caches.NCache**] declared in the *configSections* subsection, then the properties under the *ncache* section are used to populate the static *applicationConfiguration* instance. The xml parsing logic that returns an instance of the **ApplicationConfig** class can be inspected [here](.\src\Configurations\NCacheSectionHandler.cs).

  [Back to top](#table-of-contents)  

- #### ApplicationConfig Class

  As mentioned in the [*NCacheProvider Class*](#ncacheprovider-class) section, instances of the [**ApplicationConfig**](.\src\Configurations\ApplicationConfig.cs) are used to encapsulate all the information required for setting up region entity and query caches in an NHibernate application using NCache as the caching provider. Instances of this class have the following .NET properties:

  - *CacheConfigurations*

     This is a dictionary whose values are instances of the [**CacheConfig**](.\src\Configurations\CacheConfig.cs) class and the keys are the **CacheConfigId** property values of those instances. The *CacheConfigurations* property encapsulates all the information required to set up the NCache cache handles used in the different NHibernate regions for carring out cache CRUD operations. 
   
     Please see information on the **CacheConfig** class in the [*CacheConfig Class*](#cacheconfig-class) section.

  - *CacheRegions*

    This is a dictionary whose values are instances of the [**RegionConfig**](.\src\Configurations\RegionConfig.cs) class and the keys are the **RegionName** property values of those instances. The *CacheRegions* property encapsulates information about the [**CacheConfig**](.\src\Configurations\CacheConfig.cs) instances to use for cache CRUD operations and the caching strategies to be applied on the items cached in the different regions. 
   
    These strategies include cache expiration modes (*default*, *sliding* or *none*), the cache expiration periods, locking strategies when [READ-WRITE](https://nhibernate.info/doc/nhibernate-reference/performance.html#performance-cache-readwrite) cache concorrency is used, as well as the circuit breaker properties for resilience and locking retry policies for optimistic concurrency control in case of NHibernate cache locking during READ-WRITE mode.

    Furthermore, the database dependencies to be added to the NHibernate entity cached items in the different regions are also including on a per-region basis within the **RegionConfig** instances.

    Please see more information on the **RegionConfig** class in the [*RegionConfig Class*](#regionconfig-class) section.

  - *QueryDependencies*

    This is a hash set containing instances of [**QueryDependencyConfiguration**](.\src\Abstractions\QueryDependencyConfiguration.cs) instances. This is used to configure database dependencies on the cached query results in case query cache is enabled **and** [**NCacheQueryFactory**](.\src\QueryCache\NCacheQueryCacheFactory.cs) is used to spin up query caches in the default *NHibernate.Cache.StandardQueryCache* region as well as other regions where query results may be cached.

  - *EntityDependencies*

     This is a hash set containing instances of [**DependencyConfig**](.\src\Abstractions\DependencyConfig.cs) which represent the database dependencies added on the cached NHibernate entities.
  
  

  More information about database dependencies on cached entities and query results, including information about **QueryDependencyConfiguration** and its derived classes, can be found in the [NCache NHibernate Second-Level Cache Database Dependencies](#ncache-nhibernate-second-level-cache-database-dependencies) section.

  [Back to top](#table-of-contents)    

- #### CacheConfig Class

  The [**CacheConfig**](.\src\Configurations\CacheConfig.cs) class is used to store the configurations needed for initializing [**CacheHandle**](.\src\Common\CacheHandle.cs) instances that are responsible for accessing and operating on the NCache through their **Cache** properties.

  The **CacheConfig** class serves as a thin wrapper around the NCache [**CacheConnectionOptions**](https://www.alachisoft.com/resources/docs/ncache/dotnet-api-ref/Alachisoft.NCache.Client.CacheConnectionOptions.html) instance *connectionOptions* and its various properties internally configure this **CacheConnectionOptions** instance.

  Following are the public properties of the **CacheConfig** class:

  | Property  | Type  | Default Value  | Description  |
  |:-:|:-:|:-:|---|
  | RegionPrefix  | **System.String**  | "nhibernate"  | If the NHibernate configuration has a set default region prefix, then this should be set with that value.  |
  | CacheId  | **System.String**  | "nhibernate"  | The Id of the NCache cluster e.g. "demoCache  |
  | IsDefault  | **bool**  | *false*  | Sets the **CacheConfig** instance as the default to be used in case multiple instances are registered. ***Only one*** instance with a give *RegionPrefix* value should be set as default by setting this value to *true*. This property does not matter if only one **CacheConfig** is registered for given *RegionPrefix*  |
  | ConnectionTimeout  | **System.Nullable\<double>**  | null  | Gets or sets the *connectionOptions* **ConnectionTimeout** property  |
  | KeepAliveInterval  | **System.Nullable\<double>**  | null  | Gets or sets the *connectionOptions* **KeepAliveInterval** property |
  | EnableKeepAlive  | **System.Nullable\<bool>**  | null  | Gets or sets the *connectionOptions* **EnableKeepAlive** property |
  | CommandRetryInterval  | **System.Nullable\<double>**  | null  | Gets or sets the *connectionOptions* **CommandRetryInterval** property  |
  | CommandRetries  | **System.Nullable\<int>**  | null  | Gets or sets the *connectionOptions* **CommandRetries** property  |
  | RetryConnectionDelay  | **System.Nullable\<double>**  | null  | Gets or sets the *connectionOptions* **RetryConnectionDelay** property  |
  | RetryInterval  | **System.Nullable\<double>**  | null  | Gets or sets the *connectionOptions* **RetryInterval** property  |
  | ConnectionRetries  | **System.Nullable\<int>**  | null  | Gets or sets the *connectionOptions* **ConnectionRetries** property  |
  | EnableClientLogs  | **System.Nullable\<bool>**  | null  | Gets or sets the *connectionOptions* **EnableClientLogs** property  |
  | ClientRequestTimeout  | **System.Nullable\<double>**  | null  | Gets or sets the *connectionOptions* **ClientRequestTimeOut** property  |
  | LoadBalance  | **System.Nullable\<bool>**  | null  | Gets or sets the *connectionOptions* **LoadBalance** property  |
  | AppName  | **System.String**  | Process Id  | Gets or sets the *connectionOptions* **AppName** property  |
  | ClientBindIP  | **System.String**  | null  | Gets or sets the *connectionOptions* **ClientBindIP** property  |
  | LogLevel  | **System.Nullable\<[**NCacheLogLevel**](.\src\Configurations\NCacheLogLevel.cs)>**  | null  | Gets or sets the *connectionOptions* **LogLevel** property  |
  | Mode  | **System.Nullable\<[**NCacheIsolationLevel**](.\src\Configurations\NCacheIsolationLevel.cs)>**  | null  | Gets or sets the *connectionOptions* **Mode** property  |
  | ServerList  | **System.Collections.Generic.List\<[**NCacheServerInfo**](#ncacheserverinfo-class)>**  | null  | Gets or sets the *connectionOptions* **ServerList** property  |
  | UserCredentials  | [**NCacheCredentials**](.\src\Configurations\NCacheCredentials.cs)  | null  | Gets or sets the *connectionOptions* **UserCredentials** property  |
  | SyncMode  | **System.Nullable\<[**NCacheClientSyncMode**](.\src\Configurations\NCacheClientSyncMode.cs)>**  | null  | Gets or sets the *connectionOptions* **ClientCacheMode** property  |

  Once set, a **CachConfig** instance can be passed as an argument to the [**CacheHandle**](.\src\Common\CacheHandle.cs) constructor to instantiate the **Cache** NCache cache handle property using the **CacheId** and **connectionOptions** fields of the **CacheConfig** instance.

  - #### NCacheServerInfo Class

    The [**NCacheServerInfo**](.\src\Configurations\NCacheServerInfo.cs) class is used to set the connection information about individual NCache servers making up the NCache cache. Following are its properties:

    | Property  | Type  | Default Value  | Description  |
    |:-:|:-:|:-:|---|
    | Port  | **int**  | 9800  | The port on which the NCache server will be listening for client CRUD requests  |
    | Name | **System.String**| null| The IPv4 string or DNS name of the server|
    | IP | **System.Net.IPAddress** | null | The IPv4 address of the server |
    | Priority| **short** | 0 | Applies to the [**Replicated**](https://www.alachisoft.com/ncache/caching-topology.html#replicated) topology where you prioritize which server the client connects to. Has no effect for other NCache [topologies](https://www.alachisoft.com/ncache/caching-topology.html) |

    Only one of the *Name* and the *IP* properties have to be set.

  [Back to top](#table-of-contents)  

- #### RegionConfig Class

  When using the NCache NHibernate provider, each NHibernate region is assigned an instance of the [**RegionConfig**](.\src\Configurations\RegionConfig.cs) class that determines how different caching strategies to during cache read and write operations, as mentioned in the [*ApplicationConfig Class*](#applicationconfig-class) section.

  Following are the properties of the **RegionConfig** class:

  | Property  | Type  | Default Value  | Description  |
  |:-:|:-:|:-:|---|
  | CacheId  | **System.String**  | null  | Gets or sets the  value that determines the cache Id of the NCache cluster this region will map to. If not given, the default cache Id given in the **CacheConfig** instances registered with NCacheProvider will be assigned.  |
  | RegionName  | **System.String**  | null | Gets or sets the name of the region for which this **RegionConfig** instance will be used. The region name should be in the format *[NHibernate default region prefix.]regionId* where the expression in the block parenteses can be left out if no default region-prefix is assigned  |
  | Expiration  | **System.Nullable\<System.TimeSpan>**  | null  | If not null, overrides the default NHiberate configuration expiration period of the items cached in the region. If null and NHibernate default expiration is not set, items in the region will not be expired  |
  | UseSlidingExpiration  | **System.Nullable\<bool>**  | null  | If not null, overrides the default NHibernate UseSlidingExpiration value. If null and default NHibernate UseSlidingExpiration is not set, then Absolute expiration is used. **Note:** This property only applies if expiration is enabled on the cached items either through the **Expiration** property or NHibernate default expiration is set.  |
  | CacheKeyPrefix  | **System.String**  | "NHibernate-NCache-"  | Gets or sets the cache key prefix as part of the string cache key against which the items are cached in NCache  |
  | RegionPrefix  | **System.String**  | **String**.*Empty*  | If not null or empty, overrides the default NHibernate RegionPrefix value. Used as part of string cache keys against which the items will be cached in NCache |
  | LockKeyTimeout  | **System.TimeSpan**  | **TimeSpan**.*FromSeconds*(5)  | Used in [READ-WRITE](https://nhibernate.info/doc/nhibernate-reference/performance.html#performance-cache-readwrite) mode to specify the amount of time cached items can be locked  |
  | LockAcquireTimeout | **System.TimeSpan**   | **TimeSpan**.*FromSeconds*(5)  | The amount of time given to acquire lock on a cached item in [READ-WRITE](https://nhibernate.info/doc/nhibernate-reference/performance.html#performance-cache-readwrite) mode |
  | LockRetryTimes  | **int**  | 5  | The maximum number of attempts that can be made to acquire locks on cached items in [READ-WRITE](https://nhibernate.info/doc/nhibernate-reference/performance.html#performance-cache-readwrite) mode. If max. attempts is not reached before *LockAcquireTimeout* is reached, then further attempts are cancelled |
  | LockMaxRetryDelay  | **System.TimeSpan**  | **TimeSpan**.*FromMilliseconds*(500)  | The maximum delay between attemps to acquire lock in [READ-WRITE](https://nhibernate.info/doc/nhibernate-reference/performance.html#performance-cache-readwrite) mode  |
  | LockMinRetryDelay  | **System.TimeSpan**  | **TimeSpan**.*FromMilliseconds*(20)  | The minimum delay between attemps to acquire lock in [READ-WRITE](https://nhibernate.info/doc/nhibernate-reference/performance.html#performance-cache-readwrite) mode  |
  | CircuitBreakerExceptionsAllowed   | **int**  | 1  | The number of times a call to NCache results in a *No Server is available* before the circuit breaker is applied  |
  | CircuitBreakerDurationOfBreak   | **int**  | 30  | The duration, in seconds,for which a circuit breaker stays in *Open* state before transitioning to *Half-open*  |

  If no **RegionConfig** instance is specifically declared for a cache region, an instance with the default values for the properties of the **RegionConfig** class will be assigned and the *CacheId* property will be set to the *CacheId* of the default **CacheConfig** instance. 

  [Back to top](#table-of-contents) 

- #### ConfigurationSettings Class

  The [**ConfigurationSettings**](.\src\Configurations\ConfigurationSettings.cs) class is mainly used as a data binding mechanism when populating [**ApplicationConfig**](.\src\Configurations\ApplicationConfig.cs) from a custom xml or json file in addition to using the default *AppConfig* xml file. This class can be used with [**IConfiguration**](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.iconfiguration?view=dotnet-plat-ext-3.1), for example, to get the data from *appsettings.json* and environmental variables.

  Ofcourse, you can instantiate an instance of this class, setting up all the cache and region configurations together with query dependencies programmatically as well.

  An instance of this class, once populated, can then be passed as an argument to the static *SetApplicationConfiguration* method of the [**NCacheProvider**](.\src\NCacheProvider.cs) class to configure the NHibernate session factory NCache second-level cache. 

  [Back to top](#table-of-contents) 

### NCache NHibernate Second-Level Cache Database Dependencies

NCache provides notification-based and polling-based database dependency features to mitigate issues related to stale cached data. Database dependencies are set as meta-data on cached items through the [**CacheItem**](https://www.alachisoft.com/resources/docs/ncache/dotnet-api-ref/Alachisoft.NCache.Client.CacheItem.html) [***Dependency***](https://www.alachisoft.com/resources/docs/ncache/dotnet-api-ref/Alachisoft.NCache.Client.CacheItem.Dependency.html#Alachisoft_NCache_Client_CacheItem_Dependency) property.

NCache notification-based dependencies are applicable for [*MS SQL Server*](https://www.alachisoft.com/resources/docs/ncache/prog-guide/sql-dependency.html) *2005 and later* and [*Oracle*](https://www.alachisoft.com/resources/docs/ncache/prog-guide/oracle-dependency.html?tabs=net%2Cnet1) *10g and later*. In this case, NCache makes use of the database notification features to register SQL commands (SQL queries and/or stored procedures) with the database. In the event of a database change event that effects the corresponding cached items with notification-based dependencies registered, the database will notify NCache and, in response, NCache will remove the relevant data.

Although notification-based dependencies provide real-time change notification support, having too many notifications being sent in response to database changes can cause chattiness in the network. In such cases, polling-based dependencies can be used as explained [*here*](https://www.alachisoft.com/resources/docs/ncache/prog-guide/oledb-dependency.html?tabs=net%2Cnet1).

The NCache NHibernate provider provides the flexibility to mix and match these two modes of database dependencies for MS SQL Server 2005 and later as well as Oracle 10g and later. 

[Back to top](#table-of-contents) 

#### Database Dependencies on NHibernate Entities and Query Results

There are three very important points to remember when using NCache database dependencies in your NHibernate application:

- #### Entity and Query Result Dependencies Go Hand-in-Hand
    
   - Any time a dependency is set on cached instances of an NHibernate entity, any query results cached with identifiers belonging to those instances ***must*** also have ***table-wide*** dependencies set on them. Two scenarios that highlight this necessity are as follows:

     - Suppose you query for the IDs of all the rows in a certain table. If query caching is enabled, all the individual IDs are returned and cached. Suppose now that row-specific database dependecies are set up on instances of the NHibernate entity corresponding to that table. If you delete a row in the database using a process external to NHibernate, the corresponding cached entity is also invalidated and removed. Now, the entity does not exist in either the database or the cache. 

       Since query results are cached separately by NHibernate in the query cache, if you don't set dependencies on the query results, the object identifiers of the now non-existant entities remain since NHibernate has no information about database changes outside its scope. 
  
       Any subsequent calls to the same query will now be serviced by the cached query results but since one of the identifiers does not have the related entity in the database ***OR*** the cache, an NHibernate **ObjectNotFound** exception will be thrown. 

     - Suppose you query for the customer IDs based on the country they belong to and cache the query results as well as the entities. If a database  dependency is set up on the entity and an update is made to the country field in a row which has been cached, the entity is removed from the cache but the query results containing the identifiers remain.

       A subsequent call on the same query will then be serviced by the invalid cached query results. Using the identifiers, the entity will first be searched in the cache, but since it is not there due to dependency change, the updated entity information will be retrieved and cached. 

       Now you may have a query over customers from, say, "USA" but you get back an ID of a customer whose country has been changed from "USA" to "Canada" suppose. 

     These two cases show that if dependencies are not set up on the query results but are set up on the entities they refer to, invalid cache results are obtained even when the cached entities are up to date.

     [Back to top](#table-of-contents) 

 -  #### Handle Polling-Based Dependencies With **Care**

    By query result dependencies being related to entity dependencies, what is meant is the relationship between the dependencies of entities and the dependencies of the query results which are dependent on the given entities. For example, the dependencies of a query result set that is comprised of all customer IDs in the database are related to dependencies on the *Customer* NHibernate entity.

    Query dependencies are *slower* than the related entity dependencies if the former are polling-based while the latter are notification-based; with notification-based dependencies, we get real-time notifications from the databases about database changes while with polling-based dependencies, there is polling-interval delay between when database changes occur and when NCache becomes aware of the changes through polling. 

    Such a mismatch in the timings of the dependencies triggering can cause problems as the following scenarios illustrate:

    - Revisiting the example given before, suppose a query is made to get all customers from the database for a given country with the query results containing the customer identifiers stored in the query cache region and the customer entities stored in the entity cache region.
      
      Suppose the query result is cached with polling dependency and the customer entities are cached with row-based notification dependency. Now if one of the rows for that given country has the country column changed, then the corresponding Customer entity is removed from cache immediately upon notification. 

      On the other hand, the query result is not ***immediately*** invalidated by dependency change since there is polling interval in between checks for database changes. If the query is called against before the query results are invalidated, then we will get invalid result since the list of customers in the result contains a customer whose country ***was changed***.

    - Even if polling dependencies are used with both query results and the related entities, invalid cache results can still occur if the query cache region and entity cache region are in physically separate NCache clusters. This is possible since the two regions could correspond to different cache configurations that map to separate NCache clusters.

      Now if database changes occur and the entity is invalidated before the query result, then in the time till the query results are finally invalidated, invalid cache results may be obtained.

    In such cases, ***always*** try to set the query result region to be in the same physical cache as the related entity region when using polling-based dependencies. 
    
    Furthermore, if multiple cache regions map to different cache clusters and the entities involved in the query are in separate physical caches, then use notification-based dependencies.  

    [Back to top](#table-of-contents) 

  - #### Use [**NCacheQueryCache**](.\src\QueryCache\NCacheQueryCache.cs) When Working with Database Dependencies

    The NHibernate [***StandardQueryCache***](https://github.com/nhibernate/nhibernate-core/blob/5.3.x/src/NHibernate/Cache/StandardQueryCache.cs) is great at maintaining cache consistency so long as all database writes happen through NHibernate. It is the cache staleness caused due to database writes by external processes that we need database dependencies in the first place.

    The **StandardQueryCache**, however, has no mechanism for integrating database dependencies with cached query results. To account for this, the [**NCacheQueryCache**](.\src\QueryCache\NCacheQueryCache.cs) class is present which extends **StandardQueryCache** and uses an instance of [**NCacheClient**](.\src\NCacheClient.cs) to insert query results with database dependencies into the NCache cache. Use the accompanying [**NCacheQueryCacheFactory**](.\src\QueryCache\NCacheQueryCacheFactory.cs) class in your NHibernate application to spin up instances of **NCacheQueryCache**.

    [Back to top](#table-of-contents) 

#### Entity Database Dependencies Classes

The hierarchy of the classes that define the NCache NHibernate provider database dependency support for NHibernate entities are as follows:

- #### [**DependencyConfig**](.\src\Abstractions\DependencyConfig.cs)

  This is the abstract class for database dependencies on NHibernate cached entities and has the following members which are inherited by the derived classes:

  - *DependencyID* (**int**)

    This is a unique dependency ID for a given *RegionPrefix*.

  - *EntityClassFullName* (**System.String**)

    This is the ***fully qualified*** name of the NHibernate entity. For example, the sample consists of the NHibernate entity *Customers* with fully-qualified name of *Sample.CustomerService.Domain.Customers*. 

  - *RegionPrefix* (**System.String**)

     If a default region prefix is assigned in the NHibernate configuration, then this value should be set to that value. Defaults to "nhibernate".

   - *DatabaseDependencyType* ([**DatabaseDependencyType**](.\src\Abstractions\DatabaseDependencyType.cs))

     This is a read-only enum type that can take the following possible values:

     -  *Sql* - SQL Server notification-based dependency
     -  *Oracle* - Oracle notification-based dependency
     -  *Oledb* - Polling-based dependency

    - *GetCacheDependency* 

      This is an abstract method implemented in the derived classes that returns an instance of [**CacheDependency**](https://www.alachisoft.com/resources/docs/ncache/dotnet-api-ref/Alachisoft.NCache.Runtime.Dependencies.CacheDependency.html). It has the following parameters:

      - *connectionString* (**System.String**)

        This is the connection string used by NCache to connect with the database and set up the notification or polling-based dependency on the database side.

      - *key* (**System.Object**)

        This is the primary key of the entity used for cases where the database dependency is set on a specific row instead of being table-wide. If no query parameters are provided with the either notification-based or polling-based dependencies, then this is disregarded. 

        ***IMPORTANT***: Row-specific dependencies, whether notification-based or polling-based, only work with entities that have ***single*** primary keys and their type is restricted to one of the types- **System.String**, **bool**, **byte**, **sbyte**, **char**, **decimal**, **double**, **float**, **int**, **uint**, **long**, **ulong**, **short**. No such restriction is placed in case of table-wide dependencies and so they can work with entities with composite Ids.

    [Back to top](#table-of-contents) 

- #### [**CommandDependencyConfig**](.\src\Abstractions\CommandDependencyConfig.cs)

  This abstract class is derived from [**DependencyConfig**](.\src\Abstractions\DependencyConfig.cs) and contains members to set up *MS SQL Server* and *Oracle* notification-based dependency instances. In addition to the inherited members , it has the following additional members:

  - *SQLStatement* (**System.String**)

    This is the SQL Select query or name of the SQL Select stored procedure that will be registered with the database notification service.

  - *IsStoredProcedure* (**bool**)

    Determines whether the SQL statement is a Select stored procedure or not. Defaults to *false*.

  - *PrimaryKeyInputParameter* (**System.String**)

    Used in cases where row-specific dependency is to be used. For Select queries, it serves to replace the placeholder ***?*** in the query whereas for stored procedures, it should be ***exactly*** the input parameter name in the stored procedure declaration. This property is not required to be set for table-wide dependencies.

  - *PrimaryKeyDbType* (**System.String**)

    This is the database-specific Sql db type of the primary key.

  - *OutputParametersAndDbTypes* (**System.Collections.Dictionary\<System.String, System.String>**)

    This is applicable to the case where a stored procedure with output parameters is to be registered with the database notification service. The keys of the dictionary are the ***exact*** values of the parameters in the SQL procedure declaration and the values are the corresponding database-specific db types.

  [Back to top](#table-of-contents) 

- #### [**SQLDependencyConfig**](.\src\EntityDependencies\SQLDependencyConfig.cs)

  This class is derived from [**CommandDependencyConfig**](.\src\Abstractions\CommandDependencyConfig.cs) and provides the concrete implementation for SQL Server notification-based dependencies. The inherited **DatabaseDependencyType** property is set to *DatabaseDependencyType.Sql* and the overridden **GetCacheDependency** method returns an instance of [**SqlCacheDependency**](https://www.alachisoft.com/resources/docs/ncache/dotnet-api-ref/Alachisoft.NCache.Runtime.Dependencies.SqlCacheDependency.html).

  The following are the possible (***case-insensitive***) values for the SQL Server parameter db type strings used when specifying input and output parameter types:

  | Parameter String Value  | SQL Server Type  |
  |:-:|:-:|
  | "bigint"  | **BigInt**  |
  | "binary"  | **Binary**  |
  | "bit"  | **Bit**  |
  | "char"  | **Char**  |
  | "date"  | **Date**  |
  | "datetime"  | **DateTime**  |
  | "datetime2"  | **DateTime2**  |
  | "datetimeoffset"  | **DateTimeOffset**  |
  | "decimal"  | **Decimal**  |
  | "float"  | **Float**  |
  | "int"  | **Int**  |
  | "money"  | **Money**  |
  | "nchar"  | **NChar**  |
  | "nvarchar"  | **NVarChar**  |
  | "real"  | **Real**  |
  | "smalldatetime" | **SmallDateTime** |
  | "smallint"  | **SmallInt**  |
  | "smallmoney"  | **SmallMoney**  |
  | "structured" | **Structured** |
  | "time"  | **Time**  |
  | "timestamp"  | **Timestamp**  |
  | "tinyint"  | **TinyInt**  |
  | "udt"  | **Udt**  |
  | "uniqueidentifier"  | **UniqueIdentifier**  |
  | "varbinary"  | **VarBinary**  |
  | "varchar"  | **VarChar**  |
  | "variant"  | **Variant**  |
  | "xml"  | **Xml**  |
  
  [Back to top](#table-of-contents) 

- #### [**OracleDependencyConfig**](.\src\EntityDependencies\OracleDependencyConfig.cs)

  This class is derived from [**CommandDependencyConfig**](.\src\Abstractions\CommandDependencyConfig.cs) and provides the concrete implementation for Oracle notification-based dependencies. The inherited **DatabaseDependencyType** property is set to *DatabaseDependencyType.Oracle* and the overridden **GetCacheDependency** method returns an instance of [**OracleCacheDependency**](https://www.alachisoft.com/resources/docs/ncache/dotnet-api-ref/Alachisoft.NCache.Runtime.Dependencies.OracleCacheDependency.html).

  The following are the possible (***case-insensitive***) values for the Oracle parameter db type strings used when specifying input and output parameter types:

  | Parameter String Value  | SQL Server Type  |
  |:-:|:-:|
  | "bfile"  | **BFile**  |
  | "blob"  | **Blob**  |
  | "byte"  | **Byte**  |
  | "char"  | **Char**  |
  | "clob"  | **Clob**  |
  | "date"  | **Date**  |
  | "decimal"  | **Decimal**  |
  | "double"  | **Double**  |
  | "int16"  | **Int16**  |
  | "int32"  | **Int32**  |
  | "int64"  | **Int64**  |
  | "intervalids"  | **IntervalIDS**  |
  | "intervalym"  | **IntervalYM**  |
  | "long"  | **Long**  |
  | "longraw"  | **LongRaw**  |
  | "nchar" | **NChar** |
  | "nclob"  | **NClob**  |
  | "nvarchar2"  | **NVarChar2**  |
  | "raw" | **Raw** |
  | "refcursor"  | **RefCursor**  |
  | "single"  | **Single**  |
  | "timestamp"  | **TimeStamp**  |
  | "timestampltz"  | **TimeStampLTZ**  |
  | "timestamptz"  | **TimeStampTZ**  |
  | "varchar2"  | **Varchar2**  |
  | "xmltype"  | **XmlType**  |

  [Back to top](#table-of-contents) 

- #### [**OleDbDependencyConfig**](.\src\EntityDependencies\OleDbDependencyConfig.cs)
    
  This class is derived from [**DependencyConfig**](.\src\Abstractions\DependencyConfig.cs) and contains members to set up MS SQL Server and Oracle polling-based dependency instances with the inherited **DatabaseDependencyType** property is set to *DatabaseDependencyType.Oledb*. In addition to the inherited members , it has the following additional members:

  - *QualifiedTableName* (**System.String**)

    This is the ***schema-qualified*** table name on which polling dependency is to be set up.

  - *DbCacheKey* (**System.String**)

    This is the format of the ***cache-key*** field in the [*NCache polling dependency table*](https://www.alachisoft.com/resources/docs/ncache/admin-guide/set-oledb-environment.html) with which the polling dependency will be registered in the database. It ***must*** contain exactly ***one*** occurrence of the substring *[en]* which is a place holder for the *QualifiedTableName* value. 
    
    If you are using the dependency on a per-row basis, then the string must also contain ***one*** occurrence of the substring *[pk]* which is the place holder for the *primary key* value.

  - *DatabaseType* ([**DatabaseType**](.\src\Common\DatabaseType.cs))

    This is an enum type that takes the following values:

    - *Sql* - Polling dependency on SQL Server
    - *Oracle* - Polling dependency on Oracle

  The overridden **GetCacheDependency** method returns an instance of [**DBCacheDependency**](https://www.alachisoft.com/resources/docs/ncache/dotnet-api-ref/Alachisoft.NCache.Runtime.Dependencies.DBCacheDependency.html).

  [Back to top](#table-of-contents) 
        

#### Query Result Set Database Dependencies Classes

The hierarchy of the classes that define the NCache NHibernate provider database dependency support for NHibernate query results are as follows:

- #### [**QueryDependencyConfiguration**](.\src\Abstractions\QueryDependencyConfiguration.cs)

  This abstract class provides the base functionality of database dependencies on query results. Following are its members:

  - *QualifiedTableName* (**System.String**)

    The is the ***schema-qualified*** table name of an entity involved in the query results in the format in which it appears in the [generated sql query as part of the query key](https://github.com/nhibernate/nhibernate-core/blob/5.3.x/src/NHibernate/Cache/QueryKey.cs).

  - *RegionPrefix* (**System.String**)

     If a default region prefix is assigned in the NHibernate configuration, then this value should be set to that value. Defaults to "nhibernate".

  - *DatabaseType* ([**DatabaseType**](.\src\Common\DatabaseType.cs))

    If the database dependency is to be set on MS SQL Server, the value should be **DatabaseType**.*Sql* else it should be **DatabaseType**.*Oracle*. Defaults to **DatabaseType**.*Sql*.

  - *IsPollingDependencyUsed* (**bool**)

    This is a read-only value set in the derived classes that determines whether dependency is notification-based or polling-based.

  - *CreateDependency*

    This method is implemented in the derived classes and returns an instance of [**CacheDependency**](https://www.alachisoft.com/resources/docs/ncache/dotnet-api-ref/Alachisoft.NCache.Runtime.Dependencies.CacheDependency.html). It takes a single parameter *connectionString* of type **System.String** that is used to access the database and register the dependency with it. 

  [Back to top](#table-of-contents) 

- #### [**QueryCommandDependencyConfiguration**](.\src\QueryDependencies\QueryCommandDependencyConfiguration.cs)

  This class extends [**QueryDependencyConfiguration**](.\src\Abstractions\QueryDependencyConfiguration.cs) and provides functionality for notification-based database dependencies on cached query results. Consequently, the inherited property *IsPollingDependencyUsed* is set to false. Besides the inherited members, additional members of this class are as follows:

  - *KeyColumnNames* (**System.String[]**)

    This is a string array of column names of an entity in the corresponding database table as they appear in the [generated sql query as part of the query key](https://github.com/nhibernate/nhibernate-core/blob/5.3.x/src/NHibernate/Cache/QueryKey.cs). 

  The *KeyColumnNames*, *QualifiedTableName* and *DatabaseType* properties are used in the implemented *CreateDependency* method to return either an instance of [**SqlCacheDependency**](https://www.alachisoft.com/resources/docs/ncache/dotnet-api-ref/Alachisoft.NCache.Runtime.Dependencies.SqlCacheDependency.html) or [**OracleCacheDependency**](https://www.alachisoft.com/resources/docs/ncache/dotnet-api-ref/Alachisoft.NCache.Runtime.Dependencies.OracleCacheDependency.html).

  [Back to top](#table-of-contents) 

- #### [**QueryTableDependencyConfiguration**](.\src\QueryDependencies\QueryTableDependencyConfiguration.cs)

  This class extends [**QueryDependencyConfiguration**](.\src\Abstractions\QueryDependencyConfiguration.cs) and provides polling-based database dependencies on cached query results.

  The overridden *CreateDependency* method return an instance of [**DBCacheDependency**](https://www.alachisoft.com/resources/docs/ncache/dotnet-api-ref/Alachisoft.NCache.Runtime.Dependencies.DBCacheDependency.html) with the *PrimaryKey* value set to the inherited *QualifiedTableName* value and the dependency is registered in the [NCache polling dependency table](https://www.alachisoft.com/resources/docs/ncache/admin-guide/set-oledb-environment.html#step-1-create-table-in-database) with *cache_key* value set to *QualifiedTableName:ALL*.

  As such, the relevant trigger has to be present in the database to modify the *modified* field of this row to 1 post-delete and post-update. Examples of such triggers can be found [here](.\sample\DatabaseObjects\SQL\SQLPollingDependencyTriggers.cs) for SQL Server and [here](.\sample\DatabaseObjects\Oracle\OraclePollingDependencyTriggers.cs) for Oracle database.

  [Back to top](#table-of-contents)   

### Sample Demonstration

The accompanying [sample](.\sample) can be used to demonstrate NCache as an NHibernate provider. It has been developed and tested with *MS SQL Server 2017* as well as *Oracle 12c Enterprise*.

- #### Overview
  NCache as a caching provider has been introduced in the sample via the [NHibernateHelperSQL.cs](.\sample\NHibernateHelpers\NHibernateHelperSQL.cs) on line 68 and [NHibernateHelperOracle.cs](.\sample\NHibernateHelpers\NHibernateHelperOracle.cs) on line 76 as follows:

  ```csharp
  // NHibernateHelperSQL.cs
  config.Cache(cache =>
  {
    cache.DefaultExpiration = 600;
    cache.RegionsPrefix = "sql-server-db";
    cache.UseQueryCache = true;
    cache.Provider<NCacheProvider>();
    cache.QueryCacheFactory<NCacheQueryCacheFactory>();
  });

  config.Properties.Add("cache.use_sliding_expiration", "true");

  ```

  ```csharp
  // NHibernateHelperOracle.cs
  config.Cache(cache =>
  {
    cache.DefaultExpiration = 600;
    cache.RegionsPrefix = "oracle-db";
    cache.UseQueryCache = true;
    cache.Provider<NCacheProvider>();
    cache.QueryCacheFactory<NCacheQueryCacheFactory>();
  });

  config.Properties.Add("cache.use_sliding_expiration", "true");

  ```

  In the above code, **config** is an instance of the [**Configuration**](https://github.com/nhibernate/nhibernate-core/blob/5.3.x/src/NHibernate/Cfg/Configuration.cs) class and we use its **Cache** method to configure NCache as the caching provider.

  The above code snippet configures NHibernate 2nd level caching as follows:

  - The default expiration for the cache items will be 600s (10min).
  - The default cache region prefix is *sql-server-db* for the SQL Server database and is set as *oracle-db* for the Oracle database.
  - NHibernate query caching is enabled as explained [here](https://nhibernate.info/doc/nhibernate-reference/performance.html#performance-querycache).
  - The [**ICacheProvider**](https://github.com/nhibernate/nhibernate-core/blob/5.3.x/src/NHibernate/Cache/ICacheProvider.cs) is configured to be [**NCacheProvider**](./src/NCacheProvider.cs) that is used by the [**ISessionFactory**](https://nhibernate.info/doc/nhibernate-reference/architecture.html#architecture-overview) to create instances of [**NCacheClient**](./src/NCacheClient.cs).
  - The [**IQueryCacheFactory**](https://github.com/nhibernate/nhibernate-core/blob/5.3.x/src/NHibernate/Cache/IQueryCacheFactory.cs) is configured to be [**NCacheQueryCacheFactory**](./src/QueryCache/NCacheQueryCacheFactory.cs) which will be responsible for spinning up instances of the [**NCacheQueryCache**](./src/QueryCache/NCacheQueryCache.cs) to allow introducing database dependencies on cached query results.
  - Finally, we set the default cache item expiration strategy to be [**Sliding**](https://nhibernate.info/doc/nhibernate-reference/caches.html#NHibernate.Caches-howto) which NCache supports.

  [Back to top](#table-of-contents)

- #### Prerequisites

  Make sure the following requirements are met before running the application:

  * .NET Core 3.1 sdk and runtime are installed for building and running the sample.

  * *NCache* 5.0 SP2 or later Enterprise edition is available.

  * If you are using *MS SQL Server*, make sure it is version 2005 or later to use SQL Server notification-based database dependencies. Create a database and get the connection string. 
  
    Make sure you have the necessary priveleges to receive *SQL Server* notifications for dependency changes. More information about this can be found [here](https://www.alachisoft.com/resources/docs/ncache/admin-guide/set-sql-environment.html).

    Also make sure you have all the priveleges needed for using polling dependency triggers and stored procedures.

  * If you are using *Oracle*, make sure it is version 10g or later to use Oracle notification-based database dependencies. Create a database with a given user account and get the connection string.

    Make sure you have the necessary priveleges to receive *Oracle* notifications for dependency changes. To enable Oracle dependency notifications, you may run the following PL/SQL script:

    ```sql

    CONNECT sys/{your system password} as SYSDBA;
    GRANT CHANGE NOTIFICATION TO {your user};
    GRANT EXECUTE ON DBMS_CHANGE_NOTIFICATION TO {your user};
    ALTER SYSTEM SET "job_queue_processes" = 1;
    
    ```
    Also make sure you have all the priveleges needed for using polling dependency triggers and stored procedures.  

  * Create two NCache clusters *regioncache1* and *regioncache2* using the steps given [here](https://www.alachisoft.com/resources/docs/ncache/admin-guide/create-new-cache-cluster.html?tabs=windows#using-ncache-web-manager) and [run them](https://www.alachisoft.com/resources/docs/ncache/admin-guide/start-cache.html#using-ncache-web-manager).

  * In the [*client.ncconf*](.\sample\client.ncconf) file, update the *name* attribute value of *server* node to the IPv4 address of *regioncache1* as shown below:

    ```xml

      <?xml version="1.0" encoding="UTF-8"?>
      <configuration>
      <!--Change value of local-server-ip address to that of machine on which sample is running --> 
        <ncache-server 
          connection-retries="5" 
          retry-connection-delay="0" 
          retry-interval="1" 
          command-retries="3" 
          command-retry-interval="0.1" 
          client-request-timeout="90" 
          connection-timeout="5" 
          port="9800" 
          local-server-ip="yy.yy.yy.yy" 
          enable-keep-alive="False" 
          keep-alive-interval="0"/>
        <cache 
          id="regioncache1" 
          client-cache-id=""     
          client-cache-syncmode="optimistic" 
          default-readthru-provider="" 
          default-writethru-provider="" 
          load-balance="False" 
          enable-client-logs="True" 
          log-level="debug">
          <!--Change ip address below to that of one of regioncache1 servers-->    
          <server name="xx.xx.xx.xx"/> 
        </cache>
      </configuration>
 
    ```   

  * If you are using *Oracle* as your database, skip to the next step. Otherwise, if you are using *MS SQL Server*,  make the following changes:

    * In the *appSettings* section of the [*AppConfig*](.\sample\App.Config) file, change the value of the key *database_type* to ***sql***, if not already set to that value.

    * In the *connectionStrings* section of the [*AppConfig*](.\sample\App.Config) file, change the connection string information for your SQL Server database for connection string name *sql* as given below with user id that has the required adminstrative priviledges:

      ```xml

        <?xml version="1.0" encoding="UTF-8"?>
        <configuration>
          <!--Beginning configurations -->
          <connectionStrings>
            <add name="sql"
              providerName="System.Data.SqlClient"
              connectionString="{your sql server connection string here}"/>
          </connectionStrings>
          <!--Remaining configurations-->
        </configuration>
 
      ```  
    * In the *ncache* section of the [*AppConfig*](.\sample\App.Config) file, make the changes as shown below:

      ```xml

        <?xml version="1.0" encoding="UTF-8"?>
        <configuration>
          <!--Beginning configurations -->
          <ncache>
            <cache-configurations>
              <cache-configuration region-prefix="sql-server-db" cache-name="regioncache1" />
              <cache-configuration region-prefix="sql-server-db" cache-name="regioncache2" >
                <servers>
                  <!--Change the server-ip attribute value to ipv4 address of one of regioncache2 servers -->
                  <server server-ip="xx.xx.xx.xx"></server>
                </servers>
              </cache-configuration>
            </cache-configurations>
          <!--Remaining configurations-->
        </configuration>
 
      ```

    The prerequisites for testing with SQL Server are complete at this point and you can move onto the [Build and Run the Sample](#build-and-run-the-sample) section.   
  
  * For the *Oracle* database, make the following changes:

    * In the *appSettings* section of the [*AppConfig*](.\sample\App.Config) file, change the value of the key *database_type* to ***oracle***, if not already set to that value.

    * In the *connectionStrings* section of the [*AppConfig*](.\sample\App.Config) file, change the connection string information for your Oracle database for connection string name *oracle* with user id that has the required adminstrative priviledges as given below:

      ```xml

        <?xml version="1.0" encoding="UTF-8"?>
        <configuration>
          <!--Beginning configurations -->
          <connectionStrings>
            <add name="oracle"
              providerName="Oracle.ManagedDataAccess.Client"
              connectionString="{your oracle connection string here}"/>
          </connectionStrings>
          <!--Remaining configurations-->
        </configuration>
 
      ```   
  
    * Go to [*appsettings.json*](.\sample\appsettings.json) file and make the following changes:

      ```json

      // Beginning of file
      "CacheConfigs": [
          // Beginning of code
          {
            "RegionPrefix": "oracle-db",
            "CacheId": "regioncache2",
            "ServerList": [
              {
                // Change this to IPv4 address of one of the regioncache2 servers
                "Name": "xx.xx.xx.xx" 
              }
            ]
          }
        ]
      // Rest of the file
      ```
    * In the [NHibernateHelperOracle.cs](.\sample\NHibernateHelpers\NHibernateHelperOracle.cs) file, make the following change:

      ```csharp

      // Beginning code
         
         config.Mappings(map =>
         {
            map.DefaultSchema = "{The user ID as it appears in the Oracle connection string given in App.config file}";
         });

      // Rest of code

      ```

  [Back to top](#table-of-contents)

- #### Build and Run the Sample

  If the *database_type* key in the [AppConfig](.\sample\App.Config) file is set to *sql*, the logic in [NHibernateHelperSQL](.\sample\NHibernateHelpers\NHibernateHelperSQL.cs) file will be run that will create the necessary tables, triggers, stored procedures in the SQL Server database as well as enable [SQL Server Service Broker service](.\sample\DatabaseObjects\SQL\ServiceBrokerSettings.cs) to enable notification dependencies. For the polling-based dependency, the NCache polling table will also be added using the [polling table mapping information](.\sample\NCachePollingDependencyTable\NCachePollingTableMap.cs).

  Similary, if the *database_type* key in the [AppConfig](.\sample\App.Config) file is set to *oracle*, the logic in [NHibernateHelperOracle](.\sample\NHibernateHelpers\NHibernateHelperOracle.cs) will be run that creates similar triggers for polling dependency demonstration.

### Additional Resources

##### Documentation
The complete online documentation for NCache is available at:
http://www.alachisoft.com/resources/docs/#ncache


##### Programmers' Guide
The complete programmers guide of NCache is available at:
http://www.alachisoft.com/resources/docs/ncache/prog-guide/

### Technical Support

Alachisoft &copy; provides various sources of technical support. 

- Please refer to http://www.alachisoft.com/support.html to select a support resource you find suitable for your issue.
- To request additional features in the future, or if you notice any discrepancy regarding this document, please drop an email to [support@alachisoft.com](mailto:support@alachisoft.com).

### Copyrights

&copy; 2020 Alachisoft


[Back to top](#table-of-contents)


  



