package java.com.alachisoft.ncache.samples.EventListeners;

import com.alachisoft.ncache.runtime.caching.MessageEventArgs;
import com.alachisoft.ncache.runtime.caching.messaging.MessageReceivedListener;

public class OnMessageReceivedListener implements MessageReceivedListener {
    @Override
    public void onMessageReceived(Object o, MessageEventArgs messageEventArgs) {
        System.out.println("Message Recieved for: " + messageEventArgs.getTopicName());
    }
}
