import time
from ncache.client.CacheManager import CacheManager
from ncache.client.QueryCommand import QueryCommand
from ncache.client.ContinuousQuery import ContinuousQuery
from ncache.client.enum.EventType import EventType
from ncache.client.enum.EventDataFilter import EventDataFilter
from sample_data.product import Product


class ContinuousQuerySample:
    cache = None

    @staticmethod
    def run():
        # Initialize new cache
        ContinuousQuerySample.initialize_cache()

        # Generate instance of Product
        product = ContinuousQuerySample.create_new_product()
        key = ContinuousQuerySample.get_key(product)

        # Adding item to cache
        ContinuousQuerySample.add_object_to_cache(key, product)

        # Build Query Command
        query_command = ContinuousQuerySample.build_query_command()

        # Register continuous query on cache
        continuous_query = ContinuousQuerySample.register_query(query_command)

        # Query keys in cache as per criteria
        ContinuousQuerySample.query_keys_in_cache(query_command)

        # Query data in cache as per criteria
        ContinuousQuerySample.query_data_in_cache(query_command)

        # Update an item in cache that is within query criteria to raise data modification event
        ContinuousQuerySample.update_object_in_cache(key, product)

        # Delete the existing object
        ContinuousQuerySample.delete_object_from_cache(key)

        # Sleep for a while to allow events to be processed
        time.sleep(5)

        # Unregister query
        ContinuousQuerySample.unregister_query(continuous_query)

        # Dispose the cache once done
        ContinuousQuerySample.cache.close()

    @staticmethod
    def initialize_cache():
        cache_id = "demoCache"
        ContinuousQuerySample.cache = CacheManager.get_cache(cache_id)
        ContinuousQuerySample.cache.clear()
        print(f"\nCache '{cache_id}' is initialized.")

    @staticmethod
    def add_object_to_cache(key, product):
        ContinuousQuerySample.cache.add(key, product)
        print("\nItem is added to cache.")

    @staticmethod
    def build_query_command():
        query = "SELECT * FROM sample_data.product.Product WHERE id = ?"
        query_command = QueryCommand(query)
        parameters = {"id": "1001"}
        query_command.set_parameters(parameters)
        return query_command

    @staticmethod
    def register_query(query_command):
        continuous_query = ContinuousQuery(query_command)

        # Add query notifications
        continuous_query.add_data_modification_listener(ContinuousQuerySample.query_data_modified, [EventType.ITEM_ADDED], EventDataFilter.NONE)

        # Item update notification with DataWithMetadata as event data filter to receive modified item on updation
        continuous_query.add_data_modification_listener(ContinuousQuerySample.query_data_modified, [EventType.ITEM_UPDATED], EventDataFilter.DATA_WITH_META_DATA)

        # Item delete notification
        continuous_query.add_data_modification_listener(ContinuousQuerySample.query_data_modified, [EventType.ITEM_REMOVED], EventDataFilter.NONE)

        # Register continuous query on server
        ContinuousQuerySample.cache.get_messaging_service().register_cq(continuous_query)

        print("\nContinuous Query is registered.")
        return continuous_query

    @staticmethod
    def query_keys_in_cache(query_command):
        cache_reader = ContinuousQuerySample.cache.get_search_service().execute_reader(query_command, False, 512)
        if not cache_reader.get_is_closed():
            print("\nFollowing keys are fetched with Continuous Query.")
            while cache_reader.read():
                key = cache_reader.get_value(str, 0)
                print("Key: " + str(key))

    @staticmethod
    def query_data_in_cache(query_command):
        cache_reader = ContinuousQuerySample.cache.get_search_service().execute_reader(query_command, True, 512)
        if not cache_reader.get_is_closed():
            print("\nFollowing items are fetched with Continuous Query.")
            while cache_reader.read():
                id = cache_reader.get_value(str, None, "id")
                name = cache_reader.get_value(str, None, "name")
                unit_price = cache_reader.get_value(int, None, "unit_price")
                units_available = cache_reader.get_value(int, None, "units_available")
                print(f"\nID: {id}\nName: {name}\nUnit Price: {unit_price}\nUnits Available: {units_available}")

    @staticmethod
    def update_object_in_cache(key, product):
        product.units_available = 10
        ContinuousQuerySample.cache.insert(key, product)
        print("\nItem is updated in cache.")

    @staticmethod
    def unregister_query(continuous_query):
        ContinuousQuerySample.cache.get_messaging_service().un_register_cq(continuous_query)
        print("\nContinuous Query is unregistered.")

    @staticmethod
    def delete_object_from_cache(key):
        ContinuousQuerySample.cache.remove(key, Product)
        print("\nObject is deleted from cache.")

    @staticmethod
    def query_data_modified(key, cq_event_arg):
        print("\nContinuous Query data modification event is received from cache.")
        event_type = cq_event_arg.get_event_type()
        if event_type == EventType.ITEM_ADDED:
            print(key + " is added to cache.")
        elif event_type == EventType.ITEM_REMOVED:
            print(key + " is removed from cache.")
        elif event_type == EventType.ITEM_UPDATED:
            print(key + " is updated in cache.")
            if cq_event_arg.get_item() is not None:
                product = cq_event_arg.get_item().get_value(Product)
                print(product)

    @staticmethod
    def create_new_product():
        product = Product(id="1001", name="Tea", unit_price=50, units_available=20)
        return product

    @staticmethod
    def get_key(product):
        key = f"Product:{product.id}"
        return key


ContinuousQuerySample.run()
