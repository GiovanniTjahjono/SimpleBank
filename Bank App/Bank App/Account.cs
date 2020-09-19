using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Collections;

namespace Bank_App
{
    class Account
    {
        private string firstname, lastname, address, email;
        private int phone, accountNumber, balance;
        private List<String> bankStatement = new List<string>();

        public Account()
        {

        }

        public string Firstname { get => firstname; set => firstname = value; }
        public string Lastname { get => lastname; set => lastname = value; }
        public string Address { get => address; set => address = value; }
        public int Phone { get => phone; set => phone = value; }
        public string Email { get => email; set => email = value; }
        public int AccountNumber { get => accountNumber; set => accountNumber = value; }
        public List<string> BankStatement { get => bankStatement; set => bankStatement = value; }
        public int Balance { get => balance; set => balance = value; }

        public int GenerateUniqueAccountNumber()
        {
            Random random = new Random();
            string accountNumber = "";
            bool accountNumberIsValid = false;
            while (!accountNumberIsValid)
            {
                accountNumber = "";
                for (int i = 0; i < 8; i++)
                {
                    accountNumber += random.Next(0, 9);
                }
                if (File.Exists("../../../Accounts/" + accountNumber + ".txt"))
                {
                    continue;
                } else
                {
                    accountNumberIsValid = true;
                }
            }

            return int.Parse(accountNumber);
        }

