namespace MainLogic;

/**
 * Kod vytvoreny s pomocou AI, zdokumentovane v kapitole 1
 */
public abstract class BaseStatQueue<T>
{
    protected double _weightSum = 0.0;
    protected double _lastChange;
    protected double _startTime;

    // Each subclass must provide its own count
    public abstract int Count { get; }
    public bool IsEmpty() => Count == 0;

    protected BaseStatQueue(double startTime)
    {
        _lastChange = startTime;
        _startTime = startTime;
    }

    protected void UpdateStats(double currentTime)
    {
        _weightSum += Count * (currentTime - _lastChange);
        if (_weightSum < 0) throw new Exception("Statistics error: WeightSum is negative.");
        _lastChange = currentTime;
    }

    public virtual void Reset(double currentTime)
    {
        _weightSum = 0.0;
        _lastChange = currentTime;
        _startTime = currentTime;
    }

    public double GetAverageQueueLength(double currentTime)
    {
        var finalWeightedSum = _weightSum + (Count * (currentTime - _lastChange));
        var totalTime = currentTime - _startTime;
        return totalTime <= 0 ? Count : finalWeightedSum / totalTime;
    }
}