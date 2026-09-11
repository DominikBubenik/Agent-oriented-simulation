namespace MainLogic;

public class FinishedScanningEvent(AirportSimulationCore core, Luggage luggage, SecurityLane lane) : AirportEvent(core, lane)
{
    private Luggage _luggage = luggage;
    public override void Execute()
    {
        GetAirportCore().NotifyLogger($"Finished luggage scan Event {_luggage.ToString()},   Time= {_core.CurrentSimulationTime}   Lane= {Lane.Id}");
        
        Lane.LuggageInsideDetector.RemoveAt(0);
        if (Lane.WaitForLuggageQueue.IsEmpty())
        {
            Lane.EnqueueItemToQueue(GetAirportCore().CommonAfterLugDetectorTrackQueueStats, Lane._afterDetectorTrack, _luggage,  _core.CurrentSimulationTime);
        }
        else
        {
            var waitingPassenger = Lane.WaitForLuggageQueue.Peek();
            waitingPassenger.CurrentlyHolding++;
            if (waitingPassenger.CurrentlyHolding == waitingPassenger.LuggageCount)
            {
                // GlobalLogger.PrintLog($"Passenger exit system {waitingPassenger.ToString()}", _core.CurrentSimulationTime);
                Lane.DequeueItemFromQueue(GetAirportCore().CommonWaitForLuggageQueueStats, Lane.WaitForLuggageQueue, _core.CurrentSimulationTime);
                GetAirportCore().avgTimeInSystem.AddSample(_core.CurrentSimulationTime - waitingPassenger.ArrivalTime);
            }
        }
        Lane.IsLuggageScannerBusy = false;
        StartLuggageScanning();
        
        GetAirportCore().NotifyRefresh();
    }
}