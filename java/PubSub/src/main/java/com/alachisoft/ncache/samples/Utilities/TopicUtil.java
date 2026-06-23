package com.alachisoft.ncache.samples.Utilities;

import com.alachisoft.ncache.client.Cache;
import com.alachisoft.ncache.runtime.caching.Topic;

/**
 * Class to handle Topic related utility functions
 */
public class TopicUtil {

    public static Topic getOrCreateTopic(Cache cache, String topicName) throws Exception {

        Topic topic = cache.getMessagingService().getTopic(topicName);

        if (topic == null) {
            topic = cache.getMessagingService().createTopic(topicName);
        }

        return topic;
    }
}
