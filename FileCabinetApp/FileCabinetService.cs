using System.Globalization;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private const char GenderFemale = 'F';
        private const char GenderMale = 'M';
        private const char GenderNotSpecified = 'N';
        private const string DateTimeFormat = "M/d/yyyy";

        private readonly List<FileCabinetRecord> _list = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> _firstNameDictionary = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> _lastNameDictionary = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> _dateOfBirthDictionary = new ();

        public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth, short areaCode, decimal savings, char gender)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new ArgumentNullException(nameof(firstName), "First name cannot be null or white space.");
            }

            if (firstName.Length < 2 || firstName.Length > 60)
            {
                throw new ArgumentException("First name cannot be shorter than 2 characters and longer than 60 characters.");
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentNullException(nameof(lastName), "Last name cannot be null or white space.");
            }

            if (lastName.Length < 2 || lastName.Length > 60)
            {
                throw new ArgumentException("Last name cannot be shorter than 2 characters and longer than 60 characters.");
            }

            if (dateOfBirth < new DateTime(1950, 1, 1) || dateOfBirth > DateTime.Today)
            {
                throw new ArgumentException("Date of birth cannot be earlier than 1-Jan-1950 or later than the current date.");
            }

            if (areaCode < 0)
            {
                throw new ArgumentException("Area code cannot be a negative number.");
            }

            if (savings < 0)
            {
                throw new ArgumentException("Savings cannot be a negative number.");
            }

            if (gender != GenderFemale && gender != GenderMale && gender != GenderNotSpecified)
            {
                throw new ArgumentException("Gender can only be F, M or N.");
            }

            var record = new FileCabinetRecord
            {
                Id = this._list.Count + 1,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                AreaCode = areaCode,
                Savings = savings,
                Gender = gender,
            };

            var dateOfBirthKey = record.DateOfBirth.ToString(DateTimeFormat, CultureInfo.InvariantCulture);

            AddRecordToDictionary(this._firstNameDictionary, record, record.FirstName);
            AddRecordToDictionary(this._lastNameDictionary, record, record.LastName);
            AddRecordToDictionary(this._dateOfBirthDictionary, record, dateOfBirthKey);

            this._list.Add(record);

            return record.Id;
        }

        public FileCabinetRecord[] GetRecords()
        {
            var records = new FileCabinetRecord[this._list.Count];

            for (int i = 0; i < this._list.Count; i++)
            {
                records[i] = this._list[i];
            }

            return records;
        }

        public FileCabinetRecord? GetRecord(int id)
        {
            var record = this._list.FirstOrDefault(x => x.Id == id);

            return record;
        }

        public int GetStat()
        {
            return this._list.Count;
        }

        public void EditRecord(int id, string firstName, string lastName, DateTime dateOfBirth, short areaCode, decimal savings, char gender)
        {
            var record = this._list.FirstOrDefault(x => x.Id == id);

            if (record is null)
            {
                throw new ArgumentException($"There is no record with this Id - {id}");
            }

            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new ArgumentNullException(nameof(firstName), "First name cannot be null or white space.");
            }

            if (firstName.Length < 2 || firstName.Length > 60)
            {
                throw new ArgumentException("First name cannot be shorter than 2 characters and longer than 60 characters.");
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentNullException(nameof(lastName), "Last name cannot be null or white space.");
            }

            if (lastName.Length < 2 || lastName.Length > 60)
            {
                throw new ArgumentException("Last name cannot be shorter than 2 characters and longer than 60 characters.");
            }

            if (dateOfBirth < new DateTime(1950, 1, 1) || dateOfBirth > DateTime.Today)
            {
                throw new ArgumentException("Date of birth cannot be earlier than 1-Jan-1950 or later than the current date.");
            }

            if (areaCode < 0)
            {
                throw new ArgumentException("Area code cannot be a negative number.");
            }

            if (savings < 0)
            {
                throw new ArgumentException("Savings cannot be a negative number.");
            }

            if (gender != GenderFemale && gender != GenderMale && gender != GenderNotSpecified)
            {
                throw new ArgumentException("Gender can only be F, M or N.");
            }

            var updatedRecord = new FileCabinetRecord()
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                AreaCode = areaCode,
                Savings = savings,
                Gender = gender,
            };

            var oldDateOfBirthString = record.DateOfBirth.ToString(DateTimeFormat, CultureInfo.InvariantCulture);
            var newDateOfBirthString = updatedRecord.DateOfBirth.ToString(DateTimeFormat, CultureInfo.InvariantCulture);

            UpdateRecordInDictionary(this._firstNameDictionary, updatedRecord, record.FirstName, updatedRecord.FirstName);
            UpdateRecordInDictionary(this._lastNameDictionary, updatedRecord, record.LastName, updatedRecord.LastName);
            UpdateRecordInDictionary(this._dateOfBirthDictionary, updatedRecord, oldDateOfBirthString, newDateOfBirthString);

            record.FirstName = firstName;
            record.LastName = lastName;
            record.DateOfBirth = dateOfBirth;
            record.AreaCode = areaCode;
            record.Savings = savings;
            record.Gender = gender;
        }

        public FileCabinetRecord[] FindByFirstName(string firstName)
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

        public FileCabinetRecord[] FindByLastName(string lastName)
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

        public FileCabinetRecord[] FindByDateOfBirth(DateTime dateOfBirth)
        {
            var dateOfBirthKey = dateOfBirth.ToString(DateTimeFormat, CultureInfo.InvariantCulture);

            if (this._dateOfBirthDictionary.ContainsKey(dateOfBirthKey))
            {
                return this._dateOfBirthDictionary[dateOfBirthKey].ToArray();
            }

            return Array.Empty<FileCabinetRecord>();
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
