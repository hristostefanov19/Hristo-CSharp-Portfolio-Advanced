using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class TaskItem
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public bool Completed { get; set; }
}

class Program
{
    static string filePath = "tasks.txt";

    static void Main()
    {
        List<TaskItem> tasks = LoadTasks();

        while (true)
        {
            Console.WriteLine("\n--- Task Manager ---");
            Console.WriteLine("1. Show all tasks");
            Console.WriteLine("2. Add task");
            Console.WriteLine("3. Mark task as completed");
            Console.WriteLine("4. Remove task");
            Console.WriteLine("5. Exit");
            Console.Write("Choose an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ShowTasks(tasks);
                    break;
                case "2":
                    AddTask(tasks);
                    break;
                case "3":
                    CompleteTask(tasks);
                    break;
                case "4":
                    RemoveTask(tasks);
                    break;
                case "5":
                    SaveTasks(tasks);
                    return;
                default:
                    Console.WriteLine("Invalid choice!");
                    break;
            }
        }
    }

    static List<TaskItem> LoadTasks()
    {
        List<TaskItem> tasks = new List<TaskItem>();
        if (File.Exists(filePath))
        {
            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                var parts = line.Split('|');
                tasks.Add(new TaskItem
                {
                    Title = parts[0],
                    Description = parts[1],
                    DueDate = DateTime.Parse(parts[2]),
                    Completed = bool.Parse(parts[3])
                });
            }
        }
        return tasks;
    }

    static void SaveTasks(List<TaskItem> tasks)
    {
        var lines = tasks.Select(t => $"{t.Title}|{t.Description}|{t.DueDate}|{t.Completed}");
        File.WriteAllLines(filePath, lines);
    }

    static void ShowTasks(List<TaskItem> tasks)
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("No tasks found!");
            return;
        }

        foreach (var t in tasks)
        {
            Console.WriteLine($"{t.Title} | {t.Description} | Due: {t.DueDate.ToShortDateString()} | Completed: {t.Completed}");
        }
    }

    static void AddTask(List<TaskItem> tasks)
    {
        Console.Write("Title: "); string title = Console.ReadLine();
        Console.Write("Description: "); string desc = Console.ReadLine();
        Console.Write("Due Date (yyyy-mm-dd): "); DateTime due = DateTime.Parse(Console.ReadLine());
        tasks.Add(new TaskItem { Title = title, Description = desc, DueDate = due, Completed = false });
        SaveTasks(tasks);
    }

    static void CompleteTask(List<TaskItem> tasks)
    {
        Console.Write("Enter title of task to complete: "); string title = Console.ReadLine();
        var task = tasks.FirstOrDefault(t => t.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        if (task != null)
        {
            task.Completed = true;
            SaveTasks(tasks);
            Console.WriteLine("Task marked as completed.");
        }
        else Console.WriteLine("Task not found!");
    }

    static void RemoveTask(List<TaskItem> tasks)
    {
        Console.Write("Enter title of task to remove: "); string title = Console.ReadLine();
        int removed = tasks.RemoveAll(t => t.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        if (removed > 0)
        {
            SaveTasks(tasks);
            Console.WriteLine("Task removed.");
        }
        else Console.WriteLine("Task not found!");
    }
}

