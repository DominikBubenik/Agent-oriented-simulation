namespace MainLogic;

public class PersonalInspectionEvent(AirportSimulationCore core, Passenger passenger, SecurityLane lane) : AirportEvent(core, lane)
{
    private Passenger _passenger = passenger;
    public override void Execute()
    {
        GetAirportCore().NotifyLogger($"Personal Inspection Event {_passenger.ToString()},   Time= {_core.CurrentSimulationTime}    Lane= {lane.Id}");
        Lane.IsSecurityGuardBusy = false;
        Lane.PassengerAtPersonalInspection.RemoveAt(0);

        ExitAfterSecurityCheck(_passenger);
        NextSecurityControl();
        
        GetAirportCore().NotifyRefresh();
    }
}