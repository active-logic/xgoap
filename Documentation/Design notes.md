# XGOAP design notes

## The `Clonable` interface

This interface is for users to provide their own clone implementation; necessary because deep clones via serialization are very slow.

`Clone()` receives an instance of the model; **user must override all fields** because the instance (by design) may contain dirty state (To avoid allocation overheads, instances of the model are reused whenever possible - even when pooling is disabled)

When implementing `Allocate()`, call a no-arg constructor or similar. This interface is required because `new`ing generic types directly is (in the author's experience) surprisingly slow (also read [here](http://www.philosophicalgeek.com/2012/04/02/activator-createinstance-slow-vs-less-slow/))
