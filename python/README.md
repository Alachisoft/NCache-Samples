# NCache Python Client — Code Samples

A collection of Python samples demonstrating core features of [NCache](https://www.alachisoft.com/ncache/), a distributed in-memory cache for .NET, Java, and Python applications. Each sample is self-contained and focused on a single feature area.

---

## Prerequisites

- Python 3.6+
- NCache server running and accessible (default cache name: `demoCache`)
- `ncache` Python client library installed:

```bash
pip install ncache-client
```

---

## Project Structure

```
.
├── async_operations/
│   └── async_operations.py         # Async add, insert, and remove
├── basic_operations/
│   └── basic_operations.py         # Sync CRUD (add, get, update, remove)
├── bulk_operations/
│   └── bulk_operations.py          # add_bulk, get_bulk, insert_bulk, remove_bulk
├── cache_item_versioning/
│   └── cache_item_versioning.py    # Optimistic concurrency via item versions
├── continuous_query/
│   └── continuous_query.py         # Real-time query-based change notifications
├── dependencies/
│   ├── file_based_dependency/
│   │   └── file_dependency/
│   │       └── file_dependency.py  # Invalidate cache items on file change
│   └── key_based_dependency/
│       └── key_dependency.py       # Invalidate items when a parent key changes
├── events/
│   └── events.py                   # Cache-wide and per-key event listeners
├── groups_and_tags/
│   ├── groups.py                   # Logical grouping of cache items
│   ├── named_tags.py               # Key-value metadata tags + SQL-style queries
│   └── tags.py                     # Simple tag-based retrieval and removal
├── item_locking/
│   └── item_locking.py             # Pessimistic locking with LockHandle
├── pub_sub/
│   ├── durable_subscriber/
│   │   └── durable_subscriber.py   # Durable (shared/exclusive) topic subscriptions
│   ├── non_durable_subscriber/
│   │   └── non_durable_subscriber.py # Lightweight transient subscriptions
│   └── publisher/
│       └── publisher.py            # Publish messages, bulk publish, async publish
└── sample_data/
    ├── product.py                  # Shared Product model used across all samples
    └── product.json                # Sample product JSON fixture
```

---

## Samples

### Basic Operations

Demonstrates synchronous CRUD operations using `CacheItem`.

```bash
python basic_operations/basic_operations.py
```

Covers: `cache.add()`, `cache.get()`, `cache.insert()`, `cache.remove()`, `cache.get_count()`

---

### Async Operations

Same CRUD operations executed asynchronously using Python's `asyncio`.

```bash
python async_operations/async_operations.py
```

Covers: `cache.add_async()`, `cache.insert_async()`, `cache.remove_async()`

---

### Bulk Operations

Adds, retrieves, updates, and removes multiple cache items in a single call.

```bash
python bulk_operations/bulk_operations.py
```

Covers: `cache.add_bulk()`, `cache.get_bulk()`, `cache.insert_bulk()`, `cache.remove_bulk()`

---

### Cache Item Versioning

Uses optimistic concurrency control — operations succeed only if the item version matches the expected version.

```bash
python cache_item_versioning/cache_item_versioning.py
```

Covers: `CacheItem.set_cache_item_version()`, `cache.get_if_newer()`, version-conditional `cache.insert()` and `cache.remove()`

---

### Continuous Query

Registers a persistent, SQL-style query against the cache and receives real-time events when matching items are added, updated, or removed.

```bash
python continuous_query/continuous_query.py
```

Covers: `QueryCommand`, `ContinuousQuery`, `add_data_modification_listener()`, `register_cq()`, `un_register_cq()`

---

### Dependencies

#### File-Based Dependency

A cache item is automatically invalidated when a watched file on disk is modified.

```bash
cd dependencies/file_based_dependency/file_dependency
python file_dependency.py
```

Covers: `FileDependency`, `CacheItem.set_dependency()`

#### Key-Based Dependency

A dependent cache item is automatically removed when its parent key is deleted or modified.

```bash
python dependencies/key_based_dependency/key_dependency.py
```

Covers: `KeyDependency`, `CacheItem.set_dependency()`

---

### Events

Registers cache-wide and per-key event listeners for item add, update, and remove operations.

```bash
python events/events.py
```

Covers: `add_cache_notification_listener()`, `remove_cache_notification_listener()`, `EventType`, `EventDataFilter`

---

### Groups and Tags

#### Groups

Organizes cache items into named logical groups. Items can be retrieved or bulk-removed by group name.

```bash
python groups_and_tags/groups.py
```

Covers: `CacheItem.set_group()`, `get_group_keys()`, `remove_group_data()`

#### Named Tags

Attaches key-value metadata to cache items and queries them using a SQL-like syntax.

```bash
python groups_and_tags/named_tags.py
```

Covers: `NamedTagsDictionary`, `CacheItem.set_named_tags()`, `QueryCommand`, `execute_reader()`

#### Tags

Tags items with one or more string labels, then retrieves or removes by matching any or all tags.

```bash
python groups_and_tags/tags.py
```

Covers: `Tag`, `CacheItem.set_tags()`, `get_keys_by_tag()`, `get_keys_by_tags()`, `TagSearchOptions`, `remove_by_tag()`, `remove_by_tags()`

---

### Item Locking

Acquires pessimistic locks on cache items to prevent concurrent writes, using a `LockHandle` and configurable lock duration.

```bash
python item_locking/item_locking.py
```

Covers: `LockHandle`, `cache.lock()`, `cache.unlock()`, `cache.get()` with lock acquisition, `TimeSpan`

---

### Pub/Sub Messaging

#### Publisher

Publishes messages to named topics using single-message, bulk, and async modes.

```bash
python pub_sub/publisher/publisher.py
```

Covers: `get_topic()`, `create_topic()`, `Message`, `topic.publish()`, `topic.publish_bulk()`, `topic.publish_async()`, `DeliveryOption`

#### Non-Durable Subscriber

Creates transient subscriptions that receive messages only while the subscriber process is running.

```bash
python pub_sub/non_durable_subscriber/non_durable_subscriber.py
```

Covers: `topic.create_subscription()`, pattern-based subscriptions with `TopicSearchOptions.BY_PATTERN`

#### Durable Subscriber

Creates persistent subscriptions (shared or exclusive) that survive client disconnects and buffer messages for replay.

```bash
python pub_sub/durable_subscriber/durable_subscriber.py
```

Covers: `topic.create_durable_subscription()`, `SubscriptionPolicy.SHARED`, `SubscriptionPolicy.EXCLUSIVE`

---

## Configuration

All samples connect to a cache named `demoCache` via `CacheManager.get_cache("demoCache")`. To use a different cache, update the `cache_name` / `cache_id` variable at the top of each sample file.

NCache server connection settings (host, port, security) are configured in your NCache client configuration file (`client.ncconf`), not in the application code.

---

## Sample Data

All samples use a shared `Product` model located in `sample_data/product.py`:

```python
class Product:
    def __init__(self, id=None, name=None, unit_price=None, units_available=None):
        ...
```

A sample JSON representation is available at `sample_data/product.json`.

---

## Resources

- [NCache Documentation](https://www.alachisoft.com/resources/docs/)
- [NCache Python Client on PyPI](https://pypi.org/project/ncache-client/)
- [NCache GitHub](https://github.com/Alachisoft/NCache)
