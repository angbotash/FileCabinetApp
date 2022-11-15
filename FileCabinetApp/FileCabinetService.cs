using System.Collections.ObjectModel;
using System.Globalization;

#pragma warning disable SA1309 // Field names should not begin with underscore

namespace FileCabinetApp
{
    /// <summary>
    /// <inheritdoc cref="IFileCabinetService"/>
    /// </summary>
    public abstract class FileCabinetService : IFileCabinetService
    {
        private const string DateTimeFormat = "M/d/yyyy";

        private readonly List<FileCabinetRecord> _list = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> _firstNameDictionary = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> _lastNameDictionary = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> _dateOfBirthDictionary = new ();
        private readonly IRecordValidator _validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetService"/> class.
        /// </summary>
        /// <param name="validator">Validator type.</param>
        protected FileCabinetService(IRecordValidator validator)
        {
            this._validator = validator;
        }

        /// <summary>
        /// Creates a new record.
        /// </summary>
        /// <param name="recordData">The data of a new record.</param>
        /// <returns>The unique Id of the created record.</returns>
        public virtual int CreateRecord(RecordDataArgs recordData)
        {
            this._validator.ValidateParameters(recordData);

            var record = new FileCabinetRecord
            {
                Id = this._list.Count + 1,
                FirstName = recordData.FirstName,
                LastName = recordData.LastName,
                DateOfBirth = recordData.DateOfBirth,
                AreaCode = recordData.AreaCode,
                Savings = recordData.Savings,
                Gender = recordData.Gender,
            };

            var dateOfBirthKey = record.DateOfBirth.ToString(DateTimeFormat, CultureInfo.InvariantCulture);

            AddRecordToDictionary(this._firstNameDictionary, record, record.FirstName);
            AddRecordToDictionary(this._lastNameDictionary, record, record.LastName);
            AddRecordToDictionary(this._dateOfBirthDictionary, record, dateOfBirthKey);

            this._list.Add(record);

            return record.Id;
        }

        /// <summary>
        /// Gets all records.
        /// </summary>
        /// <returns>The array of all records.</returns>
        public virtual ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            var records = new List<FileCabinetRecord>();

            foreach (var rec in this._list)
            {
                records.Add(rec);
            }

            return new ReadOnlyCollection<FileCabinetRecord>(records);
        }

        /// <summary>
        /// Gets a record with a specific Id.
        /// </summary>
        /// <param name="id">The Id of a record.</param>
        /// <returns>The record with a matching Id.</returns>
        public virtual FileCabinetRecord? GetRecord(int id)
        {
            var record = this._list.FirstOrDefault(x => x.Id == id);

            return record;
        }

        /// <summary>
        /// Gets the amount of records.
        /// </summary>
        /// <returns>The amount of records.</returns>
        public virtual int GetStat()
        {
            return this._list.Count;
        }

        /// <summary>
        /// Edits a record with a specific Id.
        /// </summary>
        /// <param name="id">The Id of the record.</param>
        /// <param name="recordData">The data of a new record.</param>
        /// <exception cref="ArgumentNullException">Thrown if "record" is null.</exception>
        public virtual void EditRecord(int id, RecordDataArgs recordData)
        {
            var record = this._list.FirstOrDefault(x => x.Id == id);

            if (record is null)
            {
                throw new ArgumentException($"There is no record with this Id - {id}");
            }

            this._validator.ValidateParameters(recordData);

            var updatedRecord = new FileCabinetRecord()
            {
                Id = id,
                FirstName = recordData.FirstName,
                LastName = recordData.LastName,
                DateOfBirth = recordData.DateOfBirth,
                AreaCode = recordData.AreaCode,
                Savings = recordData.Savings,
                Gender = recordData.Gender,
            };

            var oldDateOfBirthString = record.DateOfBirth.ToString(DateTimeFormat, CultureInfo.InvariantCulture);
            var newDateOfBirthString = updatedRecord.DateOfBirth.ToString(DateTimeFormat, CultureInfo.InvariantCulture);

            UpdateRecordInDictionary(this._firstNameDictionary, updatedRecord, record.FirstName, updatedRecord.FirstName);
            UpdateRecordInDictionary(this._lastNameDictionary, updatedRecord, record.LastName, updatedRecord.LastName);
            UpdateRecordInDictionary(this._dateOfBirthDictionary, updatedRecord, oldDateOfBirthString, newDateOfBirthString);

            record.FirstName = recordData.FirstName;
            record.LastName = recordData.LastName;
            record.DateOfBirth = recordData.DateOfBirth;
            record.AreaCode = recordData.AreaCode;
            record.Savings = recordData.Savings;
            record.Gender = recordData.Gender;
        }

