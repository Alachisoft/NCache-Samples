package com.alachisoft.ncache.samples;

import com.alachisoft.ncache.samples.cacheLoaderUsage.RefresherUsage;

public class Main {

    public static void main(String[] args) {
        try {
            RefresherUsage.run();
        } catch (Exception e) {
            System.out.println(e.getMessage());
        }
    }
}
