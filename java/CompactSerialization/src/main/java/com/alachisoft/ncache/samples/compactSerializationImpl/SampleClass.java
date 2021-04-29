package com.alachisoft.ncache.samples.compactSerializationImpl;

import com.alachisoft.ncache.serialization.core.io.ICompactSerializable;
import com.alachisoft.ncache.serialization.core.io.NCacheObjectInput;
import com.alachisoft.ncache.serialization.core.io.NCacheObjectOutput;

import java.io.IOException;

/**
 * Compact serialization inherited objects
 * SampleClass that implements ICompactSerializable
 */
public class SampleClass implements ICompactSerializable {
    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    private String name = "SampleClass";

    @Override
    public void serialize(NCacheObjectOutput nCacheObjectOutput) throws IOException {
        nCacheObjectOutput.writeObject(name);
    }

    @Override
    public void deserialize(NCacheObjectInput nCacheObjectInput) throws IOException, ClassNotFoundException {
        Object object = nCacheObjectInput.readObject();
        name = object instanceof String ? (String)object : null;
    }
}
