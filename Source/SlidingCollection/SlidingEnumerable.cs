using System.Collections;

namespace SlidingCollection;

/// <summary>
/// A collection that accepts a given max size of values
/// This collection is NOT Thread-Save for performance reasons
/// The caller needs to deal with race-conditions
/// </summary>
/// <typeparam name="T"></typeparam>
public class SlidingEnumerable<T> : IEnumerable<T>
{
    public int Length { get; private set; }
    private int _startIndexOffset;
    private int _itemsInCollection;
    private readonly T[] _items;

    public SlidingEnumerable(int size)
    {
        if (size < 1)
        {
            throw new ApplicationException($"Size of '{size}' is not valid, only sizes > 0 are valid");
        }

        Length = size;
        _startIndexOffset = 0;
        _items = new T[size];

        ResetAllValues();
    }

    public T GetAtIndex(int index)
    {
        if (index < 0 || index >= Length)
        {
            throw new IndexOutOfRangeException($"Index of {index} is outside of the specified size of {Length}");
        }

        var i = GetInternalIndex(index);

        return _items[i];
    }

    public T First()
    {
        if (_itemsInCollection < 1)
        {
            throw new ApplicationException("No Items added");
        }

        return GetAtIndex(0);
    }

    public T Last()
    {
        if (_itemsInCollection < 1)
        {
            throw new ApplicationException("No Items added");
        }

        return GetAtIndex(_itemsInCollection - 1);
    }

    int GetInternalIndex(int externalIndex)
    {
        if (_itemsInCollection > 0)
        {
            if (_itemsInCollection < Length)
            {
                return (externalIndex + _startIndexOffset);
            }

            return (externalIndex + _startIndexOffset) % Length;
        }
        return externalIndex;
    }

    public void Add(T item)
    {
        var index = GetInternalIndex(_itemsInCollection);
        _items[index] = item;
        if (_itemsInCollection < Length)
        {
            _itemsInCollection++;
        }
        else
        {
            _startIndexOffset = (_startIndexOffset + 1) % Length;
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < _itemsInCollection; i++)
        {
            var idx  = GetInternalIndex(i);
            yield return _items[idx];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    /// <summary>
    /// Resets the internal Tracking
    /// If <see cref="resetValues"/> is true, all existing Values are overwritten
    /// If <see cref="resetValues"/> is true, the old Values remain and the caller has to deal with this
    /// </summary>
    /// <param name="resetValues"></param>
    public void Clear(bool resetValues = false)
    {
        _startIndexOffset = 0;

        if (resetValues)
        {
            ResetAllValues();
        }
    }

    private void ResetAllValues()
    {
        for (int i = 0; i < Length; i++)
        {
            _items[i] = default(T);
        }
    }
}