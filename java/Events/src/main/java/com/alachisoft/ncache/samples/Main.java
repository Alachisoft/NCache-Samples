package com.alachisoft.ncache.samples;

public class Main {
    public static void main(String[] args) {
        try {
            Events.run();
        } catch (Exception e) {
            System.out.println(e.getMessage());
            System.exit(0);
        }
    }
}
