// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// NCache Customer Class used by samples
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================

package com.alachisoft.ncache.samples.data;

import java.io.Serializable;

public class Customer implements Serializable
{
    public Customer(String name, int age, String contactNo, String address) {
        this.name = name;
        this.age = age;
        this.contactNo = contactNo;
        this.address = address;
    }

    private String name;
    private int age;
    public String contactNo;
    public String address;

    public Customer() {

    }

    public void setName(String name){
        this.name = name;
    }

    public String getName(){
        return this.name;
    }

    public void setAge(int age) {
        this.age = age;
    }

    public int getAge() {
        return this.age;
    }

    public void setContactNo(String contactNo) {
        this.contactNo = contactNo;
    }

    public String getContactNo() {
        return this.contactNo;
    }

    public void setAddress(String address) {
        this.address = address;
    }

    public String getAddress() {
        return this.address;
    }
}
