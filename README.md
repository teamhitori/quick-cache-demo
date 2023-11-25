# Objective
Our application – handles large volumes of data, and also needs to be fast. One strategy to
improve the execution speed of our code is to cache regularly-used data in memory.

You have been asked to create a generic in-memory cache component, which other
developers can use in their applications.

This component should be able to store arbitrary types of objects, which are added and retrieved
using a unique key (similar to a dictionary).

To avoid the risk of running out of memory, the cache will need to have a configurable threshold for
the maximum number of items which it can hold at any one time. If the cache becomes full, any
attempts to add additional items should succeed, but will result in another item in the cache being
evicted. The cache should implement the ‘least recently used’ approach when selecting which item
to evict.

The cache component is intended to be used as a singleton. As such, you should ideally make your
component thread-safe for all methods, but you can skip this feature if you run out of time.
Another useful feature would be some kind of mechanism which allows the consumer to know when
items get evicted. Again, if you run out of time, you can skip this feature too.

## Constraints
• Please write the solution in C# and .NET
• You may use any .NET framework version you wish
• You may use any development tools you wish
• You are permitted to use external libraries (e.g. nuget packages)

## User Requirments

**Create Repo and Define Branching Strategy**: Provision a new Github repo and define a branching strategy / feature toggles.

**Documentation**: Create a Wiki page for project and user documentation. This includes examples of common use cases and API documentation.

**Automated Build/Test Pipeline**: In order to demonstrate stability and reliability from the outset, set up an automated build and test pipleline using Github actions. The pipeline should build the application and run any unit tests. Comprehensive testing is essential for ensuring that the cache behaves as expected under various scenarios, including concurrent access and edge cases. Later, this pipeline can be extended to cover automated deployment.

**Test Harness**: Create a test harnes for demonstrating performance characteristics and benchmarks again other cache implementations such as Redis.

**Extensibility and Maintainability**: Design the cache component in a way that allows for future extensions and easy maintenance. This includes using clear coding practices, comments, and documentation.

**Generic Data Storage**: Create generic data store with the ability to store any type of object is a core function of the cache. The cache should be designed using generics in C# to accommodate different data types.

**Key-Based Data Retrieval**: Create an efficient, reliable and intuitive retrieval of data using unique keys.

**Configurable Size Limit**: Implement a configurable threshold for the maximum number of items the cache can hold. This is crucial for memory management and preventing potential out-of-memory issues.

**Least Recently Used (LRU) Eviction Policy**: The cache should use the LRU algorithm for eviction in order to maintaining the cache size. This ensures that when the cache is full and a new item is added, the least recently used item gets evicted.

**Thread Safety**: Ensure that the cache is thread-safe, especially given that it's intended to be used as a singleton in a multi-threaded application. This will involve implementing synchronization mechanisms to manage concurrent access.

**Eviction Notifications**: Provide a mechanism to notify users when an item is evicted from the cache. This feature, while useful, is not as critical as the others and can be considered an enhancement.

**Performance Optimization**: Once the fundamental features are in place, focus on optimising for performance. This includes minimizing lock contention, efficient memory usage, and quick access times.

**Advanced Features (Optional)**: Depending on time and complexity, add additional features like cache item expiration, statistics reporting (hits, misses, load factor), and cache warming.

## System Design

![alt text](https://github.com/[teamhitori]/[quick-cache-demo]/blob/[mater]/img/system.svg?raw=true)
