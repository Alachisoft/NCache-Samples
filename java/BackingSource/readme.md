# BACKING SOURCE

### Table of contents

* [Introduction](#introduction)
* [Prerequisites](#prerequisites)
* [Build and Run the sample](#build-and-run-the-sample)
* [Additional Resources](#additional-resources)
* [Technical Support](#technical-support)
* [Copyrights](#copyrights)

### Introduction

- This sample explains how NCache keeps the cached items synchronized with the database, and how NCache silently 
    loads the expired items from the datasource using its 'ReadThru' feature and similarly silently updates 
    items using its 'WriteThru' feature.

- This sample deals with the following features of NCache.
	i)	 ReadThru
	ii)	 WriteThru
	
This sample uses SampleData project as a reference for model class "Customer".

### Prerequisites

Before the sample application executed make sure that:

- **config.properties** have been changed according to the configurations. 
	- change the cache name
- ByDefault this sample uses 'demoCache', make sure the cache is running. 
- Oracle database must be configured.
- Database UserName, password and ConnectionString must be configured in config.properties.
- For backingSource add Oracle database credentials in web manager while registering backing source.
- Build the sample as guided in build step, from the root of the java samples using command :
	``` - mvn clean package ```
- Before running this sample make sure backing source is enable and following providers registered.
	For Read Thru
		SqlReadThruProvider
	For Write Thru
		SqlWriteThruProvider
		
- To enable and register backing source,
	- Start NCache Web Manager and create a clustered cache with the name specified in config.properties. 
	- Click on the **view details** of the cache in web manager.
	- Scroll down to Advanced settings.
	- Now select the 'Backing Source' tab in the "Advanced Settings" of cache's details page. 
	- To enable Read Through Backing Source,
		- Click the checkbox labelled "Enable Read Through". Click on "Add Provider" button next to this checkbox.
		- Provide a unique provider name ("SqlReadThruProvider", for example).
		- Click on "Browse" button for library field. Select library "BackingSource.jar".
		- Select class "com.alachisoft.ncache.samples.ReadThru" from the now populated drop down list.
		- Specify connection string as 'connString' parameter for database that specified in config.properties. 
		- Specify user name string as 'user' parameter for database that specified in config.properties. 
		- Specify password string as 'pass' parameter for database that specified in config.properties. 
	- Similarly, to enable Write Thru backing source, follow the same steps as above. 
	    Choose "com.alachisoft.ncache.samples.WriteThru" from the class drop down list.
	- Backing source provider files need to be deployed.
		- Click 'Deploy Backing Source Provider' to deploy backing source. 
		- Locate and select 'BackingSource.jar'.
	- Click 'Ok' and save changes.

### Build and Run the Sample
- Run the following commands:
    ``` mvn clean package ```
- Open your project in your favourite compiler and run the sample application.
- OR 
	- To run with java: 
	- ``` cd target ```
	- ``` java -cp target/* com.alachisoft.ncache.samples.Main ```

### Additional Resources

##### Documentation
The complete online documentation for NCache is available at:
http://www.alachisoft.com/resources/docs/#ncache

##### Programmers' Guide
The complete programmers guide of NCache is available at:
http://www.alachisoft.com/resources/docs/ncache/prog-guide/

### Technical Support

Alachisoft [C] provides various sources of technical support. 

- Please refer to http://www.alachisoft.com/support.html to select a support resource you find suitable for 
    your issue.
- To request additional features in the future, or if you notice any discrepancy regarding this document, 
    please drop an email to [support@alachisoft.com](mailto:support@alachisoft.com).

### Copyrights

[C] Copyright 2021 Alachisoft 