namespace FileCabinetApp
{
    /// <summary>
    /// This class represents a record with personal information about a person.
    /// </summary>
    public class FileCabinetRecord
    {
        /// <summary>
        /// Gets or sets the Id of a record.
        /// </summary>
        /// <value>The Id of a record.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the person's first name.
        /// </summary>
        /// <value>The person's first name.</value>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the person's last name.
        /// </summary>
        /// <value>The person's last name.</value>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the person's date of birth.
        /// </summary>
        /// <value>The person's date of birth.</value>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the area code of the person's phone number.
        /// </summary>
        /// <value>The area code of a person's phone number.</value>
        public short AreaCode { get; set; }

        /// <summary>
        /// Gets or sets the person's amount of savings.
        /// </summary>
        /// <value>The person's amount of savings.</value>
        public decimal Savings { get; set; }

        /// <summary>
        /// Gets or sets the person's gender.
        /// </summary>
        /// <value>The person's gender.</value>
        public char Gender { get; set; }
    }
}