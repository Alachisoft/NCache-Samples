package java.com.alachisoft.ncache.samples.EventListeners;

import com.alachisoft.ncache.runtime.caching.MessageFailedEventArgs;
import com.alachisoft.ncache.runtime.caching.TopicDeleteEventArgs;
import com.alachisoft.ncache.runtime.caching.TopicListener;

public class EventListeners implements TopicListener {
    @Override
    public void onTopicDeleted(Object o, TopicDeleteEventArgs topicDeleteEventArgs) {

    }

    @Override
    public void onMessageDeliveryFailure(Object o, MessageFailedEventArgs args) {
        System.out.println("Message not delivered. Reason: " + args.getMessgeFailureReason());
    }
}
