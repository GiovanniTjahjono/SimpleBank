using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bank_App
{
    class User
    {
        //--Declare the class properties
        private string username, password;
        //--Create the class contructor
        public User(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }
        //--Create getter and setter
        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }

        //--Check the credential
        public bool CheckCredential()
        {
            bool isValid = false;
            string savedUsername, savedPassword;
            //--Try to read the file
            try
            {
                StreamReader loginPool = new StreamReader("../../../login.txt");
                String data = loginPool.ReadLine();
                while(data != null)
                {
                    string[] dataRow = data.Split('|');
                    savedUsername = dataRow[0];
                    savedPassword = dataRow[1];
                    //--If the username and password is match, return true
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
            //--Return the result
            return isValid;
        }
    }
}
