using Core;
using DatabaseContext;
using DatabaseContext.Repositories;
using DatabaseModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    private static async Task Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
            .AddDbContext<AppDbContext>(
                options => 
                {
                    options.UseNpgsql("Host=localhost;Port=5432;Database=lab9db;Username=postgres;Password=123");
                })
            
            .AddScoped<ExhibitionFacade>()
            .BuildServiceProvider();
        
        var facade = serviceProvider.GetService<ExhibitionFacade>();
        if (facade == null)
        {
            Console.WriteLine("No exhibition facade found");
            return;
        }

        while (true)
        {
            var choice = GetChoice();

            switch (choice)
            {
                case "1":
                    var exhibitionChoice = GetExhibitionChoice();
                    
                    switch (exhibitionChoice)
                    {
                        case "1":
                            await facade.AddExhibition();
                            break;
                        
                        case "2":
                            await facade.UpdateExhibition();
                            break;

                        case "3":
                            await facade.DeleteExhibition();
                            break;
                        
                        case "4":
                            await facade.GetAllExhibitions();
                            break;
                        
                        case "0":
                            break;
                        
                        default:
                            Console.WriteLine("Неверный ввод!");
                            break;
                    }

                    break;

                case "2":
                    var visitorChoice = GetVisitorChoice();
                    
                    switch (visitorChoice)
                    {
                        case "1":
                            await facade.AddVisitor();
                            break;
                        
                        case "2":
                            await facade.UpdateVisitor();
                            break;

                        case "3":
                            await facade.DeleteVisitor();
                            break;
                        
                        case "4":
                            await facade.GetAllVisitors();
                            break;
                        
                        case "0":
                            break;
                        
                        default:
                            Console.WriteLine("Неверный ввод!");
                            break;
                    }

                    break;

                case "3":
                    var ticketChoice = GetTicketChoice();
                    
                    switch (ticketChoice)
                    {
                        case "1":
                            await facade.AddTicket();
                            break;
                        
                        case "2":
                            //await facade.UpdateVisitor();
                            break;

                        case "3":
                            await facade.DeleteTicket();
                            break;
                        
                        case "4":
                            await facade.GetAllTickets();
                            break;
                        
                        case "0":
                            break;
                        
                        default:
                            Console.WriteLine("Неверный ввод!");
                            break;
                    }

                    break;
                
                case "4":
                    Console.Write("Введите название выставки: ");
                    var exhibitionNameForTickets = Console.ReadLine()!;
                    var ticketsSold = await facade.GetTicketsSoldByName(exhibitionNameForTickets);
                    Console.WriteLine($"Продано билетов: {ticketsSold}");
                    break;

                case "5":
                    Console.Write("Enter Visitor ID: ");
                    var visitorFullnameForExhibitions = Console.ReadLine()!;
                    var uniqueExhibitions = await facade.GetUniqueExhibitionsVisitedAsync(visitorFullnameForExhibitions);
                    Console.WriteLine($"Unique Exhibitions: {uniqueExhibitions}");
                    break;

                case "6":
                    Console.Write("Enter Exhibition ID: ");
                    var exhibitionNameForDiscount = Console.ReadLine()!;
                    var avgDiscount = await facade.GetAverageDiscountAsync(exhibitionNameForDiscount);
                    Console.WriteLine($"Average Discount: {avgDiscount:F2}%");
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine("Invalid choice!");
                    break;
            }
        }
    }

    private static string GetChoice()
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
    
    private static string GetExhibitionChoice()
    {
        Console.WriteLine("1. Добавить выставку");
        Console.WriteLine("2. Изменить выставку");
        Console.WriteLine("3. Удалить выставку");
        Console.WriteLine("4. Вывести список всех выставок");
        
        Console.WriteLine("0. Назад");
        
        return Console.ReadLine() ?? string.Empty;
    }
    
    private static string GetTicketChoice()
    {
        Console.WriteLine("1. Добавить билет");
        Console.WriteLine("2. Изменить билет");
        Console.WriteLine("3. Удалить билет");
        Console.WriteLine("4. Вывести список всех билетов");
        
        Console.WriteLine("0. Назад");
        
        return Console.ReadLine() ?? string.Empty;
    }
    
    private static string GetVisitorChoice()
    {
        Console.WriteLine("1. Добавить посетителя");
        Console.WriteLine("2. Изменить посетителя");
        Console.WriteLine("3. Удалить посетителя");
        Console.WriteLine("4. Вывести список всех посетителей");
        
        Console.WriteLine("0. Назад");
        
        return Console.ReadLine() ?? string.Empty;
    }
}
