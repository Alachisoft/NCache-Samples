// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================

package com.alachisoft.ncache.samples;
/// ******************************************************************************
/// <summary>
/// A sample program that demonstrates how to utilize the synchronous CRUD api in NCache.
///
/// Requirements:
///     1. A running NCache cache
///     2. Connection attributes in config.properties
/// </summary>

public class Main {
    public static void main(String[] args)
    {
        try
        {
            BasicOperationsWithJSON.run();
        }
        catch (Exception exception)
        {
            System.out.println(exception.getMessage());
        }
    }

}
