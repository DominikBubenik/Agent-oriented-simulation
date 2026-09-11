namespace MainLogic;

public class StatPriorityQueue<T> : BaseStatQueue<T>
{
    private class PriorityItem : IComparable<PriorityItem>
    {
        public T Data { get; init; }
        public int Priority { get; init; }
        public double ArrivalTime { get; init; }

        public int CompareTo(PriorityItem? other)
        {
            if (other == null) return 1;
            int res = Priority.CompareTo(other.Priority);
            // Secondary sort: Arrival Time (First-In-First-Out for same priority)
            return res != 0 ? res : ArrivalTime.CompareTo(other.ArrivalTime);
        }
    }

    private readonly List<PriorityItem> _items = new();

    public StatPriorityQueue(double startTime) : base(startTime) { }

    public override int Count => _items.Count;

    // Renamed for clarity, though kept logic same as your original Pop
    public T Pop() => _items.Count > 0 ? _items[0].Data : throw new InvalidOperationException("Queue empty");

    public void Enqueue(T item, int priority, double arrival, double currentTime)
    {
        UpdateStats(currentTime);
        var newItem = new PriorityItem { Data = item, Priority = priority, ArrivalTime = arrival };
        
        // OPTIMIZATION: Use Binary Search to find the correct insertion point O(log N)
        int index = _items.BinarySearch(newItem);
        
        // If not found, BinarySearch returns the bitwise complement of the next larger element
        if (index < 0) index = ~index;
        
        // Insert is O(N), but much faster than O(N log N) Sort()
        _items.Insert(index, newItem);
    }

    public T Dequeue(double currentTime)
    {
        if (_items.Count == 0) throw new InvalidOperationException("Queue empty");
        
        UpdateStats(currentTime);
        var item = _items[0];
        _items.RemoveAt(0); // O(N) shift
        return item.Data;
    }

    public override void Reset(double currentTime)
    {
        base.Reset(currentTime);
        _items.Clear();
    }
    
    public List<T> GetAllItems()
    {
        // Allocation optimization: define capacity immediately
        var result = new List<T>(_items.Count);
        foreach (var item in _items) result.Add(item.Data);
        return result;
    }
}