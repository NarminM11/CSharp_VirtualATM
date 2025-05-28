using System;
using System.Net.NetworkInformation;
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

        // foreach (var user in users)
        // {
        //     Console.WriteLine(user);
        // }

        Bank bank = new Bank();
        bank.Clients = users;

        List<(DateTime Time, string Description)> operationHistory = new List<(DateTime, string)>(); //userin etdiyi emeliyyatlari liste yigmaqa

        //pin-i dogrulama
        User currentUser;
        Console.WriteLine(@"+------------------------------------------------+
|              WELCOME TO GENERAL BANK ATM       |
+------------------------------------------------+");
        string pin;
        Console.Write("Please enter PIN number: ");
        pin = Console.ReadLine();
        while (!bank.CheckPin(pin))
        {
            Console.WriteLine("Incorrect PIN. Try again.");
            Console.Write("Please enter PIN number: ");
            pin = Console.ReadLine();
        }
        bank.SearchByPIN(pin);
        while (true)
        {
            currentUser = bank.FindUserByPIN(pin);
            if (currentUser == null)
            {
                Console.WriteLine("User not found for this PIN.");
                continue;
            }
            var card = currentUser.CreditCard;

            Console.WriteLine(@"+------------------------------------------------+
| 1. Check Balance                               |
| 2. Withdraw Cash                               |
| 3. Transaction History                         |
| 4. Transfer Money                              |
| 5. Exit                                        |
+------------------------------------------------+
Please enter your choice: _
");
            //Console.WriteLine("\nSelect operation:");
            //Console.WriteLine("0.Show card informations");
            //Console.WriteLine("1. Check Balance");
            //Console.WriteLine("2. Withdraw Cash");
            //Console.WriteLine("3. Check bank card history");
            //Console.WriteLine("4. Transfer money");
            //Console.WriteLine("5. Exit");
            //Console.Write("Your choice: ");

            string input = Console.ReadLine();
            int operation_choice;
            string operation;

            if (!int.TryParse(input, out operation_choice))
            {
                Console.WriteLine("Please enter a valid number!");
                continue;
            }

            if (operation_choice == 0)
            {
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
            }
            else if (operation_choice == 1)
            {
                bank.UserBalance(pin);
                operationHistory.Add((DateTime.Now, "Withdrew 10 AZN from balance."));

            }
            else if (operation_choice == 2)
            {
                Console.WriteLine("Enter cash amount: 1. 10AZN  2. 20AZN  3. 50AZN  4. 100AZN  5. Enter your own amount");
                string cashInput = Console.ReadLine();
                int cash_choice;

                if (!int.TryParse(cashInput, out cash_choice))
                {
                    Console.WriteLine("Invalid input!");
                    continue;
                }

                if (cash_choice == 1)
                {
                    bank.Cash(pin, 10);
                    operationHistory.Add((DateTime.Now, "Withdrew 10 AZN from balance."));
                }
                else if (cash_choice == 2) { 
                    bank.Cash(pin, 20);
                    operationHistory.Add((DateTime.Now, "Withdrew 20 AZN from balance."));
                }
                else if (cash_choice == 3) { 
                    bank.Cash(pin, 50);
                    operationHistory.Add((DateTime.Now, "Withdrew 50 AZN from balance."));
                }
                else if (cash_choice == 4)
                {
                    bank.Cash(pin, 100);
                    operationHistory.Add((DateTime.Now, "Withdrew 100 AZN from balance."));

                }
                else if (cash_choice == 5)
                {
                    Console.Write("Enter custom cash amount: ");
                    if (int.TryParse(Console.ReadLine(), out int customAmount))
                    {
                        bank.Cash(pin, customAmount);
                        operationHistory.Add((DateTime.Now, $"Withdrew {customAmount} AZN from balance."));

                    }
                    else
                    {
                        Console.WriteLine("Invalid amount entered.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid choice.");
                }

            }
            else if (operation_choice == 3)
            {
                Console.WriteLine("Choose: 1.Last 1 day 2.Last 5 day 3.Last 10 day");
                string choiceInput = Console.ReadLine();
                int choice;
                if (!int.TryParse(choiceInput, out choice))
                {
                    Console.WriteLine("Invalid input!");
                    continue;
                }

                List<(DateTime Time, string Description)> filteredHistory = null;

                if (choice == 1)
                {
                    filteredHistory = operationHistory
                                            .Where(op => op.Time >= DateTime.Now.AddDays(-1))
                                            .ToList();
                }
                else if (choice == 2)
                {
                    filteredHistory = operationHistory
                                            .Where(op => op.Time >= DateTime.Now.AddDays(-5))
                                            .ToList();
                }
                else if (choice == 3)
                {
                    filteredHistory = operationHistory
                                            .Where(op => op.Time >= DateTime.Now.AddDays(-10))
                                            .ToList();
                }
                else
                {
                    Console.WriteLine("Invalid choice.");
                    continue;
                }

                Console.WriteLine("\n--- Transaction History ---");
                foreach (var entry in filteredHistory)
                {
                    Console.WriteLine($"{entry.Time}: {entry.Description}");
                }
            }
            else if (operation_choice == 4)
            {
                Console.Write("Please enter second PIN number: ");
                string second_pin = Console.ReadLine();
                User recipientUser = bank.FindUserByPIN(second_pin);
                if (recipientUser == null)
                {
                    Console.WriteLine("The receiver's PIN is invalid.");
                    continue;
                }
                bank.SearchByPIN(second_pin);
                Console.Write("Enter custom cash amount: ");
                if (int.TryParse(Console.ReadLine(), out int transfer_amount))
                {
                    bank.TransferMoney(pin, second_pin, transfer_amount);
                    operationHistory.Add((DateTime.Now, $"Transferred {transfer_amount} AZN to card with PIN {second_pin}"));

                }
            }
            else if (operation_choice == 5)
            {
                Console.WriteLine("Exiting the system. Goodbye!");
                break;
            }
            else
            {
                Console.WriteLine("Invalid operation choice.");
            }
        }
    }
}