package com.alachisoft.ncache.samples.compactSerializationImpl;

import com.alachisoft.ncache.serialization.core.io.ICompactSerializable;
import com.alachisoft.ncache.serialization.core.io.NCacheObjectInput;
import com.alachisoft.ncache.serialization.core.io.NCacheObjectOutput;

import java.io.IOException;
import java.time.LocalDate;

public class BiggerObject implements ICompactSerializable {

    private String _name;
    public byte[] _data = new byte[50 * 1024];
    private LocalDate _time;

    public String get_name() {
        return _name;
    }

    public void set_name(String _name) {
        this._name = _name;
    }

    public byte[] get_data() {
        return _data;
    }

    public void set_data(byte[] _data) {
        this._data = _data;
    }

    public LocalDate get_time() {
        return _time;
    }

    public void set_time(LocalDate _time) {
        this._time = _time;
    }

    @Override
    public void serialize(NCacheObjectOutput nCacheObjectOutput) throws IOException {
        nCacheObjectOutput.writeObject(_name);
        nCacheObjectOutput.write(_data);
        nCacheObjectOutput.writeObject(_time);
    }

    @Override
    public void deserialize(NCacheObjectInput nCacheObjectInput) throws IOException, ClassNotFoundException {
        Object name = nCacheObjectInput.readObject();
        _name = name instanceof String ? (String)name : null;
        int size = nCacheObjectInput.read(_data);
        Object time = nCacheObjectInput.readObject();
        _time = time instanceof LocalDate ? (LocalDate)time : null;
    }
}
