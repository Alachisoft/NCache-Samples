from ncache.client.CacheManager import CacheManager
from ncache.client.enum.TopicSearchOptions import TopicSearchOptions
from ncache.client.enum.SubscriptionPolicy import SubscriptionPolicy
from ncache.runtime.util.TimeSpan import TimeSpan


class DurableSubscriber:
    cache = None

    @staticmethod
    def run():
        DurableSubscriber.initialize_cache()

        electronics_shared_subs = DurableSubscriber.run_subscriber("ElectronicsProducts", "ElectronicsSubscription", SubscriptionPolicy.SHARED)
        dairy_exclusive_subs = DurableSubscriber.run_subscriber("DairyProducts", "DairySubscription", SubscriptionPolicy.EXCLUSIVE)

        all_products_subscription = DurableSubscriber.run_pattern_based_subscriber("*Products", "AllProductsSubscription")

        i = input("\nPress any key to stop all subscribers...")
        electronics_shared_subs.un_subscribe()
        dairy_exclusive_subs.un_subscribe()
        all_products_subscription.un_subscribe()

    @staticmethod
    def run_subscriber(topic_name, subscription_name, subscription_policy):
        topic = DurableSubscriber.cache.get_messaging_service().get_topic(topic_name)
        if topic is None:
            topic = DurableSubscriber.cache.get_messaging_service().create_topic(topic_name)

        return topic.create_durable_subscription(subscription_name, subscription_policy, DurableSubscriber.message_received_callback, TimeSpan(ticks=0, hours=0, minutes=1, seconds=0))

    @staticmethod
    def initialize_cache():
        cache_id = "demoCache"
        DurableSubscriber.cache = CacheManager.get_cache(cache_id)
        print(f"\nCache '{cache_id}' is initialized.")

    @staticmethod
    def run_pattern_based_subscriber(topic_pattern, subscription_name):
        topic = DurableSubscriber.cache.get_messaging_service().get_topic(topic_pattern, TopicSearchOptions.BY_PATTERN)

        if topic is not None:
            return topic.create_durable_subscription(subscription_name, SubscriptionPolicy.SHARED, DurableSubscriber.message_received_callback_pattern_based, TimeSpan(ticks=0, hours=0, minutes=1, seconds=0))

        return None

    @staticmethod
    def message_received_callback(sender, args):
        print(f"\nMessage Received for {args.get_topic_name()}")

    @staticmethod
    def message_received_callback_pattern_based(sender, args):
        print(f"\nMessage Received on Pattern Based subscription for {args.get_topic_name()}")


DurableSubscriber.run()
