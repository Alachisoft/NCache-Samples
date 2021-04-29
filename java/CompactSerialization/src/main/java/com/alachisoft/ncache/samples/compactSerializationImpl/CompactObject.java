package com.alachisoft.ncache.samples.compactSerializationImpl;

import com.alachisoft.ncache.serialization.core.io.ICompactSerializable;
import com.alachisoft.ncache.serialization.core.io.NCacheObjectInput;
import com.alachisoft.ncache.serialization.core.io.NCacheObjectOutput;
import com.alachisoft.ncache.serialization.standard.io.CompactReader;
import com.alachisoft.ncache.serialization.standard.io.CompactWriter;

import java.io.IOException;
import java.time.LocalDate;

public class CompactObject implements ICompactSerializable {

    private String _name;
    private byte[] _data = new byte[512];
    private LocalDate _time;
    private SampleObject _sampleObject = new SampleObject();

    public String get_name() {
        return _name;
    }

    public void set_name(String _name) {
        this._name = _name;
    }

    public byte[] get_data() {
        return _data;
    }

    public LocalDate get_time() {
        return _time;
    }

    public void set_time(LocalDate _time) {
        this._time = _time;
    }

    public void set_data(byte[] _data) {
        this._data = _data;
    }

    public SampleObject get_sampleObject() {
        return _sampleObject;
    }

    public void set_sampleObject(SampleObject _sampleObject) {
        this._sampleObject = _sampleObject;
    }

    @Override
    public void serialize(NCacheObjectOutput nCacheObjectOutput) throws IOException {
        nCacheObjectOutput.writeObject(_name);
        nCacheObjectOutput.write(_data);
        nCacheObjectOutput.writeObject(_time);
        nCacheObjectOutput.writeObject(_sampleObject);
    }

    @Override
    public void deserialize(NCacheObjectInput nCacheObjectInput) throws IOException, ClassNotFoundException {
        Object name = nCacheObjectInput.readObject();
        _name = name instanceof String ? (String)name : null;
        int size = nCacheObjectInput.read(_data);
        Object time = nCacheObjectInput.readObject();
        _time = time instanceof LocalDate ? (LocalDate)time : null;
        Object sampleObject = nCacheObjectInput.readObject();
        _sampleObject = sampleObject instanceof SampleObject ? (SampleObject)sampleObject : null;
    }
}
