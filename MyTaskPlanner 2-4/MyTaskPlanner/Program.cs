using Microsoft.Extensions.DependencyInjection;
using MyTaskPlanner.Domain.Logic;
using MyTaskPlanner.Domain.Models.Enums;
using MyTaskPlanner.Domain.Models.Models;
using TaskPlanner.DataAccess.Abstractions;

internal static class Program
{
    private static SimpleTaskPlanner _simpleTaskPlanner;
    public static void Main(string[] args)
    {
        var service = new ServiceCollection()
            .AddSingleton<SimpleTaskPlanner>()
            .AddScoped<IWorkItemsRepository, FileWorkItemsRepository>()
            .BuildServiceProvider();

        _simpleTaskPlanner = service.GetService<SimpleTaskPlanner>();

        PrintMenu();
    }
    public static void PrintMenu()
    {
        void Actions(string choice, ref bool isRunning)
        {
            switch (choice)
            {
                case "A":
                    _simpleTaskPlanner.WorkRepository.Add(InitItem());
                    break;
                case "B":
                    var lists = _simpleTaskPlanner.CreatePlan();
                    foreach (var item in lists)
                        Console.WriteLine(item.ToString());

                    break;
                case "M":
                    Console.WriteLine("Введіть ключ");
                    _simpleTaskPlanner.WorkRepository.Get(Guid.Parse(Console.ReadLine())).IsCompleted = true;
                    _simpleTaskPlanner.WorkRepository.SaveChanges();
                    break;
                case "R":
                    Console.WriteLine("Введіть ключ");
                    _simpleTaskPlanner.WorkRepository.Remove(Guid.Parse(Console.ReadLine()));
                    break;
                case "Q":
                    isRunning = false;
                    break;
                default:
                    Console.WriteLine("Невідома команда. Спробуйте ще раз.");
                    break;
            }
        }

        List<string> workItems = new List<string>();
        bool isRunning = true;

        while (isRunning)
        {
            Console.WriteLine("Оберіть операцію:");
            Console.WriteLine("[A]dd work item");
            Console.WriteLine("[B]uild a plan");
            Console.WriteLine("[M]ark work item as completed");
            Console.WriteLine("[R]emove a work item");
            Console.WriteLine("[Q]uit the app");

            string choice = Console.ReadLine();

            Actions(choice, ref isRunning);
        }
    }

    public static WorkItem InitItem()
    {
        try
        {
            WorkItem workItem = new();
            Console.WriteLine("Title:");
            workItem.Title = Console.ReadLine();

            workItem.CreationDate = DateTime.Now;
            workItem.IsCompleted = false;
            workItem.DueDate = workItem.CreationDate.AddDays(1);

            Console.WriteLine("Due Date");
            workItem.DueDate = DateTime.Parse(Console.ReadLine());

            Console.WriteLine("Description");
            workItem.Description = Console.ReadLine();
            Console.WriteLine("Complexity");
            workItem.Complexity = (Complexity)int.Parse(Console.ReadLine());
            Console.WriteLine("Priority");
            workItem.Priority = (Priority)int.Parse(Console.ReadLine());

            return workItem;
        }
        catch (Exception)
        {
            Console.WriteLine("Помилка введення!");
            throw;
        }
    }
}