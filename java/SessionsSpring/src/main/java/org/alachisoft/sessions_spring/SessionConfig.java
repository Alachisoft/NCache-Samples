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

import com.alachisoft.ncache.spring.NCacheCacheManager;
import com.alachisoft.ncache.spring.configuration.SpringConfigurationManager;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import java.nio.file.Path;

/**
 * Spring configuration class responsible for setting up
 * NCache integration within the application context.
 */
@Configuration
public class SessionConfig {

    /**
     * Creates and configures the {@link NCacheCacheManager} bean.
     * <p>
     * The cache manager is initialized using NCache's
     * Spring configuration file, which defines cache-related
     * settings.
     * </p>
     *
     * @return a configured {@link NCacheCacheManager} instance
     */
    @Bean
    public NCacheCacheManager cacheManager() {
        String resource =
                Path.of("C:/Program Files/NCache/config/ncache-spring.xml").toString();

        SpringConfigurationManager springConfigurationManager =
                new SpringConfigurationManager();
        springConfigurationManager.setConfigFile(resource);

        NCacheCacheManager cacheManager = new NCacheCacheManager();
        cacheManager.setSpringConfigurationManager(springConfigurationManager);

        return cacheManager;
    }
}

