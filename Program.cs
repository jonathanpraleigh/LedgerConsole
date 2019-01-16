using System;
using System.Collections.Generic;
using System.Linq;

namespace bankingLedger
{
    class Transaction
    {
        public DateTime TransactionDate;
        public string Description;
        public decimal TransactionAmount;
        public string TransactionMemo;
        public string UserAccountId;
    }

    class UserInfo
    {
        private string _userName;
        private string _password;
        private string _userAccountId;
        private decimal _balance;

        private List<Transaction> _history = new List<Transaction>();

        public void SetUserDetails(string name, string pw, string uAccountIdNum)
        {
            _userName = name;
            _password = pw;
            _userAccountId = uAccountIdNum;
        }

        public string GetUserName()
        {
            return _userName;
        }

        public string GetPassword()
        {
            return _password;
        }

        public string GetUserAccountNumber()
        {
            return _userAccountId;
        }

        public decimal GetBalance()
        {
            return _balance;
        }

        public void RecordTransaction(DateTime date, string payeeName, decimal amount, string memo, string uId)
        {
            Transaction toAdd = new Transaction
            {
                TransactionDate = date,
                Description = payeeName,
                TransactionAmount = amount,
                TransactionMemo = memo,
                UserAccountId = uId
            };

            _history.Add(toAdd);

            _balance += amount;
        }

        public void PrintTransactions()
        {
            Console.Clear();
            if (_history != null)
            {
                foreach (Transaction t in _history)
                {
                    Console.WriteLine("Date: " + t.TransactionDate + " Description: " + t.Description + " Amount: " + t.TransactionAmount + " Memo: " + t.TransactionMemo);       
                }
                Console.WriteLine("Press any key.");
                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                Console.WriteLine("There are no transactions. Get spending!.");
                Console.WriteLine("Press any key.");
                Console.ReadKey();
                Console.Clear();
            }

        }
    }

    static class Program
    {
        private static void Main()
        {
           
            List<UserInfo> userList = new List<UserInfo>(); 

            LoggedOutMenu(ref userList);
        }

        static void LoggedOutMenu(ref List<UserInfo> users)
        {
            for (;;)
            {
                Console.WriteLine("Banking Ledger - Menu Options:");
                Console.WriteLine("------------------------------");
                Console.WriteLine("1) Log In");
                Console.WriteLine("2) Create Account");
                Console.WriteLine("3) Exit");
                Console.WriteLine("------------------------------");
                Console.WriteLine("Option: ");

                var selection = Console.ReadKey();

                switch (selection.KeyChar)
                {
                    case '1':
                        Console.Clear();
                        LogIn(ref users);
                        Console.Clear();
                        break;
                    case '2':
                        Console.Clear();
                        CreateAccount(ref users, (users.Count + 1).ToString());
                        Console.Clear();
                        break;
                    case '3':
                        return;
                    default:
                        Console.Clear();
                        Console.WriteLine("Invalid Selection!");
                        Console.WriteLine("Press any key.");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                }
            }
        }

        static void LoggedInMenu(ref UserInfo currentUser)
        {
            for (;;)
            {
                Console.WriteLine("Banking Ledger - Menu Options:");
                Console.WriteLine("------------------------------");
                Console.WriteLine("1) Record a Deposit");
                Console.WriteLine("2) Record a Withdrawal");
                Console.WriteLine("3) Check Your Balance");
                Console.WriteLine("4) Check Your Transaction History");
                Console.WriteLine("5) Log Out");
                Console.WriteLine("------------------------------");
                Console.WriteLine("Option: ");

                var selection = Console.ReadKey();

                switch (selection.KeyChar)
                {
                    case '1':
                        Console.Clear();
                        Transact(ref currentUser, 1);
                        Console.Clear();
                        break;
                    case '2':
                        Console.Clear();
                        Transact(ref currentUser, -1);
                        Console.Clear();
                        break;
                    case '3':
                        Console.Clear();
                        Console.WriteLine(currentUser.GetBalance());
                        Console.WriteLine("Press any key.");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case '4':
                        Console.Clear();
                        Console.Clear();
                        currentUser.PrintTransactions();
                        Console.Clear();
                        break;
                    case '5':
                        Console.Clear();
                        return;
                    default:
                        Console.Clear();
                        Console.WriteLine("Invalid Selection!");
                        Console.WriteLine("Press any key.");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                }
            }
        }

