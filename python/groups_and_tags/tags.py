from ncache.client.CacheManager import CacheManager
from ncache.client.CacheItem import CacheItem
from ncache.runtime.caching.Tag import Tag
from ncache.client.enum.TagSearchOptions import TagSearchOptions
from SampleData.Product import Product


def initialize_cache():
    cache_name = "demoCache"
    cache = CacheManager.get_cache(cache_name)
    print("\nCache initialized successfully.")
    cache.clear()
    return cache


def add_items_with_tags(cache, tags):
    product_1 = Product(id="1001", name="Butter", unit_price=50, units_available=20)
    product_2 = Product(id="1002", name="Cheese", unit_price=30, units_available=30)
    products = [product_1, product_2]
    for product in products:
        key = product.id
        item = CacheItem(product)
        item.set_tags(tags)
        cache.add(key, item)
    print("\nItems are added in cache.")


def get_tag_keys(cache, tag):
    fetched_keys = cache.get_search_service().get_keys_by_tag(tag)
    if len(fetched_keys) != 0:
        print(f"\nThe keys with tag '{tag.get_tag_name()}' are:")
        for key in fetched_keys:
            print(key)


def get_keys_by_multiple_tags(cache, tags):
    fetched_keys = cache.get_search_service().get_keys_by_tags(tags, TagSearchOptions.BY_ALL_TAGS)
    if len(fetched_keys) != 0:
        print(f"\nThe keys with all matching tags are:")
        for key in fetched_keys:
            print(key)


def get_keys_by_any_tag(cache, tags):
    fetched_keys = cache.get_search_service().get_keys_by_tags(tags, TagSearchOptions.BY_ANY_TAG)
    if len(fetched_keys) != 0:
        print(f"\nThe keys with any matching tag are:")
        for key in fetched_keys:
            print(key)


def remove_items_by_tag(cache, tag):
    cache.get_search_service().remove_by_tag(tag)
    print(f"\nItems with tag '{tag.get_tag_name()}' are removed from cache.")


def remove_items_by_multiple_tags(cache, tags):
    cache.get_search_service().remove_by_tags(tags, TagSearchOptions.BY_ALL_TAGS)
    print("\nItems with all matching tags are removed from cache.")


def remove_items_by_any_tag(cache, tags):
    cache.get_search_service().remove_by_tags(tags, TagSearchOptions.BY_ANY_TAG)
    print("\nItems with any matching tag are removed from cache.")


def dispose_cache(cache):
    cache.close()
    print("\nCache closed.")


# Initialize instance of the cache to perform operations
cache = initialize_cache()

# Creating tags list
tags = [Tag("Dairy"), Tag("InStock"), Tag("NotExpired")]

# Add items with tags in cache
add_items_with_tags(cache, tags)

# Get keys by a specific tag
get_tag_keys(cache, tags[0])

# Get keys where all the tags match
get_keys_by_multiple_tags(cache, tags)

# Get keys where any item in tags matches
get_keys_by_any_tag(cache, tags)

# Remove items by a specific tag
remove_items_by_tag(cache, tags[0])

# Remove items where all the tags match
remove_items_by_multiple_tags(cache, tags)

# Remove items where any item in tags matches
remove_items_by_any_tag(cache, tags)

# Dispose the cache once done
dispose_cache(cache)
