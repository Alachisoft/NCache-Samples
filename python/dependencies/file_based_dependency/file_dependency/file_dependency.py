import os
import time
from datetime import datetime
from ncache.client.CacheManager import CacheManager
from ncache.client.CacheItem import CacheItem
from ncache.runtime.dependencies.FileDependency import FileDependency
from SampleData.Product import Product


class FileBasedDependency:
    cache = None

    @staticmethod
    def run():
        # Initialize cache
        FileBasedDependency.initialize_cache()
        # Add a single key and modify file to see if dependent is removed
        FileBasedDependency.add_file_based_dependency()
        # Dispose the cache once done
        FileBasedDependency.cache.close()

    @staticmethod
    def initialize_cache():
        cache_id = "demoCache"
        FileBasedDependency.cache = CacheManager.get_cache(cache_id)
        print(f"\nCache '{cache_id}' is initialized.")

    @staticmethod
    def add_file_based_dependency():
        dependency_file = os.path.join(os.path.abspath(os.path.join(os.getcwd(), os.pardir)), "DependencyFile", "foobar.txt")
        product = Product(id="1001", name="Tea", unit_price=50, units_available=20)
        cache_item = CacheItem(product)
        cache_item.set_dependency(FileDependency(dependency_file))
        FileBasedDependency.cache.add(f"Product:{product.id}", cache_item)
        print(f"\nItem 'Product:{product.id}' added to cache with file dependency.")

        # Any change in the file will cause invalidation of the cache item.
        # Following code modifies the file and then verifies the existence of the item in the cache.
        time.sleep(1)
        # Modify file programmatically
        FileBasedDependency.modify_dependency_file(dependency_file)
        time.sleep(30)

        item = FileBasedDependency.cache.get(f"Product:{product.id}", Product)
        if item is None:
            print("\nItem has been removed due to file dependency.")
        else:
            print("\nFile-based dependency did not work. Dependency file located at {dependency_file} might be missing or file not changed within the given interval.")

    @staticmethod
    def modify_dependency_file(path):
        with open(path, "a") as file:
            file.write(f"\n{datetime.now()}\tFile is modified.")

        print(f"\nFile '{path}' is modified programmatically.")


FileBasedDependency.run()
