namespace lab7;

public class Log
{
    public static void Write(string message)
    {
        Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff} - {message}");
    }
}