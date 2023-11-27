from ncache.client.CacheManager import CacheManager
from ncache.client.enum.TopicSearchOptions import TopicSearchOptions


class NonDurableSubscriber:
    cache = None

    @staticmethod
    def run():
        NonDurableSubscriber.initialize_cache()

        electronics_subscription = NonDurableSubscriber.run_subscriber("ElectronicsProducts")
        dairy_subscription = NonDurableSubscriber.run_subscriber("DairyProducts")

        all_products_subscription = NonDurableSubscriber.run_pattern_based_subscriber("*Products")

        i = input("\nPress any key to stop subscribers...")
        electronics_subscription.un_subscribe()
        dairy_subscription.un_subscribe()
        all_products_subscription.un_subscribe()

    @staticmethod
    def run_subscriber(topic_name):
        topic = NonDurableSubscriber.cache.get_messaging_service().get_topic(topic_name)
        if topic is None:
            topic = NonDurableSubscriber.cache.get_messaging_service().create_topic(topic_name)

        return topic.create_subscription(NonDurableSubscriber.message_received_callback)

    @staticmethod
    def run_pattern_based_subscriber(pattern):
        pattern_topic = NonDurableSubscriber.cache.get_messaging_service().get_topic(pattern, TopicSearchOptions.BY_PATTERN)

        if pattern_topic is not None:
            return pattern_topic.create_subscription(NonDurableSubscriber.pattern_message_received_callback)

        return None

    @staticmethod
    def initialize_cache():
        cache_id = "demoCache"
        NonDurableSubscriber.cache = CacheManager.get_cache(cache_id)
        print(f"\nCache '{cache_id}' is initialized.")

    @staticmethod
    def message_received_callback(sender, args):
        print(f"\nMessage Received for {args.get_topic_name()}")

    @staticmethod
    def pattern_message_received_callback(sender, args):
        print(f"\nMessage Received on pattern based subscription for {args.get_topic_name()}")


NonDurableSubscriber.run()
