namespace MainLogic;

public class StatQueue<T>
{
    private Queue<T>  _queue = new Queue<T>();
    private double _lastChange;
    private double _weightSum = 0.0;
    private double _startTime;

    public StatQueue(double startTime)
    {
        _lastChange = startTime;
        _startTime = startTime;
    }

    private void UpdateStats(double currentTime)
    {
        _weightSum += _queue.Count * (currentTime -  _lastChange);
        if (_weightSum < 0) throw new Exception("Sth is wrong in UpdateStats");
        _lastChange = currentTime;
    }

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

    public T Peek()
    {
        return _queue.Peek();
    }

    public double GetAverageQueueLength(double currentTime)
    {
        var finalWeightedSum = _weightSum + (_queue.Count * (currentTime - _lastChange));
        var totalTime = currentTime - _startTime;
        var result = finalWeightedSum / totalTime;
        if (result < 0) throw new Exception("Sth is wrong in GetAverageQueueLength");
        return result;
    }

    // Kod vygenerovany s pomocou AI, zdokumentovane v kapitole 24
    public double GetAverageQueueLength()
    {
        var totalTime = _lastChange - _startTime;
        if (totalTime <= 0)
        {
            // No elapsed simulation time yet; return current queue count as approximation
            return _queue.Count;
        }
        var finalWeightedSum = _weightSum; // already accumulated up to _lastChange
        var result = finalWeightedSum / totalTime;
        if (result < 0) throw new Exception("Sth is wrong in GetAverageQueueLength()");
        return result;
    }
    
    public void Reset(double currentTime)
    {
        _weightSum = 0.0;
        _lastChange = currentTime;
        _startTime = currentTime; 
    }
    
    public int Count => _queue.Count;
    public bool IsEmpty() => _queue.Count == 0;
    
    public List<T> GetAllItems() => _queue.ToList();
}