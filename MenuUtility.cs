using System;

namespace mis_221_pa_5_sydneymarch
{
    public static class MenuUtility
    {
        public static string LoggedInEmail = null;

        public static void Clear()
        {
            Console.Clear();
        }

        public static void Pause()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey(true);
        }

        public static string Login()
        {


            bool keepGoing = true;

            while (keepGoing)
            {
                User[] users = new User[100];
                int userCount = LoadUsers(users);

                Clear();
                string[] options = { "Login", "Register", "Exit" };
                int choice = SelectionMenu(options, "🍕 Welcome to Papa's Pizzeria");

                if(choice == 2) return "exit";
                if (choice == 1)
                {
                    Register();
                    continue;
                }

                // Login flow
                Clear();
                string username = GetStringInput("Enter username:");
                if (username == "-1") continue;

                Clear();
                string password = GetStringInput("Enter password:");
                if (password == "-1") continue;

                for (int i = 0; i < userCount; i++)
                {
                    if (users[i] != null && 
                        users[i].GetUsername() == username && 
                        users[i].GetPassword() == password)
                    {
                        LoggedInEmail = username;
                        return users[i].GetRole();
                    }
                }

                Console.WriteLine("Invalid credentials. Try again.");
                Pause();
            }

            return "exit";
        }

        public static void Register()
        {
            User[] users = new User[100];
            int userCount = LoadUsers(users);

            Clear();
            Console.WriteLine("Register New Account\n");

            string username = GetStringInput("Enter a username:");
            if (username == "-1") return;

            // Check for duplicate username
            for (int i = 0; i < userCount; i++)
            {
                if (users[i].GetUsername() == username)
                {
                    Console.WriteLine("Username already exists. Please try again.");
                    Pause();
                    return;
                }
            }

            string password = GetStringInput("Enter a password:");
            if (password == "-1") return;

            string role = "customer";

            StreamWriter outFile = new StreamWriter("users.txt", true); // append mode
            outFile.WriteLine($"{username}#{password}#{role}");
            outFile.Close();

            Console.WriteLine("Registration successful!");
            Pause();
        }

        public static int SelectionMenu(string[] options)
        {
            return SelectionMenu(options, null);
        }

