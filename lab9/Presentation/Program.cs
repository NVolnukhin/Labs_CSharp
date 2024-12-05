using Core;
using Core.Services;
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

        while (true)
        {
            Console.WriteLine("1. Добавить выставку");
            Console.WriteLine("2. Добавить посетителя");
            Console.WriteLine("3. Добавить билет");
            Console.WriteLine("4. Количество билетов, проданных на выставку");
            Console.WriteLine("5. Количество выставок, посещенных опредленным человеком");
            Console.WriteLine("6. Средняя скидка на выставку");
            Console.WriteLine("7. Выход");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    facade.AddExhibition();
                    
                    break;

                case "2":
                    Console.Write("Enter Visitor Name: ");
                    var visitorName = Console.ReadLine();
                    Console.Write("Enter Visitor Email: ");
                    var email = Console.ReadLine(); 

                    // Save Visitor (Пример сохранения вручную)
                    // ...
                    break;

                case "3":
                    Console.Write("Enter Exhibition ID: ");
                    var exhibitionId = int.Parse(Console.ReadLine()!);
                    Console.Write("Enter Visitor ID: ");
                    var visitorId = int.Parse(Console.ReadLine()!);
                    Console.Write("Enter Discount (%): ");
                    var discount = double.Parse(Console.ReadLine()!);

                    // Save Ticket (Пример сохранения вручную)
                    // ...
                    break;

                case "4":
                    Console.Write("Enter Exhibition ID: ");
                    var exhibitionNameForTickets = Console.ReadLine()!;
                    var ticketsSold = await facade.GetTicketsSoldAsync(exhibitionNameForTickets);
                    Console.WriteLine($"Tickets Sold: {ticketsSold}");
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

                case "7":
                    return;

                default:
                    Console.WriteLine("Invalid choice!");
                    break;
            }
        }
    }
}
