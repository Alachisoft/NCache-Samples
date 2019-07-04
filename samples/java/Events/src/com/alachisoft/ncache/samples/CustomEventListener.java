// ===============================================================================
// Alachisoft (R) NCache Sample Code
// NCache Events sample
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================

package com.alachisoft.ncache.samples;

import com.alachisoft.ncache.event.CustomEvent;
import com.alachisoft.ncache.event.CustomListener;

public class CustomEventListener implements CustomListener
{

    @Override
    public void customEventOccured(CustomEvent ce)
    {
        System.out.println("Object with key: " + ce.getKey() + " has raised custom event.");
    }
    
}
