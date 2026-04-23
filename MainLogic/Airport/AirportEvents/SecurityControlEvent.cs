namespace MainLogic;

public class SecurityControlEvent(AirportSimulationCore core, Passenger passenger, SecurityLane lane) : AirportEvent(core, lane)
{
    private Passenger _passenger = passenger;
    public override void Execute()
    {
        GetAirportCore().NotifyLogger($"Security Control Event {_passenger.ToString()},   Time= {_core.CurrentSimulationTime}   Lane= {lane.Id}");
        
        Lane.IsSecurityGuardBusy = false;
        Lane.PassengerInsideDetector.RemoveAt(0);

        if (_passenger.CurrentlyHolding > 0) throw new Exception("Passenger has luggage on security control");
        
        if (GetAirportCore().GoToPersonalInspection())
        {
            Lane.IsSecurityGuardBusy = true;  
            Lane.PassengerAtPersonalInspection.Add(_passenger);
            var personalInspection = new PersonalInspectionEvent(GetAirportCore(), _passenger, Lane)
            {
                Time = _core.CurrentSimulationTime + GetAirportCore().GetPersonalInspectionDuration()
            };
            _core.PlanEvent(personalInspection);
        }
        else
        {
            ExitAfterSecurityCheck(_passenger);
            NextSecurityControl();
        }
        
        GetAirportCore().NotifyRefresh();
    }
}