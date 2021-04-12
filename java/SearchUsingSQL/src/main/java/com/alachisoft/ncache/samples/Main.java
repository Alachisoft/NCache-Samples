package com.alachisoft.ncache.samples;

public class Main {
    public static void main(String[] args) throws Exception {
        try {
            SearchUsingSQL.run();
        }catch (Exception e){
            System.out.println(e.getMessage());
        }
    }
}
