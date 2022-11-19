using SocialMedia;
using SocialMedia.MongoDbEnt;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SocialMedia.Menu
{
    public class Menu
    {
        private MongoDB mongoDB = new MongoDB();
        private Neo4J neo4J = new Neo4J();

        public void ShowMenu()
        {
            char userInput;
            Console.Write("Sing in? (Y/N): ");
            userInput = Console.ReadKey().KeyChar;
            if (userInput != 'Y' && userInput == 'N')
            {
                Console.WriteLine("\nWrong command selected");
            }
            else
            {
                Authentication(userInput);
                showMenu();
            }
        }

        private void showMenu()
        {
            char userInput = ' ';
            while (userInput != '0')
            {
                Console.Clear();
                Console.WriteLine("     ~~~~~~~Social Media~~~~~~~\n");
                Console.WriteLine(" Select an option:\n");
                Console.WriteLine("1-- Show Posts");
                Console.WriteLine("2-- Show follows");
                Console.WriteLine("3-- Create new user");
                Console.WriteLine("4-- Show shortest path");
                Console.WriteLine("0-- Exit");
                Console.Write("Option: ");
                userInput = Console.ReadKey().KeyChar;
                selectShowMenuOption(userInput);
            }
        }

        public void Authentication(char userInput)
        {
            bool authenticationRes;
            while (userInput == 'Y')
            {
                Console.Clear();
                authenticationRes = checkAuthentication();
                if (authenticationRes)
                {
                    break;
                }
                else
                {
                    Console.Write("\nSomething went wrong... Try again? (Y/N): ");
                    userInput = Console.ReadKey().KeyChar;
                }
            }
        }

        private bool checkAuthentication()
        {
            Console.Write("Enter username: ");
            string username = Console.ReadLine();

            Console.Write("Enter password: ");
            string password = readPassword();

            var successAuthentication = mongoDB.Authtentification(username, password);
            neo4J.Authtentification(username, password);

            return successAuthentication;
        }

        public static string readPassword()
        {
            string password = "";
            ConsoleKeyInfo info = Console.ReadKey(true);
            while (info.Key != ConsoleKey.Enter)
            {
                if (!char.IsControl(info.KeyChar))
                {
                    Console.Write("*");
                    password += info.KeyChar;
                }
                else if (info.Key == ConsoleKey.Backspace)
                {
                    if (!string.IsNullOrEmpty(password))
                    {
                        password = password.Substring(0, password.Length - 1);
                        int pos = Console.CursorLeft;
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                        Console.Write(" ");
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                    }
                }
                info = Console.ReadKey(true);
            }
            Console.WriteLine();
            return password;
        }

        private void selectShowMenuOption(char userInput)
        {
            switch (userInput)
            {
                case '1':
                    showPosts(posts);
                    break;
                case '2':
                    showFollows();
                    break;
                case '3':
                    createNewUser();
                    break;
                case '4':
                    Console.WriteLine("Enter user`s name\n");
                    string Username = Console.ReadLine();
                    DistanceBetweenUsers(Username);
                    break;
                case '0':
                    break;
                default:
                    Console.WriteLine("\nWrong option selected");
                    break;
            }
        }
        private void showFollows()
        {
            char userInput = ' ';
            List<User> follows;
            while (userInput != '0')
            {
                Console.Clear();
                follows = mongoDB.ShowFollows();
                Console.WriteLine("Follows:\n");
                if (follows.Count() == 0)
                {
                    Console.WriteLine("You have no followers");
                }
                else
                {
                    foreach (var f in follows)
                    {
                        Console.WriteLine(f);
                    }
                }
            }
        }
        private void createNewUser()
        {
            Console.Clear();
            string userName;
            Console.Write("Your username : ");
            userName = Console.ReadLine();
            string firstName;
            Console.Write("Your first name : ");
            firstName = Console.ReadLine();
            string lastName;
            Console.Write("Your last name : ");
            lastName = Console.ReadLine();
            string email;
            Console.Write("Your email : ");
            email = Console.ReadLine();
            string password;
            Console.Write("Your password : ");
            password = readPassword();
            List<string> follows = new List<string>();

            mongoDB.NewUser(userName, firstName, lastName, password, email, follows);
            neo4J.CreateUser(userName, firstName, lastName, password, email);
        }
        private void showPosts(List<Post> posts)
        {
            int index = 0;
            char userInput;
            if (posts.Count == 0)
            {
                Console.WriteLine("There is no post");
                return;
            }
            while (index < posts.Count)
            {
                Console.Clear();
                Console.WriteLine(post);
                Console.WriteLine("1-- Show comments\n2-- Go to the next post\n0-- Exit");
                Console.Write("Option : ");
                userInput = Console.ReadKey().KeyChar;
                switch (userInput)
                {
                    case '1':
                        showComments(post);
                        break;
                    case '3':
                        index++;
                        break;
                    case '0':
                        break;
                    default:
                        Console.WriteLine("\nWrong command selected");
                        break;
                }
                if (userInput == '0') break;
            }
            if (index == posts.Count)
            {
                Console.WriteLine("\nThat was last post.");
                Console.ReadLine();
            }
        }
        private void showComments(Post post)
        {
            char userInput = ' ';
            while (userInput != '0')
            {
                Console.Clear();
                foreach (Comment comment in post)
                {
                    Console.WriteLine(comment);
                    Console.WriteLine();
                }
                Console.WriteLine("1) Write comment\n0) Exit");
                Console.Write("Your choice : ");
                userInput = Console.ReadKey().KeyChar;
                switch (userInput)
                {
                    case '1':
                        string userComment;
                        Console.Write("Your comment : ");
                        userComment = Console.ReadLine();
                        mongoDB.CreateComment(post, userComment);
                        break;
                    case '0':
                        break;
                    default:
                        Console.WriteLine("\nWrong command selected");
                        break;
                }
            }
        }

        //Task Neo4J(distance between users)
        private void DistanceBetweenUsers(string username)
        {
            var Relationships = neo4J.Relationship(username);
            if (Relationships.Count() != 0)
            {
                Console.WriteLine("\nYou have relationship");
                Console.WriteLine($"The distance is: {neo4J.shortestPath(username)}");
            }
            else
            {
                Console.WriteLine("\nYou don`t have relationship");
                Console.WriteLine($"The distance is: {neo4J.shortestPath(username)}");
            }
        }
    }
}