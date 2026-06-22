# NCache JSP Sessions

### Table of contents

* [Introduction](#introduction)
* [Prerequisites](#prerequisites)
* [How To Configure](#how-to-configure)
* [Build and Run the sample](#build-and-run-the-sample)
* [Additional Resources](#additional-resources)
* [Technical Support](#technical-support)
* [Copyrights](#copyrights)


### Introduction

This sample program demonstrates the usage of NCache with JSP Sessions.

### PREREQUISITES

Before deploying the war file please make sure that:
	
  - Files session.xml and client.ncconf must pe present at the specified config path.
  - Caches specified in session.xml must exist in the 'client.ncconf' with correct server ip address and these caches should be started.

### How To Configure
 
To ensure the correct caching of sessions, you must apply this filter as the
first filter in your deployment descriptor. Filters are executed in the
order they are defined; therefore, this filter must appear before any other filters.
```
   <filter>
    <filter-name>NCacheSessionProvider</filter-name>
    <filter-class>com.alachisoft.ncache.web.session.NCacheSessionProvider</filter-class>
   </filter>
   <filter-mapping>
        <filter-name>NCacheSessionProvider</filter-name>
        <url-pattern>/*</url-pattern>
   </filter-mapping>

   NCache session filter should be configured with following parameter.
   <init-param>
            <description>configPath is used to load the client.ncconf and session.xml files.</description>
            <param-name>configPath</param-name>
            <param-value>config</param-value>
   </init-param>
```
- The configPath must be updated to point to the actual NCache config directory where the files exist
i.e NCacheHome/config
- The configuration files themselves are already provided and do not need to be recreated.

## Build and Run the sample

Download tomcat https://tomcat.apache.org/download-90.cgi#9.0.113

Follow these steps to build the war file:

- Run the following command from terminal while in the SessionsJakarta directory:


**For Windows/Linux/macOS:**
```bash
mvn package
```
- Or open your project in your favourite IDE and build using maven.
- guessgame.war will be created in the target folder.

### Running from IntelliJ IDEA (Recommended)

Steps:

- Open the project in IntelliJ IDEA
- Go to Run → Edit Configurations
- Add a new Tomcat Server (Local) configuration
- Select your Tomcat installation
- Navigate to the Deployment tab:
- Add the WAR artifact (exploded or packaged)
- Set the context path (for example: /guessgame)
- Apply and start the server using the Run button

**Note**: You may need an IntelliJ ultimate subscription in order to setup 
a Tomcat server configuration

### Running from TOMCAT

Note: Following instructions are used for Tomcat version 9.

  - NCache uses 'log4j' for logging.  Create the file 'log4j.properties' and place it at the following location: 
      $CATALINA_HOME/lib
     ----- $CATALINA_HOME is the location where Tomcat is installed. Insert the following content:


  ```properties
status = warn

appender.console.type = Console
appender.console.name = Console
appender.console.layout.type = PatternLayout
appender.console.layout.pattern = %d{yyyy-MM-dd HH:mm:ss} %-5p %c - %m%n

rootLogger.level = info
rootLogger.appenderRefs = console
rootLogger.appenderRef.console.ref = Console
```

- Restart server.

- Deploy the 'guessgame.war' file, it must be copied into the correct
  directory so Tomcat can find it.  
  Move the file to the directory $CATALINA_HOME/webapps.

- Navigate to the bin folder and execute the script startup.sh:

**For Windows**
 ```bash
startup.bat
```

**For Linux/Mac**
 ```bash
  chmod +x startup.sh 
./startup.sh
```


  - Open the web browser and access the following url: 
    http://localhost:8080/GuessGame/index.jsp (specify correct host and port 
	according to the system specifications).
    Where 'host' will be the address/IP of the machine where Tomcat is installed in case 
	of local machine it should be 'localhost' and 'port' where Tomcat is listening the request. 
	Make sure that you are using the correct port. The default port for Tomcat is 8080.


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

[C] Copyright 2026 Alachisoft 