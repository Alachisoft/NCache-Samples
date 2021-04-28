# NCache JSP Sessions

### Table of contents

* [Introduction](#introduction)
* [Prerequisites](#prerequisites)
* [HOW TO CONFIGURE](#HOW TO CONFIGURE)
* [Build and Run the sample](#build-and-run-the-sample)
* [Additional Resources](#additional-resources)
* [Technical Support](#technical-support)
* [Copyrights](#copyrights)

### Introduction

This sample program demonstrates the usage of NCache with JSP Sessions.
          
### PREREQUISITES

Before deploying the war file please make sure that:   
	Files session.xml and client.ncconf must pe present at the specified config path.
	Caches specified in session.xml must exist in the 'client.ncconf' with correct 
	server ip address and these caches should be started.          

### HOW TO CONFIGURE
    
To ensure the correct caching of sessions, you must apply this filter as the
first filter in your deployment descriptor. Filters are executed in the 
order they are defined in deployment descriptor. To create a filter, you must 
define it first under the <filter> tag and then provide a URL mapping for the 
filter using <filter-mapping> tag. The following filter configuration means that 
the filter will be applied to all the URLs in a web application. 

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


### Build and Run the sample

Follow these steps to build the war file:
	Run the following command: mvn package
	Or open your project in your favourite IDE and build using maven.
	guessgame.war will be created in the target folder.

	  
#### Application Deployment

Use appropriate sample depending upon the api supported by the web server.(i.e. javax.* or jakarta.*).
For Example: TOMCAT 10 onwards please refer to our sessions_jakarta sample.

#### TOMCAT

Note: Following instructions are used for Tomcat version 9.0.26. Steps remain the same but 
the location of the files might change depending upon the version of the tomcat.

	- Stop server.

	- NCache uses 'log4j' for logging. Place the 'log4j' at the following location. 
      $CATALINA_HOME/lib
      $CATALINA_HOME is the location where Tomcat is installed.

	- Create the 'log4j.properties' file with the following contents.
   
	  log4j.rootLogger=DEBUG, R 
	  log4j.appender.R=org.apache.log4j.RollingFileAppender 
	  log4j.appender.R.File=${catalina.home}/logs/nc-web.guessgame.log
	  log4j.appender.R.MaxFileSize=10MB 
	  log4j.appender.R.MaxBackupIndex=10 
      log4j.appender.R.layout=org.apache.log4j.PatternLayout   

	- Place 'log4j.properties' in the following location.
      $CATALINA_HOME/lib
      $CATALINA_HOME is the location where Tomcat is installed.

	- Download the 'commons-logging-x.x.x-bin.zip' from http://commons.apache.org/logging/download_logging.cgi, extract it and copy the 
      commons-logging-x.x.x.jar file into the following location.
      $CATALINA_HOME/lib
      $CATALINA_HOME is the location where Tomcat is installed.

	- Restart server.

	- Deploy the 'guessgame.war' file, it must be copied into the correct
	  directory so Tomcat can find it.  
	  Move the file to the directory $CATALINA_HOME/webapps.
      "$CATALINA_HOME" is the location where Tomcat is installed.  

	- Open the web browser and try to access the following url: 
      http://localhost:8080/GuessGame/index.jsp (specify correct host and port 
	  according to the system specifications).
      Where 'host' will be the address/IP of the machine where Tomcat is installed in case 
	  of local machine it should be 'localhost' and 'port' where Tomcat is listening the request. 
	  Make sure that you are using the correct port. The default port for Tomcat is 8080.

	  
#### WildFly

	- NCache uses 'log4j' for logging. WildFly by default uses 'log4j' for logging so no need to place it.

	- Deploy the 'guessgame.war' file, it must be copied into the correct directory so WildFly can find it.  
      Move the file to the directory $WildFly_HOME/server/default/deploy.
	  "$WildFly_HOME" is the location where WildFly is installed. 
								OR
					Deploy using WildFly admin console.	

	- Open the web browser and try to access the following url: 
      http://localhost:8080/GuessGame/index.jsp (specify correct host and port 
	  according to the system specifications). Where 'host' will be the address/IP of
	  the machine where WildFly is installed in case of local machine it should be 'localhost' 
	  and 'port' where WildFly is listening the request. Make sure that you are using 
	  the correct port. The default port for WildFly is 8080.


#### WEBLOGIC

	- NCache uses 'log4j' for logging. Place the 'log4j' at the following location and restart server: 
      $WEBLOGIC_HOME/user_projects/domains/DOMAIN_NAME/lib
      $WEBLOGIC_HOME is the location where WebLogic is installed.
      DOMAIN_NAME is the domain already created.

	- Deploy the 'guessgame.war' file, it must be copied into the correct directory so 
	  WebLogic can find it.  Move the file to the directory 
      $WEBLOGIC_HOME/user_projects/domains/DOMAIN_NAME/autodeploy
      $WEBLOGIC_HOME is the location where WebLogic is installed.  
      DOMAIN_NAME is the domain already created.
									OR
					Deploy using WEBLOGIC admin console.	
 
	- Open the web browser and try to access the following url: 
      http://localhost:7001/GuessGame/index.jsp (specify correct host and port
	  according to the system specifications). Where 'host' will be the address/IP of 
	  the machine where WebLogic is installed in case of local machine it should be 'localhost'.
      and 'port' where WebLogic is listening the request. Make sure that you are using the correct port. 
      The default port for WebLogic is 7001.

	  
#### WEBSPHERE

	- NCache uses 'log4j' for logging. Place the 'log4j' at the following location and restart server: 
      $WEBSPHERE_HOME/AppServer/lib/ext
      $WEBSPHERE_HOME is the location where WebSphere is installed.

    - If WebSphere is installed properly, you will be able to access the admin console through 
	  browser with the help of the following url: http://ip-address:port/ibm/console/ 
      By default, 9060 is the port used by WAS. Make sure that your are using the 
	  correct port if you changed it during installation.

    - You can go to Applications -> New Application -> New Enterprise Application.

    - Specify the path of the 'GuessGame.war' file and click Next.

	- Select 'Fast Path' installation mode and click Next.

	- Specify the application name and application directory and click Next. 
	  Both application name and application directory should be 'GuessGame'.

	- Map the application to the server, Select 'GuessGame.war' from the list 
	  with the default settings and click Next.

	- Map the application to the virtual host, Select 'GuessGame.war' from the 
	  list with the default settings and click Next.

	- Map the context root to the application, type 'GuessGame' into the textbox 
	  resulting in '/GuessGame' as the contents of the textbox 
      and click Next.

	- Click the Finish button to complete the deployment.

	- Go to Applications -> Application Types -> WebSphere enterprise applications.

	- You should see the GuessGame application in the list, select the GuessGame 
	  application by clicking on the checkbox and press the start button at the 
	  top left, you should see a green icon showing that application has started.

	- Open the web browser and try to access the following url: 
      http://localhost:9080/GuessGame/index.jsp (specify correct host and 
	  port according to the system specifications).
      Where 'host' will be the address/IP of the machine where WebSphere is installed 
	  in case of local machine it should be 'localhost' and 'port' where WebSphere is 
	  listening the request. Make sure that you are using the correct port. 
      The default port for WebSphere is 9080.

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
	  