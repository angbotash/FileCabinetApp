namespace FileCabinetApp.InputValidation
{
    /// <summary>
    /// Declares validation methods for input data.
    /// </summary>
    public interface IInputValidator
    {
        /// <summary>
        /// Converts name.
        /// </summary>
        /// <param name="nameInput">Name input.</param>
        /// <returns>Information about conversion - true/false, message, result of the conversion.</returns>
        public Tuple<bool, string, string> NameConverter(string nameInput);

        /// <summary>
        /// Converts date of birth from string to DateTime.
        /// </summary>
        /// <param name="dateInput">Date of birth input.</param>
        /// <returns>Information about conversion - true/false, message, result of the conversion.</returns>
        public Tuple<bool, string, DateTime> DateConverter(string dateInput);

        /// <summary>
        /// Converts area code from string to short.
        /// </summary>
        /// <param name="areCodeInput">Area code input.</param>
        /// <returns>Information about conversion - true/false, message, result of the conversion.</returns>
        public Tuple<bool, string, short> AreaCodeConverter(string areCodeInput);

        /// <summary>
        /// Converts savings from string to decimal.
        /// </summary>
        /// <param name="savingsInput">Amount of savings input.</param>
        /// <returns>Information about conversion - true/false, message, result of the conversion.</returns>
        public Tuple<bool, string, decimal> SavingsConverter(string savingsInput);

        /// <summary>
        /// Converts gender from string to char.
        /// </summary>
        /// <param name="genderInput">Gender input.</param>
        /// <returns>Information about conversion - true/false, message, result of the conversion.</returns>
        public Tuple<bool, string, char> GenderConverter(string genderInput);

        /// <summary>
        /// Validated converted name.
        /// </summary>
        /// <param name="nameInput">Converted name input.</param>
        /// <returns>Information about validation - true/false, message.</returns>
        public Tuple<bool, string> NameValidator(string nameInput);

        /// <summary>
        /// Validates converted date of birth.
        /// </summary>
        /// <param name="dateInput">Converted date of birth input.</param>
        /// <returns>Information about validation - true/false, message.</returns>
        public Tuple<bool, string> DateValidator(DateTime dateInput);

        /// <summary>
        /// Validates converted area code.
        /// </summary>
        /// <param name="areCodeInput">Converted area code input.</param>
        /// <returns>Information about validation - true/false, message.</returns>
        public Tuple<bool, string> AreaCodeValidator(short areCodeInput);

        /// <summary>
        /// Validates converted savings.
        /// </summary>
        /// <param name="savingsInput">Converted savings input.</param>
        /// <returns>Information about validation - true/false, message.</returns>
        public Tuple<bool, string> SavingsValidator(decimal savingsInput);

        /// <summary>
        /// Validates converted gender.
        /// </summary>
        /// <param name="genderInput">Converted gender input.</param>
        /// <returns>Information about validation - true/false, message.</returns>
        public Tuple<bool, string> GenderValidator(char genderInput);
    }
}
