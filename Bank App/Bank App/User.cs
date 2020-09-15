using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bank_App
{
    class User
    {
        private string username, password;

        public User(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }

        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }

        public bool CheckCredential()
        {
            bool isValid = false;
            string savedUsername, savedPassword;
            try
            {
                StreamReader loginPool = new StreamReader("../../../login.txt");
                String data = loginPool.ReadLine();
                while(data != null)
                {
                    string[] dataRow = data.Split('|');
                    savedUsername = dataRow[0];
                    savedPassword = dataRow[1];
                    
                    if(savedUsername == this.Username && savedPassword == this.Password)
                    {
                        isValid = true;
                        break;
                    }
                    data = loginPool.ReadLine();
                }
            } 
            catch(IOException e)
            {
                Console.WriteLine(e.Message);
            }
            return isValid;
        }
    }
}
