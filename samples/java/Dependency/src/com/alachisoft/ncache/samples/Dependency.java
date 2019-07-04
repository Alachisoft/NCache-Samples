// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// NCache Dependency sample
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================

package com.alachisoft.ncache.samples;

public class Dependency {

    public static void main(String[] args) {

        KeyBasedDependency.runKeyBasedDependencyDemo();
        FileBasedDependency.runFileBasedDependencyDemo();

        //Notes:
        //Oracle Dependency is only available for Oracle database 10g release 2 or later.
        //OracleDependency requires oracle instance.        
        //Make sure to specify your own connection string 
        //in OracleDependency.runOracleDependencyDemo() method.
        
        //OracleDependency.runOracleDependencyDemo();  --Uncomment to run
        //DBDependency.runDBDependencyDemo(); //uncomment to run 
        
        System.exit(0);
    }
}
