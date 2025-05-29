using System;
using System.Collections.Generic;
using System.Linq;
using C_Lesson12_Bank.Models;

public class HelloWorld
{
    public static void Main(string[] args)
    {
        User[] users = new User[5];

        users[0] = new User("Ali", "Mammadov", new BankCard("Kapital Bank", "Ali Mammadov", "1234567890123456", "4569", "123", new DateTime(2026, 5, 31), 1000));
        users[1] = new User("Aysel", "Aliyeva", new BankCard("ABB", "Aysel Aliyeva", "2345678901234567", "1256", "456", new DateTime(2025, 12, 31), 2000));
        users[2] = new User("Murad", "Ismayilov", new BankCard("PASHA Bank", "Murad Ismayilov", "3456789012345678", "7823", "789", new DateTime(2024, 10, 30), 1500));
        users[3] = new User("Nigar", "Huseynova", new BankCard("Bank Respublika", "Nigar Huseynova", "4567890123456789", "9641", "321", new DateTime(2027, 6, 30), 3000));
        users[4] = new User("Elchin", "Quliyev", new BankCard("AccessBank", "Elchin Quliyev", "5678901234567890", "3265", "654", new DateTime(2025, 9, 15), 500));

        Bank bank = new Bank();
        bank.Clients = users;

        List<(DateTime Time, string Description)> operationHistory = new List<(DateTime, string)>();

        Console.WriteLine(@"+------------------------------------------------+
|              WELCOME TO GENERAL BANK ATM       |
+------------------------------------------------+");

        Console.Write("Please enter PIN number: ");
        string pin = Console.ReadLine();
        while (!bank.CheckPin(pin))
        {
            Console.WriteLine("Incorrect PIN. Try again.");
            Console.Write("Please enter PIN number: ");
            pin = Console.ReadLine();
        }

        User currentUser = bank.FindUserByPIN(pin);
        if (currentUser == null)
        {
            Console.WriteLine("User not found.");
            return;
        }

        string[] menuOptions = new string[]
        {
            "Show Card Information",
            "Check Balance",
            "Withdraw Cash",
            "Transaction History",
            "Transfer Money",
            "Exit"
        };

        int selectedIndex = 0;

        while (true)
        {
            Console.Clear();
            Console.WriteLine("+------------------------------------------------+");
            Console.WriteLine("|              GENERAL BANK MAIN MENU            |");
            Console.WriteLine("+------------------------------------------------+\n");

            for (int i = 0; i < menuOptions.Length; i++)
            {
                if (i == selectedIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"-> {menuOptions[i]}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"   {menuOptions[i]}");
                }
            }

            ConsoleKeyInfo key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.UpArrow)
            {
                selectedIndex = (selectedIndex == 0) ? menuOptions.Length - 1 : selectedIndex - 1;
            }
            else if (key.Key == ConsoleKey.DownArrow)
            {
                selectedIndex = (selectedIndex == menuOptions.Length - 1) ? 0 : selectedIndex + 1;
            }
            else if (key.Key == ConsoleKey.Enter)
            {
                Console.Clear();
                var card = currentUser.CreditCard;

                switch (selectedIndex)
                {
                    case 0:
                        Console.WriteLine($@"
┌────────────────────────────────────────────────────────────────────────────┐
│                             CREDIT CARD                                    │
│                                                                            │
│   Card Holder:   {currentUser.Name} {currentUser.SurName,-54}│
│   Card Number:   {card.PAN,-58}│
│   Expiry Date:   {card.ExpireDate.ToString("dd/MM/yy"),-58}│
│                                                                            │
│                                                          Bank: {card.BankName,-10}│
└────────────────────────────────────────────────────────────────────────────┘
");
                        break;

                    case 1:
                        bank.UserBalance(pin);
                        break;

                    case 2: // money cash
                        string[] cashOptions = { "10 AZN", "20 AZN", "50 AZN", "100 AZN", "Custom Amount", "Back" };
                        int cashIndex = 0;
                        while (true)
                        {
                            Console.Clear();
                            Console.WriteLine("Choose an amount to withdraw:\n");

                            for (int i = 0; i < cashOptions.Length; i++)
                            {
                                if (i == cashIndex)
                                {
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine($"-> {cashOptions[i]}");
                                    Console.ResetColor();
                                }
                                else
                                {
                                    Console.WriteLine($"   {cashOptions[i]}");
                                }
                            }

                            var cashKey = Console.ReadKey(true);
                            if (cashKey.Key == ConsoleKey.UpArrow)
                                cashIndex = (cashIndex == 0) ? cashOptions.Length - 1 : cashIndex - 1;
                            else if (cashKey.Key == ConsoleKey.DownArrow)
                                cashIndex = (cashIndex == cashOptions.Length - 1) ? 0 : cashIndex + 1;
                            else if (cashKey.Key == ConsoleKey.Enter)
                            {
                                if (cashIndex == cashOptions.Length - 1) break; 
                                int amount = 0;
                                if (cashIndex == 0) amount = 10;
                                else if (cashIndex == 1) amount = 20;
                                else if (cashIndex == 2) amount = 50;
                                else if (cashIndex == 3) amount = 100;
                                else if (cashIndex == 4)
                                {
                                    Console.Write("Enter custom amount: ");
                                    int.TryParse(Console.ReadLine(), out amount);
                                }

                                if (amount > 0)
                                {
                                    bank.Cash(pin, amount);
                                    operationHistory.Add((DateTime.Now, $"Withdrew {amount} AZN from balance."));
                                }

                                Console.WriteLine("\nPress any key to return...");
                                Console.ReadKey();
                                break;
                            }
                        }
                        break;

                    case 3: // history
                        string[] historyOptions = { "Last 1 day", "Last 5 days", "Last 10 days", "Back" };
                        int historyIndex = 0;
                        while (true)
                        {
                            Console.Clear();
                            Console.WriteLine("View Transaction History:\n");
                            for (int i = 0; i < historyOptions.Length; i++)
                            {
                                if (i == historyIndex)
                                {
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine($"-> {historyOptions[i]}");
                                    Console.ResetColor();
                                }
                                else
                                {
                                    Console.WriteLine($"   {historyOptions[i]}");
                                }
                            }

                            var hKey = Console.ReadKey(true);
                            if (hKey.Key == ConsoleKey.UpArrow)
                                historyIndex = (historyIndex == 0) ? historyOptions.Length - 1 : historyIndex - 1;
                            else if (hKey.Key == ConsoleKey.DownArrow)
                                historyIndex = (historyIndex == historyOptions.Length - 1) ? 0 : historyIndex + 1;
                            else if (hKey.Key == ConsoleKey.Enter)
                            {
                                if (historyIndex == 3) break;
                                int days = historyIndex switch
                                {
                                    0 => 1,
                                    1 => 5,
                                    2 => 10,
                                    _ => 0
                                };

                                var filtered = operationHistory
                                    .Where(op => op.Time >= DateTime.Now.AddDays(-days)).ToList();

                                Console.Clear();
                                Console.WriteLine($"Transactions from last {days} day(s):\n");
                                if (filtered.Count == 0)
                                    Console.WriteLine("No transactions.");
                                else
                                    foreach (var entry in filtered)
                                        Console.WriteLine($"{entry.Time}: {entry.Description}");

                                Console.WriteLine("\nPress any key to return...");
                                Console.ReadKey();
                                break;
                            }
                        }
                        break;

                    case 4:
                        Console.Write("Enter recipient's PIN: ");
                        string secondPin = Console.ReadLine();
                        var recipient = bank.FindUserByPIN(secondPin);
                        if (recipient == null)
                        {
                            Console.WriteLine("Invalid PIN.");
                            break;
                        }
                        Console.Write("Enter amount to transfer: ");
                        if (int.TryParse(Console.ReadLine(), out int transferAmount))
                        {
                            bank.TransferMoney(pin, secondPin, transferAmount);
                            operationHistory.Add((DateTime.Now, $"Transferred {transferAmount} AZN to {secondPin}"));
                        }
                        break;

                    case 5:
                        Console.WriteLine("Thank you for using General Bank ATM. Goodbye!");
                        return;
                }

                //Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey(true);
            }
        }
    }
}
