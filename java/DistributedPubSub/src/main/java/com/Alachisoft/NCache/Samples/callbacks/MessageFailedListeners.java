package com.Alachisoft.NCache.Samples.callbacks;

import com.alachisoft.ncache.runtime.caching.MessageFailedEventArgs;
import com.alachisoft.ncache.runtime.caching.TopicDeleteEventArgs;
import com.alachisoft.ncache.runtime.caching.TopicListener;

public class MessageFailedListeners implements TopicListener {
    @Override
    public void onTopicDeleted(Object o, TopicDeleteEventArgs topicDeleteEventArgs) {

    }

    @Override
    public void onMessageDeliveryFailure(Object o, MessageFailedEventArgs args) {
        System.out.println("Message not delivered. Reason: " + args.getMessgeFailureReason());
    }
}
