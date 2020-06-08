using System;
using System.Collections.Generic;
using System.Linq;
using JokeGenerator.Application;

namespace JokeGenerator.ConsolePresentation
{
    /// <summary>
    /// The class implements facade of activity on presentation layer.
    /// You can implement a new presentation. But you have to support a contract <typeparam name="IPresentationBehavior"/>.
    /// </summary>
    internal sealed class ConsoleMaster : IPresentationBehavior
    {
        private readonly ConsoleWriter _consoleWriter;
        private string category_name;
        private string firstname;
        private string lastname;
        private string number_of_jokes;

        public ConsoleMaster(ConsoleWriter consoleWriter)
        {
            _consoleWriter = consoleWriter;
        }

        public void LoadingInformation()
        {
            Console.Clear();
            Console.WriteLine();
            _consoleWriter.WriteLine("Please wait...", ConsoleColor.DarkYellow);
        }

        /// <summary>
        /// Displays the joke parameters.
        /// Will display the default values if some values have not been provided by the user.
        /// </summary>
        public void DisplayProgress()
        {
            string name = null;
            if (!string.IsNullOrEmpty(firstname) || !string.IsNullOrEmpty(lastname))
            {
                name = firstname + " " + lastname;
            }

            _consoleWriter.WriteLine("Current Joke Configuration:\n");
            _consoleWriter.WriteLine(string.Format("Selected Name: {0}", name ?? "Chuck Norris"), ConsoleColor.DarkYellow);
            _consoleWriter.WriteLine(string.Format("Selected Category: {0}", category_name ?? "None"), ConsoleColor.DarkYellow);
            _consoleWriter.WriteLine(string.Format("Jokes To View: {0}\n", number_of_jokes ?? "1"), ConsoleColor.DarkYellow);
        }

        /// <summary>
        /// Displays the main menu to select the actions for joke configuration.
        /// </summary>
        public ConsoleResults MakeChoose()
        {
            Console.Clear();

            Console.WriteLine();
            _consoleWriter.WriteLine("**************************************************************************************************** ", ConsoleColor.Green);
            _consoleWriter.WriteLine("                                 Welcome to the Joke Factory ", ConsoleColor.Green);
            _consoleWriter.WriteLine("**************************************************************************************************** ", ConsoleColor.Green);
            Console.WriteLine();
            DisplayProgress();

            _consoleWriter.WriteLine("Press one of the following keys to configure your joke: ");
            Console.WriteLine();

            var key = QuestionsForSingleKeyStroke(
                new[] { "| J | To generate a joke",  "| A | To choose the number of jokes", "| C | To select a category",
                    "| N | To provide the name you wish", "| R | To generate a random name", "| Q | To quit" },
                new[] { 'j', 'c', 'r', 'n', 'a', 'q' });
            switch (key)
            {
                case 'j':
                case 'J':
                    Console.WriteLine();
                    _consoleWriter.WriteLine(" Working on your jokes..... ", ConsoleColor.Green);
                    return ConsoleResults.J;
                case 'c':
                case 'C':
                    return ConsoleResults.C;
                case 'r':
                case 'R':
                    return ConsoleResults.R;
                case 'n':
                case 'N':
                    return ConsoleResults.N;
                case 'a':
                case 'A':
                    return ConsoleResults.A;
                default:
                    return ConsoleResults.Q;
            }
        }

        /// <summary>
        /// Sets the values for the global strings "firstname" and "lastname" from the 
        /// </summary>
        public void SetRandomName(string randomfirstname, string randomlastname)
        {
            if (!string.IsNullOrEmpty(randomfirstname) || !string.IsNullOrEmpty(randomlastname))
            {
                firstname = randomfirstname;
                lastname = randomlastname;
            }
        }

        /// <summary>
        /// Displays the available joke's categories 
        /// </summary>
        public void WriteCategories(IList<string> categories)
        {
            DisplayCategory(categories);
        }

