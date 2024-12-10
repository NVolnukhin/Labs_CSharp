using Core;
using DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation;
    

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
            Console.WriteLine("Не найден Exhibition Facade");
            return;
        }

        while (true)
        {
            var choice = Choices.GetChoice();

            switch (choice)
            {
                case "1":
                    var exhibitionChoice = Choices.GetExhibitionChoice();
                    
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
                    var visitorChoice = Choices.GetVisitorChoice();
                    
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
                    var ticketChoice = Choices.GetTicketChoice();
                    
                    switch (ticketChoice)
                    {
                        case "1":
                            await facade.AddTicket();
                            break;
                        
                        case "2":
                            await facade.UpdateTicket();
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
                    Console.Write("Введите имя посетителя: ");
                    var visitorFullnameForExhibitions = Console.ReadLine()!;
                    var uniqueExhibitions = await facade.GetUniqueExhibitionsVisitedAsync(visitorFullnameForExhibitions);
                    Console.WriteLine($"Посещено уникальных выставок: {uniqueExhibitions}");
                    break;

                case "6":
                    Console.Write("Введите название выставки: ");
                    var exhibitionNameForDiscount = Console.ReadLine()!;
                    var avgDiscount = await facade.GetAverageDiscountAsync(exhibitionNameForDiscount);
                    Console.WriteLine($"Средняя скидка: {avgDiscount:0.00}%");
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine("Неверный ввод!");
                    break;
            }
        }
    }
}
