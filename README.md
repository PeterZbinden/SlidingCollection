[NuGet.org](https://www.nuget.org/packages/SlidingCollection)

# Sliding Collection
A library that implements a collection to simplify processing data using a 'moving window' of x items.


### Add using
```csharp
using SlidingCollection;
```

### Usage
```csharp
var maxSize = 5;
/// Create a new Collection
var collection = new SlidingEnumerable<int>(maxSize);

// Add new values
collection.Add(1);
collection.Add(2);
collection.Add(3);
collection.Add(4);
collection.Add(5);
collection.Add(6);

// Read the values
foreach (var value in collection)
{
    Console.WriteLine(value);
}

// Console-Output
//2
//3
//4
//5
//6

// The first element '1' was removed as soon as value '6' was added
// because the defined 'maxSize' only allows for 5 Values in the collection
```