        /// <summary>
        /// Finds records with the same first name.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <returns>The array of records with the same first name.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the parameter "firstName" is null or contains only white spaces.</exception>
        public virtual ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new ArgumentNullException(nameof(firstName), "First name cannot be null or white space.");
            }

            var resultsFirstName = new List<FileCabinetRecord>();
            var firstNameUpperCase = firstName.ToUpperInvariant();

            if (this._firstNameDictionary.ContainsKey(firstNameUpperCase))
            {
                resultsFirstName = this._firstNameDictionary[firstNameUpperCase];

                return new ReadOnlyCollection<FileCabinetRecord>(resultsFirstName);
            }

            return new ReadOnlyCollection<FileCabinetRecord>(resultsFirstName);
        }

        /// <summary>
        /// Finds records with the same last name.
        /// </summary>
        /// <param name="lastName">The last name.</param>
        /// <returns>The array of records with the same last name.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the parameter "lastName" is null or contains only white spaces.</exception>
        public virtual ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentNullException(nameof(lastName), "Last name cannot be null or white space.");
            }

            var resultsLastName = new List<FileCabinetRecord>();
            var lastNameUpperCase = lastName.ToUpperInvariant();

            if (this._lastNameDictionary.ContainsKey(lastNameUpperCase))
            {
                resultsLastName = this._lastNameDictionary[lastNameUpperCase];

                return new ReadOnlyCollection<FileCabinetRecord>(resultsLastName);
            }

            return new ReadOnlyCollection<FileCabinetRecord>(resultsLastName);
        }

        /// <summary>
        /// Finds records with the same date of birth.
        /// </summary>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <returns>The array of records with the same date of birth.</returns>
        public virtual ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            var dateOfBirthKey = dateOfBirth.ToString(DateTimeFormat, CultureInfo.InvariantCulture);
            var resultsDateOfBirth = new List<FileCabinetRecord>();

            if (this._dateOfBirthDictionary.ContainsKey(dateOfBirthKey))
            {
                resultsDateOfBirth = this._dateOfBirthDictionary[dateOfBirthKey];

                return new ReadOnlyCollection<FileCabinetRecord>(resultsDateOfBirth);
            }

            return new ReadOnlyCollection<FileCabinetRecord>(resultsDateOfBirth);
        }

        // Dictionary methods
        private static void AddRecordToDictionary(Dictionary<string, List<FileCabinetRecord>> dictionary, FileCabinetRecord record, string key)
        {
            var keyUpperCase = key.ToUpperInvariant();

            if (dictionary.ContainsKey(keyUpperCase))
            {
                dictionary[keyUpperCase].Add(record);
            }
            else
            {
                dictionary.Add(keyUpperCase, new List<FileCabinetRecord>() { record });
            }
        }

        private static void UpdateRecordInDictionary(Dictionary<string, List<FileCabinetRecord>> dictionary, FileCabinetRecord record, string oldKey, string newKey)
        {
            var oldKeyUpperCase = oldKey.ToUpperInvariant();
            var newKeyUpperCase = newKey.ToUpperInvariant();

            if (oldKeyUpperCase == newKeyUpperCase)
            {
                var recordDictionary = dictionary[oldKeyUpperCase].FirstOrDefault(x => x.Id == record.Id);

                if (recordDictionary != null)
                {
                    recordDictionary.FirstName = record.FirstName;
                    recordDictionary.LastName = record.LastName;
                    recordDictionary.DateOfBirth = record.DateOfBirth;
                    recordDictionary.AreaCode = record.AreaCode;
                    recordDictionary.Savings = record.Savings;
                    recordDictionary.Gender = record.Gender;
                }
            }
            else if (oldKeyUpperCase != newKeyUpperCase)
            {
                var recordDictionary = dictionary[oldKeyUpperCase].FirstOrDefault(x => x.Id == record.Id);

                if (recordDictionary != null)
                {
                    dictionary[oldKeyUpperCase].Remove(recordDictionary);
                    AddRecordToDictionary(dictionary, record, newKey);
                }
            }
        }
    }
}
