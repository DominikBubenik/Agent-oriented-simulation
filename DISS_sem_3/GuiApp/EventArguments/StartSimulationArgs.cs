namespace DISS_SEM_GUI.EventsArguments;

public class StartSimulationArgs(
    int seed,
    int replications,
    bool observationMode,
    double endSimulationTime,
    int nursesCount,
    int doctorsCount,
    int afterDetectorCount,
    double timeIntervalSeconds,
    int systemCapacity,
    int refreshRate,
    bool warmUpProof,
    double warmUp = 15000,
    bool sensibilityRequested = false,
    int sensCapacity = 1000,
    int sensReplications = 10,
    int sensGraphPoints = 10,
    bool findSensitivityRequested = false,
    double findComfortSeconds = 600.0,
    int findEntryQueueAvg = 20,
    int findLuggageQueueAvg = 10,
    bool csvGenerateRequested = false,
    string? csvDirectory = null,
    string? csvFileName = null,
    bool turboMode = false)
{
    public int Seed { get; set; } = seed;
    public int Replications { get; set; } = replications;
    public bool ObservationMode { get; set; } = observationMode;
    public double EndSimulationTime { get; set; } = endSimulationTime; //v hodinach
    public int NursesCount { get; set; } = nursesCount;
    public int DoctorsCount { get; set; } = doctorsCount;
    public double TimeIntervalSeconds { get; set; } = timeIntervalSeconds;
    public int SystemCapacity { get; set; } = systemCapacity;
    public int RefreshRate { get; set; } = refreshRate;
    public bool WarmUpProof { get; set; } = warmUpProof;
    public double WarmUp { get; set; } = warmUp;

    // Sensitivity / sweep options
    public bool SensibilityRequested { get; set; } = sensibilityRequested;
    public int SensCapacity { get; set; } = sensCapacity;
    public int SensReplications { get; set; } = sensReplications;
    public int SensGraphPoints { get; set; } = sensGraphPoints;

    // Find sensitivity threshold options
    public bool FindSensitivityRequested { get; set; } = findSensitivityRequested;
    // comfort threshold in seconds (e.g. 600 = 00:10:00)
    public double FindComfortSeconds { get; set; } = findComfortSeconds;
    public int FindEntryQueueAvg { get; set; } = findEntryQueueAvg;
    public int FindLuggageQueueAvg { get; set; } = findLuggageQueueAvg;

    // CSV generation options
    public bool CsvGenerateRequested { get; set; } = csvGenerateRequested;
    public string? CsvDirectory { get; set; } = csvDirectory;
    public string? CsvFileName { get; set; } = csvFileName;
    public bool TurboMode { get; set; } = turboMode;
}