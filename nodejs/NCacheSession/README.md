# GUESS GAME with SESSION STORE PROVIDER

### Table of contents

* [Introduction](#introduction)
* [Prerequisites](#prerequisites)
* [Build and Run the sample](#build-and-run-the-sample)
* [Additional Resources](#additional-resources)
* [Technical Support](#technical-support)
* [Copyrights](#copyrights)

### Introduction

It is a simple node.js application integrated with NCache Session Store Provider and provides a simple guessgame. 
User is asked to guess a number between 1 and 100. 
The guesses made by the user are stored in the cache and are displayed on the same web page.

### Prerequisites

Before the sample application is executed make sure that:

- NCache 5.2 or later is required
- app.config.json have been changed according to the configurations. 
	- change the cache name
- By default this sample uses 'demoCache', make sure that cache is running. 

### Build and Run the Sample
- Run command 
	- '''npm install'''
- To Run the sample application, execute following command
	- ''' node ./test/test-express.js'''

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