using System;

abstract class BankAccount
{
    public string AccountNumber { get; private set; }
    public string AccountHolder { get; private set; }
    public decimal Balance { get; protected set; }

    public BankAccount(string accountNumber, string accountHolder, decimal balance)
    {
        AccountNumber = accountNumber;
        AccountHolder = accountHolder;
        Balance = balance;
    }

    public virtual void GetAccountSummary()
    {
        Console.WriteLine($"Account Holder: {AccountHolder}");
        Console.WriteLine($"Account Number: {AccountNumber}");
        Console.WriteLine($"Current Balance: {Balance:C}");
    }

    public virtual void Deposit(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Deposit amount must be greater than zero");
        }

        Console.WriteLine($"Deposit: {amount:C}");
        Balance += amount;
        Console.WriteLine($"New Balance: {Balance:C}");
    }

    public virtual void GetBalance()
    {
        Console.WriteLine($"Balance: {Balance:C}");
    }

    public abstract void Withdraw(decimal amount);
}

class SavingsAccount : BankAccount
{
    public decimal InterestRate { get; private set; }

    public SavingsAccount(string accountNumber, string accountHolder, decimal balance, decimal interestRate)
        : base(accountNumber, accountHolder, balance)
    {
        InterestRate = interestRate;
        Console.WriteLine("Savings Account Created");
        Console.WriteLine($"Account Holder: {AccountHolder}");
        Console.WriteLine($"Account Number: {AccountNumber}");
        Console.WriteLine($"Initial Balance: {Balance:C}");
        Console.WriteLine();
    }

    public override void GetAccountSummary()
    {
        base.GetAccountSummary();
        Console.WriteLine($"Interest Rate: ({InterestRate}%)");
        Console.WriteLine();
    }

    public override void Deposit(decimal amount)
    {
        base.Deposit(amount);

        Console.WriteLine($"Calculating Interest ({InterestRate}%)...");
        decimal interest = Balance * InterestRate / 100;
        Console.WriteLine($"Interest Earned: {interest:C}");
        Balance += interest;
        Console.WriteLine($"New Balance: {Balance:C}");
        Console.WriteLine();
    }

    public override void Withdraw(decimal amount)
    {
        Console.WriteLine($"Withdrawal Attempt: {amount:C}");
        if (amount > Balance)
        {
            Console.WriteLine($"Error: Insufficient funds. Current balance: {Balance:C}");
        }
        else
        {
            Balance -= amount;
            Console.WriteLine("Transaction successful");
            Console.WriteLine($"New balance: {Balance:C}");
        }
        Console.WriteLine();
    }
}

class CheckingAccount : BankAccount
{
    public decimal OverdraftLimit { get; private set; }

    public CheckingAccount(string accountNumber, string accountHolder, decimal balance, decimal overdraftLimit)
        : base(accountNumber, accountHolder, balance)
    {
        OverdraftLimit = overdraftLimit;
        Console.WriteLine("Checking Account Created");
        Console.WriteLine($"Account Holder: {AccountHolder}");
        Console.WriteLine($"Account Number: {AccountNumber}");
        Console.WriteLine($"Initial Balance: {Balance:C}");
        Console.WriteLine();
    }

    public override void GetAccountSummary()
    {
        base.GetAccountSummary();
        Console.WriteLine($"Overdraft Limit: {OverdraftLimit:C}");
        Console.WriteLine($"Available Credit: {(Balance < 0 ? OverdraftLimit + Balance : OverdraftLimit):C}");
        Console.WriteLine();
    }

    public override void Deposit(decimal amount)
    {
        bool deficit = Balance < 0;
        base.Deposit(amount);

        if (deficit && Balance > 0)
        {
            Console.WriteLine("Notice: Account no longer in overdraft");
        }
        Console.WriteLine();
    }

    public override void GetBalance()
    {
        base.GetBalance();
        Console.WriteLine($"Overdraft Limit: {OverdraftLimit:C}");
        Console.WriteLine($"Available Funds (including overdraft): {Balance + OverdraftLimit:C}");
        Console.WriteLine();
    }

    public override void Withdraw(decimal amount)
    {
        Console.WriteLine($"Withdrawal Attempt: {amount:C}");
        if (amount > Balance + OverdraftLimit)
        {
            Console.WriteLine($"Error: Insufficient funds.");
            GetBalance();
        }
        else
        {
            Balance -= amount;
            Console.WriteLine("Transaction successful");
            Console.WriteLine($"New balance: {Balance:C}");
            if (Balance < 0)
            {
                Console.WriteLine("Warning: Account is using overdraft protection");
            }
        }
        Console.WriteLine();
    }
}

public class Program
{
    public static void Main()
    {
        Console.WriteLine("=== Creating Accounts ===");
        SavingsAccount savings = new SavingsAccount("SA001", "John Doe", 1000m, 5m);
        CheckingAccount checking = new CheckingAccount("CA001", "Jane Doe", 2000m, 500m);

        Console.WriteLine("=== Savings Account Transactions ===");
        savings.Deposit(500);
        savings.Withdraw(2000);

        Console.WriteLine("=== Checking Account Transactions ===");
        checking.GetBalance();
        checking.Withdraw(2200);
        checking.Deposit(300);

        Console.WriteLine("=== Account Summary ===");
        savings.GetAccountSummary();
        checking.GetAccountSummary();
    }
}