        static void LogIn(ref List<UserInfo> checkNameAndPw)
        {
            Console.Clear();
            Console.WriteLine("Enter user name: ");
            var nameInput = Console.ReadLine();
            Console.WriteLine("Enter password: ");
            var pwInput = Console.ReadLine();
            if (checkNameAndPw != null)
            {
                foreach (UserInfo user in checkNameAndPw)
                {
                    if (nameInput == user.GetUserName() && pwInput == user.GetPassword())
                    {
                        var u = user;
                        Console.Clear();
                        LoggedInMenu(ref u);
                        Console.Clear();
                        return;
                    }
                }
                //Console.Clear();
                Console.WriteLine("Account Not Found.");
                Console.WriteLine("Press any key.");
                Console.ReadKey();
                Console.Clear();
                return;
            }
            //Console.Clear();
            Console.WriteLine("Account Not Found.");
            Console.WriteLine("Press any key.");
            Console.ReadKey();
            Console.Clear();
        }

        static void CreateAccount(ref List<UserInfo> nameAndPwToUpdate, string userIdNumber) 
        {
            string newNameInput = null;
            string newPwInput = null;
            var nameMatch = true;
            var pwMatch = false;

            while (nameMatch)
            {
                Console.Clear();
                Console.WriteLine("Enter a user name: ");
                newNameInput = Console.ReadLine();

                if (nameAndPwToUpdate.Any())
                {
                    foreach (UserInfo user in nameAndPwToUpdate)
                    {
                        if (user.GetUserName() != newNameInput)
                        {
                            nameMatch = false;
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("This name has been taken. Please re-enter a username.");
                            Console.WriteLine("Press any key.");
                            Console.ReadKey();
                            Console.Clear();
                        }
                    }
                }
                else
                {
                    nameMatch = false;
                }
            }

            while (pwMatch == false)
            {
                Console.WriteLine("Enter a password: ");
                newPwInput = Console.ReadLine();

                Console.WriteLine("Re-enter your password to confirm: ");
                var comparePwInput = Console.ReadLine();

                if (newPwInput == comparePwInput)
                {
                    pwMatch = true;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Passwords do not match. Please re-enter.");
                    Console.WriteLine("Press any key.");
                    Console.ReadKey();
                    Console.Clear();
                }
            }

            UserInfo newUser = new UserInfo();
            newUser.SetUserDetails(newNameInput, newPwInput, userIdNumber);

            nameAndPwToUpdate.Add(newUser);
        }

        static void Transact(ref UserInfo userAccount, int type)
        {
            bool validYear = false;
            bool validMonth = false;
            bool validDay = false;
            bool validAmount = false;
            int year = 0;
            int month = 0;
            int day = 0;
            decimal amount = 0;

            while (validYear == false)
            {
                Console.WriteLine("Please enter the year.");
                var newYear = Console.ReadLine();

                if (Int32.TryParse(newYear, out year) && year > 1900 )
                {
                    validYear = true;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Not a valid number.");
                    Console.WriteLine("Press any key.");
                    Console.ReadKey();
                    Console.Clear();
                }
            }

            while (validMonth == false)
            {
                Console.WriteLine("Please enter the month.");
                var newMonth = Console.ReadLine();

                if (Int32.TryParse(newMonth, out month) && month < 13 && month > 0)
                {
                    validMonth = true;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Not a valid month.");
                    Console.WriteLine("Press any key.");
                    Console.ReadKey();
                    Console.Clear();
                }
            }

            while (validDay == false)
            {
                Console.WriteLine("Please enter the day.");
                var newDay = Console.ReadLine();

                if (int.TryParse(newDay, out day) && day < 32 && day > 0)
                {
                    validDay = true;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Not a valid day.");
                    Console.WriteLine("Press any key.");
                    Console.ReadKey();
                    Console.Clear();
                }
            }

            DateTime depositDate = new DateTime(year, month, day);

            Console.WriteLine("Please enter a description.");
            var newPayee = Console.ReadLine();
            Console.Clear();


            while (validAmount == false)
            {
                Console.WriteLine("Please enter the amount.");
                var newAmount = Console.ReadLine();

                if (decimal.TryParse(newAmount, out amount))
                {
                    validAmount = true;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Not a valid amount.");
                    Console.WriteLine("Press any key.");
                    Console.ReadKey();
                    Console.Clear();
                }
            }


            Console.WriteLine("Please enter a memo for this transaction.");
            var newMemo = Console.ReadLine();
            Console.Clear();

            userAccount.RecordTransaction(depositDate, newPayee, amount * type, newMemo, userAccount.GetUserAccountNumber());
        }
    }
}
