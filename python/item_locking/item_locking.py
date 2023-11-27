from ncache.client.CacheManager import CacheManager
from ncache.client.CacheItem import CacheItem
from ncache.client.LockHandle import LockHandle
from ncache.runtime.util.TimeSpan import TimeSpan
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


def add_item_in_cache(cache, key, product):
    item = CacheItem(product)
    cache.add(key, item)
    print("\nObject is added in cache.")


def get_item_from_cache(cache, key, lock_handle, time_span):
    fetched_product = cache.get(key, Product, None, None, True, time_span, lock_handle)
    print("\nObject is fetched from cache.")
    print(fetched_product)
    print("\nLock acquired on: ", lock_handle.get_lock_id())


def lock_item_in_cache(cache, key, lock_handle, time_span):
    is_locked = cache.lock(key, time_span, lock_handle)
    if not is_locked:
        print("\nLock acquired on: ", lock_handle.get_lock_id())


def unlock_item_in_cache(key, lock_handle):
    cache.unlock(key, lock_handle)
    print("\nThe item is unlocked.")


def remove_item_from_cache(cache, key):
    # Count before removal
    count = cache.get_count()
    print("\nNumber of items in cache before removal: ", count)
    cache.remove(key, Product)
    print("\nObject is removed from cache.")
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

# Add item in cache
add_item_in_cache(cache, key, product)

# Create new lock handle to fetch item using locking
lock_handle = LockHandle()

# Time span for which lock will be taken
time_span = TimeSpan(ticks=0, hours=0, minutes=0, seconds=20)

# Get item from cache
get_item_from_cache(cache, key, lock_handle, time_span)

# Lock item in cache
lock_item_in_cache(cache, key, lock_handle, time_span)

# Unlock item in cache
unlock_item_in_cache(key, lock_handle)

# Remove item from cache
remove_item_from_cache(cache, key)

# Dispose the cache once done
dispose_cache(cache)
