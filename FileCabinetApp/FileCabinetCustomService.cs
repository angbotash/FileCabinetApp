using System.Globalization;

#pragma warning disable SA1309 // Field names should not begin with underscore

namespace FileCabinetApp
{
    /// <summary>
    /// This class stores custom validation criteria for record's data.
    /// </summary>
    public class FileCabinetCustomService : FileCabinetService
    {
        private const char GenderFemale = 'F';
        private const char GenderMale = 'M';
        private const string DateTimeFormat = "M/d/yyyy";

        private readonly List<FileCabinetRecord> _list = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> _firstNameDictionary = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> _lastNameDictionary = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> _dateOfBirthDictionary = new ();

        /// <summary>
        /// Creates a new record.
        /// </summary>
        /// <param name="recordData">The data of a new record.</param>
        /// <returns>The unique Id of the created record.</returns>
        public override int CreateRecord(RecordDataArgs recordData)
        {
            this.ValidateParameters(recordData);

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
        public override FileCabinetRecord[] GetRecords()
        {
            var records = new FileCabinetRecord[this._list.Count];

            for (int i = 0; i < this._list.Count; i++)
            {
                records[i] = this._list[i];
            }

            return records;
        }

        /// <summary>
        /// Gets a record with a specific Id.
        /// </summary>
        /// <param name="id">The Id of a record.</param>
        /// <returns>The record with a matching Id.</returns>
        public override FileCabinetRecord? GetRecord(int id)
        {
            var record = this._list.FirstOrDefault(x => x.Id == id);

            return record;
        }

        /// <summary>
        /// Gets the amount of records.
        /// </summary>
        /// <returns>The amount of records.</returns>
        public override int GetStat()
        {
            return this._list.Count;
        }

        /// <summary>
        /// Edits a record with a specific Id.
        /// </summary>
        /// <param name="id">The Id of the record.</param>
        /// <param name="recordData">The data of a new record.</param>
        /// <exception cref="ArgumentNullException">Thrown if "record" is null.</exception>
        public override void EditRecord(int id, RecordDataArgs recordData)
        {
            var record = this._list.FirstOrDefault(x => x.Id == id);

            if (record is null)
            {
                throw new ArgumentException($"There is no record with this Id - {id}");
            }

            this.ValidateParameters(recordData);

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
        public override FileCabinetRecord[] FindByFirstName(string firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new ArgumentNullException(nameof(firstName), "First name cannot be null or white space.");
            }

            var firstNameUpperCase = firstName.ToUpperInvariant();

            if (this._firstNameDictionary.ContainsKey(firstNameUpperCase))
            {
                return this._firstNameDictionary[firstNameUpperCase].ToArray();
            }

            return Array.Empty<FileCabinetRecord>();
        }

        /// <summary>
        /// Finds records with the same last name.
        /// </summary>
        /// <param name="lastName">The last name.</param>
        /// <returns>The array of records with the same last name.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the parameter "lastName" is null or contains only white spaces.</exception>
        public override FileCabinetRecord[] FindByLastName(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentNullException(nameof(lastName), "Last name cannot be null or white space.");
            }

            var lastNameUpperCase = lastName.ToUpperInvariant();

            if (this._lastNameDictionary.ContainsKey(lastNameUpperCase))
            {
                return this._lastNameDictionary[lastNameUpperCase].ToArray();
            }

            return Array.Empty<FileCabinetRecord>();
        }

        /// <summary>
        /// Finds records with the same date of birth.
        /// </summary>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <returns>The array of records with the same date of birth.</returns>
        public override FileCabinetRecord[] FindByDateOfBirth(DateTime dateOfBirth)
        {
            var dateOfBirthKey = dateOfBirth.ToString(DateTimeFormat, CultureInfo.InvariantCulture);

            if (this._dateOfBirthDictionary.ContainsKey(dateOfBirthKey))
            {
                return this._dateOfBirthDictionary[dateOfBirthKey].ToArray();
            }

            return Array.Empty<FileCabinetRecord>();
        }

        /// <summary>
        /// Validates record's data using default criteria.
        /// </summary>
        /// <param name="recordData">The data of a new record.</param>
        /// <exception cref="ArgumentNullException">Thrown if the parameters "FirstName" or "lastName" are null or contain only white spaces.</exception>
        /// <exception cref="ArgumentException">Thrown if the parameters "FirstName" or "LastName" shorter than 2 characters or longer than 30 characters.</exception>
        /// <exception cref="ArgumentException">Thrown if the parameter "DateOfBirth" is earlier than 01.01.1920 or later than the current date.</exception>
        /// <exception cref="ArgumentException">Thrown if the parameters "AreaCode" and "Savings" are less than 0.</exception>
        /// <exception cref="ArgumentException">Thrown if the parameter "Gender" is not equal 'F' or 'M'.</exception>
        protected override void ValidateParameters(RecordDataArgs recordData)
        {
            if (string.IsNullOrWhiteSpace(recordData.FirstName))
            {
                throw new ArgumentNullException(nameof(recordData.FirstName), "First name cannot be null or white space.");
            }

            if (recordData.FirstName.Length < 2 || recordData.FirstName.Length > 30)
            {
                throw new ArgumentException("First name cannot be shorter than 2 characters and longer than 30 characters.");
            }

            if (string.IsNullOrWhiteSpace(recordData.LastName))
            {
                throw new ArgumentNullException(nameof(recordData.LastName), "Last name cannot be null or white space.");
            }

            if (recordData.LastName.Length < 2 || recordData.LastName.Length > 30)
            {
                throw new ArgumentException("Last name cannot be shorter than 2 characters and longer than 30 characters.");
            }

            if (recordData.DateOfBirth < new DateTime(1950, 1, 1) || recordData.DateOfBirth > DateTime.Today)
            {
                throw new ArgumentException("Date of birth cannot be earlier than 1-Jan-1920 or later than the current date.");
            }

            if (recordData.AreaCode < 0 || recordData.AreaCode > 999)
            {
                throw new ArgumentException("Area code cannot be a negative number or be longer than 3 digits.");
            }

            if (recordData.Savings < 0)
            {
                throw new ArgumentException("Savings cannot be a negative number.");
            }

            if (recordData.Gender != GenderFemale && recordData.Gender != GenderMale)
            {
                throw new ArgumentException("Gender can only be F or M.");
            }
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
