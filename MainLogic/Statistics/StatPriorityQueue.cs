namespace MainLogic;

/**
 * SEM 3
 * Kod vytvoreny s pomocou AI, zdokumentovane v kapitole 1
 */
public class StatPriorityQueue<T> : BaseStatQueue<T>
{
    private class PriorityItem
    {
        public T Data { get; init; }
        public int Priority { get; init; }
        public double ArrivalTime { get; init; }
    }

    private readonly List<PriorityItem> _items = new();

    public StatPriorityQueue(double startTime) : base(startTime) { }

    public override int Count => _items.Count;

    public void Enqueue(T item, int priority, double currentTime)
    {
        UpdateStats(currentTime);
        _items.Add(new PriorityItem { Data = item, Priority = priority, ArrivalTime = currentTime });
        
        _items.Sort((a, b) => {
            int res = a.Priority.CompareTo(b.Priority);
            return res != 0 ? res : a.ArrivalTime.CompareTo(b.ArrivalTime);
        });
    }

    public T Dequeue(double currentTime)
    {
        UpdateStats(currentTime);
        var item = _items[0];
        _items.RemoveAt(0);
        return item.Data;
    }

    public override void Reset(double currentTime)
    {
        base.Reset(currentTime);
        _items.Clear();
    }
    
    public List<T> GetAllItems()
    {
        int count = _items.Count;
        if (count == 0) return new List<T>();
        var result = new List<T>(count);

        for (int i = 0; i < count; i++) result.Add(_items[i].Data);
        return result;
    }
}