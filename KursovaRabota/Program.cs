using System;

interface IBankAccount
{
    void Deposit(double DepositSum);
    void Withdrawal(double WithdrawalSum);
    double Sum { get; }
    string Username { get; }
    string Password { get; }
}

class BankAccount : IBankAccount
{
    private string username, password;
    private double sum;

    public BankAccount() { this.sum = 0; }
    public BankAccount(string username, string password, double sum)
    {
        this.username = username;
        this.password = password;
        this.sum = sum;
    }

    public void Deposit(double DepositSum)
    {
        sum += DepositSum;
        Console.WriteLine("Make deposit: " + DepositSum);
        Console.WriteLine("Current balance: " + sum);
    }

    public void Withdrawal(double WithdrawalSum)
    {
        if (sum >= WithdrawalSum)
        {
            sum -= WithdrawalSum;
            Console.WriteLine("Make withdrawal: " + WithdrawalSum);
            Console.WriteLine("Current balance: " + sum);
        }
        else
        {
            Console.WriteLine("There is not enough money.");
        }
    }

    public double Sum { get { return sum; } }
    public string Username { get { return username; } }
    public string Password { get { return password; } }
}

class BankAccountProxy : IBankAccount
{
    private BankAccount realAccount;

    public BankAccountProxy(BankAccount account)
    {
        this.realAccount = account;
    }

    public void Deposit(double DepositSum)
    {
        Console.WriteLine("Enter password for deposit:");
        string inputPassword = Console.ReadLine();
        if (inputPassword == realAccount.Password)
        {
            realAccount.Deposit(DepositSum);
        }
        else
        {
            Console.WriteLine("Incorrect password. Deposit failed.");
        }
    }

    public void Withdrawal(double WithdrawalSum)
    {
        Console.WriteLine("Enter password for withdrawal:");
        string inputPassword = Console.ReadLine();
        if (inputPassword == realAccount.Password)
        {
            realAccount.Withdrawal(WithdrawalSum);
        }
        else
        {
            Console.WriteLine("Incorrect password. Withdrawal failed.");
        }
    }

    public double Sum
    {
        get
        {
            Console.WriteLine("Enter password to check balance:");
            string inputPassword = Console.ReadLine();
            if (inputPassword == realAccount.Password)
            {
                return realAccount.Sum;
            }
            else
            {
                Console.WriteLine("Incorrect password. Cannot retrieve balance.");
                return 0;
            }
        }
    }

    public string Username { get { return realAccount.Username; } }
    public string Password { get { return realAccount.Password; } }
}

class Bank
{
    private IBankAccount[] account = new IBankAccount[10];
    private int numAccounts;

    public Bank() { numAccounts = 0; }

    public bool AddAccount(string username, string password, double sum)
    {
        for (int i = 0; i < numAccounts; i++)
        {
            if (account[i].Username.Equals(username))
            {
                Console.WriteLine("There is the same username.");
                return false;
            }
        }
        BankAccount realAccount = new BankAccount(username, password, sum);
        account[numAccounts] = new BankAccountProxy(realAccount);
        numAccounts++;
        Console.WriteLine("The account is added.");
        return true;
    }

    public IBankAccount SearchInAccount(string username, string password)
    {
        for (int i = 0; i < numAccounts; i++)
        {
            if (account[i].Username.Equals(username))
            {
                if (account[i].Password.Equals(password))
                {
                    return account[i];
                }
                else
                {
                    Console.WriteLine("Wrong password.");
                    return null;
                }
            }
        }
        Console.WriteLine("The username is invalid.");
        return null;
    }
}

class Program
{
    static void Main(string[] args)
    {
        Bank bank = new Bank();

        bank.AddAccount("Adi", "123", 200);

        Console.WriteLine("Enter username:");
        string username = Console.ReadLine();
        Console.WriteLine("Enter password:");
        string password = Console.ReadLine();

        IBankAccount account = bank.SearchInAccount(username, password);
        if (account != null)
        {
            Console.WriteLine("How much money do you want to deposit?");
            double depositAmount = Double.Parse(Console.ReadLine());
            account.Deposit(depositAmount);

            Console.WriteLine("How much money do you want to withdraw?");
            double withdrawalAmount = Double.Parse(Console.ReadLine());
            account.Withdrawal(withdrawalAmount);

            Console.WriteLine("Do you want balance? Y/N");
            if (Console.ReadLine().ToUpper() == "Y")
            {
                Console.WriteLine("The balance is: " + account.Sum);
            }
            else
            {
                Console.WriteLine("Ok, bye!");
            }
        }
    }
}