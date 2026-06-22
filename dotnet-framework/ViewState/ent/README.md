# VIEW STATE CACHING

## Table of contents

* [Introduction](#introduction)
* [Prerequisites](#prerequisites)
* [Build and Run the sample](#build-and-run-the-sample)
* [Notes](#notes)
* [References](#references)
* [Additional Resources](#additional-resources)
* [Technical Support](#technical-support)
* [Copyrights](#copyrights)

## Introduction

This sample is a .NET Framework 4.8.1 WebForms application that demonstrates how to use NCache ViewState Caching to store ViewState externally instead of embedding it inside the page. By moving ViewState data from the page to NCache, the application reduces page size, improves performance, and ensures more efficient ViewState management during postbacks—especially for data-heavy controls such as GridView.

The Home Page initially displays a Grid-View showing Customers Data loaded from `northwind.xml`.
-  On first render no ViewState is stored.
- When the user selects a particuar customer, postback occurs, ViewState for the Customers GridView is retrieved from NCache and the Orders for the selected customer are filtered and displayed.
- Now when a user selcts a particular order to view OrderDetails another postback occurs, ViewState for both the Customers and Orders GridView is restored from NCache and Order details are loaded and displayed in the third GridView.

On starting the single page web application, the index page displays the Customers data displayed in rows and columns in a Grid.

![Page view](../../../../docs/assets/sample-images/ViewState-Sample-framework-ent.png)


## Prerequisites

Before the sample application is executed make sure that:

- .NET Framework 4.8.1 Developer Pack or Visual Studio (recommended) installed.
- NCache is installed and running in an accessible location.
- Ensure that `demoCache` (or another cache of your choice) is running.
	- By default, the cache name `demoCache` is configured in `Web.config`. Adjust as needed.
- NuGet package required: AspNet.ViewState.NCache (>= 5.3.6.1). The package reference is already included in the project file.
- The data source for this application has been configured to use `northwind.xml` that is present in 'App_Data' folder (present in the sample's source code). Make sure it exists.

- Create an `appbrowser.browser` file under the directory `App_Browsers` in root project folder. Now plug in page adapters in the app_browser file as follows:
    ```xml
	<browsers>
    <!-- NCache Plug page adapters in the app browser file as follows:. -->
    <browser refID="Default">
        <controlAdapters>
        <adapter controlType="System.Web.UI.Page"
        adapterType="Alachisoft.NCache.Adapters.PageAdapter">
        </controlAdapters>
    </browser>
    </browsers>
    ```

## Build and Run the Sample
    
Before building this sample, if you need to change the cache name, open `Web.config` from the project root directory and change the `cacheName` value:

```xml
<ncContentOptimization>
    	<settings enableViewstateCaching="true" enableTrace="false">
        	<cacheSettings cacheName="demoLocalCache" connectionRetryInterval="300">
            	<expiration type="None" duration="100"/>
            </cacheSettings>
        </settings>
    </ncContentOptimization>
```

From Visual Studio (Recommended):
- Open `ViewState.sln`.
- Let NuGet packages restore.
- Build and run.

From command line (optional):
```bash
# Replace <path-to-sample> with the sample location on your machine,
# e.g. d:\ncache-samples\ViewState
cd "<path-to-sample>"
nuget restore ViewState.sln
msbuild ViewState.sln /p:Configuration=Debug
ViewState\bin\Debug\ViewState.exe
```

## Notes
- If NuGet packages are not restored properly:
	- Visual Studio: Right‑click the solution → Restore NuGet Packages.
	- Command line: run `nuget restore ViewState.sln` in the sample folder (see Build and Run section).


## References

Reference documentation is available at:
https://www.alachisoft.com/resources/docs/ncache/prog-guide/view-state-caching.html


## Additional Resources

### Documentation
The complete online documentation for NCache is available at:
http://www.alachisoft.com/resources/docs/#ncache

### Programmers' Guide
The complete programmers guide of NCache is available at:
http://www.alachisoft.com/resources/docs/ncache/prog-guide/


## Technical Support

Alachisoft&copy; provides various sources of technical support. 

- Please refer to http://www.alachisoft.com/support.html to select a support resource you find suitable for your issue.
- To request additional features in the future, or if you notice any discrepancy regarding this document, please drop an email to [support@alachisoft.com](mailto:support@alachisoft.com).


## Copyrights

Copyright 2026 Alachisoft&copy;