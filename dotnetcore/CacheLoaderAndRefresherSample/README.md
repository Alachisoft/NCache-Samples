# Cache Loader And REFRESHER SAMPLE

### Table of contents

* [Introduction](#introduction)
* [Prerequisites](#prerequisites)
* [Build and Run the sample](#build-and-run-the-sample)
* [Additional Resources](#additional-resources)
* [Technical Support](#technical-support)
* [Copyrights](#copyrights)

### Introduction

NCache provides you with Cache Startup Loader, a facility of adding the specified data in the cache on cache startup. 
It means every time the cache will be started the data will be loaded in the cache without the user, manually adding it. 
NCache also provides support of Cache Refresher that refreshes this loaded data after a certain period of time to ensure 
that the data loaded on startup does not go stale.

This sample uses SampleData project as a reference for model class "Product" "Supplier".

### Prerequisites

Before the sample application is executed make sure that:

- Execute the NorthWind.sql script attached with the sample.
- This project is a ClassLibrary and should be deployed in the NCache WebManager to make it functional.
- To deploy the RefresherSample.dll follow the following steps:
    - Open web manager, select the "View Details" option with the cache.
    - Stop the cache if running.
    - Scroll down to Advanced Settings.
    - Click on "CacheLoader / Refresher".
    - Click on the check box Enable Cache Loader and Refresher.
    - Click on Browse and find the assemble RefresherSample.dll, it will be in bin/Debug/netstandard2.0.
    - Select Refresher class in Class Name option.
    - Add parameters for database connection. 
        - ConnectionString: the connection string for the database.
    - Add dataset in DataSets options. The unique identifires for the different kind of data you want to fetch from database.
    - Add the following datasets for the RefresherSample and configure their schedule to refresh:
        - products
        - suppliers
    - In the end deploy RefresherSample.dll by clicking on the Deploy Providers and select the dll.
- By default this sample uses 'demoCache', make sure that cache is running. 

### Build and Run the Sample
    
- Run the sample application.
- Now run the Sql UpdateScript shipped with sample.
- Run the sample again after the configured scheduled refresh time and observe the output.

### Additional Resources

##### Documentation
The complete online documentation for NCache is available at:
http://www.alachisoft.com/resources/docs/#ncache

##### Programmers' Guide
The complete programmers guide of NCache is available at:
http://www.alachisoft.com/resources/docs/ncache/prog-guide/

### Technical Support

Alachisoft [C] provides various sources of technical support. 

- Please refer to http://www.alachisoft.com/support.html to select a support resource you find suitable for your issue.
- To request additional features in the future, or if you notice any discrepancy regarding this document, please drop an email to [support@alachisoft.com](mailto:support@alachisoft.com).

### Copyrights

[C] Copyright 2021 Alachisoft 