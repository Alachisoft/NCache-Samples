# JAVA SAMPLES

### Table of contents

* [Introduction](#introduction)
* [Startup Guide](#startupguide)
* [Run Sample](#runsample)

### Introduction
NCache provides 100% native Java clients for both UNIX and Windows. You can use NCache from any of Java applications including Java Web Sessions, web services, Grid Computing applications, and any other server-type applications with high transactions.

NCache Java client uses the same proprietary socket-level protocol to talk to the cache server that the .NET client does. So, the performance and scalability of Java client is the same as .NET client.
For more details go to this link: https://www.alachisoft.com/ncache/java-support.html

### Startup Guide

#### To Compile and Run via NetBeans:

    - Open Netbeans IDE.
    - Select File->Open Project
	- For more information follow this guide: http://netbeans.apache.org/kb/docs/java/quickstart.html

#### To Compile and Run via Eclipse:
	- Open File->Import
	- Select "Existing Projects into Workspace" from the Selection Wizard
	- Select Next to get the Import Wizzard. Browse to find the location of the Project
	- For more information follow this guide: http://people.cs.uchicago.edu/~kaharris/10200/tutorials/eclipse/import.html
	
#### To Compile and Run via Eclipse:
	- Launch IntelliJ IDEA.
	- If the Welcome screen opens, click Open.
	- Otherwise, from the main menu, select File | Open.
	- In the dialog that opens, select the directory in which your sources, libraries, and other assets are located and click Open.
	- For more information follow this guide: https://www.jetbrains.com/help/idea/import-project-or-module-wizard.html#open-project

### Run Sample:
	- From root of the samples run the following commands:
	- To compile all projects:
		- ``` mvn clean package ```
	- To compile single project:
		- ``` mvn clean package -pl BasicOperations -am ```
	- To run single project:
		- ``` java -cp BasicOperations/target/* com.alachisoft.ncache.samples.Main ```
	- OR
		- ``` cd BasicOperations ```
		- ``` mvn exec:java -Dexec.mainClass=com.alachisoft.ncache.samples.Main ```










