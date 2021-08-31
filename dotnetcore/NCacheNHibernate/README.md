# NCache NHibernate Second-Level Cache Provider

## [Table of contents](#table-of-contents)

* [Introduction](#introduction)
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