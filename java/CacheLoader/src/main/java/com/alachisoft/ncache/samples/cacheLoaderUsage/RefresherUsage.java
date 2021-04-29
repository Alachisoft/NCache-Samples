package com.alachisoft.ncache.samples.cacheLoaderUsage;

import com.alachisoft.ncache.samples.cacheLoaderImpl.Refresher;

import java.io.*;
import java.util.HashMap;
import java.util.Map;
import java.util.Properties;

public class RefresherUsage {
   // private static Cache _cache;

    public static void run() throws Exception {
        // Initialize cache
        Properties properties = getProperties();
        // Initialize cache
        String cacheName = properties.getProperty("CacheID");
        String connString = properties.getProperty("connString");
        String user = properties.getProperty("user");
        String pass = properties.getProperty("pass");

        //initializeCache(cacheName);

        Map<String, String> parameters = new HashMap<>();
        parameters.put("ConnectionString", connString);
        parameters.put("User", user);
        parameters.put("Password", pass);
        Refresher refresher = new  Refresher();
        refresher.init(parameters, cacheName);
        refresher.loadDatasetOnStartup("products");
    }

    /**
     This method returns property file
     */
    private static Properties getProperties() throws IOException
    {
        String path = "config.properties";
        InputStream inputStream = RefresherUsage.class.getClassLoader().getResourceAsStream(path);
        Properties properties=new Properties();
        if (inputStream != null) {
            properties.load(inputStream);
        } else {
            throw new FileNotFoundException("property file '" + path + "' not found in the classpath");
        }
        return properties;
    }
}