        public bool isEmailValid(string email)
        {
            try
            {
                return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(200));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        public string SendingEmail(string reciever, string subject, string messageType)
        {
            string messageBody = "";
            switch (messageType)
            {
                case "createNewAccount":
                    messageBody = @"<html>" +
                                   "<body>" +
                                       "<p>Dear " + Firstname + "</p>" +
                                       "<p>Thank you for opening new account on Simple Bank. here it is your detail account</p> " +
                                       "<p><strong>Firstname: </strong>" + Firstname + "</p>" +
                                       "<p><strong>Lastname: </strong>" + Lastname + "</p>" +
                                       "<p><strong>Address: </strong>" + Address + "</p>" +
                                       "<p><strong>Firstname: </strong>" + Firstname + "</p>" +
                                       "<p><strong>Firstname: </strong>" + Firstname + "</p>" +
                                       "<p>Please do not hesitate to contact us if there is any issues" +
                                       "<br>" +
                                       "<br>" +
                                       "<p>Sincerely,</p>" +
                                       "<p>Head of Customer Relationship</p>" +
                                       "<p>Giovanni Tjahjono</p>" +
                                   "</body>" +
                                "</html>";
                    break;
                default:
                    break;
            }
            try
            {
                MailAddress emailFrom = new MailAddress("simplebankuts@gmail.com", "13389984");
                
                MailMessage email = new MailMessage();
                SmtpClient server = new SmtpClient("smtp.gmail.com");

                email.From = new MailAddress("ivankissling@gmail.com");
                email.To.Add(reciever);
                email.Subject = subject;
                email.IsBodyHtml = true;
                email.Body = messageBody;
                
                server.Port = 587;
                server.Credentials = new System.Net.NetworkCredential("simplebankuts@gmail.com", "13389984");
                server.EnableSsl = true;
                server.Send(email);
                return "sukses";
            }
            catch(Exception e)
            {
                return e.Message;
            }
        }

        public void CreateNewAccount()
        {
            bool isValid = false;
            while(!isValid)
            {
                Console.Clear();
                Console.WriteLine("\t ====================================================");
                Console.WriteLine("\t |                                                  |");
                Console.WriteLine("\t |                CREATE A NEW ACCOUNT              |");
                Console.WriteLine("\t |                                                  |");
                Console.WriteLine("\t ====================================================");
                Console.WriteLine("\t |                 ENTER THE DETAILS                |");
                Console.WriteLine("\t |                                                  |");
                Console.Write("\t |   Firstname: ");
                int firstnameCursorX = Console.CursorTop;
                int firstnameCursorY = Console.CursorLeft;
                Console.WriteLine("                                    |");
                Console.Write("\t |    Lastname: ");
                int lastnameCursorX = Console.CursorTop;
                int lastnameCursorY = Console.CursorLeft;
                Console.WriteLine("                                    |");
                Console.Write("\t |     Address: ");
                int addressCursorX = Console.CursorTop;
                int addressCursorY = Console.CursorLeft;
                Console.WriteLine("                                    |");
                Console.Write("\t |       Phone: ");
                int phoneCursorX = Console.CursorTop;
                int phoneCursorY = Console.CursorLeft;
                Console.WriteLine("                                    |");
                Console.Write("\t |       Email: ");
                int emailCursorX = Console.CursorTop;
                int emailCursorY = Console.CursorLeft;
                Console.WriteLine("                                    |");
                Console.WriteLine("\t |                                                  |");
                Console.WriteLine("\t ====================================================");
                Console.Write("\t | ");
                int messageCursorX = Console.CursorTop;
                int messageCursorY = Console.CursorLeft;
                Console.WriteLine("                                                 |");
                Console.Write("\t | ");
                int messageSecondCursorX = Console.CursorTop;
                int messageSecondCursorY = Console.CursorLeft;
                Console.WriteLine("                                                 |");
                Console.WriteLine("\t ====================================================");

                Console.SetCursorPosition(firstnameCursorY, firstnameCursorX);
                Firstname = Console.ReadLine();
                if(Firstname == "")
                {
                    Console.SetCursorPosition(messageCursorY, messageCursorX);
                    Console.WriteLine("Please input at least a character");
                    Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                    Console.WriteLine("Press any key to try again");
                    Console.ReadKey();
                    continue;
                }
                Console.SetCursorPosition(lastnameCursorY, lastnameCursorX);
                Lastname = Console.ReadLine();
                if (Lastname == "")
                {
                    Console.SetCursorPosition(messageCursorY, messageCursorX);
                    Console.WriteLine("Please input at least a character");
                    Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                    Console.WriteLine("Press any key to try again");
                    Console.ReadKey();
                    continue;
                }
                Console.SetCursorPosition(addressCursorY, addressCursorX);
                Address = Console.ReadLine();
                if (Address == "")
                {
                    Console.SetCursorPosition(messageCursorY, messageCursorX);
                    Console.WriteLine("Please input at least a character");
                    Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                    Console.WriteLine("Press any key to try again");
                    Console.ReadKey();
                    continue;
                }
                Console.SetCursorPosition(phoneCursorY, phoneCursorX);
                int phone;
                if (int.TryParse(Console.ReadLine(), out phone))
                {
                    Phone = phone;
                }
                else
                {
                    Console.SetCursorPosition(messageCursorY, messageCursorX);
                    Console.WriteLine("Wrong Input: number only and less than 10 digit");
                    Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                    Console.WriteLine("Press any key to try again");
                    Console.ReadKey();
                    continue;
                }
                Console.SetCursorPosition(emailCursorY, emailCursorX);
                Email = Console.ReadLine();
                if(!isEmailValid(Email))
                {
                    Console.SetCursorPosition(messageCursorY, messageCursorX);
                    Console.WriteLine("Please input the right email format xxx@xxx.xxx");
                    Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                    Console.WriteLine("Press any key to try again");
                    Console.ReadKey();
                    continue;
                }

                bool isCorrect = false;
                while(!isCorrect)
                {
                    Console.SetCursorPosition(messageCursorY, messageCursorX);
                    Console.Write("Is the information correct? (y/n)");
                    string confirm = Console.ReadLine();
                    if(confirm.ToLower() == "y")
                    {
                        isCorrect = true;
                        isValid = true;
                        
                        AccountNumber = GenerateUniqueAccountNumber();
                        try
                        {
                            File.WriteAllText("../../../Accounts/"+AccountNumber + ".txt", "Firstname|" + Firstname + "\n" +
                                                        "Lastname|" + Lastname + "\n" +
                                                        "Address|" + Address + "\n" +
                                                        "Phone|" + Phone + "\n" +
                                                        "Email|" + Email + "\n" +
                                                        "AccountNo|" + accountNumber + "\n" +
                                                        "Balance|0" + "\n");
                        } catch(IOException e)
                        {
                            Console.SetCursorPosition(messageCursorY, messageCursorX);
                            Console.Write(new string(' ', Console.WindowWidth));
                            Console.SetCursorPosition(messageCursorY, messageCursorX);
                            Console.Write(e.Message);
                        }

                        SendingEmail(Email, "New Account is Created", "a");
                        Console.SetCursorPosition(messageCursorY, messageCursorX);
                        Console.Write(new string(' ', Console.WindowWidth));
                        Console.SetCursorPosition(messageCursorY, messageCursorX);
                        Console.Write("Account is created, check email for the detail");
                        Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                        Console.Write(new string(' ', Console.WindowWidth));
                        Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                        Console.Write("Account number is " + accountNumber);

                        Console.ReadLine();
                    } else if (confirm.ToLower() == "n")
                    {
                        isCorrect = true;
                    } else
                    {
                        Console.SetCursorPosition(messageCursorY, messageCursorX);
                        Console.Write("Please input only 'y' for yes or 'n' for no ");
                        Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                        Console.WriteLine("Press any key to try again");
                        Console.ReadKey();
                        Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                        Console.Write(new string(' ', Console.WindowWidth));
                        Console.SetCursorPosition(messageCursorY, messageCursorX);
                        Console.Write(new string(' ', Console.WindowWidth));
                        continue;
                    }
                }
               

            }
           
        }

        public void SearchAnAccount()
        {
            bool isValid = false;
            while (!isValid)
            {
                Console.Clear();
                Console.WriteLine("\t ====================================================");
                Console.WriteLine("\t |                                                  |");
                Console.WriteLine("\t |                 SEARCH AN ACCOUNT                |");
                Console.WriteLine("\t |                                                  |");
                Console.WriteLine("\t ====================================================");
                Console.WriteLine("\t |                 ENTER THE DETAILS                |");
                Console.WriteLine("\t |                                                  |");
                Console.Write("\t |   Account Number: ");
                int accountNumberCursorX = Console.CursorTop;
                int accountNumberCursorY = Console.CursorLeft;
                Console.WriteLine("                               |");
                Console.WriteLine("\t |                                                  |");
                Console.WriteLine("\t ====================================================");
                Console.Write("\t | ");
                int messageCursorX = Console.CursorTop;
                int messageCursorY = Console.CursorLeft;
                Console.WriteLine("                                                 |");
                Console.Write("\t | ");
                int messageSecondCursorX = Console.CursorTop;
                int messageSecondCursorY = Console.CursorLeft;
                Console.WriteLine("                                                 |");
                Console.WriteLine("\t ====================================================");

                Console.SetCursorPosition(accountNumberCursorY, accountNumberCursorX);
                string accountNumber = Console.ReadLine();
                int result = 0;
                if (accountNumber.Length <= 10)
                {
                    if (int.TryParse(accountNumber, out result))
                    {
                        AccountNumber = result;
                    }
                    else
                    {
                        Console.SetCursorPosition(messageCursorY, messageCursorX);
                        Console.WriteLine("Please input numeric character");
                        Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                        Console.WriteLine("Press any key to try again");
                        Console.ReadKey();
                        continue;
                    }
                }
                else
                {
                    Console.SetCursorPosition(messageCursorY, messageCursorX);
                    Console.WriteLine("Please input maximum 10 character");
                    Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                    Console.WriteLine("Press any key to try again");
                    Console.ReadKey();
                    continue;
                }

                if (File.Exists("../../../Accounts/" + AccountNumber + ".txt"))
                {
                    Console.SetCursorPosition(messageCursorY, messageCursorX);
                    Console.WriteLine("Account found");

                    try
                    {
                        StreamReader accountFile = new StreamReader("../../../Accounts/" + AccountNumber + ".txt");
                        String data = accountFile.ReadLine();
                        ArrayList dataPool = new ArrayList();
                        while (data != null)
                        {
                            string[] dataRow = data.Split('|');
                            dataPool.Add(dataRow[1]);
                            data = accountFile.ReadLine();
                        }
                        Firstname = dataPool[0].ToString();
                        Lastname = dataPool[1].ToString();
                        Address = dataPool[2].ToString();
                        Phone = int.Parse(dataPool[3].ToString());
                        Email = dataPool[4].ToString();
                        AccountNumber = int.Parse(dataPool[5].ToString());
                        Balance = int.Parse(dataPool[6].ToString());

                        int whiteSpaceLeft = 51;

                        Console.WriteLine("\t ====================================================");
                        Console.WriteLine("\t |                  ACCOUNT DETAILS                 |");
                        Console.WriteLine("\t |                                                  |");
                        Console.Write("\t | Firstname: " + Firstname);
                        whiteSpaceLeft -= (Firstname.Length + 13);
                        for (int i = 0; i < whiteSpaceLeft; i++)
                        {
                            Console.Write(" ");
                        }
                        Console.WriteLine("|");
                        whiteSpaceLeft = 50;

                        Console.Write("\t | Lastname: " + Lastname);
                        whiteSpaceLeft -= (Lastname.Length + 11);
                        for (int i = 0; i < whiteSpaceLeft; i++)
                        {
                            Console.Write(" ");
                        }
                        Console.WriteLine("|");
                        whiteSpaceLeft = 50;

                        Console.Write("\t | Address: " + Address);
                        whiteSpaceLeft -= (Address.Length + 10);
                        for (int i = 0; i < whiteSpaceLeft; i++)
                        {
                            Console.Write(" ");
                        }
                        Console.WriteLine("|");
                        whiteSpaceLeft = 50;

                        Console.Write("\t | Phone: " + Phone);
                        whiteSpaceLeft -= (Phone.ToString().Length + 8);
                        for (int i = 0; i < whiteSpaceLeft; i++)
                        {
                            Console.Write(" ");
                        }
                        Console.WriteLine("|");
                        whiteSpaceLeft = 50;

                        Console.Write("\t | Email: " + Email);
                        whiteSpaceLeft -= (Email.Length + 8);
                        for (int i = 0; i < whiteSpaceLeft; i++)
                        {
                            Console.Write(" ");
                        }
                        Console.WriteLine("|");
                        whiteSpaceLeft = 50;

                        Console.Write("\t | Balance: $" + Balance);
                        whiteSpaceLeft -= (Balance.ToString().Length + 11);
                        for (int i = 0; i < whiteSpaceLeft; i++)
                        {
                            Console.Write(" ");
                        }
                        Console.WriteLine("|");
                        whiteSpaceLeft = 50;
                        Console.WriteLine("\t |                                                  |");
                        Console.WriteLine("\t ====================================================");
                        Console.Write("\t | Do you want to search another account? (y/n):");
                        int commandCursorY = Console.CursorLeft;
                        int commandCursorX = Console.CursorTop;
                        Console.WriteLine("    |");
                        int messageCommandCursorY = Console.CursorLeft;
                        int messageCommandCursorX = Console.CursorTop;
                        Console.WriteLine(" ");
                        Console.WriteLine("\t ====================================================");

                        //--Close the stream
                        accountFile.Close();

                        bool isValidCommand = false;
                        while (!isValidCommand)
                        {
                            Console.SetCursorPosition(messageCommandCursorY, messageCommandCursorX);
                            Console.WriteLine("\t |                                                  |");
                            Console.SetCursorPosition(commandCursorY, commandCursorX);
                            string command = Console.ReadLine();
                            if (command.ToLower() == "y" || command.ToLower() == "n")
                            {
                                isValidCommand = true;
                                switch (command.ToLower())
                                {
                                    case "y":
                                        continue;
                                    case "n":
                                        isValid = true;
                                        break;
                                    default:
                                        isValid = true;
                                        break;
                                }
                            }
                            else
                            {
                                Console.SetCursorPosition(messageCommandCursorY, messageCommandCursorX);
                                Console.Write(new string(' ', Console.WindowWidth));
                                Console.SetCursorPosition(messageCommandCursorY, messageCommandCursorX);
                                Console.WriteLine("\t | Please input 'Y' for Yes or 'N' for No !         |");
                                Console.SetCursorPosition(messageCommandCursorY, messageCommandCursorX);
                                Console.ReadKey();
                                Console.SetCursorPosition(messageCommandCursorY, messageCommandCursorX);
                                Console.Write(new string(' ', Console.WindowWidth));
                                Console.SetCursorPosition(messageCommandCursorY, messageCommandCursorX);
                                Console.WriteLine("                                                  |");
                                Console.SetCursorPosition(commandCursorY, commandCursorX);
                                Console.Write(new string(' ', Console.WindowWidth));
                                Console.SetCursorPosition(commandCursorY, commandCursorX);
                                Console.WriteLine("    |");
                                continue;
                            }
                        }
                    }
                    catch (IOException e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadKey();
                    }
                    continue;
                }
                else
                {
                    Console.SetCursorPosition(messageCursorY, messageCursorX);
                    Console.WriteLine("Account is not found, check the account number");
                    Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                    Console.WriteLine("Press any key to try again");
                    isValid = false;
                    Console.ReadKey();
                    continue;
                }


            }
        }

        public void Deposit()
        {
            //--Declare a var as a indicator of the function is still valid (active) or not
            bool isValid = false;

            //--While it is active, do this function
            while (!isValid)
            {
                //--Clear the bank statement from the previous operation
                BankStatement.Clear();

                //--Declare variable to be stored the amount of deposit
                int amountOfDeposit = 0;

                //--Declare variable to be stored the amount of new deposit + current balance
                int newBalance = 0;

                //--Declare variable to be writen the new user detail + bank statement
                string newRecord = "";

                //--Draw the user interface
                Console.Clear();
                Console.WriteLine("\t ====================================================");
                Console.WriteLine("\t |                                                  |");
                Console.WriteLine("\t |                       DEPOSIT                    |");
                Console.WriteLine("\t |                                                  |");
                Console.WriteLine("\t ====================================================");
                Console.WriteLine("\t |                 ENTER THE DETAILS                |");
                Console.WriteLine("\t |                                                  |");
                Console.Write("\t |   Account Number: ");
                int accountNumberCursorX = Console.CursorTop;
                int accountNumberCursorY = Console.CursorLeft;
                Console.WriteLine("                               |");
                Console.Write("\t |   Amount of Deposit: $");
                int amountOfDepositCursorX = Console.CursorTop;
                int amountOfDepositCursorY = Console.CursorLeft;
                Console.WriteLine("                           |");
                Console.WriteLine("\t |                                                  |");
                Console.WriteLine("\t ====================================================");
                Console.Write("\t | ");
                int messageCursorX = Console.CursorTop;
                int messageCursorY = Console.CursorLeft;
                Console.WriteLine("                                                 |");
                Console.Write("\t | ");
                int messageSecondCursorX = Console.CursorTop;
                int messageSecondCursorY = Console.CursorLeft;
                Console.WriteLine("                                                 |");
                Console.WriteLine("\t ====================================================");

                //--Set the cursor and get the user input of account number
                Console.SetCursorPosition(accountNumberCursorY, accountNumberCursorX);
                //--Account number input
                string accountNumber = Console.ReadLine();
                //--Set the cursor and get the user input of amount of deposit
                Console.SetCursorPosition(amountOfDepositCursorY, amountOfDepositCursorX);
                //--Amount of deposit input
                string amountOfDepositInput = Console.ReadLine();

                //--Check, is the input valid (maximum input is 10 character)  
                if (accountNumber.Length <= 10 && amountOfDepositInput.Length <= 10)
                {
                    //--Check, is the input integer (to avoid non-numeric input)
                    if (int.TryParse(accountNumber, out int resultAccountNumber) && int.TryParse(amountOfDepositInput, out int resultAmountOfDeposit))
                    {
                        //--Check, is the input minus or not (to avoid minum input)
                        if (resultAmountOfDeposit > 0)
                        {
                            AccountNumber = resultAccountNumber;
                            amountOfDeposit = resultAmountOfDeposit;
                        }
                        //--Show message that minimum deposit is $1 and minus or 0 input is not allow
                        else
                        {
                            //--While the input is not "Y" or "N"
                            bool isValidCommand = false;
                            while (!isValidCommand)
                            {
                                Console.SetCursorPosition(messageCursorY, messageCursorX);
                                Console.WriteLine("Please input minimum $1");
                                Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                                Console.Write(new string(' ', 40));
                                Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                                Console.Write("try again (y/n): ");
                                string command = Console.ReadLine();
                                //--Check, If the input is "Y"
                                if (command.ToLower() == "y")
                                {
                                    //--Re run the Deposit() function
                                    isValidCommand = true;
                                }
                                //--Check, If the input is "N"
                                else if (command.ToLower() == "n")
                                {
                                    //--Close the Deposit(), then back to the Main Menu
                                    isValidCommand = true;
                                    isValid = true;
                                }
                                else
                                {
                                    //--Give message only input "Y" or "N" and ask again
                                    Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                                    Console.WriteLine("Please input 'Y' for Yes or 'N' for No !");
                                    Console.ReadKey();
                                }
                            }
                            continue;
                        }
                    }
                    //--Show message that input should be numeric
                    else
                    {
                        //--While the input is not "Y" or "N"
                        bool isValidCommand = false;
                        while (!isValidCommand)
                        {
                            Console.SetCursorPosition(messageCursorY, messageCursorX);
                            Console.WriteLine("Please input numeric character");
                            Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                            Console.Write(new string(' ', 40));
                            Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                            Console.Write("try again (y/n): ");
                            string command = Console.ReadLine();

                            //--Check, If the input is "Y"
                            if (command.ToLower() == "y")
                            {
                                //--Re run the Deposit() function
                                isValidCommand = true;
                            }
                            //--Check, If the input is "N"
                            else if (command.ToLower() == "n")
                            {
                                //--Close the Deposit(), then back to the Main Menu
                                isValidCommand = true;
                                isValid = true;
                            }
                            else
                            {
                                //--Give message only input "Y" or "N" and ask again
                                Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                                Console.WriteLine("Please input 'Y' for Yes or 'N' for No !");
                                Console.ReadKey();
                            }
                        }
                        continue;
                    }
                }
                //--Show message that input should be maximum 10 character
                else
                {
                    bool isValidCommand = false;
                    while (!isValidCommand)
                    {
                        Console.SetCursorPosition(messageCursorY, messageCursorX);
                        Console.WriteLine("Please input maximum 10 character");
                        Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                        Console.Write(new string(' ', 40));
                        Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                        Console.Write("try again (y/n): ");
                        string command = Console.ReadLine();

                        //--Check, If the input is "Y"
                        if (command.ToLower() == "y")
                        {
                            //--Re run the Deposit() function
                            isValidCommand = true;
                        }
                        //--Check, If the input is "N"
                        else if (command.ToLower() == "n")
                        {
                            //--Close the Deposit(), then back to the Main Menu
                            isValidCommand = true;
                            isValid = true;
                        }
                        else
                        {
                            //--Give message only input "Y" or "N" and ask again
                            Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                            Console.WriteLine("Please input 'Y' for Yes or 'N' for No !");
                            Console.ReadKey();
                        }
                    }
                    continue;
                }

                //--Check, is the account exist or not
                if (File.Exists("../../../Accounts/" + AccountNumber + ".txt"))
                {
                    try
                    {
                        //--Try to read the file
                        StreamReader accountFile = new StreamReader("../../../Accounts/" + AccountNumber + ".txt");
                        //--Read the line
                        string data = accountFile.ReadLine();
                        //--Declare an array to save the account file data
                        ArrayList dataPool = new ArrayList();
                        //--Declare a variable to knowing the end line of the account detail file
                        bool isEndLineOfAccountDetail = false;
                        //--While the data is exist
                        while (data != null)
                        {
                            //--While the data is not at the end line of the user personal information
                            if (!isEndLineOfAccountDetail)
                            {
                                //--Split the data 
                                string[] dataRow = data.Split('|');
                                //--Take only the data without the data label
                                dataPool.Add(dataRow[1]);
                                //--If the current line is "Balance", it means the end of the line and the next line is the bank statement
                                if (dataRow[0].ToLower() == "balance")
                                {
                                    isEndLineOfAccountDetail = true;
                                }
                                //--Read the data
                                data = accountFile.ReadLine();
                            }
                            //--Add the bank statement to the list
                            else
                            {
                                BankStatement.Add(data);
                                //--Read the data
                                data = accountFile.ReadLine();
                            }

                        }
                        //--Assign the properties of the Account() class
                        Firstname = dataPool[0].ToString();
                        Lastname = dataPool[1].ToString();
                        Address = dataPool[2].ToString();
                        Phone = int.Parse(dataPool[3].ToString());
                        Email = dataPool[4].ToString();
                        AccountNumber = int.Parse(dataPool[5].ToString());
                        Balance = int.Parse(dataPool[6].ToString());

                        //--Try to add current balance and new amount of deposit to avoid error of out of integer maximum range
                        try
                        {
                            newBalance = Balance + amountOfDeposit;
                            //--Check, is the new balance is more than 2147483647, which is the maximum of the integer can handle or less than 0 as a double protection to avoid minus balance
                            if (newBalance > 2147483647 || newBalance < 0)
                            {
                                Console.SetCursorPosition(messageCursorY, messageCursorX);
                                Console.WriteLine("The amount is to big, the maximum is $2147483647");
                                Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                                Console.WriteLine("Press any key to try again");
                                Console.ReadKey();
                                continue;
                            }
                        }
                        //--Show the exception error
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            Console.ReadKey();
                        }

                        //--Add the new record of the deposit
                        BankStatement.Add(DateTime.Now.ToString() + "|Deposit|" + amountOfDeposit.ToString() + "|" + newBalance.ToString());

                        newRecord = "Firstname|" + Firstname + "\n" +
                                                        "Lastname|" + Lastname + "\n" +
                                                        "Address|" + Address + "\n" +
                                                        "Phone|" + Phone + "\n" +
                                                        "Email|" + Email + "\n" +
                                                        "AccountNo|" + AccountNumber + "\n" +
                                                        "Balance|" + newBalance + "\n";

                        for (int i = 0; i < BankStatement.Count; i++)
                        {
                            newRecord += BankStatement[i] + "\n";
                        }
                        //--Close the stream
                        accountFile.Close();

                        //--Store the new record to the file record
                        File.WriteAllText("../../../Accounts/" + AccountNumber + ".txt", newRecord);

                        //--Show the success message and direct back to the main menu
                        Console.SetCursorPosition(messageCursorY, messageCursorX);
                        Console.WriteLine("Deposit is success, your balance is $" + newBalance);
                        Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                        Console.WriteLine("Press any key to back to Main Menu");
                        isValid = true;
                        Console.ReadKey();

                    }
                    //--Show exception error
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadKey();
                    }

                }
                //--Show the message that the account is not exist
                else
                {
                    bool isValidCommand = false;
                    while (!isValidCommand)
                    {
                        Console.SetCursorPosition(messageCursorY, messageCursorX);
                        Console.WriteLine("Account is not found, check the account number");
                        Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                        Console.Write(new string(' ', 40));
                        Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                        Console.Write("try again (y/n): ");
                        string command = Console.ReadLine();
                        //--Check id the input is "Y"
                        if (command.ToLower() == "y")
                        {
                            isValidCommand = true;
                            isValid = false;
                        }
                        //--Check id the input is "N"
                        else if (command.ToLower() == "n")
                        {
                            isValidCommand = true;
                            isValid = true;
                        }
                        //--Show message only input "Y" or "N"
                        else
                        {
                            Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                            Console.WriteLine("Please input 'Y' for Yes or 'N' for No !");
                            Console.ReadKey();
                        }
                    }
                    continue;
                }
            }
        }

