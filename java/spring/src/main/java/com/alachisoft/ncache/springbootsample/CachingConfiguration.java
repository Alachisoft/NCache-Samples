package com.alachisoft.ncache.springbootsample;

import com.alachisoft.ncache.spring.NCacheCacheManager;
import com.alachisoft.ncache.spring.configuration.SpringConfigurationManager;
import org.springframework.cache.CacheManager;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

import java.net.URL;

@Configuration
public class CachingConfiguration
{
    @Bean
    public CacheManager cacheManager()
    {
        SpringConfigurationManager springConfigurationManager = new SpringConfigurationManager();
        URL resource = getClass().getClassLoader().getResource("ncache-spring.xml");
        springConfigurationManager.setConfigFile(resource.getPath());
        NCacheCacheManager cacheManager = new NCacheCacheManager();
        cacheManager.setSpringConfigurationManager(springConfigurationManager);
        return cacheManager;
    }
}
