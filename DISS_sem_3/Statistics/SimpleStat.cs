namespace MainLogic;

public class SimpleStat
{
    public double SumX { get; set; }
    public double SumX2 { get; set; }
    private int _count = 0;

    public void AddSample(double value)
    {
        SumX += value;
        SumX2 += Math.Pow(value, 2) ;
        _count++;
    }
    
    /**
    * s = sqrt( (sum(Xi^2) - (sum(Xi)^2 / n)) / (n - 1) )
    */
    public double GetStandardDeviation()
    {
        var variance = (SumX2 - (Math.Pow(SumX, 2) / _count)) / (_count - 1);
        return Math.Sqrt(variance); 
    }
    
    public (double avg, double Lower, double Upper) GetConfidenceInterval(double confidenceLevel = 0.95)
    {
        var avg = GetAverage();
        if (_count < 30) return (avg,0,0);   
        var tAlpha = confidenceLevel switch
        {
            0.90 => 1.645,
            0.95 => 1.96,
            0.99 => 2.576,
            _ => 1.96 
        };
        
        var s = GetStandardDeviation();
        
        // H = (s * tAlpha) / sqrt(n)
        var halfWidth = (s * tAlpha) / Math.Sqrt(_count);

        return (avg, avg - halfWidth, avg + halfWidth);
    }

    public string GetConfidenceIntervalString()
    {
        if (_count < 30) return $"Not enough values < 30. Only {_count}";
        var interval = GetConfidenceInterval();
        return $"Avg = {GetAverage():F5}, Confidence = ({interval.Lower:F5}; {interval.Upper:F5})";
    }

    public double GetAverage() => _count > 0 ? SumX / _count : 0;
    public int GetCount() => _count;
    
    public void Reset()
    {
        SumX = 0;
        SumX2 = 0;
        _count = 0;
    }
}