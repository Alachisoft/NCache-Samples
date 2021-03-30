# SESSION SHARING

### Table of contents

* [Introduction](#introduction)
* [Prerequisites](#prerequisites)
* [Build and Run the sample](#build-and-run-the-sample)
* [Additional Resources](#additional-resources)
* [Technical Support](#technical-support)
* [Copyrights](#copyrights)

### Introduction

This is a sample of sharing session betweem ASP.NET Core and ASP.NET web application that uses the NCache Session Services to store session information. 

It uses a partitioned cache named, 'myPartitionedCache', which is registered on your machine upon the installation of NCache. 

To start or re-register this cache later, you can use NCache Web Manager or NCache command line tools.


### Prerequisites

Requirements:

- Visual Studio 2017 or later.
- .net core  2.2 or later.

Before the sample application is executed make sure that:
- Both applications have the same application name and cookie name in appsettings.json in order to share the same session.
- In the configuration files of both application "enableSessionSharing" flag is added and is set to "true".
- appsettings.json have been changed according to the configurations. 
	- Change the cache name if needed
- By default this sample uses 'myPartitionedCache', make sure that cache is running. 

### Build and Run the Sample
 
- Build both web applications provided in the solution.
- One is an asp.net website and the other is an asp.net core web mvc application, run both the instances. 
- Both applications host a guess game where user has to guess a randomly generated secret number.
- Whichever web application makes the first request stores the secret number in a commonly shared session.
- All the attempts made to guess the secret are stored in a list which is stored in a commonly shared session and is displayed on the webpage.
- On swiching between the web applications, each request displays the history of attempts as a single list.
- The second web application will have the same secret number to be guessed and the history of attempts will be common to both the applications using a shared session.
- If user guesses the right number in either of the application then the other application on refresh would result into a new game. 

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