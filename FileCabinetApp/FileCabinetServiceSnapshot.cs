namespace FileCabinetApp
{
    /// <summary>
    /// Snapshot class for <see cref="FileCabinetService"/>.
    /// </summary>
    public class FileCabinetServiceSnapshot
    {
        private readonly FileCabinetRecord[] records;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.
        /// </summary>
        /// <param name="recordsList">List of records.</param>
        public FileCabinetServiceSnapshot(List<FileCabinetRecord> recordsList)
        {
            this.records = recordsList.ToArray();
        }

        /// <summary>
        /// Saves records to a CSV file through <see cref="FileCabinetRecordCsvWriter.Write"/>.
        /// </summary>
        /// <param name="writer">StreamWriter instance.</param>
        public void SaveToCsv(StreamWriter writer)
        {
            var csvWriter = new FileCabinetRecordCsvWriter(writer);

            foreach (var record in this.records)
            {
                csvWriter.Write(record);
            }
        }
    }
}
