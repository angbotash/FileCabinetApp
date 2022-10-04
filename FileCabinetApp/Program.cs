using System.Globalization;

namespace FileCabinetApp
{
    public static class Program
    {
        private const string DeveloperName = "Angela Botasheva";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        private static readonly FileCabinetService fileCabinetService = new FileCabinetService();

        private static bool isRunning = true;

        private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("exit", Exit),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("edit", Edit),
            new Tuple<string, Action<string>>("find", Find),
        };

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "stat", "shows statistics on record", "The 'stat' command shows statistics on record." },
            new string[] { "create", "creates a new record", "The 'create' command creates a new record." },
            new string[] { "list", "prints the list of all records", "The 'list' command prints the list of all records." },
            new string[] { "edit", "edits a record", "The 'edit' command edits an existing record." },
            new string[] { "find", "finds records", "The 'find' command finds existing records." },
        };

        public static void Main(string[] args)
        {
            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            Console.WriteLine(Program.HintMessage);
            Console.WriteLine();

            do
            {
                Console.Write("> ");
                var line = Console.ReadLine();
                var inputs = line != null ? line.Split(' ', 2) : new string[] { string.Empty, string.Empty };
                const int commandIndex = 0;
                var command = inputs[commandIndex];

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(Program.HintMessage);
                    continue;
                }

                var index = Array.FindIndex(commands, 0, commands.Length, i => i.Item1.Equals(command, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    const int parametersIndex = 1;
                    var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                    commands[index].Item2(parameters);
                }
                else
                {
                    PrintMissedCommandInfo(command);
                }
            }
            while (isRunning);
        }

        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }

        private static void PrintHelp(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[Program.CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][Program.ExplanationHelpIndex]);
                }
                else
                {
                    Console.WriteLine($"There is no explanation for '{parameters}' command.");
                }
            }
            else
            {
                Console.WriteLine("Available commands:");

                foreach (var helpMessage in helpMessages)
                {
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[Program.CommandHelpIndex], helpMessage[Program.DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }

        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            isRunning = false;
        }

        private static void Stat(string parameters)
        {
            var recordsCount = Program.fileCabinetService.GetStat();

            Console.WriteLine($"{recordsCount} record(s).");
        }

        private static void Create(string parameters)
        {
            var firstName = FirstNameCheck();
            var lastName = LastNameCheck();
            var dateOfBirth = DateOfBirthCheck();
            var areaCode = AreaCodeCheck();
            var savings = SavingsCheck();
            var gender = GenderCheck();

            var newRecordId = Program.fileCabinetService.CreateRecord(firstName, lastName, dateOfBirth, areaCode, savings, gender);

            Console.WriteLine($"Record #{newRecordId} was created!");
        }

        private static void List(string parameters)
        {
            var records = Program.fileCabinetService.GetRecords();

            PrintRecords(records);
        }

        private static void Edit(string parameters)
        {
            int id;

            if (int.TryParse(parameters, out id) && id > 0)
            {
                var record = fileCabinetService.GetRecord(id);

                if (record is null)
                {
                    Console.WriteLine($"#{id} record is not found.");
                    return;
                }

                var firstName = FirstNameCheck();
                var lastName = LastNameCheck();
                var dateOfBirth = DateOfBirthCheck();
                var areaCode = AreaCodeCheck();
                var savings = SavingsCheck();
                var gender = GenderCheck();

                Program.fileCabinetService.EditRecord(id, firstName, lastName, dateOfBirth, areaCode, savings, gender);

                Console.WriteLine($"Record #{id} is updated.");
            }
            else
            {
                Console.WriteLine("Please enter the Id of an existing record.");
            }
        }

        private static void Find(string parameters)
        {
            var searchData = parameters.Split(' ', 2);

            if (string.IsNullOrWhiteSpace(parameters) || (string.IsNullOrWhiteSpace(searchData[0]) || string.IsNullOrWhiteSpace(searchData[1])))
            {
                Console.WriteLine("Please enter a search category and a record data.");
                return;
            }

            var searchCategory = searchData[0].ToLowerInvariant();
            var recordData = searchData[1].ToLowerInvariant();

            switch (searchCategory)
            {
                case "firstname":
                    var records = Program.fileCabinetService.FindByFirstName(recordData);
                    PrintRecords(records);
                    break;
            }
        }

        // User input check methods
        private static string FirstNameCheck()
        {
            string? firstName;

            do
            {
                Console.Write("First name: ");
                firstName = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(firstName))
                {
                    Console.WriteLine("Please enter a first name.");
                }
                else if (firstName.Length < 2 || firstName.Length > 60)
                {
                    Console.WriteLine("The first name should be 2-60 characters long.");
                }
            }
            while (string.IsNullOrWhiteSpace(firstName) || (firstName.Length < 2 || firstName.Length > 60));

            return firstName;
        }

        private static string LastNameCheck()
        {
            string? lastName;

            do
            {
                Console.Write("Last name: ");
                lastName = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(lastName))
                {
                    Console.WriteLine("Please enter a last name.");
                }
                else if (lastName.Length < 2 || lastName.Length > 60)
                {
                    Console.WriteLine("The last name should be 2-60 characters long.");
                }
            }
            while (string.IsNullOrWhiteSpace(lastName) || (lastName.Length < 2 || lastName.Length > 60));

            return lastName;
        }

        private static DateTime DateOfBirthCheck()
        {
            DateTime dateOfBirth;
            bool correctDateFormat;

            do
            {
                Console.Write("Date of birth: ");
                var dateOfBirthString = Console.ReadLine();
                correctDateFormat = DateTime.TryParse(dateOfBirthString, out dateOfBirth);

                if (correctDateFormat && (dateOfBirth < new DateTime(1950, 1, 1) || dateOfBirth > DateTime.Today))
                {
                    correctDateFormat = false;
                    Console.WriteLine("The date of birth cannot be earlier than 1-Jan-1950 or later than the current date.");
                }
                else if (!correctDateFormat)
                {
                    Console.WriteLine("Please enter the date of birth in the correct format. Ex.: mm/dd/yyyy - 01/01/1973");
                }
            }
            while (!correctDateFormat);

            return dateOfBirth;
        }

        private static short AreaCodeCheck()
        {
            short areaCode;
            bool correctAreCodeFormat;

            do
            {
                Console.Write("Area code: ");
                var areaCodeString = Console.ReadLine();
                correctAreCodeFormat = short.TryParse(areaCodeString, out areaCode);

                if (correctAreCodeFormat && areaCode < 0)
                {
                    correctAreCodeFormat = false;
                    Console.WriteLine("The area code cannot be a negative number.");
                }
                else if (!correctAreCodeFormat)
                {
                    Console.WriteLine("Please enter an area code.");
                }
            }
            while (!correctAreCodeFormat);

            return areaCode;
        }

        private static decimal SavingsCheck()
        {
            decimal savings;
            bool correctSavingsFormat;

            do
            {
                Console.Write("Amount of savings: ");
                var savingsString = Console.ReadLine();
                correctSavingsFormat = decimal.TryParse(savingsString, out savings);

                if (correctSavingsFormat && savings < 0)
                {
                    correctSavingsFormat = false;
                    Console.WriteLine("The amount of savings cannot be a negative number.");
                }
                else if (!correctSavingsFormat)
                {
                    Console.WriteLine("Please enter an amount of savings.");
                }
            }
            while (!correctSavingsFormat);

            return savings;
        }

        private static char GenderCheck()
        {
            char gender;
            bool correctGenderFormat;

            do
            {
                Console.Write("Gender: ");
                var genderString = Console.ReadLine();
                correctGenderFormat = char.TryParse(genderString, out gender);

                if (correctGenderFormat)
                {
                    if (gender != 'F' && gender != 'M' && gender != 'N')
                    {
                        correctGenderFormat = false;
                        Console.WriteLine("The gender can only be F, M or N.");
                    }
                }
                else if (!correctGenderFormat)
                {
                    Console.WriteLine("Please enter the gender in a correct format. Ex: 'F', 'M' or 'N'.");
                }
            }
            while (!correctGenderFormat);

            return gender;
        }

        // Prints records
        private static void PrintRecords(FileCabinetRecord[] records)
        {
            if (records.Length == 0)
            {
                Console.WriteLine("No records found.");
                return;
            }

            foreach (var record in records)
            {
                Console.WriteLine($"#{record.Id}, " +
                                  $"{record.FirstName}, " +
                                  $"{record.LastName}, " +
                                  $"{record.DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture)}, " +
                                  $"+{record.AreaCode}, " +
                                  $"{record.Savings}, " +
                                  $"{record.Gender}.");
            }
        }
    }
}