from ncache.client.CacheManager import CacheManager
from ncache.client.CacheItem import CacheItem
from sample_data.product import Product


def initialize_cache():
    cache_name = "demoCache"
    cache = CacheManager.get_cache(cache_name)
    print("\nCache initialized successfully.")
    return cache


def create_products():
    product_1 = Product(id="1001", name="Tea", unit_price=50, units_available=20)
    product_2 = Product(id="1002", name="Milk", unit_price=30, units_available=30)
    product_3 = Product(id="1003", name="Butter", unit_price=30, units_available=20)
    product_4 = Product(id="1004", name="Cheese", unit_price=40, units_available=10)
    products = [product_1, product_2, product_3, product_4]
    return products


def get_keys(products):
    keys = []
    for product in products:
        keys.append("Product:" + product.id)
    return keys


def create_dictionary(keys, products):
    items = {}
    for i in range(len(products)):
        item = CacheItem(products[i])
        items[keys[i]] = item
    return items


def add_multiple_objects_in_cache(cache, items):
    keys_failed = cache.add_bulk(items)
    if len(keys_failed) == 0:
        print("\nAll items are added in cache.")
    else:
        print("\nOne or more items could not be added in cache. The items failed are:")
        for key in keys_failed.keys():
            print("\n", key)


def update_multiple_objects_in_cache(cache, keys, items):
    for i in range(len(items)):
        item = items[keys[i]]
        product = item.get_value(Product)
        product.unit_price = 100
        items[keys[i]] = CacheItem(product)

    keys_failed = cache.insert_bulk(items)
    if len(keys_failed) == 0:
        print("\nAll items are updated in cache.")
    else:
        print("\nOne or more items could not be updated in cache. The items failed are:")
        for key in keys_failed.keys():
            print("\n", key)


def get_multiple_objects_from_cache(cache, keys):
    fetched_products = cache.get_bulk(keys, Product)
    if len(fetched_products) != 0:
        for key in fetched_products:
            product = fetched_products[key]
            print(product)


def remove_multiple_objects_from_cache(cache, keys):
    removed_items = cache.remove_bulk(keys, Product)
    if len(removed_items) == len(keys):
        print("\nAll items are removed from cache.")
    else:
        print("\nOne or more keys failed to remove.")


def dispose_cache(cache):
    cache.close()
    print("\nCache closed.")


# Initialize instance of the cache to perform operations
cache = initialize_cache()

# Create multiple Product objects
products = create_products()
keys = get_keys(products)

# Create dictionary of keys and products
items = create_dictionary(keys, products)

# Adding multiple items in cache
add_multiple_objects_in_cache(cache, items)

# Update multiple items in cache
update_multiple_objects_in_cache(cache, keys, items)

# Get multiple items from cache
get_multiple_objects_from_cache(cache, keys)

# Remove the existing objects
remove_multiple_objects_from_cache(cache, keys)

# Dispose the cache once done
dispose_cache(cache)