        public static int SelectionMenu(string[] options, string instructions)
        {
            int curr = 0;
            bool hasPickedOption = false;

            while (!hasPickedOption)
            {
                Clear();
                if (!string.IsNullOrEmpty(instructions))
                {
                    Console.WriteLine(instructions + "\n");
                }

                for (int i = 0; i < options.Length; i++)
                {
                    if (i == curr)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    Console.WriteLine(options[i]);
                    Console.ResetColor();
                }

                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.DownArrow || key == ConsoleKey.S)
                {
                    curr = (curr + 1) % options.Length;
                }
                else if (key == ConsoleKey.UpArrow || key == ConsoleKey.W)
                {
                    curr = (curr - 1 + options.Length) % options.Length;
                }
                else if (key == ConsoleKey.Enter)
                {
                    hasPickedOption = true;
                }
            }
            return curr;
        }

        public static int GetIntInput(string prompt)
        {
            Console.WriteLine($"{prompt} (or enter -1 to cancel):");
            string input = Console.ReadLine();

            if (input == "-1") return -1;

            if (int.TryParse(input, out int value))
            {
                return value;
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
                Pause();
                Clear();
                return GetIntInput(prompt);
            }
        }

        public static double GetDoubleInput(string prompt)
        {
            Console.WriteLine($"{prompt} (or enter -1 to cancel):");
            string input = Console.ReadLine();

            if (input == "-1") return -1;

            if (double.TryParse(input, out double value))
            {
                return value;
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
                Pause();
                Clear();
                return GetDoubleInput(prompt);
            }
        }

        public static string GetStringInput(string prompt)
        {
            Console.WriteLine($"{prompt} (or enter -1 to cancel):");
            string input = Console.ReadLine();

            if (input == "-1") return "-1";

            if (!string.IsNullOrWhiteSpace(input))
            {
                return input;
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid string.");
                Pause();
                Clear();
                return GetStringInput(prompt);
            }
        }

        private static int LoadUsers(User[] users)
        {
            int count = 0;
            StreamReader inFile = new StreamReader("users.txt");
            string line = inFile.ReadLine();

            while (line != null && count < users.Length)
            {
                string[] temp = line.Split('#');
                if (temp.Length == 3)
                {
                    users[count] = new User(temp[0], temp[1], temp[2]);
                    count++;
                }

                line = inFile.ReadLine();
            }

            inFile.Close();
            return count;
        }

        public static void DisplaySplashScreen()
        {
            Console.Clear();

            string[] frame1 = new string[]
            {
                "⬜⬜⬜⬜⬜⬛⬛⬛⬛⬛⬛⬛⬛⬜⬜⬜⬜⬜",
                "⬜⬛⬛⬛⬛🟫🟫🟫🟫🟫🟫🟫🟫⬛⬛⬛⬛⬜",
                "⬛🟫🟫🟫🟫🟫🟫🟫🟫🟫🟫🟫🟫🟫🟫🟫🟫⬛",
                "⬛🟫🟫🟫🟨🟨🟨🟥🟥🟥🟨🟨🟨🟨🟫🟫🟫⬛",
                "⬜⬛🟨🟨🟨🟨🟨🟥🟥🟥🟨🟨🟨🟨🟥🟥⬛⬜",
                "⬜⬛🟨🟥🟥🟨🟨🟨🟥🟨🟨🟨🟨🟨🟥🟥⬛⬜",
                "⬜⬜⬛🟥🟥🟨🟨🟨🟨🟨🟥🟥🟨🟨🟨⬛⬜⬜",
                "⬜⬜⬛🟨🟨🟥🟥🟨🟨🟥🟥🟥🟥🟨🟨⬛⬜⬜",
                "⬜⬜⬜⬛🟥🟥🟥🟥🟨🟨🟥🟥🟨🟨⬛⬜⬜⬜",
                "⬜⬜⬜⬜⬛🟥🟥🟥🟨🟨🟨🟨🟨⬛⬜⬜⬜⬜",
                "⬜⬜⬜⬜⬛🟥🟥🟥🟨🟨🟨🟨🟨⬛⬜⬜⬜⬜",
                "⬜⬜⬜⬜⬜⬛🟥🟨🟨🟥🟥🟨⬛⬜⬜⬜⬜⬜",
                "⬜⬜⬜⬜⬜⬜⬛🟨🟨🟨🟨⬛⬜⬜⬜⬜⬜⬜",
                "⬜⬜⬜⬜⬜⬜⬛🟨🟨🟨🟨⬛⬜⬜⬜⬜⬜⬜",
                "⬜⬜⬜⬜⬜⬜⬜⬛🟨🟨⬛⬜⬜⬜⬜⬜⬜⬜",
                "⬜⬜⬜⬜⬜⬜⬜⬜⬛⬛⬜⬜⬜⬜⬜⬜⬜⬜",
            };

            string[] frame2 = new string[]
            {
                "⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜",
                "⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜",
                "⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜",
                "⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜",
                "⬜⬛⬛⬛⬛⬛⬛⬛⬛⬛⬛⬛⬛⬛⬛⬛⬛⬛⬛⬛⬛⬜    ",
                "⬛🟥🟥🟥⬛🟥⬛🟥🟥🟥🟥⬛🟥🟥🟥🟥⬛⬛🟥🟥⬛⬛      ",
                "⬛🟥⬛🟥⬛🟥⬛⬛🟥🟥⬛⬛⬛🟥🟥⬛⬛🟥⬛⬛🟥⬛",
                "⬛🟥⬛🟥⬛🟥⬛🟥🟥⬛⬛⬛🟥🟥⬛⬛⬛🟥🟥🟥🟥⬛     ",
                "⬛🟥🟥⬛⬛🟥⬛🟥🟥🟥🟥⬛🟥🟥🟥🟥⬛🟥⬛⬛🟥⬛     ",
                "⬛🟥⬛⬛⬛⬛⬛⬛⬛⬛⬛⬛⬛⬛⬛⬛⬛⬛⬛⬛⬛⬜                 ",
                "⬛🟥⬛⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜",
                "⬜⬛⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜",
                "⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜",
                "⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜",
                "⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜",
                "⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜",
            };

            for (int i = 0; i < 3; i++) // wave 3 times
            {
                Console.Clear();
                foreach (var line in frame1)
                {
                    Console.WriteLine(line);
                }
                Thread.Sleep(400);

                Console.Clear();
                foreach (var line in frame2)
                {
                    Console.WriteLine(line);
                }
                Thread.Sleep(400);
            }

            string message = "🍕 Welcome to Papa's Pizzeria 🍕";
            Console.WriteLine();

            for (int i = 0; i < message.Length; i++)
            {
                Console.Write(message[i]);
                Thread.Sleep(65); // typing speed
            }

            Thread.Sleep(300);

            MenuUtility.Pause();

        }
    }
}
