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
        //--Declare variable regarding to the account detail
        private string firstname, lastname, address, email;
        private int phone, accountNumber, balance;
        private List<String> bankStatement = new List<string>();
        
        //--Bank statement list for email
        private List<String> bankStatementToEmail = new List<string>();

        //--Default Constructor
        public Account() {}

        //--Getter and Setter
        public string Firstname { get => firstname; set => firstname = value; }
        public string Lastname { get => lastname; set => lastname = value; }
        public string Address { get => address; set => address = value; }
        public int Phone { get => phone; set => phone = value; }
        public string Email { get => email; set => email = value; }
        public int AccountNumber { get => accountNumber; set => accountNumber = value; }
        public List<string> BankStatement { get => bankStatement; set => bankStatement = value; }
        public int Balance { get => balance; set => balance = value; }

        //--Generate unique account number
        public int GenerateUniqueAccountNumber()
        {
            //--Declare Random() function and variables
            Random random = new Random();
            string accountNumber = "";
            //--Check if the account number is valid (not exist yet)
            bool accountNumberIsValid = false;
            while (!accountNumberIsValid)
            {
                accountNumber = "";
                //--Generate 8 digit account number
                for (int i = 0; i < 8; i++)
                {
                    //--Get number between 0 to 9
                    accountNumber += random.Next(0, 9);
                }
                //--Check, if account number is already exist or the account is began with 0, return false and generate new account number
                if (File.Exists("../../../Accounts/" + accountNumber + ".txt") || accountNumber.Substring(0,1) == "0")
                {
                    continue;
                } else
                {
                    accountNumberIsValid = true;
                }
            }
            //--Return the account number
            return int.Parse(accountNumber);
        }

        //--Email format checker
        public bool isEmailValid(string email)
        {
            try
            {
                //--If the email is match the regex, return true
                return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(200));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        //--Sending email function
        //--Reciever = email destination
        //--subject = email's subject
        //--messageType = type of email (create new account or account statement)
        public bool SendingEmail(string reciever, string subject, string messageType)
        {
            //--Declare the messagae body
            string messageBody = "";
            switch (messageType)
            {
                //--If want to send email about creating new account
                case "createNewAccount":
                    messageBody = @"<html>" +
                                   "<body>" +
                                       "<p>Dear " + Firstname + "</p>" +
                                       "<p>Thank you for opening new account on Simple Bank. here it is your detail account</p> " +
                                       "<p><strong>Account Number: </strong>" + AccountNumber + "</p>" +
                                       "<p><strong>Firstname: </strong>" + Firstname + "</p>" +
                                       "<p><strong>Lastname: </strong>" + Lastname + "</p>" +
                                       "<p><strong>Address: </strong>" + Address + "</p>" +
                                       "<p><strong>Phone: </strong>" + Phone + "</p>" +
                                       "<p><strong>Email: </strong>" + Email + "</p>" +
                                       "<p>Please do not hesitate to contact us if there is any issues" +
                                       "<br>" +
                                       "<br>" +
                                       "<p>Sincerely,</p>" +
                                       "<p>Head of Customer Relationship</p>" +
                                       "<p>Giovanni Tjahjono</p>" +
                                   "</body>" +
                                "</html>";
                    break;
                //--If want to send email about account statement
                case "Account Statement":
                    string accountStatementTable = "";
                    for(int i = 0; i < bankStatementToEmail.Count; i++)
                    {
                        string[] arrayData;
                        arrayData = bankStatementToEmail[i].Split("|");
                        accountStatementTable += "<tr>";
                        accountStatementTable += "<td>" + arrayData[0] + "</td>";
                        accountStatementTable += "<td>" + arrayData[1] + "</td>";
                        accountStatementTable += "<td style='text-align:right'>$" + arrayData[2] + "</td>";
                        accountStatementTable += "<td style='text-align:right'>$" + arrayData[3] + "</td>";
                        accountStatementTable += "</tr>";
                    }

                    bankStatementToEmail.Clear();
                    messageBody = @"<html>" +
                                   "<body>" +
                                       "<p>Dear " + Firstname + "</p>" +
                                       "<p>Here it is your detail account</p> " +
                                       "<p><strong>Account Number: </strong>" + AccountNumber + "</p>" +
                                       "<p><strong>Firstname: </strong>" + Firstname + "</p>" +
                                       "<p><strong>Lastname: </strong>" + Lastname + "</p>" +
                                       "<p><strong>Address: </strong>" + Address + "</p>" +
                                       "<p><strong>Phone: </strong>" + Phone + "</p>" +
                                       "<p><strong>Email: </strong>" + Email + "</p>" +
                                       "<br>" +
                                       "<p>This is your transaction details</p>" +
                                       "<table border='1'>" +
                                            "<tr>" +
                                                "<th>Date</th>" +
                                                "<th>Transaction</th>" +
                                                "<th>Amount</th>" +
                                                "<th>Balance</th>" +
                                            "</tr>" + 
                                            accountStatementTable +
                                       "</table>" +
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
                //--Declare the Mail function
                MailMessage email = new MailMessage();
                SmtpClient server = new SmtpClient("smtp.gmail.com");

                //--Setup the email structure
                email.From = new MailAddress("ivankissling@gmail.com");
                email.To.Add(reciever);
                email.Subject = subject;
                email.IsBodyHtml = true;
                email.Body = messageBody;
                
                //--Send the email
                server.Port = 587;
                server.Credentials = new System.Net.NetworkCredential("simplebankuts@gmail.com", "13389984");
                server.EnableSsl = true;
                server.Send(email);

                //--If success, return true
                return true;
            }
            catch(Exception e)
            {
                //--To avoid the warning, show the message
                Console.WriteLine(e.Message);
                //--If failed, return false
                return false;
            }
        }

        //--Generate new account
        public void CreateNewAccount()
        {
            //--as long as isValid active, means show the create new account menu
            bool isValid = false;
            while(!isValid)
            {
                //--Show the interface
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

                //--Firstname input
                Console.SetCursorPosition(firstnameCursorY, firstnameCursorX);
                Firstname = Console.ReadLine();
                //--if there's no input, show message that the user should input at least a character
                if(Firstname == "")
                {
                    Console.SetCursorPosition(messageCursorY, messageCursorX);
                    Console.WriteLine("Please input at least a character");
                    Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                    Console.WriteLine("Press any key to try again");
                    Console.ReadKey();
                    continue;
                }

                //--Lastname input
                Console.SetCursorPosition(lastnameCursorY, lastnameCursorX);
                Lastname = Console.ReadLine();
                //--if there's no input, show message that the user should input at least a character
                if (Lastname == "")
                {
                    Console.SetCursorPosition(messageCursorY, messageCursorX);
                    Console.WriteLine("Please input at least a character");
                    Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                    Console.WriteLine("Press any key to try again");
                    Console.ReadKey();
                    continue;
                }

                //--Address input
                Console.SetCursorPosition(addressCursorY, addressCursorX);
                Address = Console.ReadLine();
                //--if there's no input, show message that the user should input at least a character
                if (Address == "")
                {
                    Console.SetCursorPosition(messageCursorY, messageCursorX);
                    Console.WriteLine("Please input at least a character");
                    Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                    Console.WriteLine("Press any key to try again");
                    Console.ReadKey();
                    continue;
                }

                //--Phone input
                Console.SetCursorPosition(phoneCursorY, phoneCursorX);
                int phone;
                //--Check, the input should be numeric
                if (int.TryParse(Console.ReadLine(), out phone))
                {
                    Phone = phone;
                }
                //--if not numeric, show message that the user should input numeric
                else
                {
                    Console.SetCursorPosition(messageCursorY, messageCursorX);
                    Console.WriteLine("Wrong Input: number only and less than 10 digit");
                    Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                    Console.WriteLine("Press any key to try again");
                    Console.ReadKey();
                    continue;
                }

                //--Email input
                Console.SetCursorPosition(emailCursorY, emailCursorX);
                Email = Console.ReadLine();
                //--if the email format is not right, show message that the user should input the right email format
                if (!isEmailValid(Email))
                {
                    Console.SetCursorPosition(messageCursorY, messageCursorX);
                    Console.WriteLine("Please input the right email format xxx@xxx.xxx");
                    Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                    Console.WriteLine("Press any key to try again");
                    Console.ReadKey();
                    continue;
                }

                //--Confirmation
                bool isCorrect = false;
                while(!isCorrect)
                {
                    Console.SetCursorPosition(messageCursorY, messageCursorX);
                    Console.Write("Is the information correct? (y/n):");
                    string confirm = Console.ReadLine();
                    
                    //--If the user has agree and type "y"
                    if(confirm.ToLower() == "y")
                    {
                        //--set true to make this function does not loop again
                        isCorrect = true;
                        isValid = true;
                        
                        //--Generate account number
                        AccountNumber = GenerateUniqueAccountNumber();

                        //--Save the detail user into a file
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
                        //--Show message please wait until the operation is finished
                        Console.SetCursorPosition(messageCursorY, messageCursorX);
                        Console.WriteLine("Creating account, please wait...                 |");

                        //--If email is managed to be sent, show message success
                        if (SendingEmail(Email, "New Account is Created", "createNewAccount"))
                        {
                            Console.SetCursorPosition(messageCursorY, messageCursorX);
                            Console.WriteLine("Account is created, check email for the detail   |");
                            Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                            Console.WriteLine("Account number is " + accountNumber);

                            Console.ReadLine();
                        }
                        //--Show failed if the operation is failed
                        else
                        {
                            Console.SetCursorPosition(messageCursorY, messageCursorX);
                            Console.Write(new string(' ', 24));
                            Console.SetCursorPosition(messageCursorY, messageCursorX);
                            Console.Write("Failed to create account");
                            Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                            Console.Write(new string(' ', 24));
                            Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                            Console.Write("Try again");
                            Console.ReadLine();
                        }
                        
                       
                    }
                    //--If the user want to change the detail, loop again
                    else if (confirm.ToLower() == "n")
                    {
                        isCorrect = true;
                    }
                    //--If user input other than "y" or "n"
                    else
                    {
                        Console.SetCursorPosition(messageCursorY, messageCursorX);
                        Console.Write("Please input only 'y' for yes or 'n' for no ");
                        Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                        Console.WriteLine("Press any key to try again");
                        Console.ReadKey();
                        Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                        Console.Write(new string(' ', 26));
                        Console.SetCursorPosition(messageCursorY, messageCursorX);
                        Console.Write(new string(' ', 44));
                        continue;
                    }
                }
            }
        }

        //--Search an account
        public void SearchAnAccount()
        {
            //--as long as isValid active, means show the search an account menu
            bool isValid = false;
            while (!isValid)
            {
                //--Show user interface
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
                //--Check, if the input of account number is less than or equals to 10, continue the process
                if (accountNumber.Length <= 10)
                {
                    //--Try to parse it to integer to make sure it is numeric 
                    if (int.TryParse(accountNumber, out result))
                    {
                        AccountNumber = result;
                    }
                    //--If not numeric, show message
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
                //--If more than 10 digit, show message
                else
                {
                    Console.SetCursorPosition(messageCursorY, messageCursorX);
                    Console.WriteLine("Please input maximum 10 character");
                    Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                    Console.WriteLine("Press any key to try again");
                    Console.ReadKey();
                    continue;
                }
                //--Try to search the account
                if (File.Exists("../../../Accounts/" + AccountNumber + ".txt"))
                {
                    Console.SetCursorPosition(messageCursorY, messageCursorX);
                    Console.WriteLine("Account found");
                    //--Try to read the file data
                    try
                    {
                        StreamReader accountFile = new StreamReader("../../../Accounts/" + AccountNumber + ".txt");
                        String data = accountFile.ReadLine();
                        ArrayList dataPool = new ArrayList();
                        //--Get the file data and save it temporary to and datapool arrayList
                        while (data != null)
                        {
                            string[] dataRow = data.Split('|');
                            dataPool.Add(dataRow[1]);
                            data = accountFile.ReadLine();
                        }
                        //--Assign the class properties with the file data on the datapool
                        Firstname = dataPool[0].ToString();
                        Lastname = dataPool[1].ToString();
                        Address = dataPool[2].ToString();
                        Phone = int.Parse(dataPool[3].ToString());
                        Email = dataPool[4].ToString();
                        AccountNumber = int.Parse(dataPool[5].ToString());
                        Balance = int.Parse(dataPool[6].ToString());


                        //--This is variable to indicate the width of the user interface
                        int whiteSpaceLeft = 51;

                        //--Show the account details
                        Console.WriteLine("\t ====================================================");
                        Console.WriteLine("\t |                  ACCOUNT DETAILS                 |");
                        Console.WriteLine("\t |                                                  |");
                        Console.Write("\t | Firstname: " + Firstname);
                        //--Adjust the width size
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

                        //--Close the file stream
                        accountFile.Close();

                        //--Get the user's command
                        bool isValidCommand = false;
                        while (!isValidCommand)
                        {
                            Console.SetCursorPosition(messageCommandCursorY, messageCommandCursorX);
                            Console.WriteLine("\t |                                                  |");
                            Console.SetCursorPosition(commandCursorY, commandCursorX);
                            string command = Console.ReadLine();
                            //--If yes, search another account
                            if (command.ToLower() == "y")
                            {
                                isValidCommand = true;
                                continue;
                            }
                            //--If no, close this function and back to the main menu
                            else if (command.ToLower() == "n")
                            {
                                isValidCommand = true;
                                isValid = true;
                            }
                            //--other than yes or no, show message that should be only "y" or "n" input
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
                //--If the file is not exist, show the message
                else
                {
                    isValid = false;
                    Console.WriteLine("\t |                                                  |");
                    Console.WriteLine("\t ====================================================");
                    Console.Write("\t | Account is not found, try another? (y/n):");
                    int commandCursorY = Console.CursorLeft;
                    int commandCursorX = Console.CursorTop;
                    Console.WriteLine(" ");
                    int messageCommandCursorY = Console.CursorLeft;
                    int messageCommandCursorX = Console.CursorTop;
                    Console.WriteLine(" ");
                    Console.WriteLine("\t ====================================================");

                    bool isValidCommand = false;
                    while (!isValidCommand)
                    {
                        Console.SetCursorPosition(messageCommandCursorY, messageCommandCursorX);
                        Console.WriteLine("\t |                                                  |");
                        Console.SetCursorPosition(commandCursorY, commandCursorX);
                        string command = Console.ReadLine();
                        //--If yes, search another account
                        if (command.ToLower() == "y")
                        {
                            isValidCommand = true;
                            continue;
                               
                        }
                        //--If no, close this function and back to the main menu
                        else if (command.ToLower() == "n")
                        {
                            isValidCommand = true;
                            isValid = true;
                        }
                        //--other than yes or no, show message that should be only "y" or "n" input
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
                            Console.WriteLine("        |");
                            continue;
                        }
                    }
                }
            }
        }

        //--Deposit
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

        //--Withdrawal
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

        //--Account statement
        public void AccountStatement()
        {
            //--While this function is active, do this function
            bool isValid = false;
            while (!isValid)
            {
                //--Clear the bank statement from the previous operation
                BankStatement.Clear();
                //--Show the user interface
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

                //--Get the user input
                Console.SetCursorPosition(accountNumberCursorY, accountNumberCursorX);
                string accountNumber = Console.ReadLine();
                int result = 0;
                //--Check, is the input is less than or equals to 10
                if (accountNumber.Length <= 10)
                {
                    //--Check is the input is numeric
                    if (int.TryParse(accountNumber, out result))
                    {
                        AccountNumber = result;
                    }
                    //--If not numeric, show the message
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
                //--If the input is more than 10, show the message
                else
                {
                    Console.SetCursorPosition(messageCursorY, messageCursorX);
                    Console.WriteLine("Please input maximum 10 character");
                    Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                    Console.WriteLine("Press any key to try again");
                    Console.ReadKey();
                    continue;
                }
                //--Check, is the account is exist
                if (File.Exists("../../../Accounts/" + AccountNumber + ".txt"))
                {
                    //--Show the message that the account is found
                    Console.SetCursorPosition(messageCursorY, messageCursorX);
                    Console.WriteLine("Account found");
                    //--Try to read the file
                    try
                    {
                        StreamReader accountFile = new StreamReader("../../../Accounts/" + AccountNumber + ".txt");
                        String data = accountFile.ReadLine();
                        ArrayList dataPool = new ArrayList();
                        //--Declare a variable to knowing the end line of the account detail file
                        bool isEndLineOfAccountDetail = false;
                        while (data != null)
                        {
                            if(!isEndLineOfAccountDetail)
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
                        //--Assign the class properties 
                        Firstname = dataPool[0].ToString();
                        Lastname = dataPool[1].ToString();
                        Address = dataPool[2].ToString();
                        Phone = int.Parse(dataPool[3].ToString());
                        Email = dataPool[4].ToString();
                        AccountNumber = int.Parse(dataPool[5].ToString());
                        Balance = int.Parse(dataPool[6].ToString());

                        int whiteSpaceLeft = 51;

                        //--Show the details on the screen
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
                        
                        //--Show 5 latest transactions
                        if(BankStatement.Count >= 5)
                        {
                            for (int i = BankStatement.Count - 1; i >= BankStatement.Count - 5; i--)
                            {
                                bankStatementToEmail.Add(bankStatement[i]);
                                Console.Write("\t | " + BankStatement[i]);
                                whiteSpaceLeft -= (BankStatement[i].Length + 1);
                                for (int j = 0; j < whiteSpaceLeft; j++)
                                {
                                    Console.Write(" ");
                                }
                                Console.WriteLine("|");
                                whiteSpaceLeft = 50;
                            }
                        }
                        else if(BankStatement.Count > 0 && BankStatement.Count < 5)
                        {
                            for (int i = BankStatement.Count - 1; i >= 0; i--)
                            {
                                bankStatementToEmail.Add(bankStatement[i]);
                                Console.Write("\t | " + BankStatement[i]);
                                whiteSpaceLeft -= (BankStatement[i].Length + 1);
                                for (int j = 0; j < whiteSpaceLeft; j++)
                                {
                                    Console.Write(" ");
                                }
                                Console.WriteLine("|");
                                whiteSpaceLeft = 50;
                            }
                        }
                        else
                        {
                            Console.Write("\t | There is no transactions record");
                            whiteSpaceLeft -= 32;
                            for (int j = 0; j < whiteSpaceLeft; j++)
                            {
                                Console.Write(" ");
                            }
                            Console.WriteLine("|");
                            whiteSpaceLeft = 50;
                        }
                       
                        //--Ask confirmation to send the record via email
                        Console.WriteLine("\t |                                                  |");
                        Console.WriteLine("\t ====================================================");
                        Console.Write("\t | Email this transaction? (y/n):");
                        int commandCursorY = Console.CursorLeft;
                        int commandCursorX = Console.CursorTop;
                        Console.WriteLine("                   |");
                        int messageCommandCursorY = Console.CursorLeft;
                        int messageCommandCursorX = Console.CursorTop;
                        Console.WriteLine(" ");
                        Console.WriteLine("\t ====================================================");

                        //--Close the stream
                        accountFile.Close();

                        //--Ask the confirmation command
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
                                    //--If yes, sent the email
                                    case "y":
                                        Console.SetCursorPosition(messageCommandCursorY, messageCommandCursorX);
                                        Console.Write(new string(' ', Console.WindowWidth));
                                        Console.SetCursorPosition(messageCommandCursorY, messageCommandCursorX);
                                        Console.WriteLine("\t | Sending email, please wait...                    |");
                                        //--If email is menaged to be sent, show the message
                                        if (SendingEmail(Email, "Account Statement", "Account Statement"))
                                        {
                                            Console.SetCursorPosition(messageCommandCursorY, messageCommandCursorX);
                                            Console.Write(new string(' ', Console.WindowWidth));
                                            Console.SetCursorPosition(messageCommandCursorY, messageCommandCursorX);
                                            Console.WriteLine("\t | Email is sent, check your email                  |");
                                        }
                                        //--If email is not menaged to be sent, show the message
                                        else
                                        {
                                            Console.SetCursorPosition(messageCommandCursorY, messageCommandCursorX);
                                            Console.WriteLine("\t | Failed to send email, try again                  |");
                                        }
                                        isValid = true;
                                        Console.ReadKey();
                                        continue;
                                    //--If no, back to the main menu
                                    case "n":
                                        isValid = true;
                                        break;
                                    default:
                                        isValid = true;
                                        break;
                                }
                            }
                            //--If the input is other than "y" or "n", show the message that the input should be "y" or "n"
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
                                Console.WriteLine("                   |");
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
                //--If account is not found, show the message
                else
                {
                    isValid = false;
                    Console.WriteLine("\t |                                                  |");
                    Console.WriteLine("\t ====================================================");
                    Console.Write("\t | Account is not found, try another? (y/n):");
                    int commandCursorY = Console.CursorLeft;
                    int commandCursorX = Console.CursorTop;
                    Console.WriteLine(" ");
                    int messageCommandCursorY = Console.CursorLeft;
                    int messageCommandCursorX = Console.CursorTop;
                    Console.WriteLine(" ");
                    Console.WriteLine("\t ====================================================");

                    //--Ask fo the confirmation
                    bool isValidCommand = false;
                    while (!isValidCommand)
                    {
                        Console.SetCursorPosition(messageCommandCursorY, messageCommandCursorX);
                        Console.WriteLine("\t |                                                  |");
                        Console.SetCursorPosition(commandCursorY, commandCursorX);
                        string command = Console.ReadLine();
                        //--If the input is yes, rerun the function
                        if (command.ToLower() == "y")
                        {
                            isValidCommand = true;
                            continue;

                        }
                        //--If not, back to the main menu
                        else if (command.ToLower() == "n")
                        {
                            isValidCommand = true;
                            isValid = true;
                        }
                        //--If the input is other than "y" or "n", show the message that the input should be "y" or "n"
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
                            Console.WriteLine("        |");
                            continue;
                        }
                    }
                }


            }
        }

        //--Delete
        public void Delete()
        {
            //--Do this function as long as it still active
            bool isValid = false;
            while (!isValid)
            {
                //--Show the user interface
                Console.Clear();
                Console.WriteLine("\t ====================================================");
                Console.WriteLine("\t |                                                  |");
                Console.WriteLine("\t |                 DELETE AN ACCOUNT                |");
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

                //--Get the user input
                Console.SetCursorPosition(accountNumberCursorY, accountNumberCursorX);
                string accountNumber = Console.ReadLine();
                int result = 0;
                //--Check, is the input is less than or equals to 10
                if (accountNumber.Length <= 10)
                {
                    //--Check, is the input is numeric or not
                    if (int.TryParse(accountNumber, out result))
                    {
                        AccountNumber = result;
                    }
                    //--If not numeric, show message that the input should be numeric
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
                //--If the input is more than 10, show message that the input should be lower than 10
                else
                {
                    Console.SetCursorPosition(messageCursorY, messageCursorX);
                    Console.WriteLine("Please input maximum 10 character");
                    Console.SetCursorPosition(messageSecondCursorY, messageSecondCursorX);
                    Console.WriteLine("Press any key to try again");
                    Console.ReadKey();
                    continue;
                }
                //--Check, is the file exist
                if (File.Exists("../../../Accounts/" + AccountNumber + ".txt"))
                {
                    //--If exist, show message account is found
                    Console.SetCursorPosition(messageCursorY, messageCursorX);
                    Console.WriteLine("Account found");

                    //--Try to read the file data
                    try
                    {
                        StreamReader accountFile = new StreamReader("../../../Accounts/" + AccountNumber + ".txt");
                        String data = accountFile.ReadLine();
                        //--Store the account data temporary on the datapool ArrayList
                        ArrayList dataPool = new ArrayList();
                        while (data != null)
                        {
                            string[] dataRow = data.Split('|');
                            dataPool.Add(dataRow[1]);
                            data = accountFile.ReadLine();
                        }
                        //--Assign the class properties
                        Firstname = dataPool[0].ToString();
                        Lastname = dataPool[1].ToString();
                        Address = dataPool[2].ToString();
                        Phone = int.Parse(dataPool[3].ToString());
                        Email = dataPool[4].ToString();
                        AccountNumber = int.Parse(dataPool[5].ToString());
                        Balance = int.Parse(dataPool[6].ToString());

                        int whiteSpaceLeft = 51;
                        //Show the account detail
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

                        //--Ask confirmation
                        Console.WriteLine("\t |                                                  |");
                        Console.WriteLine("\t ====================================================");
                        Console.Write("\t | Are you sure want to delete account? (y/n):");
                        int commandCursorY = Console.CursorLeft;
                        int commandCursorX = Console.CursorTop;
                        Console.WriteLine("      |");
                        int messageCommandCursorY = Console.CursorLeft;
                        int messageCommandCursorX = Console.CursorTop;
                        Console.WriteLine(" ");
                        Console.WriteLine("\t ====================================================");

                        //--Close the stream
                        accountFile.Close();

                        //--Check the input command
                        bool isValidCommand = false;
                        while (!isValidCommand)
                        {
                            Console.SetCursorPosition(messageCommandCursorY, messageCommandCursorX);
                            Console.WriteLine("\t |                                                  |");
                            Console.SetCursorPosition(commandCursorY, commandCursorX);
                            string command = Console.ReadLine();
                            //--If the input is "y", delete the account
                            if (command.ToLower() == "y")
                            {
                                //--Try to delete the account
                                try
                                {
                                    File.Delete("../../../Accounts/" + AccountNumber + ".txt");
                                    Console.SetCursorPosition(messageCommandCursorY, messageCommandCursorX);
                                    Console.Write(new string(' ', Console.WindowWidth));
                                    Console.SetCursorPosition(messageCommandCursorY, messageCommandCursorX);
                                    Console.WriteLine("\t | Account is delete                                |");
                                    Console.SetCursorPosition(messageCommandCursorY, messageCommandCursorX);
                                    isValid = true;

                                    isValidCommand = true;
                                    Console.ReadKey();
                                }
                                //--If failed to delete the account, show the message
                                catch (Exception e)
                                {
                                    Console.SetCursorPosition(messageCommandCursorY, messageCommandCursorX);
                                    Console.WriteLine("\t | Failed to delete                                 |");
                                    Console.WriteLine("\t ====================================================");
                                    Console.WriteLine("\t" + e.Message);
                                    isValidCommand = true;
                                    isValid = true;
                                    Console.ReadKey();
                                }
                                
                            }
                            //--If the input is "n", cancel the operation
                            else if (command.ToLower() == "n")
                            {
                                isValidCommand = true;
                                isValid = true;
                            }
                            //--If the input is other than "y" or "n", show the message that the input should be "y" or "n"
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
                    //--Show the message that the system is failed to read the file account
                    catch (IOException e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadKey();
                    }
                    continue;
                }
                //--If the file is not found, show the message
                else
                {

                    isValid = false;

                    Console.WriteLine("\t |                                                  |");
                    Console.WriteLine("\t ====================================================");
                    Console.Write("\t | Account is not found, try another? (y/n):");
                    int commandCursorY = Console.CursorLeft;
                    int commandCursorX = Console.CursorTop;
                    Console.WriteLine(" ");
                    int messageCommandCursorY = Console.CursorLeft;
                    int messageCommandCursorX = Console.CursorTop;
                    Console.WriteLine(" ");
                    Console.WriteLine("\t ====================================================");

                    bool isValidCommand = false;
                    while (!isValidCommand)
                    {
                        Console.SetCursorPosition(messageCommandCursorY, messageCommandCursorX);
                        Console.WriteLine("\t |                                                  |");
                        Console.SetCursorPosition(commandCursorY, commandCursorX);
                        string command = Console.ReadLine();
                        //--If the command is yes, rerun the function
                        if (command.ToLower() == "y")
                        {
                            isValidCommand = true;
                            continue;

                        }
                        //--If the command is no, back to the main menu
                        else if (command.ToLower() == "n")
                        {
                            isValidCommand = true;
                            isValid = true;
                        }
                        //--If the input is other than "y" or "n", show the message that the input should be "y" or "n"
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
                            Console.WriteLine("        |");
                            continue;
                        }
                    }
                }
            }
        }
    }
}
