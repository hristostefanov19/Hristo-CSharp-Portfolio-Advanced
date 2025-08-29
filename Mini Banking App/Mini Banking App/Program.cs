using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class BankAccount
{
    public string AccountNumber { get; set; }
    public string Owner { get; set; }
    public decimal Balance { get; set; }

    public BankAccount(string accountNumber, string owner, decimal balance)
    {
        AccountNumber = accountNumber;
        Owner = owner;
        Balance = balance;
    }

    public virtual void Display() =>
        Console.WriteLine($"{AccountNumber} | Owner: {Owner} | Balance: {Balance:C}");
}

class SavingsAccount : BankAccount
{
    public decimal InterestRate { get; set; }
    public SavingsAccount(string accountNumber, string owner, decimal balance, decimal interestRate)
        : base(accountNumber, owner, balance)
    {
        InterestRate = interestRate;
    }

    public void ApplyInterest()
    {
        Balance += Balance * InterestRate / 100;
        Console.WriteLine($"Interest applied. New balance: {Balance:C}");
    }

    public override void Display() =>
        Console.WriteLine($"{AccountNumber} | Owner: {Owner} | Balance: {Balance:C} | Interest Rate: {InterestRate}%");
}

class Program
{
    static string filePath = "accounts.txt";

    static void Main()
    {
        List<BankAccount> accounts = LoadAccounts();

        while (true)
        {
            Console.WriteLine("\n--- Mini Banking App ---");
            Console.WriteLine("1. Show all accounts");
            Console.WriteLine("2. Add account");
            Console.WriteLine("3. Deposit");
            Console.WriteLine("4. Withdraw");
            Console.WriteLine("5. Apply interest (Savings)");
            Console.WriteLine("6. Remove account");
            Console.WriteLine("7. Exit");
            Console.Write("Choose an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    foreach (var acc in accounts) acc.Display();
                    break;
                case "2":
                    AddAccount(accounts);
                    break;
                case "3":
                    Deposit(accounts);
                    break;
                case "4":
                    Withdraw(accounts);
                    break;
                case "5":
                    ApplyInterest(accounts);
                    break;
                case "6":
                    RemoveAccount(accounts);
                    break;
                case "7":
                    SaveAccounts(accounts);
                    return;
                default:
                    Console.WriteLine("Invalid choice!");
                    break;
            }
        }
    }

    static List<BankAccount> LoadAccounts()
    {
        List<BankAccount> accounts = new List<BankAccount>();
        if (File.Exists(filePath))
        {
            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                var parts = line.Split('|');
                if (parts[0] == "Savings")
                    accounts.Add(new SavingsAccount(parts[1], parts[2], decimal.Parse(parts[3]), decimal.Parse(parts[4])));
                else
                    accounts.Add(new BankAccount(parts[1], parts[2], decimal.Parse(parts[3])));
            }
        }
        return accounts;
    }

    static void SaveAccounts(List<BankAccount> accounts)
    {
        var lines = accounts.Select(a =>
        {
            if (a is SavingsAccount s)
                return $"Savings|{s.AccountNumber}|{s.Owner}|{s.Balance}|{s.InterestRate}";
            else
                return $"Regular|{a.AccountNumber}|{a.Owner}|{a.Balance}";
        });
        File.WriteAllLines(filePath, lines);
    }

    static void AddAccount(List<BankAccount> accounts)
    {
        Console.Write("Account type (Regular/Savings): "); string type = Console.ReadLine();
        Console.Write("Account Number: "); string number = Console.ReadLine();
        Console.Write("Owner: "); string owner = Console.ReadLine();
        Console.Write("Initial Balance: "); decimal balance = decimal.Parse(Console.ReadLine());

        if (type.Equals("Savings", StringComparison.OrdinalIgnoreCase))
        {
            Console.Write("Interest Rate (%): "); decimal rate = decimal.Parse(Console.ReadLine());
            accounts.Add(new SavingsAccount(number, owner, balance, rate));
        }
        else
        {
            accounts.Add(new BankAccount(number, owner, balance));
        }
        SaveAccounts(accounts);
        Console.WriteLine("Account added successfully.");
    }

    static void Deposit(List<BankAccount> accounts)
    {
        Console.Write("Account Number: "); string number = Console.ReadLine();
        var acc = accounts.FirstOrDefault(a => a.AccountNumber == number);
        if (acc != null)
        {
            Console.Write("Amount to deposit: "); decimal amt = decimal.Parse(Console.ReadLine());
            acc.Balance += amt;
            SaveAccounts(accounts);
            Console.WriteLine("Deposit successful.");
        }
        else Console.WriteLine("Account not found.");
    }

    static void Withdraw(List<BankAccount> accounts)
    {
        Console.Write("Account Number: "); string number = Console.ReadLine();
        var acc = accounts.FirstOrDefault(a => a.AccountNumber == number);
        if (acc != null)
        {
            Console.Write("Amount to withdraw: "); decimal amt = decimal.Parse(Console.ReadLine());
            if (amt <= acc.Balance)
            {
                acc.Balance -= amt;
                SaveAccounts(accounts);
                Console.WriteLine("Withdrawal successful.");
            }
            else Console.WriteLine("Insufficient funds.");
        }
        else Console.WriteLine("Account not found.");
    }

    static void ApplyInterest(List<BankAccount> accounts)
    {
        Console.Write("Account Number: "); string number = Console.ReadLine();
        var acc = accounts.FirstOrDefault(a => a.AccountNumber == number) as SavingsAccount;
        if (acc != null)
        {
            acc.ApplyInterest();
            SaveAccounts(accounts);
        }
        else Console.WriteLine("Savings account not found.");
    }

    static void RemoveAccount(List<BankAccount> accounts)
    {
        Console.Write("Account Number: "); string number = Console.ReadLine();
        int removed = accounts.RemoveAll(a => a.AccountNumber == number);
        if (removed > 0)
        {
            SaveAccounts(accounts);
            Console.WriteLine("Account removed.");
        }
        else Console.WriteLine("Account not found.");
    }
}
