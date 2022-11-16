using System.Globalization;

namespace FileCabinetApp
{
    /// <summary>
    /// This class declares a method that writes a record from stream to a CSV file.
    /// </summary>
    public class FileCabinetRecordCsvWriter
    {
        private readonly TextWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordCsvWriter"/> class.
        /// </summary>
        /// <param name="writer">Stream writer.</param>
        public FileCabinetRecordCsvWriter(TextWriter writer)
        {
            this.writer = writer;

            this.writer.WriteLine("Id,First Name,Last Name,Date of Birth,Area code,Savings,Gender");
        }

        /// <summary>
        /// Serializes a record into a CSV file.
        /// </summary>
        /// <param name="record">A record.</param>
        /// <exception cref="ArgumentNullException">Thrown if the record is null.</exception>
        public void Write(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record), $"{record} cannot be null.");
            }

            this.writer.WriteLine(
                $"{record.Id}," +
                $"{record.FirstName}," +
                $"{record.LastName}," +
                $"{record.DateOfBirth.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)}," +
                $"{record.AreaCode}," +
                $"{record.Savings}," +
                $"{record.Gender}");
        }
    }
}
