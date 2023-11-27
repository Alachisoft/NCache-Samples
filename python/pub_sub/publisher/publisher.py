import time
from ncache.client.CacheManager import CacheManager
from ncache.client.enum.DeliveryOption import DeliveryOption
from ncache.runtime.caching.Message import Message
from ncache.runtime.util.TimeSpan import TimeSpan
from sample_data.product import Product


class Publisher:
    cache = None

    def __init__(self):
        self.topic = None

    @staticmethod
    def run():
        Publisher.initialize_cache()
        print("\nStarting 2 Publishers... Publishers will stop after publishing 30 messages each.")

        Publisher.run_publisher("ElectronicsProducts")
        Publisher.run_bulk_publisher("DairyProducts")
        Publisher.run_async_publisher("DairyProducts")

        print("\nPublishers Started.")

        i = input("\nPress any key to delete the topics.")
        Publisher.delete_topic("ElectronicsProducts")
        Publisher.delete_topic("DairyProducts")

        print("\nTopics Deleted.")

    @staticmethod
    def run_publisher(topic_name):
        publisher = Publisher()
        publisher.publish_messages(topic_name)

    @staticmethod
    def run_bulk_publisher(topic_name):
        publisher = Publisher()
        publisher.publish_bulk_messages(topic_name)

    @staticmethod
    def run_async_publisher(topic_name):
        publisher = Publisher()
        publisher.publish_async_messages(topic_name)

    def publish_messages(self, topic_name):
        self.topic = Publisher.cache.get_messaging_service().get_topic(topic_name)

        if self.topic is None:
            self.topic = Publisher.cache.get_messaging_service().create_topic(topic_name)
        self.topic.add_message_delivery_failure_listener(Publisher.message_delivery_failure)

        # Generate 30 messages at the interval of 5 seconds.
        for i in range(30):
            product = Product(id=str(i))
            message = Message(product, TimeSpan(ticks=0, hours=0, minutes=1, seconds=0))
            self.topic.publish(message, DeliveryOption.ALL, True)
            print(f"\nMessage for Product with Product ID: '{product.id}' generated.")
            time.sleep(5)

    def publish_bulk_messages(self, topic_name):
        self.topic = Publisher.cache.get_messaging_service().get_topic(topic_name)

        if self.topic is None:
            self.topic = Publisher.cache.get_messaging_service().create_topic(topic_name)
        self.topic.add_message_delivery_failure_listener(Publisher.message_delivery_failure)

        for i in range(30):
            product = Product(id=str(i))
            message_list = Publisher.generate_messages(product)

            # publishes the bulk with notifications for delivery failure
            # returns all messages that have encountered an exception
            message_exceptions = self.topic.publish_bulk(message_list, True)

            # if any exceptions are encountered, throw to client
            if message_exceptions:
                raise Exception(f"\n{self.topic.publish_bulk} has encountered {len(message_exceptions)} exceptions.")

            print(f"\n{len(message_list)} messages for Product generated.")
            time.sleep(5)

    def publish_async_messages(self, topic_name):
        self.topic = Publisher.cache.get_messaging_service().get_topic(topic_name)

        if self.topic is None:
            self.topic = Publisher.cache.get_messaging_service().create_topic(topic_name)
        self.topic.add_message_delivery_failure_listener(Publisher.message_delivery_failure)

        for i in range(30):
            product = Product(id=str(i))
            message = Message(product, TimeSpan(ticks=0, hours=0, minutes=1, seconds=0))

            # Publish the order with delivery option set as all
            # and register message delivery failure
            task = self.topic.publish_async(message, DeliveryOption.ALL, True)

            print(f"\nMessage for Product with Product ID: '{product.id}' generated.")
            time.sleep(5)

    @staticmethod
    def initialize_cache():
        cache_id = "demoCache"
        Publisher.cache = CacheManager.get_cache(cache_id)
        print(f"\nCache '{cache_id}' is initialized.")

    @staticmethod
    def delete_topic(topic_name):
        Publisher.cache = CacheManager.get_cache("cache_id")
        if topic_name is None:
            raise ValueError("\nTopic name cannot be None.")

        Publisher.cache.get_messaging_service().delete_topic(topic_name)

    @staticmethod
    def message_delivery_failure(sender, args):
        print(f"\nMessage not delivered. Reason: {args.get_message_failure_reason()}")

    @staticmethod
    def generate_messages(product):
        message_list = {}
        for i in range(5):
            message = Message(product, TimeSpan(ticks=0, hours=0, minutes=1, seconds=0))
            message_list[message] = DeliveryOption.ANY
        return message_list


Publisher.run()
