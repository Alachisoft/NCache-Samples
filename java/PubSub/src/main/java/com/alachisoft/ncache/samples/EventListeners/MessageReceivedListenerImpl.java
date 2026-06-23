package com.alachisoft.ncache.samples.EventListeners;

import com.alachisoft.ncache.runtime.caching.MessageEventArgs;
import com.alachisoft.ncache.runtime.caching.messaging.MessageReceivedListener;
import com.alachisoft.ncache.samples.Publisher.ElectronicsOrder;
import com.alachisoft.ncache.samples.Publisher.GarmentsOrder;

public class MessageReceivedListenerImpl implements MessageReceivedListener {
    @Override
    public void onMessageReceived(Object o, MessageEventArgs messageEventArgs) {
        System.out.println("Message Recieved for: " + messageEventArgs.getTopicName());
    }
}