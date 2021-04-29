// ===============================================================================
// Alachisoft (R) NCache Sample Code
// NCache Product Class used by java.com.alachisoft.ncache.samples
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
package cachekeygenerator;

import java.lang.reflect.Method;
import org.springframework.cache.interceptor.KeyGenerator;

public class NCacheCollectionKeyGenerator implements KeyGenerator{

    public Object generate(Object target, Method method, Object... params) {
        String key=method.getName();
        for(Object param: params)
        {
            key=key+":"+param.hashCode();
        }
        return key;  
    }
    
}
