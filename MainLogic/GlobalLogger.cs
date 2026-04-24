namespace MainLogic;

public class GlobalLogger
{
    public static void PrintLog(string message, double currentSimTime, double timeOfEvent = -1.0)
    {
        if (timeOfEvent < 0)
        {
            Console.WriteLine($"Current simulation time: {FormatTime(currentSimTime)}, message: {message}");
            return;
        }
        Console.WriteLine($"Current simulation time: {FormatTime(currentSimTime)}, message: {message}, time of event: {FormatTime(timeOfEvent)}");
    }

    public static string FormatTime(double time, char measurement = 's')
    {
        return measurement switch
        {
            's' => TimeSpan.FromSeconds(time).ToString(@"hh\:mm\:ss"),
            _ => TimeSpan.FromMinutes(time).ToString(@"hh\:mm\:ss")
        };
    }
}