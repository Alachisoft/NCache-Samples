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

package com.alachisoft.ncache.sample;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.boot.autoconfigure.cache.JCacheManagerCustomizer;
import org.springframework.cache.annotation.EnableCaching;

/*
 * ===============================================================================
 * NCache allows you to plug itself into user Spring applications with minimal
 * changes to the program itself. In this, users can take advantage of the
 * features provided by NCache in this Spring application.
 *
 * The spring application caches Product data into the cache via Spring's caching
 * abstractions:
 *   1. @Cacheable is used to fetch Product data from the cache if available;
 *      otherwise, data is retrieved from the repository and stored in the cache.
 *   2. @CachePut is used to update the cache whenever Product data is added or
 *      modified in the repository.
 *   3. @CacheEvict is used to remove Product data from the cache when it is
 *      deleted, ensuring cache consistency.
 *
 * This sample demonstrates how NCache works as a distributed cache provider
 * through Spring's cache abstraction without requiring direct cache API calls
 * in application business logic.
 * ===============================================================================
 */

@SpringBootApplication
@EnableCaching
public class SpringSample {
    public static void main(String[] args) {
        SpringApplication.run(SpringSample.class, args);
    }
}

