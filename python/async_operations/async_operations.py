import asyncio
from ncache.client.CacheManager import CacheManager
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


async def add_async(cache, key, product):
    task = cache.add_async(key, product)
    value = await task
    print("\nObject is added in cache.")
    fetched_product = cache.get(key, Product)
    print("\nThe added object is: ")
    print(fetched_product)


async def insert_async(cache, key, product):
    product.units_available = 10
    task = cache.insert_async(key, product)
    value = await task
    print("\nObject is updated in cache.")
    fetched_product = cache.get(key, Product)
    print("\nThe updated object is: ")
    print(fetched_product)


async def remove_async(cache, key):
    # Count before removal
    count = cache.get_count()
    print("\nNumber of items in cache before removal: ", count)
    task = cache.remove_async(key, Product)
    value = await task
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

# Adding item asynchronously
asyncio.run(add_async(cache, key, product))

# Modify the object and update in cache
asyncio.run(insert_async(cache, key, product))

# Remove the existing object asynchronously
asyncio.run(remove_async(cache, key))

# Dispose the cache once done
dispose_cache(cache)
