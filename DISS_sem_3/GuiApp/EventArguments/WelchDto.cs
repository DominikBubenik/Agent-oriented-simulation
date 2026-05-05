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
    //
    public double TotalPatientsInSystem { get; set; }
    public double TotalWalkInPatientsInSystem { get; set; }
    public double TotalAmbulancePatientsInSystem { get; set; }
    public double TotalTimeInSystemAll { get; set; }
    public double TotalTimeInSystemWalkIn { get; set; }
    public double TotalTimeInSystemAmbulance { get; set; }
    public double EntryQueueWaitingTimeWalkIn { get; set; }
    public double EntryQueueWaitingTimeAmbulanced { get; set; }
    public double EntryQueueLength { get; set; }
    public double MedicalTreatWaitingTimeA { get; set; }
    public double MedicalTreatWaitingTimeAB { get; set; }
    public double MedicalTreatWaitingTimeB { get; set; }
    public double AllDoctorsUtil { get; set; }
    public double AllNursesUtil { get; set; }
    public double AllRoomsAUtil { get; set; }
    public double AllRoomsBUtil { get; set; }
    public double FromEntryToMedicalWalkIn { get; set; }
    public double FromEntryToMedicalAmbulanced { get; set; }
    public double MedicalQueueLengthTypeA { get; set; }
    public double MedicalQueueLengthTypeB { get; set; }    
}