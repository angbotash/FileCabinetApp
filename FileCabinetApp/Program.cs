using System.Collections.ObjectModel;
using System.Globalization;
using FileCabinetApp.InputValidation;

#pragma warning disable CA1309 // Use ordinal string comparison

namespace FileCabinetApp
{
    /// <summary>
    /// This class contains the entry point of the application.
    /// </summary>
    public static class Program
    {
        private const string DeveloperName = "Angela Botasheva";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;
        private const string DateTimeFormat = "M/d/yyyy";
        private const string ValidationLong = "--inputValidation-rules";
        private const string ValidationShort = "-v";
        private const string ValidationCustomRule = "CUSTOM";
        private const string ValidationDefaultRule = "DEFAULT";

        private static IFileCabinetService fileCabinetService = new FileCabinetDefaultService();
        private static IInputValidator inputValidation = new DefaultInputValidator();

        private static bool isRunning = true;

        private static Tuple<string, Action<string>>[] commands =
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("exit", Exit),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("edit", Edit),
            new Tuple<string, Action<string>>("find", Find),
            new Tuple<string, Action<string>>("export", Export),
        };

        private static string[][] helpMessages =
        {
            new[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new[] { "stat", "shows statistics on record", "The 'stat' command shows statistics on record." },
            new[] { "create", "creates a new record", "The 'create' command creates a new record." },
            new[] { "list", "prints the list of all records", "The 'list' command prints the list of all records." },
            new[] { "edit", "edits a record", "The 'edit' command edits an existing record." },
            new[]
            {
                "find", "finds records",
                "The 'find firstname 'first name'' command finds existing records by the first name.\n" +
                "The 'find lastname 'last name'' command finds existing records by the last name.\n" +
                "The 'find dateofbirth 'date of birth'' command finds existing records by the date of birth.",
            },
            new[] { "export", "exports records", "The 'export' command exports records into a CSV file." },
        };

        /// <summary>
        /// Entry point of the application.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        public static void Main(string[] args)
        {
            var validationRule = GetValidationRule(args);
            var validationRuleMessage = GetValidationRuleMessage(args);

            if (string.IsNullOrEmpty(validationRuleMessage) || string.IsNullOrEmpty(validationRule))
            {
                Console.WriteLine("The inputValidation rule is not recognized.");
                return;
            }

            Console.WriteLine($"$ FileCabinetApp.exe {string.Join(' ', args)}");
            Console.WriteLine($"File Cabinet Application, developed by {DeveloperName}");
            Console.WriteLine(validationRuleMessage);
            Console.WriteLine(HintMessage);
            Console.WriteLine();

            if (validationRule == ValidationCustomRule)
            {
                fileCabinetService = new FileCabinetCustomService();
                inputValidation = new CustomInputValidator();
            }

            do
            {
                Console.Write("> ");
                var line = Console.ReadLine();
                var inputs = line != null ? line.Split(' ', 2) : new[] { string.Empty, string.Empty };
                const int commandIndex = 0;
                var command = inputs[commandIndex];

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(HintMessage);
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
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][ExplanationHelpIndex]);
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
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[CommandHelpIndex], helpMessage[DescriptionHelpIndex]);
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
            var recordsCount = fileCabinetService.GetStat();

