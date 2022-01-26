using System;
using System.Collections.Generic;

namespace classes
{
    public class BankAccount
    {
        private static int accountNumberSeed = 1234567890;
        //PROPERTIES
        /*Properties are data elements and can have code that enforces validation or other rules.*/
        public string Number { get; }
        public string Owner { get; set; }

        //public decimal Balance { get; }
        public decimal Balance
        {
            get
            {
                decimal balance = 0;
                foreach (var item in allTransactions)
                {
                    balance += item.Amount;
                }

                return balance;
            }
        }

        //CONSTRUCTOR
        /* A constructor is a member that has the same name as the class. It is used to initialize objects of that class type*/

        /*A constructor is responsible for initializing an object. A derived class constructor must initialize the derived class, 
         * and provide instructions on how to initialize the base class object included in the derived class.*/

        private readonly decimal minimumBalance;
        public BankAccount(string name, decimal initialBalance) : this(name, initialBalance, 0) { }
        public BankAccount(string name, decimal initialBalance, decimal minimumBalance)
        {
            this.Number = accountNumberSeed.ToString();
            accountNumberSeed++;

            this.Owner = name;
            this.minimumBalance = minimumBalance;
            if (initialBalance > 0)
                MakeDeposit(initialBalance, DateTime.Now, "Initial balance");
        }

        private List<Transaction> allTransactions = new List<Transaction>();

        //METHODS
        /*Methods are blocks of code that perform a single function.*/
        public void MakeDeposit(decimal amount, DateTime date, string note)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount of deposit must be positive");
            }
            var deposit = new Transaction(amount, date, note);
            allTransactions.Add(deposit);
        }
        public void MakeWithdrawal(decimal amount, DateTime date, string note)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount of withdrawal must be positive");
            }
            var overdraftTransaction = CheckWithdrawalLimit(Balance - amount < minimumBalance);
            var withdrawal = new Transaction(-amount, date, note);
            allTransactions.Add(withdrawal);
            if (overdraftTransaction != null)
                allTransactions.Add(overdraftTransaction);
        }
        protected virtual Transaction CheckWithdrawalLimit(bool isOverdrawn)
        {
            if (isOverdrawn)
            {
                throw new InvalidOperationException("Not sufficient funds for this withdrawal");
            }
            else
            {
                return default;
            }
        }
        public string GetAccountHistory()
        {
            var report = new System.Text.StringBuilder();

            decimal balance = 0;
            report.AppendLine("Date\t\tAmount\tBalance\tNote");
            foreach (var item in allTransactions)
            {
                balance += item.Amount;
                report.AppendLine($"{item.Date.ToShortDateString()}\t{item.Amount}\t{balance}\t{item.Notes}");
            }

            return report.ToString();
        }

        //POLYMORPHISM
        /* A virtual method is a method where any derived class may choose to reimplement. 
        * The derived classes use the override keyword to define the new implementation. 
        * Typically you refer to this as "overriding the base class implementation". 
        * The virtual keyword specifies that derived classes may override the behavior. 
        * You can also declare abstract methods where derived classes must override the behavior. 
        * The base class does not provide an implementation for an abstract method*/
        public virtual void PerformMonthEndTransactions() { }
    }
}