package com.Alachisoft.NCache.Samples.callbacks;

import com.alachisoft.ncache.runtime.caching.MessageFailedEventArgs;
import com.alachisoft.ncache.runtime.caching.TopicDeleteEventArgs;
import com.alachisoft.ncache.runtime.caching.TopicListener;

public class TopicDeletedCallback implements TopicListener {
    @Override
    public void onTopicDeleted(Object o, TopicDeleteEventArgs topicDeleteEventArgs) {
        System.out.println("Topic " + topicDeleteEventArgs.getTopicName() + " deleted.");
    }

    @Override
    public void onMessageDeliveryFailure(Object o, MessageFailedEventArgs messageFailedEventArgs) {

    }
}
