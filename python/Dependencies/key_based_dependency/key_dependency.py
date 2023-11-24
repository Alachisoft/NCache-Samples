import time
from ncache.client.CacheManager import CacheManager
from ncache.client.CacheItem import CacheItem
from ncache.runtime.dependencies.KeyDependency import KeyDependency
from SampleData.Product import Product


class KeyBasedDependency:
    cache = None

    @staticmethod
    def run():
        # Initialize cache
        KeyBasedDependency.initialize_cache()
        # Add a single key dependency and modify parent object to see if dependent is removed
        KeyBasedDependency.add_single_key_dependency()
        # Dispose the cache once done
        KeyBasedDependency.cache.close()

    @staticmethod
    def initialize_cache():
        cache_id = "demoCache"
        KeyBasedDependency.cache = CacheManager.get_cache(cache_id)
        print(f"\nCache '{cache_id}' is initialized.")

    @staticmethod
    def add_single_key_dependency():
        product_1 = Product(id="1001", name="Tea", unit_price=50, units_available=20)
        KeyBasedDependency.cache.add(product_1.id, product_1)
        print(f"\nItem '{product_1.id}' is added to cache.")
        product_2 = Product(id="1002", name="Milk", unit_price=30, units_available=30)
        cache_item = CacheItem(product_2)
        cache_item.set_dependency(KeyDependency(product_1.id))
        KeyBasedDependency.cache.add(product_2.id, cache_item)
        print(f"\nItem '{product_2.id}' dependent upon '{product_1.id}' is added to cache.")

        # Any modification in the dependent item will cause invalidation of the dependee item.
        # Thus, the item will be removed from the cache.
        time.sleep(1)
        # Delete object from cache
        KeyBasedDependency.cache.remove(product_1.id, Product)
        print(f"\nItem '{product_1.id}' is deleted from cache.")
        time.sleep(30)

        value = KeyBasedDependency.cache.get(f"{product_2.id}", Product)
        if value is None:
            print(f"\nDependent item '{product_2.id}' is successfully removed from cache.")
        else:
            print("\nError while removing the dependent item from cache.")


KeyBasedDependency.run()
