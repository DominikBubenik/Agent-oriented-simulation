using MainLogic;

public abstract class AirportEvent(AirportSimulationCore core, SecurityLane lane) : SimulationEvent(core)
{
    protected SecurityLane Lane = lane; 

    protected AirportSimulationCore GetAirportCore() => (AirportSimulationCore)_core;

    protected void ExitAfterSecurityCheck(Passenger passenger)
    {   
        if (passenger.LuggageCount == 0)
        {
            GlobalLogger.PrintLog($"Passenger exit from system {passenger.ToString()}", _core.CurrentSimulationTime);
            GetAirportCore().avgTimeInSystem.AddSample(_core.CurrentSimulationTime - passenger.ArrivalTime);
            return;
        }
        
        if (Lane._afterDetectorTrack.IsEmpty())
        {
            Lane.EnqueueItemToQueue(GetAirportCore().CommonWaitForLuggageQueueStats, Lane.WaitForLuggageQueue, passenger, _core.CurrentSimulationTime);
        }
        else
        {
            while (passenger.CurrentlyHolding < passenger.LuggageCount && !Lane._afterDetectorTrack.IsEmpty())
            {
                Lane.DequeueItemFromQueue(GetAirportCore().CommonAfterLugDetectorTrackQueueStats, Lane._afterDetectorTrack,
                    _core.CurrentSimulationTime);
                passenger.CurrentlyHolding++;
            }

            if (passenger.CurrentlyHolding == passenger.LuggageCount)
            {
                GetAirportCore().avgTimeInSystem.AddSample(_core.CurrentSimulationTime - passenger.ArrivalTime);
                GlobalLogger.PrintLog($"Passenger exit from system {passenger.ToString()}", _core.CurrentSimulationTime);
            }
            else
            {
                Lane.EnqueueItemToQueue(GetAirportCore().CommonWaitForLuggageQueueStats, Lane.WaitForLuggageQueue, passenger, _core.CurrentSimulationTime);
            }
            
            StartLuggageScanning();
        }
    }

    protected void NextSecurityControl()
    {
        if (Lane.IsSecurityGuardBusy || Lane.DetectorQueue.IsEmpty()) return;
        
        Lane.IsSecurityGuardBusy = true;
        var passenger = Lane.DequeueItemFromQueue(GetAirportCore().CommonDetectorQueueStats, Lane.DetectorQueue, _core.CurrentSimulationTime);
        Lane.PassengerInsideDetector.Add(passenger);
        var newSecurityEvent = new SecurityControlEvent(GetAirportCore(), passenger, Lane)
        {
            Time = _core.CurrentSimulationTime + GetAirportCore().GetSecurityControlDuration()
        };
        _core.PlanEvent(newSecurityEvent);
    }

    protected void StartLuggageScanning()
    {
        if (!Lane._beforeDetectorTrack.IsEmpty() && Lane._afterDetectorTrack.Count < Lane.MAX_AFTER_DETECTOR_COUNT && !Lane.IsLuggageScannerBusy)
        {
            Lane.IsLuggageScannerBusy = true;
            var nextLuggage = Lane.DequeueItemFromQueue(GetAirportCore().CommonBeforeLugDetectorTrackQueueStats, Lane._beforeDetectorTrack, _core.CurrentSimulationTime);
            Lane.LuggageInsideDetector.Add(nextLuggage);
            var newFinishedScanning = new FinishedScanningEvent(GetAirportCore(), nextLuggage, Lane);
            newFinishedScanning.Time =  _core.CurrentSimulationTime + GetAirportCore().GetLuggageScanDuration();
            _core.PlanEvent(newFinishedScanning);
        }
        StartInitiatePassengerToLoadLuggage(); 
    }

    protected void StartInitiatePassengerToLoadLuggage()
    {
        if (Lane._beforeDetectorTrack.Count >= Lane.MAX_BEFORE_DETECTOR_COUNT ||
            Lane._passengersEntryQueue.IsEmpty()) return;
        
        var passenger = Lane._passengersEntryQueue.Peek();
        LoadLuggage(passenger, true);
    }

    protected void LoadLuggage(Passenger passenger, bool dequeueIfPossible = false)
    {
        while (passenger.CurrentlyHolding > 0 && Lane._beforeDetectorTrack.Count < Lane.MAX_BEFORE_DETECTOR_COUNT)
        {
            var index = passenger.LuggageCount - passenger.CurrentlyHolding;
            Lane.EnqueueItemToQueue(GetAirportCore().CommonBeforeLugDetectorTrackQueueStats, Lane._beforeDetectorTrack, passenger.LuggageList[index], _core.CurrentSimulationTime);
            passenger.CurrentlyHolding--;
        }

        if (dequeueIfPossible && passenger.CurrentlyHolding == 0)
        {
            var nextPass = Lane.DequeueItemFromQueue(GetAirportCore().CommonEntryQueueStats, Lane._passengersEntryQueue, _core.CurrentSimulationTime);
            var loadTraysEvent = new AfterLoadTraysEvent(GetAirportCore(), Lane, nextPass, _core.CurrentSimulationTime);
            GetAirportCore().PlanEvent(loadTraysEvent);
        }
    }
}