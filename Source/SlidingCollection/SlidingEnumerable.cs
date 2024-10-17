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
    private readonly int _maxSize;
    private int _startIndexOffset;

    public int Length => _length;

    private readonly T[] _items;
    private int _length;

    public SlidingEnumerable(int maxSize)
    {
        if (maxSize < 1)
        {
            throw new ApplicationException($"Size of '{maxSize}' is not valid, only sizes > 0 are valid");
        }

        _maxSize = maxSize;
        _startIndexOffset = 0;
        _items = new T[maxSize];

        ResetAllValues();
    }

    public T GetAtIndex(int index)
    {
        if (index < 0 || index >= _maxSize)
        {
            throw new IndexOutOfRangeException($"Index of {index} is outside of the specified size of {_maxSize}");
        }

        var i = GetInternalIndex(index);

        return _items[i];
    }

    public T First()
    {
        if (Length < 1)
        {
            throw new ApplicationException("No Items added");
        }

        return GetAtIndex(0);
    }

    public T Last()
    {
        if (Length < 1)
        {
            throw new ApplicationException("No Items added");
        }

        return GetAtIndex(Length - 1);
    }

    int GetInternalIndex(int externalIndex)
    {
        return (externalIndex + _startIndexOffset) % _maxSize;
    }

    public void Add(T item)
    {
        var index = GetInternalIndex(Length);
        _items[index] = item;
        if (Length < _maxSize)
        {
            _length++;
        }
        else
        {
            _startIndexOffset = (_startIndexOffset + 1) % _maxSize;
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < Length; i++)
        {
            var idx = GetInternalIndex(i);
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
        for (int i = 0; i < _maxSize; i++)
        {
            _items[i] = default(T);
        }
    }
}