        public void Withdrawal()
        {
            //--Declare a var as a indicator of the function is still valid (active) or not
            bool isValid = false;

            //--While it is active, do this function
            while (!isValid)
            {
                //--Clear the bank statement from the previous operation
                BankStatement.Clear();

                //--Declare variable to be stored the amount of deposit
                int amountOfWithdrawal = 0;

                //--Declare variable to be stored the amount of new deposit + current balance
                int newBalance = 0;

                //--Declare variable to be writen the new user detail + bank statement
                string newRecord = "";

                //--Draw the user interface
                Console.Clear();
                Console.WriteLine("\t ====================================================");
                Console.WriteLine("\t |                                                  |");
                Console.WriteLine("\t |                    WITHDRAWAL                    |");
                Console.WriteLine("\t |                                                  |");
                Console.WriteLine("\t ====================================================");
                Console.WriteLine("\t |                 ENTER THE DETAILS                |");
                Console.WriteLine("\t |                                                  |");
                Console.Write("\t |   Account Number: ");
                int accountNumberCursorX = Console.CursorTop;
                int accountNumberCursorY = Console.CursorLeft;
                Console.WriteLine("                               |");
                Console.Write("\t |   Amount of Withdrawal: $");
                int amountOfWithdrawalCursorX = Console.CursorTop;
                int amountOfWithdrawalCursorY = Console.CursorLeft;
                Console.WriteLine("                        |");
                Console.WriteLine("\t |                                                  |");
                Console.WriteLine("\t ====================================================");
                Console.Write("\t | ");
                int messageCursorX = Console.CursorTop;
                int messageCursorY = Console.CursorLeft;
                Console.WriteLine("                                                 |");
                Console.Write("\t | ");
                int messageSecondCursorX = Console.CursorTop;
                int messageSecondCursorY = Console.CursorLeft;
                Console.WriteLine("                                                 |");
                Console.WriteLine("\t ====================================================");

                //--Set the cursor and get the user input of account number
                Console.SetCursorPosition(accountNumberCursorY, accountNumberCursorX);
                //--Account number input
                string accountNumber = Console.ReadLine();
                //--Set the cursor and get the user input of amount of deposit
                Console.SetCursorPosition(amountOfWithdrawalCursorY, amountOfWithdrawalCursorX);
                //--Amount of deposit input
                string amountOfWithdrawalInput = Console.ReadLine();

                //--Check, is the input valid (maximum input is 10 character)  
                if (accountNumber.Length <= 10 && amountOfWithdrawalInput.Length <= 10)
                {
                    //--Check, is the input integer (to avoid non-numeric input)
                    if (int.TryParse(accountNumber, out int resultAccountNumber) && int.TryParse(amountOfWithdrawalInput, out int resultamountOfWithdrawal))
                    {
                        //--Check, is the input minus or not (to avoid minum input)
                        if (resultamountOfWithdrawal > 0)
                        {
                            AccountNumber = resultAccountNumber;
                            amountOfWithdrawal = resultamountOfWithdrawal;
                        }
                        //--Show message that minimum deposit is $1 and minus or 0 input is not allow
                        else
                        {
                            //--While the input is not "Y" or "N"
                            bool isValidCommand = false;
                            while (!isValidCommand)
                            {
                                Console.SetCursorPosition(messageCursorY, messageCursorX);
                                Console.WriteLine("Please input minimum $1");
                                Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                                Console.Write(new string(' ', 40));
                                Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                                Console.Write("try again (y/n): ");
                                string command = Console.ReadLine();
                                //--Check, If the input is "Y"
                                if (command.ToLower() == "y")
                                {
                                    //--Re run the Deposit() function
                                    isValidCommand = true;
                                }
                                //--Check, If the input is "N"
                                else if (command.ToLower() == "n")
                                {
                                    //--Close the Deposit(), then back to the Main Menu
                                    isValidCommand = true;
                                    isValid = true;
                                }
                                else
                                {
                                    //--Give message only input "Y" or "N" and ask again
                                    Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                                    Console.WriteLine("Please input 'Y' for Yes or 'N' for No !");
                                    Console.ReadKey();
                                }
                            }
                            continue;
                        }
                    }
                    //--Show message that input should be numeric
                    else
                    {
                        //--While the input is not "Y" or "N"
                        bool isValidCommand = false;
                        while (!isValidCommand)
                        {
                            Console.SetCursorPosition(messageCursorY, messageCursorX);
                            Console.WriteLine("Please input numeric character");
                            Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                            Console.Write(new string(' ', 40));
                            Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                            Console.Write("try again (y/n): ");
                            string command = Console.ReadLine();

                            //--Check, If the input is "Y"
                            if (command.ToLower() == "y")
                            {
                                //--Re run the Deposit() function
                                isValidCommand = true;
                            }
                            //--Check, If the input is "N"
                            else if (command.ToLower() == "n")
                            {
                                //--Close the Deposit(), then back to the Main Menu
                                isValidCommand = true;
                                isValid = true;
                            }
                            else
                            {
                                //--Give message only input "Y" or "N" and ask again
                                Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                                Console.WriteLine("Please input 'Y' for Yes or 'N' for No !");
                                Console.ReadKey();
                            }
                        }
                        continue;
                    }
                }
                //--Show message that input should be maximum 10 character
                else
                {
                    bool isValidCommand = false;
                    while (!isValidCommand)
                    {
                        Console.SetCursorPosition(messageCursorY, messageCursorX);
                        Console.WriteLine("Please input maximum 10 character");
                        Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                        Console.Write(new string(' ', 40));
                        Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                        Console.Write("try again (y/n): ");
                        string command = Console.ReadLine();

                        //--Check, If the input is "Y"
                        if (command.ToLower() == "y")
                        {
                            //--Re run the Deposit() function
                            isValidCommand = true;
                        }
                        //--Check, If the input is "N"
                        else if (command.ToLower() == "n")
                        {
                            //--Close the Deposit(), then back to the Main Menu
                            isValidCommand = true;
                            isValid = true;
                        }
                        else
                        {
                            //--Give message only input "Y" or "N" and ask again
                            Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                            Console.WriteLine("Please input 'Y' for Yes or 'N' for No !");
                            Console.ReadKey();
                        }
                    }
                    continue;
                }

                //--Check, is the account exist or not
                if (File.Exists("../../../Accounts/" + AccountNumber + ".txt"))
                {
                    try
                    {
                        //--Try to read the file
                        StreamReader accountFile = new StreamReader("../../../Accounts/" + AccountNumber + ".txt");
                        //--Read the line
                        string data = accountFile.ReadLine();
                        //--Declare an array to save the account file data
                        ArrayList dataPool = new ArrayList();
                        //--Declare a variable to knowing the end line of the account detail file
                        bool isEndLineOfAccountDetail = false;
                        //--While the data is exist
                        while (data != null)
                        {
                            //--While the data is not at the end line of the user personal information
                            if (!isEndLineOfAccountDetail)
                            {
                                //--Split the data 
                                string[] dataRow = data.Split('|');
                                //--Take only the data without the data label
                                dataPool.Add(dataRow[1]);
                                //--If the current line is "Balance", it means the end of the line and the next line is the bank statement
                                if (dataRow[0].ToLower() == "balance")
                                {
                                    isEndLineOfAccountDetail = true;
                                }
                                //--Read the data
                                data = accountFile.ReadLine();
                            }
                            //--Add the bank statement to the list
                            else
                            {
                                BankStatement.Add(data);
                                //--Read the data
                                data = accountFile.ReadLine();
                            }

                        }
                        //--Assign the properties of the Account() class
                        Firstname = dataPool[0].ToString();
                        Lastname = dataPool[1].ToString();
                        Address = dataPool[2].ToString();
                        Phone = int.Parse(dataPool[3].ToString());
                        Email = dataPool[4].ToString();
                        AccountNumber = int.Parse(dataPool[5].ToString());
                        Balance = int.Parse(dataPool[6].ToString());

                        //--Try to add current balance and new amount of deposit to avoid error of out of integer maximum range
                        try
                        {
                            newBalance = Balance - amountOfWithdrawal;
                            //--Check, is the new balance is more than 2147483647, which is the maximum of the integer can handle or less than 0 as a double protection to avoid minus balance
                            if (newBalance < 0)
                            {
                                Console.SetCursorPosition(messageCursorY, messageCursorX);
                                Console.WriteLine("Your balance is not enough");
                                Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                                Console.WriteLine("Press any key to try again");
                                Console.ReadKey();
                                continue;
                            }
                        }
                        //--Show the exception error
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            Console.ReadKey();
                        }

                        //--Add the new record of the deposit
                        BankStatement.Add(DateTime.Now.ToString() + "|Withdrawal|" + amountOfWithdrawal.ToString() + "|" + newBalance.ToString());

                        newRecord = "Firstname|" + Firstname + "\n" +
                                                        "Lastname|" + Lastname + "\n" +
                                                        "Address|" + Address + "\n" +
                                                        "Phone|" + Phone + "\n" +
                                                        "Email|" + Email + "\n" +
                                                        "AccountNo|" + AccountNumber + "\n" +
                                                        "Balance|" + newBalance + "\n";

                        for (int i = 0; i < BankStatement.Count; i++)
                        {
                            newRecord += BankStatement[i] + "\n";
                        }
                        //--Close the stream
                        accountFile.Close();

                        //--Store the new record to the file record
                        File.WriteAllText("../../../Accounts/" + AccountNumber + ".txt", newRecord);

                        //--Show the success message and direct back to the main menu
                        Console.SetCursorPosition(messageCursorY, messageCursorX);
                        Console.WriteLine("Withrawal is success, your balance $" + newBalance + " left");
                        Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                        Console.WriteLine("Press any key to back to Main Menu");
                        isValid = true;
                        Console.ReadKey();

                    }
                    //--Show exception error
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadKey();
                    }

                }
                //--Show the message that the account is not exist
                else
                {
                    bool isValidCommand = false;
                    while (!isValidCommand)
                    {
                        Console.SetCursorPosition(messageCursorY, messageCursorX);
                        Console.WriteLine("Account is not found, check the account number");
                        Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                        Console.Write(new string(' ', 40));
                        Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                        Console.Write("try again (y/n): ");
                        string command = Console.ReadLine();
                        //--Check id the input is "Y"
                        if (command.ToLower() == "y")
                        {
                            isValidCommand = true;
                            isValid = false;
                        }
                        //--Check id the input is "N"
                        else if (command.ToLower() == "n")
                        {
                            isValidCommand = true;
                            isValid = true;
                        }
                        //--Show message only input "Y" or "N"
                        else
                        {
                            Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                            Console.WriteLine("Please input 'Y' for Yes or 'N' for No !");
                            Console.ReadKey();
                        }
                    }
                    continue;
                }
            }
        }

        public void AccountStatement()
        {
            bool isValid = false;
            while (!isValid)
            {
                Console.Clear();
                Console.WriteLine("\t ====================================================");
                Console.WriteLine("\t |                                                  |");
                Console.WriteLine("\t |                 ACCOUNT STATEMENT                |");
                Console.WriteLine("\t |                                                  |");
                Console.WriteLine("\t ====================================================");
                Console.WriteLine("\t |                 ENTER THE DETAILS                |");
                Console.WriteLine("\t |                                                  |");
                Console.Write("\t |   Account Number: ");
                int accountNumberCursorX = Console.CursorTop;
                int accountNumberCursorY = Console.CursorLeft;
                Console.WriteLine("                               |");
                Console.WriteLine("\t |                                                  |");
                Console.WriteLine("\t ====================================================");
                Console.Write("\t | ");
                int messageCursorX = Console.CursorTop;
                int messageCursorY = Console.CursorLeft;
                Console.WriteLine("                                                 |");
                Console.Write("\t | ");
                int messageSecondCursorX = Console.CursorTop;
                int messageSecondCursorY = Console.CursorLeft;
                Console.WriteLine("                                                 |");
                Console.WriteLine("\t ====================================================");

                Console.SetCursorPosition(accountNumberCursorY, accountNumberCursorX);
                string accountNumber = Console.ReadLine();
                int result = 0;
                if (accountNumber.Length <= 10)
                {
                    if (int.TryParse(accountNumber, out result))
                    {
                        AccountNumber = result;
                    }
                    else
                    {
                        Console.SetCursorPosition(messageCursorY, messageCursorX);
                        Console.WriteLine("Please input numeric character");
                        Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                        Console.WriteLine("Press any key to try again");
                        Console.ReadKey();
                        continue;
                    }
                }
                else
                {
                    Console.SetCursorPosition(messageCursorY, messageCursorX);
                    Console.WriteLine("Please input maximum 10 character");
                    Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                    Console.WriteLine("Press any key to try again");
                    Console.ReadKey();
                    continue;
                }

                if (File.Exists("../../../Accounts/" + AccountNumber + ".txt"))
                {
                    Console.SetCursorPosition(messageCursorY, messageCursorX);
                    Console.WriteLine("Account found");

                    try
                    {
                        StreamReader accountFile = new StreamReader("../../../Accounts/" + AccountNumber + ".txt");
                        String data = accountFile.ReadLine();
                        ArrayList dataPool = new ArrayList();
                        while (data != null)
                        {
                            string[] dataRow = data.Split('|');
                            dataPool.Add(dataRow[1]);
                            data = accountFile.ReadLine();
                        }
                        Firstname = dataPool[0].ToString();
                        Lastname = dataPool[1].ToString();
                        Address = dataPool[2].ToString();
                        Phone = int.Parse(dataPool[3].ToString());
                        Email = dataPool[4].ToString();
                        AccountNumber = int.Parse(dataPool[5].ToString());
                        Balance = int.Parse(dataPool[6].ToString());

                        int whiteSpaceLeft = 51;

                        Console.WriteLine("\t ====================================================");
                        Console.WriteLine("\t |                  ACCOUNT DETAILS                 |");
                        Console.WriteLine("\t |                                                  |");
                        Console.Write("\t | Firstname: " + Firstname);
                        whiteSpaceLeft -= (Firstname.Length + 13);
                        for (int i = 0; i < whiteSpaceLeft; i++)
                        {
                            Console.Write(" ");
                        }
                        Console.WriteLine("|");
                        whiteSpaceLeft = 50;

                        Console.Write("\t | Lastname: " + Lastname);
                        whiteSpaceLeft -= (Lastname.Length + 11);
                        for (int i = 0; i < whiteSpaceLeft; i++)
                        {
                            Console.Write(" ");
                        }
                        Console.WriteLine("|");
                        whiteSpaceLeft = 50;

                        Console.Write("\t | Address: " + Address);
                        whiteSpaceLeft -= (Address.Length + 10);
                        for (int i = 0; i < whiteSpaceLeft; i++)
                        {
                            Console.Write(" ");
                        }
                        Console.WriteLine("|");
                        whiteSpaceLeft = 50;

                        Console.Write("\t | Phone: " + Phone);
                        whiteSpaceLeft -= (Phone.ToString().Length + 8);
                        for (int i = 0; i < whiteSpaceLeft; i++)
                        {
                            Console.Write(" ");
                        }
                        Console.WriteLine("|");
                        whiteSpaceLeft = 50;

                        Console.Write("\t | Email: " + Email);
                        whiteSpaceLeft -= (Email.Length + 8);
                        for (int i = 0; i < whiteSpaceLeft; i++)
                        {
                            Console.Write(" ");
                        }
                        Console.WriteLine("|");
                        whiteSpaceLeft = 50;

                        Console.Write("\t | Balance: $" + Balance);
                        whiteSpaceLeft -= (Balance.ToString().Length + 11);
                        for (int i = 0; i < whiteSpaceLeft; i++)
                        {
                            Console.Write(" ");
                        }
                        Console.WriteLine("|");
                        whiteSpaceLeft = 50;

                        Console.WriteLine("\t |                                                  |");
                        Console.WriteLine("\t ====================================================");
                        Console.WriteLine("\t |                LAST 5 TRANSACTIONS               |");
                        Console.WriteLine("\t |                                                  |");


                        for (int i = 0; i < 5; i++)
                        {
                            Console.Write("\t | Email: " + Email);
                            whiteSpaceLeft -= (Email.Length + 8);
                            for (int j = 0; j < whiteSpaceLeft; j++)
                            {
                                Console.Write(" ");
                            }
                            Console.WriteLine("|");
                            whiteSpaceLeft = 50;
                        }

                        Console.WriteLine("\t |                                                  |");
                        Console.WriteLine("\t ====================================================");
                        Console.Write("\t | Do you want to search another account? (y/n):");
                        int commandCursorY = Console.CursorLeft;
                        int commandCursorX = Console.CursorTop;
                        Console.WriteLine("    |");
                        int messageCommandCursorY = Console.CursorLeft;
                        int messageCommandCursorX = Console.CursorTop;
                        Console.WriteLine(" ");
                        Console.WriteLine("\t ====================================================");

                        //--Close the stream
                        accountFile.Close();

                        bool isValidCommand = false;
                        while (!isValidCommand)
                        {
                            Console.SetCursorPosition(messageCommandCursorY, messageCommandCursorX);
                            Console.WriteLine("\t |                                                  |");
                            Console.SetCursorPosition(commandCursorY, commandCursorX);
                            string command = Console.ReadLine();
                            if (command.ToLower() == "y" || command.ToLower() == "n")
                            {
                                isValidCommand = true;
                                switch (command.ToLower())
                                {
                                    case "y":
                                        continue;
                                    case "n":
                                        isValid = true;
                                        break;
                                    default:
                                        isValid = true;
                                        break;
                                }
                            }
                            else
                            {
                                Console.SetCursorPosition(messageCommandCursorY, messageCommandCursorX);
                                Console.Write(new string(' ', Console.WindowWidth));
                                Console.SetCursorPosition(messageCommandCursorY, messageCommandCursorX);
                                Console.WriteLine("\t | Please input 'Y' for Yes or 'N' for No !         |");
                                Console.SetCursorPosition(messageCommandCursorY, messageCommandCursorX);
                                Console.ReadKey();
                                Console.SetCursorPosition(messageCommandCursorY, messageCommandCursorX);
                                Console.Write(new string(' ', Console.WindowWidth));
                                Console.SetCursorPosition(messageCommandCursorY, messageCommandCursorX);
                                Console.WriteLine("                                                  |");
                                Console.SetCursorPosition(commandCursorY, commandCursorX);
                                Console.Write(new string(' ', Console.WindowWidth));
                                Console.SetCursorPosition(commandCursorY, commandCursorX);
                                Console.WriteLine("    |");
                                continue;
                            }
                        }
                    }
                    catch (IOException e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadKey();
                    }
                    continue;
                }
                else
                {
                    Console.SetCursorPosition(messageCursorY, messageCursorX);
                    Console.WriteLine("Account is not found, check the account number");
                    Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                    Console.WriteLine("Press any key to try again");
                    isValid = false;
                    Console.ReadKey();
                    continue;
                }


            }
        }
    }
}
