from ncache.client.CacheManager import CacheManager
from ncache.client.CacheItem import CacheItem
from sample_data.product import Product


def initialize_cache():
    cache_name = "demoCache"
    cache = CacheManager.get_cache(cache_name)
    print("\nCache initialized successfully.")
    return cache


def create_product():
    product = Product(id="1001", name="Tea", unit_price=50, units_available=20)
    return product


def get_key(product):
    key = "Product:" + product.id
    return key


def add_object_to_cache(cache, key, product):
    item = CacheItem(product)
    cache.add(key, item)
    print("\nObject is added in cache.")


def get_object_from_cache(cache, key):
    fetched_product = cache.get(key, Product)
    print("\nObject is fetched from cache.")
    if fetched_product is not None:
        print(fetched_product)


def update_object_in_cache(cache, key, product):
    product.units_available = 10
    updated_item = CacheItem(product)
    cache.insert(key, updated_item)
    print("\nObject is updated in cache.")
    fetched_product = cache.get(key, Product)
    print("\nThe updated object is: ")
    print(fetched_product)


def remove_object_from_cache(cache, key):
    # Count before removal
    count = cache.get_count()
    print("\nNumber of items in cache before removal: ", count)
    removed_product = cache.remove(key, Product)
    print("\nObject is removed from cache.")
    print("\nThe removed object is:")
    print(removed_product)
    # Count after removal
    count = cache.get_count()
    print("\nNumber of items in cache after removal: ", count)


def dispose_cache(cache):
    cache.close()
    print("\nCache closed.")


# Initialize instance of the cache to perform operations
cache = initialize_cache()

# Create a Product object
product = create_product()
key = get_key(product)

# Adding item synchronously
add_object_to_cache(cache, key, product)

# Get the object from cache
get_object_from_cache(cache, key)

# Modify the object and update in cache
update_object_in_cache(cache, key, product)

# Delete the existing object
remove_object_from_cache(cache, key)

# Dispose the cache once done
dispose_cache(cache)
