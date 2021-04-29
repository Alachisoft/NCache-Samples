package com.alachisoft.ncache.samples.compactSerializationUsage;

import java.util.concurrent.ArrayBlockingQueue;
import java.util.concurrent.BlockingQueue;
public class EventLog {

    private static LogStream _eventLog;
    public static LogStream create(BlockingQueue<StringBuilder> queue, String rootPath, int port)
    {
        _eventLog = new LogStream(queue, rootPath, port);
        return _eventLog;
    }

    public static LogStream create(String rootPath, int port)
    {
        return create(new ArrayBlockingQueue<StringBuilder>(10), rootPath, port);
    }

    public static void write(Exception ex, String cls, String func)
    {
        write("Exception: " + ex.toString(), cls, func);
    }

    public static void write(String msg, String cls, String func)
    {
        _eventLog.write(msg, cls, func);
    }
}
