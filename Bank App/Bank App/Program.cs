using System;

namespace Bank_App
{
    class Program
    {
        static void Main(string[] args)
        {
            //--While the app is active, always show it
            bool credentialIsValid = false;
            while (!credentialIsValid)
            {
                //--Show the user interface
                Console.Clear();
                Console.WriteLine("\t ====================================================");
                Console.WriteLine("\t |                                                  |");
                Console.WriteLine("\t |          WELCOME to SIMPLE BANKING SYSTEM        |");
                Console.WriteLine("\t |                                                  |");
                Console.WriteLine("\t ====================================================");
                Console.WriteLine("\t |                   LOGIN TO START                 |");
                Console.WriteLine("\t |                                                  |");
                Console.Write("\t |   Username:");
                int usernameCursorX = Console.CursorTop;
                int usernameCursorY = Console.CursorLeft;
                Console.WriteLine("                                      |");
                Console.Write("\t |   Password:");
                int passwordCursorX = Console.CursorTop;
                int passwordCursorY = Console.CursorLeft;
                Console.WriteLine("                                      |");
                Console.WriteLine("\t |                                                  |");
                Console.WriteLine("\t ====================================================");


                int endlineCursorX = Console.CursorTop;
                int endlineCursorY = Console.CursorLeft;

                Console.SetCursorPosition(usernameCursorY, usernameCursorX);
                String username = Console.ReadLine();
                Console.SetCursorPosition(passwordCursorY, passwordCursorX);
                String password = "";
                char passwordChar = '*';
                bool isEnter = false;
                //--Change the input password with *
                while(!isEnter)
                {
                    var key = Console.ReadKey(intercept: true);
                    //--If the input is backspace, delete a character
                    if(key.Key == ConsoleKey.Backspace && password.Length > 0)
                    {
                        Console.Write("\b \b");
                        password = password[0..^1];
                    }
                    //--If the input is not part of the control button, assign the value
                    if(!char.IsControl(key.KeyChar))
                    {
                        Console.Write(passwordChar);
                        password += key.KeyChar;
                    }
                    //--If the input is enter, go to the next operation
                    if(key.Key == ConsoleKey.Enter)
                    {
                        isEnter = true;
                    }
                }

                Console.SetCursorPosition(endlineCursorY, endlineCursorX);
                //--Declare the class
                User user = new User(username, password);
                //--Check, is the user valid
                if (user.CheckCredential())
                {
                    Console.WriteLine("\t |                                                  |");
                    Console.WriteLine("\t |               Credential is valid                |");
                    Console.WriteLine("\t |            Press any key to continue             |");
                    Console.WriteLine("\t |                                                  |");
                    Console.WriteLine("\t ====================================================");
                    credentialIsValid = true;
                    Console.ReadKey();
                    MainMenu(user.Username);
                }
                //--If not valid, show the message
                else
                {
                    Console.WriteLine("\t |                                                  |");
                    Console.WriteLine("\t |             Credential is not valid              |");
                    Console.WriteLine("\t |            Press any key to continue             |");
                    Console.WriteLine("\t |                                                  |");
                    Console.WriteLine("\t ====================================================");
                    Console.ReadKey();
                }
            }
        }
        //--Main Menu
        static void MainMenu(string usernameLoginUser)
        {
            //--Show the main menu as long as it is active
            bool isActive = true;
            while (isActive)
            {
                //--Adjust the width of the screen
                int menuChoice = 0;
                int lineBeforeAndAfterGreeting = (52 - (usernameLoginUser.Length + 4)) / 2 - 1;
                int lineSpaceLeft = 52 - ((lineBeforeAndAfterGreeting * 2) + (usernameLoginUser.Length + 4 + 2));
                //--While is active
                while (isActive)
                {
                    //--Show the menu
                    Console.Clear();
                    Console.WriteLine("\t ====================================================");
                    Console.WriteLine("\t |                                                  |");
                    Console.Write("\t |");
                    for (int i = 0; i < lineBeforeAndAfterGreeting; i++)
                    {
                        Console.Write(" ");
                    }
                    Console.Write("Hi, {0}", usernameLoginUser);
                    for (int i = 0; i < lineBeforeAndAfterGreeting; i++)
                    {
                        Console.Write(" ");
                    }
                    if (lineSpaceLeft > 0)
                    {
                        Console.Write(" ");
                    }
                    Console.WriteLine("|");
                    Console.WriteLine("\t |          WELCOME to SIMPLE BANKING SYSTEM        |");
                    Console.WriteLine("\t |                                                  |");
                    Console.WriteLine("\t ====================================================");
                    Console.WriteLine("\t |                                                  |");
                    Console.WriteLine("\t |       1. Create a new account                    |");
                    Console.WriteLine("\t |       2. Search for an account                   |");
                    Console.WriteLine("\t |       3. Deposit                                 |");
                    Console.WriteLine("\t |       4. Withdraw                                |");
                    Console.WriteLine("\t |       5. A/C statement                           |");
                    Console.WriteLine("\t |       6. Delete account                          |");
                    Console.WriteLine("\t |       7. Exit                                    |");
                    Console.WriteLine("\t |                                                  |");
                    Console.WriteLine("\t ====================================================");
                    Console.Write("\t |   Enter yout choice (1-7): ");
                    int choiceCursorX = Console.CursorTop;
                    int choiceCursorY = Console.CursorLeft;
                    Console.WriteLine("                      |");
                    Console.Write("\t |   ");
                    int messageCursorX = Console.CursorTop;
                    int messageCursorY = Console.CursorLeft;
                    Console.WriteLine("                                               |");
                    Console.WriteLine("\t ====================================================");
                    Console.SetCursorPosition(choiceCursorY, choiceCursorX);
                    //--Get the user input
                    string inputChoice = Console.ReadLine();
                    //--Check, is the user input is numeric
                    if (int.TryParse(inputChoice, out menuChoice))
                    {
                        //--Check, is the user input is between 1 -7
                        if (menuChoice <= 7 && menuChoice >= 1)
                        {
                            //--Declare the account class
                            Account account = new Account();
                            switch (menuChoice)
                            {
                                case 1:
                                    account.CreateNewAccount();
                                    break;
                                case 2:
                                    account.SearchAnAccount();
                                    break;
                                case 3:
                                    account.Deposit();
                                    break;
                                case 4:
                                    account.Withdrawal();
                                    break;
                                case 5:
                                    account.AccountStatement();
                                    break;
                                case 6:
                                    account.Delete();
                                    break;
                                case 7:
                                    isActive = false;
                                    break;
                                default:
                                    Environment.Exit(0);
                                    break;
                            }
                        }
                        //--If the input is not between 1 - 7, show message
                        else
                        {
                            Console.SetCursorPosition(messageCursorY, messageCursorX);
                            Console.Write(new string(' ', 40));
                            Console.SetCursorPosition(messageCursorY, messageCursorX);
                            Console.Write("please only input 1 - 7");
                            Console.ReadKey();
                        }
                    }
                    //--If the input is not between 1 - 7, show message
                    else
                    {
                        Console.SetCursorPosition(messageCursorY, messageCursorX);
                        Console.Write(new string(' ', 40));
                        Console.SetCursorPosition(messageCursorY, messageCursorX);
                        Console.Write("please only input 1 - 7");
                        Console.ReadKey();
                    }
                }
            }
        }
    }
}
