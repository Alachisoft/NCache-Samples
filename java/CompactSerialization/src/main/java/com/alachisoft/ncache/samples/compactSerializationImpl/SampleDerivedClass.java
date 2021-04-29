package com.alachisoft.ncache.samples.compactSerializationImpl;

import com.alachisoft.ncache.serialization.core.io.ICompactSerializable;
import com.alachisoft.ncache.serialization.core.io.NCacheObjectInput;
import com.alachisoft.ncache.serialization.core.io.NCacheObjectOutput;

import java.io.IOException;
import java.time.LocalDate;

/**
 * SampleDerivedClass derived from SampleClass
 */
public class SampleDerivedClass extends SampleClass implements ICompactSerializable {

    private int value = 200;
    private LocalDate date = LocalDate.now();

    public int getValue() {
        return value;
    }

    public void setValue(int value) {
        this.value = value;
    }

    public LocalDate getDate() {
        return date;
    }

    public void setDate(LocalDate date) {
        this.date = date;
    }

    @Override
    public void serialize(NCacheObjectOutput nCacheObjectOutput) throws IOException {
        super.serialize(nCacheObjectOutput);
        nCacheObjectOutput.writeInt(value);
        nCacheObjectOutput.writeObject(date);
    }

    @Override
    public void deserialize(NCacheObjectInput nCacheObjectInput) throws IOException, ClassNotFoundException {
        super.deserialize(nCacheObjectInput);
        value = nCacheObjectInput.readInt();
        Object object = nCacheObjectInput.readObject();
        date = object instanceof LocalDate ? (LocalDate)object : null;
    }
}