            Console.WriteLine($"{recordsCount} record(s).");
        }

        private static void Create(string parameters)
        {
            Console.Write("First name: ");
            var firstName = ReadInput(inputValidation.NameConverter, inputValidation.NameValidator);

            Console.Write("Last name: ");
            var lastName = ReadInput(inputValidation.NameConverter, inputValidation.NameValidator);

            Console.Write("Date of birth: ");
            var dateOfBirth = ReadInput(inputValidation.DateConverter, inputValidation.DateValidator);

            Console.Write("Area code: ");
            var areaCode = ReadInput(inputValidation.AreaCodeConverter, inputValidation.AreaCodeValidator);

            Console.Write("Amount of savings: ");
            var savings = ReadInput(inputValidation.SavingsConverter, inputValidation.SavingsValidator);

            Console.Write("Gender: ");
            var gender = ReadInput(inputValidation.GenderConverter, inputValidation.GenderValidator);

            var recordData = new RecordDataArgs(firstName, lastName, dateOfBirth, areaCode, savings, gender);

            var newRecordId = fileCabinetService.CreateRecord(recordData);

            Console.WriteLine($"Record #{newRecordId} was created!");
        }

        private static void List(string parameters)
        {
            var records = fileCabinetService.GetRecords();

            PrintRecords(records);
        }

        private static void Edit(string parameters)
        {
            if (int.TryParse(parameters, out var id) && id > 0)
            {
                var record = fileCabinetService.GetRecord(id);

                if (record is null)
                {
                    Console.WriteLine($"#{id} record is not found.");
                    return;
                }

                Console.Write("First name: ");
                var firstName = ReadInput(inputValidation.NameConverter, inputValidation.NameValidator);

                Console.Write("Last name: ");
                var lastName = ReadInput(inputValidation.NameConverter, inputValidation.NameValidator);

                Console.Write("Date of birth: ");
                var dateOfBirth = ReadInput(inputValidation.DateConverter, inputValidation.DateValidator);

                Console.Write("Area code: ");
                var areaCode = ReadInput(inputValidation.AreaCodeConverter, inputValidation.AreaCodeValidator);

                Console.Write("Amount of savings: ");
                var savings = ReadInput(inputValidation.SavingsConverter, inputValidation.SavingsValidator);

                Console.Write("Gender: ");
                var gender = ReadInput(inputValidation.GenderConverter, inputValidation.GenderValidator);

                var recordData = new RecordDataArgs(firstName, lastName, dateOfBirth, areaCode, savings, gender);

                fileCabinetService.EditRecord(id, recordData);

                Console.WriteLine($"Record #{id} is updated.");
            }
            else
            {
                Console.WriteLine("Please enter the Id of an existing record.");
            }
        }

        private static void Find(string parameters)
        {
            GetSearchData(parameters, out var searchCategory, out var recordData);

            if (string.IsNullOrWhiteSpace(searchCategory) || string.IsNullOrWhiteSpace(recordData))
            {
                Console.WriteLine("Please enter a search category and a record data.");
                return;
            }

            switch (searchCategory)
            {
                case "FIRSTNAME":
                    var recordsByFirstName = fileCabinetService.FindByFirstName(recordData);
                    PrintRecords(recordsByFirstName);
                    break;

                case "LASTNAME":
                    var recordsByLastName = fileCabinetService.FindByLastName(recordData);
                    PrintRecords(recordsByLastName);
                    break;

                case "DATEOFBIRTH":
                    DateTime dateOfBirth;

                    if (!DateTime.TryParseExact(recordData, DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateOfBirth))
                    {
                        Console.WriteLine("Please enter the date of birth in the correct format. Ex.: mm/dd/yyyy - 01/01/1973");
                        break;
                    }

                    var recordsByDateOfBirth = fileCabinetService.FindByDateOfBirth(dateOfBirth);
                    PrintRecords(recordsByDateOfBirth);
                    break;

                default:
                    Console.WriteLine("Please enter a search category and a record data.");
                    break;
            }
        }

        private static void Export(string parameters)
        {
            GetExportData(parameters, out var exportCategory, out var filePath);

            if (string.IsNullOrWhiteSpace(exportCategory) || string.IsNullOrWhiteSpace(filePath))
            {
                Console.WriteLine("Please enter a file type and a file name.");
                return;
            }

            if (File.Exists(filePath))
            {
                Console.Write($"File exists - rewrite {filePath}? [Y/n] ");

                var yesOrNo = GetYesOrNo();

                switch (yesOrNo)
                {
                    case true:
                        ExportToCsv(filePath);
                        break;

                    case false:
                        break;
                }
            }
            else
            {
                ExportToCsv(filePath);
            }
        }

        private static void ExportToCsv(string filePath)
        {
            try
            {
                FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                stream.Close();
            }
            catch (IOException)
            {
                Console.WriteLine($"Export failed: can't open file {filePath}.");
                return;
            }

            var writer = new StreamWriter(filePath);
            var snapshot = fileCabinetService.MakeSnapshot();

            snapshot.SaveToCsv(writer);
            writer.Close();

            Console.WriteLine($"All records are exported to file {filePath}.");
        }

        // Validation rule input check
        private static string GetValidationRuleMessage(string[] args)
        {
            var ruleMessage = string.Empty;

            if (args.Length == 0)
            {
                ruleMessage = "Using default inputValidation rules.";
                return ruleMessage;
            }

            var rule = GetValidationRule(args);

            switch (rule)
            {
             case ValidationDefaultRule:
                 ruleMessage = "Using default inputValidation rules.";
                 return ruleMessage;
             case ValidationCustomRule:
                 ruleMessage = "Using custom inputValidation rules.";
                 return ruleMessage;
             default:
                 return ruleMessage;
            }
        }

        private static string GetValidationRule(string[] args)
        {
            var rule = string.Empty;

            if (args.Length == 0)
            {
                rule = ValidationDefaultRule;
                return rule;
            }

            var ruleTemp = string.Empty;

            if (args.Length == 1)
            {
                var argsSplit = args[0].Split('=', 2);
                var validation = argsSplit[0];
                ruleTemp = argsSplit.Length != 2 && (validation != ValidationLong || validation != ValidationShort) ? rule : argsSplit[1].ToUpperInvariant();
            }
            else if (args.Length == 2)
            {
                var validation = args[0];
                ruleTemp = validation == ValidationLong || validation == ValidationShort ? args[1].ToUpperInvariant() : rule;
            }

            switch (ruleTemp)
            {
                case ValidationDefaultRule:
                    rule = ValidationDefaultRule;
                    return rule;
                case ValidationCustomRule:
                    rule = ValidationCustomRule;
                    return rule;
                default:
                    return rule;
            }
        }

        // Prints records
        private static void PrintRecords(ReadOnlyCollection<FileCabinetRecord> records)
        {
            if (records.Count == 0)
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

        // Process user input data in Find method
        private static void GetSearchData(string parameters, out string? searchCategory, out string? recordData)
        {
            searchCategory = null;
            recordData = null;
            int searchDataItems = 2;
            var searchData = parameters.Split(' ', searchDataItems);
            bool validParameters = !string.IsNullOrWhiteSpace(parameters) && searchData.Length == searchDataItems;

            if (validParameters)
            {
                bool validRecordData = !string.IsNullOrWhiteSpace(searchData[0]) &&
                                       !string.IsNullOrWhiteSpace(searchData[1]);

                if (validRecordData)
                {
                    searchCategory = searchData[0].ToUpperInvariant();
                    recordData = searchData[1];
                }
            }
        }

        // Process user input data in Export method
        private static void GetExportData(string parameters, out string? exportCategory, out string? filePath)
        {
            exportCategory = null;
            filePath = null;
            int exportDataItems = 2;
            var exportData = parameters.Split(' ', exportDataItems);
            bool validParameters = !string.IsNullOrWhiteSpace(parameters) && exportData.Length == exportDataItems;

            if (validParameters)
            {
                bool validExportData = !string.IsNullOrWhiteSpace(exportData[0]) &&
                                       !string.IsNullOrWhiteSpace(exportData[1]);

                if (validExportData)
                {
                    exportCategory = exportData[0].ToUpperInvariant();
                    filePath = exportData[1];
                }
            }
        }

        // Gets 'yes' or 'no' for Export method
        private static bool GetYesOrNo()
        {
            var correctAnswerFormat = true;
            bool answer = false;

            do
            {
                var answerString = Console.ReadLine();
                var parse = char.TryParse(answerString, out char charAnswer);

                if (!parse || (char.ToUpperInvariant(charAnswer) != 'Y' && char.ToUpperInvariant(charAnswer) != 'N'))
                {
                    correctAnswerFormat = false;
                    Console.WriteLine("Please enter 'yes' or 'no' - Y or N.");
                    continue;
                }

                if (char.ToUpperInvariant(charAnswer) == 'Y')
                {
                    correctAnswerFormat = true;
                    answer = true;
                }

                if (char.ToUpperInvariant(charAnswer) == 'N')
                {
                    correctAnswerFormat = true;
                    answer = false;
                }
            }
            while (!correctAnswerFormat);

            return answer;
        }

        // Input validation
        private static T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
        {
            do
            {
                T value;

                var input = Console.ReadLine();
                var conversionResult = converter(input);

                if (!conversionResult.Item1)
                {
                    Console.WriteLine($"Conversion failed: {conversionResult.Item2} Please, correct your input.");
                    continue;
                }

                value = conversionResult.Item3;

                var validationResult = validator(value);

                if (!validationResult.Item1)
                {
                    Console.WriteLine($"Validation failed: {validationResult.Item2} Please, correct your input.");
                    continue;
                }

                return value;
            }
            while (true);
        }
    }
}