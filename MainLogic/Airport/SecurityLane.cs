namespace MainLogic;

public class SecurityLane
{
    public int Id { get; set; }
    public StatQueue<Passenger> _passengersEntryQueue { get; private set; }
    public StatQueue<Luggage> _beforeDetectorTrack { get; private set; }
    public StatQueue<Luggage> _afterDetectorTrack { get; private set; }
    public StatQueue<Passenger> DetectorQueue { get; private set; }
    public StatQueue<Passenger> WaitForLuggageQueue { get; private set; }
    public List<Luggage> LuggageInsideDetector  { get; set; }
    public List<Passenger> PassengerInsideDetector  { get; set; }
    public List<Passenger> PassengerAtPersonalInspection  { get; set; }
    public bool IsLuggageScannerBusy { get; set; }
    public bool IsSecurityGuardBusy { get; set; }
    public readonly int MAX_AFTER_DETECTOR_COUNT;
    public readonly int MAX_BEFORE_DETECTOR_COUNT;

    public SecurityLane(int id, double startTime, int maxBeforeDetectorCount = 4, int maxAfterDetectorCount = 5)
    {
        Id = id;
        MAX_BEFORE_DETECTOR_COUNT = maxBeforeDetectorCount;
        MAX_AFTER_DETECTOR_COUNT = maxAfterDetectorCount;
        
        _passengersEntryQueue = new StatQueue<Passenger>(startTime);
        _beforeDetectorTrack = new StatQueue<Luggage>(startTime);
        _afterDetectorTrack = new StatQueue<Luggage>(startTime);
        DetectorQueue = new StatQueue<Passenger>(startTime);
        WaitForLuggageQueue = new StatQueue<Passenger>(startTime);

        LuggageInsideDetector = [];
        PassengerInsideDetector = [];
        PassengerAtPersonalInspection = [];
    }
    
  public void EnqueueItemToQueue<T>(StatQueue<T> commonQueue, StatQueue<T> specificQueue, T item, double time)
  {
      specificQueue.Enqueue(item, time);
      commonQueue.Enqueue(item, time);
  }
  public T DequeueItemFromQueue<T>(StatQueue<T> commonQueue, StatQueue<T> specificQueue, double time)
  {
      commonQueue.Dequeue(time);
      return specificQueue.Dequeue(time);
  }
  
  public void ResetStatistics(double currentTime)
  {
      _passengersEntryQueue.Reset(currentTime);
      _beforeDetectorTrack.Reset(currentTime);
      _afterDetectorTrack.Reset(currentTime);
      DetectorQueue.Reset(currentTime);
      WaitForLuggageQueue.Reset(currentTime);
  }
}