from ncache.client.CacheManager import CacheManager
from ncache.client.CacheItem import CacheItem
from ncache.runtime.caching.NamedTagsDictionary import NamedTagsDictionary
from ncache.client.QueryCommand import QueryCommand
from SampleData.Product import Product


def initialize_cache():
    cache_name = "demoCache"
    cache = CacheManager.get_cache(cache_name)
    print("\nCache initialized successfully.")
    return cache


def add_items(cache, named_tags):
    product_1 = Product(id="1001", name="Butter", unit_price=50, units_available=20)
    product_2 = Product(id="1002", name="Cheese", unit_price=30, units_available=30)
    products = [product_1, product_2]
    for product in products:
        key = product.id
        item = CacheItem(product)
        item.set_named_tags(named_tags)
        cache.add(key, item)
    print("\nItems are added in cache.")


def get_items(cache):
    query = "SELECT * FROM SampleData.Product.Product WHERE Category = ?"
    query_command = QueryCommand(query)
    parameters = {"Category": "Dairy"}
    query_command.set_parameters(parameters)
    cache_reader = cache.get_search_service().execute_reader(query_command, True, 512)
    if not cache_reader.get_is_closed():
        print("\nThe values fetched are:")
        while cache_reader.read():
            id = cache_reader.get_value(str, None, "id")
            name = cache_reader.get_value(str, None, "name")
            unit_price = cache_reader.get_value(int, None, "unit_price")
            units_available = cache_reader.get_value(int, None, "units_available")
            print(f"\nID: {id}\nName: {name}\nUnit Price: {unit_price}\nUnits Available: {units_available}")


def dispose_cache(cache):
    cache.close()
    print("\nCache closed.")


# Initialize instance of the cache to perform operations
cache = initialize_cache()

# Creating named tags dictionary
named_tags = NamedTagsDictionary()
named_tags.add("Category", "Dairy")

# Add items in cache
add_items(cache, named_tags)

# Get items using named tags
get_items(cache)

# Dispose the cache once done
dispose_cache(cache)
