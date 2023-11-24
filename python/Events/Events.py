import time
from ncache.client.CacheManager import CacheManager
from ncache.client.enum.EventType import EventType
from ncache.client.enum.EventDataFilter import EventDataFilter
from SampleData.Product import Product


class Events:
    cache = None
    event_descriptor = None

    @staticmethod
    def run():
        # Initialize cache
        Events.initialize_cache()

        # Event notifications must be enabled in NCache manager -> Options for events to work
        Events.register_cache_notification()

        # Generate a new instance of product
        product = Events.generate_product()
        key = Events.get_key(product)

        # Add item in cache
        Events.add_item(key, product)

        # Register Notification for the given key
        Events.add_notification_on_key(key)

        # Update item to trigger key-based notification.
        Events.update_item(key, product)

        # Delete item to trigger key-based notification.
        Events.delete_item(key)

        # Sleep for a while to allow events to be processed
        time.sleep(5)

        Events.unregister_cache_notification()

        # Dispose the cache once done
        Events.cache.close()

    @staticmethod
    def get_key(product):
        key = f"Product:{product.id}"
        return key

    @staticmethod
    def initialize_cache():
        cache_id = "demoCache"
        Events.cache = CacheManager.get_cache(cache_id)
        print("\nCache initialized successfully.")

    @staticmethod
    def register_cache_notification():
        Events.event_descriptor = Events.cache.get_messaging_service().add_cache_notification_listener(
            Events.cache_data_modified, [EventType.ITEM_ADDED, EventType.ITEM_REMOVED, EventType.ITEM_UPDATED],
            EventDataFilter.META_DATA
        )
        if Events.event_descriptor.get_is_registered():
            print("\nCache Notification Registered successfully.")
        else:
            print("\nCache Notification Not Registered.")

    @staticmethod
    def unregister_cache_notification():
        Events.cache.get_messaging_service().remove_cache_notification_listener(Events.event_descriptor)
        print("\nCache Notification Unregistered successfully.")

    @staticmethod
    def add_item(key, product):
        Events.cache.add(key, product)
        print("\nObject Added in Cache.")
        time.sleep(5)

    @staticmethod
    def add_notification_on_key(key):
        Events.cache.get_messaging_service().add_cache_notification_listener(Events.key_notification_method, [EventType.ITEM_ADDED, EventType.ITEM_REMOVED, EventType.ITEM_UPDATED], EventDataFilter.META_DATA, key)
        print(f"\nEvent Register for Key: '{key}'")

    @staticmethod
    def update_item(key, product):
        product.units_available = 10
        Events.cache.insert(key, product)
        print("\nObject Updated in Cache.")
        time.sleep(5)

    @staticmethod
    def delete_item(key):
        Events.cache.remove(key, Product)
        print("\nObject Removed from Cache.")
        time.sleep(5)

    @staticmethod
    def cache_data_modified(key, cache_event_args):
        print(f"\nCache data modification notification for the item of the key: '{key}'")

    @staticmethod
    def key_notification_method(key, cache_event_args):
        print("\nKey notification event is received from cache.")
        event_type = cache_event_args.get_event_type()
        if event_type == EventType.ITEM_ADDED:
            print(f"\nKey: '{key}' is added to the cache.")
        elif event_type == EventType.ITEM_REMOVED:
            print(f"\nKey: '{key}' is removed from the cache.")
        elif event_type == EventType.ITEM_UPDATED:
            print(f"\nKey: '{key}' is updated in the cache.")

    @staticmethod
    def generate_product():
        product = Product(id="1001", name="Tea", unit_price=50, units_available=20)
        return product


Events.run()
