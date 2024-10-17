using BenchmarkDotNet.Attributes;
using SlidingCollection;

namespace Benchmark;

[MemoryDiagnoser]
public class ReadBenchmark
{
    [Params(1000)]
    public int MaxSize { get; set; } = 5;
    [Params(100_000)]
    public int ItemsAdded { get; set; } = 100;

    private SlidingEnumerable<int> _slidingEnumerable;
    private Queue<int> _queue;
    private List<int> _list;

    [GlobalSetup]
    public void Setup()
    {
        _slidingEnumerable = new SlidingEnumerable<int>(MaxSize);
        _queue = new Queue<int>(MaxSize);
        _list = new List<int>(MaxSize);

        for (int i = 0; i < ItemsAdded; i++)
        {
            _slidingEnumerable.Add(i);
            _queue.Enqueue(i);
            if (_queue.Count > MaxSize)
            {
                _queue.Dequeue();
            }
            _list.Add(i);
            if (_list.Count > MaxSize)
            {
                _list.RemoveAt(0);
            }
        }
    }

    [Benchmark]
    public int SlidingCollection()
    {
        var x = 0;

        foreach (var i in _slidingEnumerable)
        {
            x += i;
        }

        return x;
    }
    
    [Benchmark]
    public int Queue()
    {
        var x = 0;

        foreach (var i in _queue)
        {
            x += i;
        }

        return x;
    }

    [Benchmark]
    public int List()
    {
        var x = 0;

        foreach (var i in _list)
        {
            x += i;
        }

        return x;
    }
}

[MemoryDiagnoser]
public class WriteBenchmark
{
    [Params(1000)]
    public int MaxSize { get; set; } = 5;
    [Params(100_000)]
    public int ItemsAdded { get; set; } = 100;

    [Benchmark]
    public void SlidingCollection()
    {
        var collection = new SlidingEnumerable<int>(MaxSize);
        for (int i = 0; i < ItemsAdded; i++)
        {
            collection.Add(i);
        }
    }
    
    [Benchmark]
    public void Queue()
    {
        var collection = new Queue<int>();
        for (int i = 0; i < ItemsAdded; i++)
        {
            collection.Enqueue(i);
            if (collection.Count > MaxSize)
            {
                collection.Dequeue();
            }
        }
    }

    [Benchmark]
    public void List()
    {
        var collection = new List<int>();
        for (int i = 0; i < ItemsAdded; i++)
        {
            collection.Add(i);
            if (collection.Count > MaxSize)
            {
                collection.RemoveAt(0);
            }
        }
    }
}