using DISS_sem_3;

namespace DISS_SEM_GUI.EventsArguments;

public class StartSimulationArgs(
    int seed,
    int replications,
    bool observationMode,
    double endSimulationTime,
    int nursesCount,
    int doctorsCount,
    int entryMax,
    int medicalMax,
    double timeIntervalSeconds,
    ResourceAllocatingStrategy experimentVariant,
    bool warmUpProof,
    double exp4WaitTime,
    bool allocateBestRoom = false,
    double warmUp = 15000,
    bool turboMode = false)
{
    public int Seed { get; set; } = seed;
    public int Replications { get; set; } = replications;
    public bool ObservationMode { get; set; } = observationMode;
    public bool AllocateBestRoom { get; set; } = allocateBestRoom;
    public double EndSimulationTime { get; set; } = endSimulationTime;
    public int NursesCount { get; set; } = nursesCount;
    public int DoctorsCount { get; set; } = doctorsCount;
    public int EntryMax { get; set; } = entryMax;
    public int MedicalMax { get; set; } = medicalMax;
    public double TimeIntervalSeconds { get; set; } = timeIntervalSeconds;
    public ResourceAllocatingStrategy ExperimentVariant { get; set; } = experimentVariant;
    public bool WarmUpProof { get; set; } = warmUpProof;
    public double WarmUp { get; set; } = warmUp;
    public double Exp4MaxWaitTime { get; set; } = exp4WaitTime;
    public bool TurboMode { get; set; } = turboMode;
}