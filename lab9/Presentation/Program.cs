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
            .AddScoped<TicketFacade>()
            .BuildServiceProvider();
        
        var exhibitionFacade = serviceProvider.GetService<ExhibitionFacade>();
        var visitorFacade = serviceProvider.GetService<VisitorFacade>();
        var ticketFacade = serviceProvider.GetService<TicketFacade>();
        if (exhibitionFacade != null || visitorFacade != null || ticketFacade != null)
        {
            Console.WriteLine("Не найден один или несколько фасадов");
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

                case "3":
                    var ticketChoice = Choices.GetTicketChoice();
                    
                    switch (ticketChoice)
                    {
                        case "1":
                            await ticketFacade.AddTicket();
                            break;
                        
                        case "2":
                            await ticketFacade.UpdateTicket();
                            break;

                        case "3":
                            await ticketFacade.DeleteTicket();
                            break;
                        
                        case "4":
                            await ticketFacade.GetAllTickets();
                            break;
                        
                        case "0":
                            break;
                        
                        default:
                            Console.WriteLine("Неверный ввод!");
                            break;
                    }

                    break;
                
                case "4":
                    await exhibitionFacade.GetTicketsSoldByName();
                    break;

                case "5":
                    await exhibitionFacade.GetUniqueExhibitionsVisited();
                    break;

                case "6":
                    await exhibitionFacade.GetAverageDiscount();
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
