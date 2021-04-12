package com.Alachisoft.NCache.Samples;

public class Main {

    public static void main(String[] args) {
       try{
           BulkCustomDependencyUsage.run();
       }catch (Exception e)
       {
           System.out.println(e.getMessage());
       }
    }
}