        private void DisplayCategory(IList<string> strings)
        {
            Console.WriteLine();

            Console.WriteLine("List of categories:");
            Console.WriteLine();
            int counter = 0;
            foreach (string category in strings)
            {
                Console.Write($" {category,10} {"|"}");
                counter += 1;

                if ((counter % 4) == 0)
                {
                    Console.WriteLine();
                }
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Prompts user to enter the first name to use in the jokes.
        /// </summary>
        public string InputName()
        {
            firstname = AskForAName(new[] { "Please enter a first name", "[Press enter]" });
            return firstname;
        }

        /// <summary>
        /// Prompts user to enter the last name to use in the jokes.
        /// </summary>
        public string InputLastName()
        {
            lastname = AskForAName(new[] { "Please enter a last name", "[Press enter]" });
            return lastname;
        }

        /// <summary>
        /// Asks the user if one wants to specify a category from the diaplayed categories.
        /// </summary>
        public ConsoleResults WannaSpecifyCategory()
        {
            var key = QuestionsForSingleKeyStroke(new[] { "Want to specify a category? y/n" }, new[] { 'y', 'n' });

            switch (key)
            {
                case 'y':
                case 'Y':
                    return ConsoleResults.Y;
                default:
                    return ConsoleResults.N;
            }
        }

        /// <summary>
        /// Prompts user to enter category of their choice from the diaplayed categories.
        /// </summary>
        public string EnterCategory(IList<string> categories)
        {
            category_name = AskForACategory(new[] { "Enter a category:", "[Press enter]" }, categories);
            return category_name;
        }

        /// <summary>
        /// Prompts user to enter the amount of jokes that the user wants to view.
        /// </summary>
        public int HowManyJokes()
        {
            int jokes = QuestionsForNumber(new[] { "How many jokes do you want? (1-9)" });
            number_of_jokes = Convert.ToString(jokes);
            return jokes;
        }

        /// <summary>
        /// Displays the selected number of jokes.
        /// </summary>
        public void WriteJokes(IList<string> jokes)
        {
            DisplayJokes(jokes);
        }

        private void DisplayJokes(IList<string> strings)
        {
            Console.WriteLine();
            Console.Clear();
            Console.WriteLine();
            _consoleWriter.WriteLine("**************************************************************************************************** ", ConsoleColor.Green);
            _consoleWriter.WriteLine("                                                  Joke(s): ", ConsoleColor.Green);
            _consoleWriter.WriteLine("**************************************************************************************************** ", ConsoleColor.Green);
            Console.WriteLine();
            int jokecount = 1;
            foreach (string joke in strings)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("=> ", ConsoleColor.DarkYellow);
                Console.ResetColor();
                Console.Write(joke);
                Console.WriteLine();
                Console.WriteLine();
                jokecount += 1;
            }

            Console.WriteLine();
            _consoleWriter.WriteLine("Press a key to go back to the main menu", ConsoleColor.Green);
            Console.ReadKey();
        }

        /// <summary>
        /// Validates the entered key value whether it is from the given configuration options. 
        /// If not then it propts the user to select from the valid configuartion options
        /// </summary>
        private char QuestionsForSingleKeyStroke(string[] lines, char[] acceptableChars)
        {
            foreach (var line in lines)
            {
                _consoleWriter.WriteLine(line, chooseFlag: true);
            }

            // Allows maximum three incorrect attempts from the user.
            var maxAttempts = 3;
            while (maxAttempts > 0)
            {
                var keyChar = Console.ReadKey().KeyChar;
                if (acceptableChars.Any(x => char.ToLower(keyChar) == x) || acceptableChars.Any(x => char.ToUpper(keyChar) == x))
                {
                    Console.WriteLine();
                    return keyChar;
                }

                Console.WriteLine();
                _consoleWriter.WriteLine("Options to respond to the question(s) are: {0}", args: string.Join(",", acceptableChars));
                _consoleWriter.WriteLine("Please Try Again");
                maxAttempts--;
            }

            throw new ApplicationException("Please enter correct answer.");
        }

        private int QuestionsForNumber(string[] lines)
        {
            Console.WriteLine();
            foreach (var l in lines)
            {
                _consoleWriter.WriteLine(l, ConsoleColor.Green);
            }

            // Allows maximum three incorrect attempts from the user, after that the application exits.
            var maxAttempts = 3;
            while (maxAttempts > 0)
            {
                var c = Console.ReadKey(false).KeyChar;

                if (int.TryParse(c.ToString(), out var i))
                {
                    Console.WriteLine();
                    return i;
                }

                Console.WriteLine();
                _consoleWriter.WriteLine("Please enter a positive numeric value.");
                _consoleWriter.WriteLine("Please Try Again");
                maxAttempts--;
            }

            throw new ApplicationException("Please enter correct answer.");
        }


        private string AskForAName(string[] lines)
        {
            Console.WriteLine();
            foreach (var line in lines)
            {
                _consoleWriter.WriteLine(line, ConsoleColor.Green);
            }
            return Console.ReadLine();
        }

        private string AskForACategory(string[] lines, IList<string> categories)
        {
            Console.WriteLine();
            foreach (var line in lines)
            {
                _consoleWriter.WriteLine(line, ConsoleColor.Green);
            }

            // Allows maximum three incorrect attempts from the user, after that the application exits.
            var maxAttempts = 3;
            while (maxAttempts > 0)
            {
                var requestedCategory = Console.ReadLine();

                if (categories.Contains(requestedCategory))
                    return requestedCategory;

                Console.WriteLine();
                _consoleWriter.WriteLine("Please enter a valid category from the following list:");
                Console.WriteLine(string.Join(",", categories));
                _consoleWriter.WriteLine("Please Try Again");
                maxAttempts--;
            }

            Console.WriteLine();
            throw new ApplicationException("Please enter correct answer.");
        }

        public int ExpectedExceptionMessage(ApplicationException appex)
        {
            _consoleWriter.WriteLine("Something went wrong that can be fixed.");
            _consoleWriter.WriteLine(appex.Message);
            return 1;
        }
    }
}