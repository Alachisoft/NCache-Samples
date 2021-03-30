
NCache Software Development Kit (SDK) for NCache Enterprise Edition

Introduction:
This NCache Enterprise SDK targets DotNet Framework and DotNet Core. It allows you to use NCache Client on different platforms including Windows and Linux OS.

Application development:

                => NCache Enterprise SDK adds reference to NCache libraries and other dependencies.

**A guide to use NCache API can be viewed at: http://www.alachisoft.com/resources/docs/#ncache

This nuget package copies Client.ncconf and Config.ncconf to local directory of the application. NCache tries to read cache information from local configs first. If the required information is not found from local configs, it reads that information from configs inside NCache installation directory.
If NCache is not installed on the machine where application is running, please make sure that all required information is given in these local configs.

Purpose of Client.ncconf
	-Client.ncconf contains the information about the cache servers of each cache application needs to access.

Purpose of Config.ncconf
	-Config.ncconf that is copied locally is used for local inproc cache and inproc client caches. For client cache, config.ncconf must also contain the name for the clustered cache which this client cache is a part of.

In case of .NetCore projects, to run using visual studio follow these steps:
 - After editing the .ncconf file shown in the explorer as required.
 - Build your solution.
 - Copy .ncconf file(s) from bin folder to the project directory.

There is no need to follow the above steps in case you publish your project and run it through dotnet command.