package java.com.alachisoft.ncache.samples.EventListeners;

import com.alachisoft.ncache.runtime.caching.MessageEventArgs;
import com.alachisoft.ncache.runtime.caching.messaging.MessageReceivedListener;

public class PatternBasedMessageReceivedListener implements MessageReceivedListener {
    @Override
    public void onMessageReceived(Object o, MessageEventArgs messageEventArgs) {
        System.out.println("Message Recieved on pattern based subscription for " + messageEventArgs.getTopicName());
    }
}
