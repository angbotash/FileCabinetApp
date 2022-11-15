namespace FileCabinetApp
{
    /// <summary>
    /// This class represents the parameters for methods in  <see cref="FileCabinetRecord"/>.
    /// </summary>
    public class RecordDataArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecordDataArgs"/> class.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName"> The last name.</param>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <param name="areaCode">The area code of a phone number.</param>
        /// <param name="savings">The amount of savings.</param>
        /// <param name="gender">The person's gender.</param>
        public RecordDataArgs(string firstName, string lastName, DateTime dateOfBirth, short areaCode, decimal savings, char gender)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.DateOfBirth = dateOfBirth;
            this.AreaCode = areaCode;
            this.Savings = savings;
            this.Gender = gender;
        }

        /// <summary>
        /// Gets the person's first name.
        /// </summary>
        /// <value>The person's first name.</value>
        public string FirstName { get; }

        /// <summary>
        /// Gets the person's last name.
        /// </summary>
        /// <value>The person's last name.</value>
        public string LastName { get; }

        /// <summary>
        /// Gets the person's date of birth.
        /// </summary>
        /// <value>The person's date of birth.</value>
        public DateTime DateOfBirth { get; }

        /// <summary>
        /// Gets the area code of the person's phone number.
        /// </summary>
        /// <value>The area code of a person's phone number.</value>
        public short AreaCode { get; }

        /// <summary>
        /// Gets the person's amount of savings.
        /// </summary>
        /// <value>The person's amount of savings.</value>
        public decimal Savings { get; }

        /// <summary>
        /// Gets the person's gender.
        /// </summary>
        /// <value>The person's gender.</value>
        public char Gender { get; }
    }
}
