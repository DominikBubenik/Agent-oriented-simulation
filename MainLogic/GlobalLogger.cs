namespace MainLogic;

public class GlobalLogger
{
    public static void PrintLog(string message, double currentSimTime, double timeOfEvent = -1.0)
    {
        return;
        if (timeOfEvent < 0)
        {
            Console.WriteLine($"Current simulation time: {FormatTime(currentSimTime)}, message: {message}");
            return;
        }
        Console.WriteLine($"Current simulation time: {FormatTime(currentSimTime)}, message: {message}, time of event: {FormatTime(timeOfEvent)}");
    }

    public static string FormatTime(double time, char measurement = 's')
    {
        var ts = measurement == 's'
            ? TimeSpan.FromSeconds(time)
            : TimeSpan.FromMinutes(time);

        return $"{ts.Days} day {ts:hh\\:mm\\:ss}";
    }
}