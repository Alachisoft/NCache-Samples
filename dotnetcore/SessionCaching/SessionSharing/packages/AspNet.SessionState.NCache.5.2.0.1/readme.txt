AspNet.SessionState.NCache Provider for NCache Enterprise Edition

Introduction:
This package provides assemblies for using NCache Session State Provider.

This nuget package copies Client.ncconf and Config.ncconf to local directory of the application. NCache tries to read cache information from local configs first. If the required information is not found from local configs, it reads that information from configs inside NCache installation directory.
If NCache is not installed on the machine where application is running, please make sure that all required information is given in these local configs.

Purpose of Client.ncconf
	-Client.ncconf contains the information about the cache servers of each cache application needs to access.

Purpose of Config.ncconf
	-Config.ncconf that is copied locally is used for local inproc cache and inproc client caches. For client cache, config.ncconf must also contain the name for the clustered cache which this client cache is a part of.
	
Application development:

                => AspNet.SessionState.NCache Provider package adds reference to NCache Session State Provider assembly.
				=> AspNet.SessionState.NCache Provider transforms web.config file and add sessionState tag with Custom NCacheSessionProvider,For basic function, the user needs to specify a name for a preconfigured cache in the "cache-name" field. By default, "mycache" is specified.

**A guide to use NCache API can be viewed at: http://www.alachisoft.com/resources/docs/#ncache

