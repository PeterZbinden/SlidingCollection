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
    private int _offset;
    private readonly T[] _items;

    public SlidingEnumerable(int size)
    {
        if (size < 1)
        {
            throw new ApplicationException($"Size of '{size}' is not valid, only sizes > 0 are valid");
        }

        Length = size;
        _offset = 0;
        _items = new T[size];

        ResetAllValues();
    }

    public T GetAtIndex(int index)
    {
        if (index < 0 || index >= Length)
        {
            throw new IndexOutOfRangeException($"Index of {index} is outside of the specified queue-size of {Length}");
        }

        var i = GetInternalIndex(index);

        return _items[i];
    }

    public T First()
    {
        return GetAtIndex(0);
    }

    public T Last()
    {
        return GetAtIndex(Length - 1);
    }

    int GetInternalIndex(int externalIndex)
    {
        return (externalIndex + _offset) % Length;
    }

    public void Add(T item)
    {
        var index = GetInternalIndex(Length);
        _items[index] = item;
        _offset++;
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < Length; i++)
        {
            yield return _items[GetInternalIndex(i)];
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
        _offset = 0;

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