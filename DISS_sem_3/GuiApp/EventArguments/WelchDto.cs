namespace DISS_SEM_GUI.EventsArguments;

public class WelchDto
{
    public double CurrentTime { get; set; }

    public double TotalPatientsInSystem { get; set; }
    public double TotalWalkInInSystem { get; set; }
    public double TotalAmbulancedInSystem { get; set; }

    public double TotalTimeInSystem { get; set; }
    public double TotalTimeInSystemWalkIn { get; set; }
    public double TotalTimeInSystemAmbulanced { get; set; }

    public double EntryQueueWaitWalkIn { get; set; }
    public double EntryQueueWaitAmbulanced { get; set; }
    public double EntryQueueLength { get; set; }

    public double MedicalTreatWaitingTimeA { get; set; }
    public double MedicalTreatWaitingTimeAB { get; set; }
    public double MedicalTreatWaitingTimeB { get; set; }

    public double AllDoctorsUtil { get; set; }
    public double AllNursesUtil { get; set; }
    public double AllRoomAUtil { get; set; }
    public double AllRoomBUtil { get; set; }
}