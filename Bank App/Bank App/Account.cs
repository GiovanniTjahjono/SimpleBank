using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bank_App
{
    class Account
    {
        private string firstname, lastname, address, email;
        private int phone;

        public Account()
        {
            
        }

        public string Firstname { get => firstname; set => firstname = value; }
        public string Lastname { get => lastname; set => lastname = value; }
        public string Address { get => address; set => address = value; }
        public int Phone { get => phone; set => phone = value; }
        public string Email { get => email; set => email = value; }

        public string GenerateUniqueAccountNumber()
        {
            Random random = new Random();
            string accountNumber = "";
            bool accountNumberIsValid = false;
            while(!accountNumberIsValid)
            {
                accountNumber = "";
                for (int i = 0; i < 8; i++)
                {
                    accountNumber += random.Next(0, 9);
                }
                if(File.Exists("../../../Accounts/" + accountNumber + ".txt"))
                {
                    continue;
                } else
                {
                    accountNumberIsValid = true;
                }
            }
            
            return accountNumber;
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
                        
                        string accountNumber = GenerateUniqueAccountNumber();
                        try
                        {
                            File.WriteAllText("../../../Accounts/"+accountNumber + ".txt", "Firstname|" + Firstname + "\n" +
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
    }
}
