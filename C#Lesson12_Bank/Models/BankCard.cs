using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Lesson12_Bank.Models
{
    public class BankCard
    {
        public string BankName { get; set; }
        public string FullName { get; set; }
        public string PAN { get; set; }
        public string PIN { get; set; }
        public string CVC { get; set; }
        public DateTime ExpireDate { get; set; }
        public decimal Balance { get; set; }

        public BankCard()
        {

        }

        public BankCard(string bankName, string fullName, string pan, string pin, string cvc, DateTime expireDate, decimal balance)
        {
            BankName = bankName;
            FullName = fullName;
            PAN = pan;
            PIN = pin;
            CVC = cvc;
            ExpireDate = expireDate;
            Balance = balance;
        }
        public override string ToString()
        {
            return $"Bank: {BankName}\n" +
                   $"Card Holder: {FullName}\n" +
                   $"PAN: {PAN}\n" +
                   $"CVC: {CVC}\n" +
                   $"Expire Date: {ExpireDate:MM/yyyy}\n" +
                   $"Balance: {Balance} AZN";
        }
    }

}
