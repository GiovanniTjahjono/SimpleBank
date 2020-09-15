using System;

namespace Bank_App
{
    class Program
    {
        static void Main(string[] args)
        {
            bool credentialIsValid = false;
            Account newA = new Account();
            newA.CreateNewAccount();
            while(!credentialIsValid)
            {
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
                String password = Console.ReadLine();

                

                Console.SetCursorPosition(endlineCursorY, endlineCursorX);

                User user = new User(username, password);
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
        static void MainMenu(string usernameLoginUser)
        {
            bool isActive = true;
            while (isActive)
            {


                //52 chars
                bool inputChoiceIsValid = false;
                int menuChoice = 0;
                int lineBeforeAndAfterGreeting = (52 - (usernameLoginUser.Length + 4)) / 2 - 1;
                int lineSpaceLeft = 52 - ((lineBeforeAndAfterGreeting * 2) + (usernameLoginUser.Length + 4 + 2));

                while (!inputChoiceIsValid)
                {
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
                    Console.WriteLine("\t ====================================================");
                    Console.SetCursorPosition(choiceCursorY, choiceCursorX);
                    string inputChoice = Console.ReadLine();
                    if (int.TryParse(inputChoice, out menuChoice))
                    {
                        if (menuChoice <= 7 || menuChoice >= 1)
                        {
                            inputChoiceIsValid = true;
                        }
                    }
                    else
                    {
                        Console.WriteLine("please input 1 - 7");
                    }
                }

                switch (menuChoice)
                {
                    case 1:
                        Console.WriteLine("1");
                        break;
                    case 7:
                        isActive = false;
                        break;
                    default:
                        Environment.Exit(0);
                        break;
                }
            }
        }
    }
}
