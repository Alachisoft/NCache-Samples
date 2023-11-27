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
    version = cache.add(key, item)
    print("\nObject is added in cache.")
    return version


def update_object_in_cache(cache, key, product, item_version):
    product.units_available = 10
    updated_item = CacheItem(product)
    updated_item.set_cache_item_version(item_version)

    # Update item if version match
    updated_item_version = cache.insert(key, updated_item)
    if item_version.get_version() != updated_item_version.get_version():
        print("\nItem has changed since last time it was fetched.")
    return updated_item_version


def get_object_with_item_version(cache, key, item_version):
    # Get item only if version is superior to old version
    fetched_product = cache.get_if_newer(key, item_version, Product)
    if fetched_product is None:
        print("\nSpecified item version is latest.")
    else:
        print("\nCurrent version of item is: ", item_version.get_version())
        print("\nObject is fetched from cache.")
        print(fetched_product)


def remove_object_with_item_version(cache, key, item_version):
    # Remove item if version match
    removed_product = cache.remove(key, Product, None, None, item_version)
    if removed_product is None:
        print("\nThe item cannot be removed as the newer version of item exists in cache.")
    else:
        print("\nObject is removed from cache.")
        print("\nThe removed object is:")
        print(removed_product)


def dispose_cache(cache):
    cache.close()
    print("\nCache closed.")


# Initialize instance of the cache to perform operations
cache = initialize_cache()

# Create a Product object
product = create_product()
key = get_key(product)

# Adding item
item_version = add_object_to_cache(cache, key, product)

# Update item
latest_item_version = update_object_in_cache(cache, key, product, item_version)

# Get an item with cache item version
get_object_with_item_version(cache, key, item_version)

# Remove item
remove_object_with_item_version(cache, key, latest_item_version)

# Dispose the cache once done
dispose_cache(cache)
