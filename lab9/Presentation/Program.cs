using Core;
using DatabaseContext;
using DatabaseModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    private static async Task Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
            .AddDbContext<AppDbContext>(options => {
                options.UseNpgsql("Host=localhost;Port=5432;Database=lab9db;Username=postgres;Password=123");
            })
            .AddScoped<ExhibitionFacade>()
            .BuildServiceProvider();

        var facade = serviceProvider.GetService<ExhibitionFacade>();

        while (true)
        {
            Console.WriteLine("1. Add Exhibition");
            Console.WriteLine("2. Add Visitor");
            Console.WriteLine("3. Add Ticket");
            Console.WriteLine("4. Tickets Sold for Exhibition");
            Console.WriteLine("5. Unique Exhibitions Visited by Visitor");
            Console.WriteLine("6. Average Discount for Exhibition");
            Console.WriteLine("7. Exit");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Enter Exhibition Name: ");
                    var name = Console.ReadLine();
                    Console.Write("Enter Start Date (yyyy-MM-dd): ");
                    var startDate = DateTime.Parse(Console.ReadLine()!);
                    Console.Write("Enter End Date (yyyy-MM-dd): ");
                    var endDate = DateTime.Parse(Console.ReadLine()!);

                    // Save Exhibition (Пример сохранения вручную)
                    // ...
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
                    var exhibitionIdForTickets = int.Parse(Console.ReadLine()!);
                    var ticketsSold = await facade.GetTicketsSoldAsync(exhibitionIdForTickets);
                    Console.WriteLine($"Tickets Sold: {ticketsSold}");
                    break;

                case "5":
                    Console.Write("Enter Visitor ID: ");
                    var visitorIdForExhibitions = int.Parse(Console.ReadLine()!);
                    var uniqueExhibitions = await facade.GetUniqueExhibitionsVisitedAsync(visitorIdForExhibitions);
                    Console.WriteLine($"Unique Exhibitions: {uniqueExhibitions}");
                    break;

                case "6":
                    Console.Write("Enter Exhibition ID: ");
                    var exhibitionIdForDiscount = int.Parse(Console.ReadLine()!);
                    var avgDiscount = await facade.GetAverageDiscountAsync(exhibitionIdForDiscount);
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
