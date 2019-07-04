// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================

using System;
using Alachisoft.NCache.Runtime.Serialization;
using Alachisoft.NCache.Runtime.Serialization.IO;
using System.Collections;

namespace Alachisoft.NCache.Samples
{
    /// <summary>
    /// Summary description for NormalObject.
    /// </summary>
    public class SmallObject : ICompactSerializable
    {
        public string _name;
        public byte[] _data = new byte[512];
        public SampleObject _sampleObj = new SampleObject();
        public DateTime time = DateTime.Now;

        public void Serialize(CompactWriter writer)
        {
            writer.WriteObject(_name);
            writer.Write(_data);
            writer.WriteObject(_sampleObj);
            writer.Write(time);
        }

        public void Deserialize(CompactReader reader)
        {
            this._name = (string)reader.ReadObject();
            this._data = reader.ReadBytes(512);
            this._sampleObj = (SampleObject)reader.ReadObject();
            this.time = reader.ReadDateTime();
        }
    }

    /// <summary>
    /// Summary description for NormalObject.
    /// </summary>
    public class NormalObject : ICompactSerializable
    {
        public string _name;
        public byte[] _data = new byte[4 * 1024];
        public SampleObject _sampleObj;
        public DateTime time;

        public void Serialize(CompactWriter writer)
        {
            writer.WriteObject(_name);
            writer.Write(_data);
            writer.WriteObject(_sampleObj);
            writer.Write(time);
        }

        public void Deserialize(CompactReader reader)
        {
            this._name = (string)reader.ReadObject();
            this._data = reader.ReadBytes(4 * 1024);
            this._sampleObj = (SampleObject)reader.ReadObject();
            this.time = reader.ReadDateTime();
        }
    }

    /// <summary>
    /// Summary description for BigObject.
    /// </summary>
    public class BigObject : ICompactSerializable
    {
        public string _name;
        public byte[] _data = new byte[10 * 1024];
        public SampleObject _sampleObj = new SampleObject();
        public DateTime time;

        public void Serialize(CompactWriter writer)
        {
            writer.WriteObject(_name);
            writer.Write(_data);
            writer.WriteObject(_sampleObj);
            writer.Write(time);
        }

        public void Deserialize(CompactReader reader)
        {
            this._name = (string)reader.ReadObject();
            this._data = reader.ReadBytes(10 * 1024);
            this._sampleObj = (SampleObject)reader.ReadObject();
            this.time = reader.ReadDateTime();
        }
    }

    /// <summary>
    /// Summary description for BiggerObject.
    /// </summary>
    public class BiggerObject : ICompactSerializable
    {
        public string _name;
        public byte[] _data = new byte[50 * 1024];
        public SampleObject sampleObj;
        public DateTime time;

        public void Serialize(CompactWriter writer)
        {
            writer.WriteObject(_name);
            writer.Write(_data);
            writer.WriteObject(sampleObj);
            writer.Write(time);
        }

        public void Deserialize(CompactReader reader)
        {
            this._name = (string)reader.ReadObject();
            this._data = reader.ReadBytes(50 * 1024);
            this.sampleObj = (SampleObject)reader.ReadObject();
            this.time = reader.ReadDateTime();
        }
    }

    /// <summary>
    /// class to be used in side the objects for explaining Custom Serialization
    /// </summary>
    public class SampleObject : ICompactSerializable
    {
        private string _name;
        private UInt32 _id;
        private Guid _guid;
        private byte[] _byte;
        private float _float;
        private DateTime _time;
        private Hashtable _table;

        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }

        public UInt32 Id
        {
            set { _id = value; }
            get { return _id; }
        }

        public Guid GuidData
        {
            set { _guid = value; }
            get { return _guid; }
        }

        public byte[] ByteData
        {
            set { _byte = value; }
            get { return _byte; }
        }

        public float FloatData
        {
            set { _float = value; }
            get { return _float; }
        }

        public DateTime Time
        {
            set { _time = value; }
            get { return _time; }
        }

        public Hashtable Table
        {
            set { _table = value; }
            get { return _table; }
        }

        public SampleObject()
        {
            Name = "Sean";
            Id = 101;
            GuidData = Guid.NewGuid();
            ByteData = new byte[4 * 1024];
            FloatData = 10.256F;
            Time = DateTime.Now;
            Table = new Hashtable(25);
        }

        public void Serialize(CompactWriter writer)
        {
            writer.Write(Name);
            writer.Write(Id);
            writer.Write(GuidData);
            writer.Write(ByteData);
            writer.Write(FloatData);
            writer.Write(Time);
            writer.WriteObject(Table);
        }

        public void Deserialize(CompactReader reader)
        {
            Name = reader.ReadString();
            Id = reader.ReadUInt32();
            GuidData = reader.ReadGuid();
            ByteData = reader.ReadBytes(4 * 1024);
            FloatData = reader.ReadSingle(); ;
            Time = reader.ReadDateTime();
            Table = (Hashtable)reader.ReadObject();
        }
    }


    /// Compact serialization inherited objects
    /// SampleClass that implements ICompactSerializable
    class SampleClass : ICompactSerializable
    {
        private String name = "SampleClass";

        public void Serialize(CompactWriter writer)
        {
            writer.Write(name);
        }
        public void Deserialize(CompactReader reader)
        {
            name = reader.ReadString();
        }
    }

    /// SampleDerivedClass derived from SampleClass
    class SampleDerivedClass : SampleClass, ICompactSerializable
    {
        private int someint = 200;
        private DateTime date = DateTime.Now;

        void Serialize(CompactWriter writer)
        {
            base.Serialize(writer);
            writer.Write(someint);
            writer.Write(date);
        }
        void Deserialize(CompactReader reader)
        {
            base.Deserialize(reader);
            someint = reader.ReadInt32();
            date = reader.ReadDateTime();
        }
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++
}


