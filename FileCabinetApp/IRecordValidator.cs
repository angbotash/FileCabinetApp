namespace FileCabinetApp
{
    /// <summary>
    /// Declares a validation method for record's data.
    /// </summary>
    public interface IRecordValidator
    {
        /// <summary>
        /// Validates record's data.
        /// </summary>
        /// <param name="recordData">The data of a new record.</param>
        public void ValidateParameters(RecordDataArgs recordData);
    }
}
