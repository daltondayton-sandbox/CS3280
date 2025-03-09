using System;
using System.Collections.Generic;

public class BankAccount
{
    private decimal balance;
    private string accountNumber;
    private List<Transaction> transactions;

    public BankAccount(string accountNumber, decimal initialBalance)
    {
        this.accountNumber = accountNumber;
        balance = 0;
        transactions = new List<Transaction>();

        if (initialBalance > 0)
        {
            ProcessTransaction(new Transaction { Amount = initialBalance, Type = TransactionType.Deposit });
        }
        else if (initialBalance <= 0)
        {
            ProcessTransaction(new Transaction { Amount = initialBalance, Type = TransactionType.Withdrawal });
        }
    }

    public void ProcessTransaction(Transaction transaction)
    {
        if (transaction == null)
            return;
        transactions.Add(transaction);

        if (transaction.Type == TransactionType.Deposit)
        {
            balance += transaction.Amount;
        }
        else if (transaction.Type == TransactionType.Withdrawal)
        {
            if (transaction.Amount > balance)
            {
                Console.WriteLine("Insufficient funds");
                return;
            }
            balance -= transaction.Amount;
        }
    }

    public decimal GetBalance()
    {
        decimal calculatedBalance = 0;
        foreach (var transaction in transactions)
        {
            if (transaction.Type == TransactionType.Deposit)
                calculatedBalance += transaction.Amount;
            else if (transaction.Type == TransactionType.Withdrawal)
                if (transaction.Amount < calculatedBalance)
                    calculatedBalance -= transaction.Amount;
        }
        return calculatedBalance;
    }
}

public class Transaction
{
    public decimal Amount { get; set; }

    public TransactionType Type { get; set; }
    public DateTime Date { get; set; }
}

public enum TransactionType
{
    Deposit,
    Withdrawal
}

public class Program
{
    public static void Main()
    {
        // Test code
        var account = new BankAccount("1234", 1000);

        // Test various scenarios
        account.ProcessTransaction(new Transaction { Amount = 500, Type = TransactionType.Deposit });
        account.ProcessTransaction(new Transaction { Amount = 200, Type = TransactionType.Withdrawal });
        account.ProcessTransaction(null);
        account.ProcessTransaction(new Transaction { Amount = 2000, Type = TransactionType.Withdrawal });
        Console.WriteLine($"Balance: {account.GetBalance()}");
    }
}
