/*
 * ===============================================================================
 * Alachisoft (R) NCache Sample Code.
 * NCache Basic Operations sample
 * ===============================================================================
 * Copyright © Alachisoft.  All rights reserved.
 * THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
 * OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
 * LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
 * FITNESS FOR A PARTICULAR PURPOSE.
 * ===============================================================================
 */

package org.alachisoft.sessions_spring;

import com.alachisoft.ncache.spring.sessions.config.annotation.web.http.EnableNCacheHttpSession;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;

/*
 * ===============================================================================
 * Main entry point for the Spring Boot application.
 *
 * This application demonstrates the use of NCache-backed HTTP sessions
 * by enabling NCache as the session store provider through the
 * {EnableNCacheHttpSession} annotation.
 * Spring Session simplifies session management in distributed
 * environments by decoupling session state from the web container.
 * It allows multiple instances of a web application to share the same session data,
 * enabling true horizontal scalability.
 *
 * NCache Spring Session provides high-performance session storage, clustering, and persistence.
 * By integrating Spring Session with NCache, this application demonstrates how
 * HTTP session data can be persisted and retained outside the application's context.
 * You can get and set session attributes using session.get() / session.set(). NCache handles
 * storing the session itself.
 * ===============================================================================
 */
@SpringBootApplication
@EnableNCacheHttpSession(cacheName = "demoCache")
public class SessionsSpringApplication {

    /**
     * Starts the Spring Boot application.
     *
     * @param args command-line arguments passed to the application
     */
    public static void main(String[] args) {
        SpringApplication.run(SessionsSpringApplication.class, args);
    }
}
