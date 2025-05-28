using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C_Lesson12_Bank.Models;
namespace C_Lesson12_Bank.Models
{
    public class Bank
    {
        public User[] Clients { get; set; }
        public BankCard[] Cards { get; set; }


        public void SearchByPIN(string pin)
        {
            User user = FindUserByPIN(pin);

            if (user != null)
            {
                Console.WriteLine($"Welcome, {user.Name} {user.SurName}");
            }
            else
            {
                Console.WriteLine("No user with such PIN was found.");
            }
        }

        public User FindUserByPIN(string pin)
        {
            if (Clients == null) return null;

            foreach (User user in Clients)
            {
                if (user != null && user.CreditCard != null && user.CreditCard.PIN == pin)
                {
                    return user;
                }
            }

            return null;
        }


        public void UserBalance(string pin)
        {
            foreach (User user in Clients)
            {
                if (user != null && user.CreditCard != null && user.CreditCard.PIN == pin)
                {
                    Console.WriteLine($"Your balance is {user.CreditCard.Balance}");
                    return;
                }
            }

            Console.WriteLine("Invalid PIN or user not found.");


        }


        //bool tipinde yoxlayir ki bele pin var mi
        public bool CheckPin(string pin)
        {
            foreach (User user in Clients)
            {
                if (user != null && user.CreditCard != null && user.CreditCard.PIN == pin)
                {
                    return true;
                }
            }
            return false;
        }


        public void Cash(string pin, int cash_amount)
        {
            foreach (User user in Clients)
            {

                if (user != null && user.CreditCard != null && user.CreditCard.PIN == pin)
                {
                    if (cash_amount > user.CreditCard.Balance)
                    {
                        Console.WriteLine("There is no enough amount in your card");

                    }
                    else
                    {
                        user.CreditCard.Balance -= cash_amount;
                        Console.WriteLine($"Your new balance is {user.CreditCard.Balance}");
                        return;

                    }


                }

            }

        }

        public void TransferMoney(string senderPin, string receiverPin, int amount)
        {
            if (senderPin == receiverPin)
            {
                Console.WriteLine("You cannot transfer money to yourself.");
                return;
            }

            User sender = FindUserByPIN(senderPin);
            User receiver = FindUserByPIN(receiverPin);

            if (sender == null || sender.CreditCard == null)
            {
                Console.WriteLine("Sender not found or sender's credit card is not available.");
                return;
            }

            if (receiver == null || receiver.CreditCard == null)
            {
                Console.WriteLine("Receiver not found or receiver's credit card is not available.");
                return;
            }

            if (amount <= 0)
            {
                Console.WriteLine("Amount must be greater than zero.");
                return;
            }

            if (sender.CreditCard.Balance < amount)
            {
                Console.WriteLine("Not enough balance.");
                return;
            }

            sender.CreditCard.Balance -= amount;
            receiver.CreditCard.Balance += amount;

            Console.WriteLine($"Transfer successful. Your new balance is {sender.CreditCard.Balance} AZN.");
        }

    }

}
