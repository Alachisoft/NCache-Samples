from ncache.client.CacheManager import CacheManager
from ncache.client.CacheItem import CacheItem
from sample_data.product import Product


def initialize_cache():
    cache_name = "demoCache"
    cache = CacheManager.get_cache(cache_name)
    print("\nCache initialized successfully.")
    return cache


def add_items_in_group(cache):
    product_1 = Product(id="1001", name="Mobile", unit_price=50, units_available=20)
    product_2 = Product(id="1002", name="TV", unit_price=30, units_available=30)
    product_3 = Product(id="1003", name="Butter", unit_price=30, units_available=20)
    product_4 = Product(id="1004", name="Cheese", unit_price=40, units_available=10)
    electronics_products = [product_1, product_2]
    dairy_products = [product_3, product_4]
    for product in electronics_products:
        key = product.id
        item = CacheItem(product)
        item.set_group("Electronics")
        cache.add(key, item)
    for product in dairy_products:
        key = product.id
        item = CacheItem(product)
        item.set_group("Dairy")
        cache.add(key, item)
    print("\nItems are added in cache.")


def get_keys_by_group(cache):
    group = "Electronics"
    fetched_keys = cache.get_search_service().get_group_keys(group)
    if len(fetched_keys) != 0:
        print(f"\nThe keys in group '{group}' are:")
        for key in fetched_keys:
            print(key)


def update_items_in_group(cache):
    product = Product(id="1004", name="Milk", unit_price=40, units_available=10)
    key = product.id
    item = CacheItem(product)
    item.set_group("Dairy")
    cache.insert(key, item)
    print("\nItem is updated in cache.")
    fetched_product = cache.get(key, Product)
    print("\nThe updated item is:")
    print(fetched_product)


def remove_items_by_group(cache):
    # Count before removal
    count = cache.get_count()
    print("\nNumber of items in cache before removal: ", count)
    cache.get_search_service().remove_group_data("Electronics")
    print("\nItems are removed from cache.")
    # Count after removal
    count = cache.get_count()
    print("\nNumber of items in cache after removal: ", count)


def dispose_cache(cache):
    cache.close()
    print("\nCache closed.")


# Initialize instance of the cache to perform operations
cache = initialize_cache()

# Adding items in same group
add_items_in_group(cache)

# Getting group keys
get_keys_by_group(cache)

# Updating items in group
update_items_in_group(cache)

# Remove group data
remove_items_by_group(cache)

# Dispose the cache once done
dispose_cache(cache)
