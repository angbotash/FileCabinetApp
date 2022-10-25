namespace FileCabinetApp
{
    /// <summary>
    /// This class stores default validation criteria for record's data.
    /// </summary>
    public class FileCabinetDefaultService : FileCabinetService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetDefaultService"/> class.
        /// </summary>
        public FileCabinetDefaultService()
            : base(new DefaultValidator())
        {
        }

        // private const char GenderFemale = 'F';
        // private const char GenderMale = 'M';
        // private const char GenderNotSpecified = 'N';

        ///// <summary>
        ///// Validates record's data using default criteria.
        ///// </summary>
        ///// <param name="recordData">The data of a new record.</param>
        ///// <exception cref="ArgumentNullException">Thrown if the parameters "FirstName" or "lastName" are null or contain only white spaces.</exception>
        ///// <exception cref="ArgumentException">Thrown if the parameters "FirstName" or "LastName" shorter than 2 characters or longer than 60 characters.</exception>
        ///// <exception cref="ArgumentException">Thrown if the parameter "DateOfBirth" is earlier than 01.01.1950 or later than the current date.</exception>
        ///// <exception cref="ArgumentException">Thrown if the parameters "AreaCode" and "Savings" are less than 0.</exception>
        ///// <exception cref="ArgumentException">Thrown if the parameter "Gender" is not equal 'F', 'M' or 'N'.</exception>
        // protected override void ValidateParameters(RecordDataArgs recordData)
        // {
        //    if (string.IsNullOrWhiteSpace(recordData.FirstName))
        //    {
        //        throw new ArgumentNullException(nameof(recordData.FirstName), "First name cannot be null or white space.");
        //    }

        // if (recordData.FirstName.Length < 2 || recordData.FirstName.Length > 60)
        //    {
        //        throw new ArgumentException("First name cannot be shorter than 2 characters and longer than 60 characters.");
        //    }

        // if (string.IsNullOrWhiteSpace(recordData.LastName))
        //    {
        //        throw new ArgumentNullException(nameof(recordData.LastName), "Last name cannot be null or white space.");
        //    }

        // if (recordData.LastName.Length < 2 || recordData.LastName.Length > 60)
        //    {
        //        throw new ArgumentException("Last name cannot be shorter than 2 characters and longer than 60 characters.");
        //    }

        // if (recordData.DateOfBirth < new DateTime(1950, 1, 1) || recordData.DateOfBirth > DateTime.Today)
        //    {
        //        throw new ArgumentException("Date of birth cannot be earlier than 1-Jan-1950 or later than the current date.");
        //    }

        // if (recordData.AreaCode < 0)
        //    {
        //        throw new ArgumentException("Area code cannot be a negative number.");
        //    }

        // if (recordData.Savings < 0)
        //    {
        //        throw new ArgumentException("Savings cannot be a negative number.");
        //    }

        // if (recordData.Gender != GenderFemale && recordData.Gender != GenderMale && recordData.Gender != GenderNotSpecified)
        //    {
        //        throw new ArgumentException("Gender can only be F, M or N.");
        //    }
        // }

        ///// <summary>
        ///// Returns an instance of <see cref="DefaultValidator"></see> with record validation method.
        ///// </summary>
        ///// <returns>An instance of <see cref="DefaultValidator"></see> with record validation method.</returns>
        // protected override IRecordValidator CreateValidator()
        // {
        //    return new DefaultValidator();
        // }
    }
}
