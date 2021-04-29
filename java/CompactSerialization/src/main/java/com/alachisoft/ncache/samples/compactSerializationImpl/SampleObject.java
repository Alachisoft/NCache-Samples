package com.alachisoft.ncache.samples.compactSerializationImpl;

import com.alachisoft.ncache.serialization.core.io.ICompactSerializable;
import com.alachisoft.ncache.serialization.core.io.NCacheObjectInput;
import com.alachisoft.ncache.serialization.core.io.NCacheObjectOutput;

import java.io.IOException;
import java.time.LocalDate;
import java.util.HashMap;
import java.util.UUID;

public class SampleObject implements ICompactSerializable {

    private String _name;
    private long _id;
    private UUID _uuid;
    private byte[] _byte;
    private float _float;
    private LocalDate _time;
    private HashMap<String, String> _table;

    public SampleObject() {
        _name = "Sean";
        _id = 101;
        _uuid = UUID.randomUUID();
        _byte = new byte[4 * 1024];
        _time = LocalDate.now();
        _table = new HashMap<>();
        _table.put(_uuid.toString(), _name);
    }

    public String get_name() {
        return _name;
    }

    public void set_name(String _name) {
        this._name = _name;
    }

    public long get_id() {
        return _id;
    }

    public void set_id(long _id) {
        this._id = _id;
    }

    public UUID get_uuid() {
        return _uuid;
    }

    public void set_uuid(UUID _uuid) {
        this._uuid = _uuid;
    }

    public byte[] get_byte() {
        return _byte;
    }

    public void set_byte(byte[] _byte) {
        this._byte = _byte;
    }

    public float get_float() {
        return _float;
    }

    public void set_float(float _float) {
        this._float = _float;
    }

    public LocalDate get_time() {
        return _time;
    }

    public void set_time(LocalDate _time) {
        this._time = _time;
    }

    public HashMap<String, String> get_table() {
        return _table;
    }

    public void set_table(HashMap<String, String> _table) {
        this._table = _table;
    }

    @Override
    public void serialize(NCacheObjectOutput nCacheObjectOutput) throws IOException {
        nCacheObjectOutput.writeObject(_name);
        nCacheObjectOutput.writeUInt32(_id);
        nCacheObjectOutput.writeObject(_uuid);
        nCacheObjectOutput.write(_byte);
        nCacheObjectOutput.writeObject(_time);
        nCacheObjectOutput.writeObject(_table);
    }

    @SuppressWarnings("unchecked")
    @Override
    public void deserialize(NCacheObjectInput nCacheObjectInput) throws IOException, ClassNotFoundException {
        Object name = nCacheObjectInput.readObject();
        _name = name instanceof String ? (String)name : null;
        _id = nCacheObjectInput.readUInt32();
        Object uuid = nCacheObjectInput.readObject();
        _uuid = uuid instanceof UUID ? (UUID) uuid : null;
        int size = nCacheObjectInput.read(_byte);
        Object time = nCacheObjectInput.readObject();
        _time = time instanceof LocalDate ? (LocalDate)time : null;
        Object table = nCacheObjectInput.readObject();
        _table = table instanceof HashMap ? (HashMap<String, String>) table : null;
    }
}
