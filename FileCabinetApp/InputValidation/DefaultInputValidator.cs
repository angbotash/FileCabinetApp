using System.Globalization;

namespace FileCabinetApp.InputValidation
{
    /// <summary>
    /// <inheritdoc cref="IInputValidator"/>
    /// </summary>
    public class DefaultInputValidator : IInputValidator
    {
        private const char GenderFemale = 'F';
        private const char GenderMale = 'M';
        private const char GenderNotSpecified = 'N';
        private const string DateTimeFormat = "M/d/yyyy";

        /// <summary>
        /// Converts name.
        /// </summary>
        /// <param name="nameInput">Name input.</param>
        /// <returns>Information about conversion - true/false, message, result of the conversion.</returns>
        public Tuple<bool, string, string> NameConverter(string nameInput)
        {
            return new Tuple<bool, string, string>(true, string.Empty, nameInput);
        }

        /// <summary>
        /// Converts date of birth from string to DateTime.
        /// </summary>
        /// <param name="dateInput">Date of birth input.</param>
        /// <returns>Information about conversion - true/false, message, result of the conversion.</returns>
        public Tuple<bool, string, DateTime> DateConverter(string dateInput)
        {
            var correctDateFormat = DateTime.TryParseExact(dateInput, DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateOfBirth);

            if (correctDateFormat && (dateOfBirth < new DateTime(1920, 1, 1) || dateOfBirth > DateTime.Today))
            {
                return new Tuple<bool, string, DateTime>(
                    false,
                    "The date of birth cannot be earlier than 1-Jan-1920 or later than the current date.",
                    dateOfBirth);
            }

            if (!correctDateFormat)
            {
                return new Tuple<bool, string, DateTime>(
                    false,
                    "Please enter the date of birth in the correct format. Ex.: mm/dd/yyyy - 01/01/1973",
                    dateOfBirth);
            }

            return new Tuple<bool, string, DateTime>(true, string.Empty, dateOfBirth);
        }

        /// <summary>
        /// Converts area code from string to short.
        /// </summary>
        /// <param name="areCodeInput">Area code input.</param>
        /// <returns>Information about conversion - true/false, message, result of the conversion.</returns>
        public Tuple<bool, string, short> AreaCodeConverter(string areCodeInput)
        {
            var correctAreCodeFormat = short.TryParse(areCodeInput, out short areaCode);

            if (correctAreCodeFormat && areaCode < 0)
            {
                return new Tuple<bool, string, short>(false, "The area code cannot be a negative number.", areaCode);
            }

            if (!correctAreCodeFormat)
            {
                return new Tuple<bool, string, short>(false, "Please enter an area code.", areaCode);
            }

            return new Tuple<bool, string, short>(true, string.Empty, areaCode);
        }

        /// <summary>
        /// Converts savings from string to decimal.
        /// </summary>
        /// <param name="savingsInput">Amount of savings input.</param>
        /// <returns>Information about conversion - true/false, message, result of the conversion.</returns>
        public Tuple<bool, string, decimal> SavingsConverter(string savingsInput)
        {
            var correctSavingsFormat = decimal.TryParse(savingsInput, out decimal savings);

            if (correctSavingsFormat && savings < 0)
            {
                return new Tuple<bool, string, decimal>(false, "The amount of savings cannot be a negative number.", savings);
            }

            if (!correctSavingsFormat)
            {
                return new Tuple<bool, string, decimal>(false, "Please enter an amount of savings.", savings);
            }

            return new Tuple<bool, string, decimal>(true, string.Empty, savings);
        }

        /// <summary>
        /// Converts gender from string to char.
        /// </summary>
        /// <param name="genderInput">Gender input.</param>
        /// <returns>Information about conversion - true/false, message, result of the conversion.</returns>
        public Tuple<bool, string, char> GenderConverter(string genderInput)
        {
            var correctGenderFormat = char.TryParse(genderInput, out char gender);

            if (correctGenderFormat)
            {
                if (gender != GenderFemale && gender != GenderMale)
                {
                    return new Tuple<bool, string, char>(false, "The gender can only be F or M.", gender);
                }
            }
            else if (!correctGenderFormat)
            {
                return new Tuple<bool, string, char>(false, "Please enter the gender in a correct format. Ex: 'F' - female or 'M' - male.", gender);
            }

            return new Tuple<bool, string, char>(true, string.Empty, gender);
        }

        /// <summary>
        /// Validated converted name.
        /// </summary>
        /// <param name="nameInput">Converted name input.</param>
        /// <returns>Information about validation - true/false, message.</returns>
        public Tuple<bool, string> NameValidator(string nameInput)
        {
            if (string.IsNullOrWhiteSpace(nameInput))
            {
                return new Tuple<bool, string>(false, "First or last name cannot be null or white space.");
            }

            if (nameInput.Length < 2 || nameInput.Length > 60)
            {
                return new Tuple<bool, string>(false, "First or last name cannot be shorter than 2 characters and longer than 60 characters.");
            }

            return new Tuple<bool, string>(true, string.Empty);
        }

        /// <summary>
        /// Validates converted date of birth.
        /// </summary>
        /// <param name="dateInput">Converted date of birth input.</param>
        /// <returns>Information about validation - true/false, message.</returns>
        public Tuple<bool, string> DateValidator(DateTime dateInput)
        {
            if (dateInput < new DateTime(1950, 1, 1) || dateInput > DateTime.Today)
            {
                return new Tuple<bool, string>(false, "Date of birth cannot be earlier than 1-Jan-1920 or later than the current date.");
            }

            return new Tuple<bool, string>(true, string.Empty);
        }

        /// <summary>
        /// Validates converted area code.
        /// </summary>
        /// <param name="areCodeInput">Converted area code input.</param>
        /// <returns>Information about validation - true/false, message.</returns>
        public Tuple<bool, string> AreaCodeValidator(short areCodeInput)
        {
            if (areCodeInput < 0 || areCodeInput > 999)
            {
                return new Tuple<bool, string>(false, "Area code cannot be a negative number or be longer than 3 digits.");
            }

            return new Tuple<bool, string>(true, string.Empty);
        }

        /// <summary>
        /// Validates converted savings.
        /// </summary>
        /// <param name="savingsInput">Converted savings input.</param>
        /// <returns>Information about validation - true/false, message.</returns>
        public Tuple<bool, string> SavingsValidator(decimal savingsInput)
        {
            if (savingsInput < 0)
            {
                return new Tuple<bool, string>(false, "Savings cannot be a negative number.");
            }

            return new Tuple<bool, string>(true, string.Empty);
        }

        /// <summary>
        /// Validates converted gender.
        /// </summary>
        /// <param name="genderInput">Converted gender input.</param>
        /// <returns>Information about validation - true/false, message.</returns>
        public Tuple<bool, string> GenderValidator(char genderInput)
        {
            if (genderInput != GenderFemale && genderInput != GenderMale && genderInput != GenderNotSpecified)
            {
                return new Tuple<bool, string>(false, "Gender can only be F, M or N.");
            }

            return new Tuple<bool, string>(true, string.Empty);
        }
    }
}
