namespace Presentation;

public class Choices
{
    public static string GetChoice()
    {
        Console.WriteLine("1. Действия с выставками");
        Console.WriteLine("2. Действия с посетителями");
        Console.WriteLine("3. Действия с билетами");
        Console.WriteLine("4. Количество билетов, проданных на выставку");
        Console.WriteLine("5. Количество выставок, посещенных опредленным человеком");
        Console.WriteLine("6. Средняя скидка на выставку");
        
        Console.WriteLine("0. Выход");
        
        return Console.ReadLine() ?? string.Empty;
    }
    
    public static string GetExhibitionChoice()
    {
        Console.WriteLine("1. Добавить выставку");
        Console.WriteLine("2. Изменить выставку");
        Console.WriteLine("3. Удалить выставку");
        Console.WriteLine("4. Вывести список всех выставок");
        
        Console.WriteLine("0. Назад");
        
        return Console.ReadLine() ?? string.Empty;
    }
    
    public static string GetTicketChoice()
    {
        Console.WriteLine("1. Добавить билет");
        Console.WriteLine("2. Изменить билет");
        Console.WriteLine("3. Удалить билет");
        Console.WriteLine("4. Вывести список всех билетов");
        
        Console.WriteLine("0. Назад");
        
        return Console.ReadLine() ?? string.Empty;
    }
    
    public static string GetVisitorChoice()
    {
        Console.WriteLine("1. Добавить посетителя");
        Console.WriteLine("2. Изменить посетителя");
        Console.WriteLine("3. Удалить посетителя");
        Console.WriteLine("4. Вывести список всех посетителей");
        
        Console.WriteLine("0. Назад");
        
        return Console.ReadLine() ?? string.Empty;
    }
}