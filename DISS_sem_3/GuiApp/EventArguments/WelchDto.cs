namespace DISS_SEM_GUI.EventsArguments;

public class WelchDto
{
    public double CurrentTime { get; set; }

    public int CurrentPatientCount { get; set; }
    public int CurrentWalkInPatientCount { get; set; }
    public int CurrentAmbulancePatientCount { get; set; }
    public int CurrentEntryQueueLength { get; set; }
    public int CurrentMedicalTreatWaitingCount { get; set; }
    public double CurrentAllDoctorsUtil { get; set; }
    public double CurrentAllNursesUtil { get; set; }
    public double CurrentAllRoomAUtil { get; set; }
    public double CurrentAllRoomBUtil { get; set; }
}