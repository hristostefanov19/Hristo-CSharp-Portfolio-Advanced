using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Contact
{
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
}

class Program
{
    static string filePath = "contacts.txt";

    static void Main()
    {
        List<Contact> contacts = LoadContacts();

        while (true)
        {
            Console.WriteLine("\n--- Contact Book ---");
            Console.WriteLine("1. Show all contacts");
            Console.WriteLine("2. Add contact");
            Console.WriteLine("3. Edit contact");
            Console.WriteLine("4. Remove contact");
            Console.WriteLine("5. Search contacts");
            Console.WriteLine("6. Exit");
            Console.Write("Choose an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ShowContacts(contacts);
                    break;
                case "2":
                    AddContact(contacts);
                    break;
                case "3":
                    EditContact(contacts);
                    break;
                case "4":
                    RemoveContact(contacts);
                    break;
                case "5":
                    SearchContacts(contacts);
                    break;
                case "6":
                    SaveContacts(contacts);
                    return;
                default:
                    Console.WriteLine("Invalid choice!");
                    break;
            }
        }
    }

    static List<Contact> LoadContacts()
    {
        List<Contact> contacts = new List<Contact>();
        if (File.Exists(filePath))
        {
            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                var parts = line.Split('|');
                contacts.Add(new Contact { Name = parts[0], Phone = parts[1], Email = parts[2] });
            }
        }
        return contacts;
    }

    static void SaveContacts(List<Contact> contacts)
    {
        var lines = contacts.Select(c => $"{c.Name}|{c.Phone}|{c.Email}");
        File.WriteAllLines(filePath, lines);
    }

    static void ShowContacts(List<Contact> contacts)
    {
        if (contacts.Count == 0)
        {
            Console.WriteLine("No contacts found!");
            return;
        }

        foreach (var c in contacts)
            Console.WriteLine($"{c.Name} | Phone: {c.Phone} | Email: {c.Email}");
    }

    static void AddContact(List<Contact> contacts)
    {
        Console.Write("Name: "); string name = Console.ReadLine();
        Console.Write("Phone: "); string phone = Console.ReadLine();
        Console.Write("Email: "); string email = Console.ReadLine();

        contacts.Add(new Contact { Name = name, Phone = phone, Email = email });
        SaveContacts(contacts);
        Console.WriteLine("Contact added.");
    }

    static void EditContact(List<Contact> contacts)
    {
        Console.Write("Enter name of contact to edit: "); string name = Console.ReadLine();
        var contact = contacts.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        if (contact != null)
        {
            Console.Write("New Phone: "); contact.Phone = Console.ReadLine();
            Console.Write("New Email: "); contact.Email = Console.ReadLine();
            SaveContacts(contacts);
            Console.WriteLine("Contact updated.");
        }
        else Console.WriteLine("Contact not found.");
    }

    static void RemoveContact(List<Contact> contacts)
    {
        Console.Write("Enter name of contact to remove: "); string name = Console.ReadLine();
        int removed = contacts.RemoveAll(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        if (removed > 0)
        {
            SaveContacts(contacts);
            Console.WriteLine("Contact removed.");
        }
        else Console.WriteLine("Contact not found.");
    }

    static void SearchContacts(List<Contact> contacts)
    {
        Console.Write("Enter search term: "); string term = Console.ReadLine();
        var results = contacts.Where(c => c.Name.Contains(term, StringComparison.OrdinalIgnoreCase)
                                       || c.Phone.Contains(term)
                                       || c.Email.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList();
        if (results.Count == 0) Console.WriteLine("No matching contacts found.");
        else foreach (var c in results) Console.WriteLine($"{c.Name} | {c.Phone} | {c.Email}");
    }
}
