using MainLogic;

/**
 * SEM 3
 * Kod upraveny s pomocou AI, zdokumentovane v kapitole 1
 */
public class StatQueue<T> : BaseStatQueue<T>
{
    private readonly Queue<T> _queue = new();

    public StatQueue(double startTime) : base(startTime) { }

    public override int Count => _queue.Count;

    public void Enqueue(T item, double currentTime)
    {
        UpdateStats(currentTime);
        _queue.Enqueue(item);
    }

    public T Dequeue(double currentTime)
    {
        UpdateStats(currentTime);
        return _queue.Dequeue();
    }

    public T Peek() => _queue.Peek();
    public List<T> GetAllItems() => _queue.ToList();
}