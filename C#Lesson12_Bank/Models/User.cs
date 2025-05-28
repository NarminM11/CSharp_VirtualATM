using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Lesson12_Bank.Models
{
    public class User
    {
        public string Name { get; set; }
        public string SurName { get; set; }
        public BankCard CreditCard { get; set; }

        public User(string name, string surname, BankCard card)
        {
            Name = name;
            SurName = surname;
            CreditCard = card;
        }
        public override string ToString()
        {
            return $"Name: {Name} {SurName}\n{CreditCard}\n" + new string('-', 30);
        }
    }
}
