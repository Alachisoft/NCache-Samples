package com.alachisoft.ncache.springbootsample;

import org.springframework.boot.autoconfigure.cache.JCacheManagerCustomizer;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

import javax.cache.CacheManager;
import javax.cache.configuration.MutableConfiguration;
import javax.cache.expiry.CreatedExpiryPolicy;
import javax.cache.expiry.Duration;

@Configuration
public class CacheConfiguration implements JCacheManagerCustomizer {
    @Override
    public void customize(CacheManager cacheManager) {
        MutableConfiguration mutableConfiguration = new MutableConfiguration();
        mutableConfiguration.setExpiryPolicyFactory(CreatedExpiryPolicy.factoryOf(Duration.ONE_MINUTE));
        cacheManager.createCache("demoCache", mutableConfiguration);
    }
}
