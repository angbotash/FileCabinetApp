using System.Collections.ObjectModel;

namespace FileCabinetApp
{
    /// <summary>
    /// This interface describes all the methods for <see cref="FileCabinetRecord"/>.
    /// </summary>
    public interface IFileCabinetService
    {
        /// <summary>
        /// Creates a new record.
        /// </summary>
        /// <param name="recordData">The data of a new record.</param>
        /// <returns>The unique Id of the created record.</returns>
        public int CreateRecord(RecordDataArgs recordData);

        /// <summary>
        /// Gets all records.
        /// </summary>
        /// <returns>The array of all records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords();

        /// <summary>
        /// Gets a record with a specific Id.
        /// </summary>
        /// <param name="id">The Id of a record.</param>
        /// <returns>The record with a matching Id.</returns>
        public FileCabinetRecord? GetRecord(int id);

        /// <summary>
        /// Gets the amount of records.
        /// </summary>
        /// <returns>The amount of records.</returns>
        public int GetStat();

        /// <summary>
        /// Edits a record with a specific Id.
        /// </summary>
        /// <param name="id">The Id of the record.</param>
        /// <param name="recordData">The data of a new record.</param>
        public void EditRecord(int id, RecordDataArgs recordData);

        /// <summary>
        /// Finds records with the same first name.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <returns>The array of records with the same first name.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName);

        /// <summary>
        /// Finds records with the same last name.
        /// </summary>
        /// <param name="lastName">The last name.</param>
        /// <returns>The array of records with the same last name.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName);

        /// <summary>
        /// Finds records with the same date of birth.
        /// </summary>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <returns>The array of records with the same date of birth.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth);

        /// <summary>
        /// Makes a snapshot of a record.
        /// </summary>
        /// <returns>An instance of <see cref="FileCabinetServiceSnapshot"/>.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot();
    }
}
