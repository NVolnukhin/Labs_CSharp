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
            .AddScoped<VisitorFacade>()
            .BuildServiceProvider();
        
        var exhibitionFacade = serviceProvider.GetService<ExhibitionFacade>();
        var visitorFacade = serviceProvider.GetService<VisitorFacade>();
        

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
                            await exhibitionFacade.AddExhibition();
                            break;
                        
                        case "2":
                            await exhibitionFacade.UpdateExhibition();
                            break;

                        case "3":
                            await exhibitionFacade.DeleteExhibition();
                            break;
                        
                        case "4":
                            await exhibitionFacade.GetAllExhibitions();
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
                            await visitorFacade.AddVisitor();
                            break;
                        
                        case "2":
                            await visitorFacade.UpdateVisitor();
                            break;

                        case "3":
                            await visitorFacade.DeleteVisitor();
                            break;
                        
                        case "4":
                            await visitorFacade.GetAllVisitors();
                            break;
                        
                        case "0":
                            break;
                        
                        default:
                            Console.WriteLine("Неверный ввод!");
                            break;
                    }

                    break;
/*
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
 */               
                case "4":
                    Console.Write("Введите название выставки: ");
                    var exhibitionNameForTickets = Console.ReadLine()!;
                    var ticketsSold = await exhibitionFacade.GetTicketsSoldByName(exhibitionNameForTickets);
                    Console.WriteLine($"Продано билетов: {ticketsSold}");
                    break;

                case "5":
                    Console.Write("Введите имя посетителя: ");
                    var visitorFullnameForExhibitions = Console.ReadLine()!;
                    var uniqueExhibitions = await exhibitionFacade.GetUniqueExhibitionsVisitedAsync(visitorFullnameForExhibitions);
                    Console.WriteLine($"Посещено уникальных выставок: {uniqueExhibitions}");
                    break;

                case "6":
                    Console.Write("Введите название выставки: ");
                    var exhibitionNameForDiscount = Console.ReadLine()!;
                    var avgDiscount = await exhibitionFacade.GetAverageDiscountAsync(exhibitionNameForDiscount);
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
