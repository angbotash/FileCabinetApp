using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> _list = new List<FileCabinetRecord>();
        private readonly Dictionary<string, List<FileCabinetRecord>> _firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();

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

            if (gender != 'F' && gender != 'M' && gender != 'N')
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

            this.AddToFirstNameDictionary(record);

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

            if (gender != 'F' && gender != 'M' && gender != 'N')
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

            this.UpdateFirstNameDictionary(updatedRecord, record.FirstName);

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
            var records = this._list.Where(x => x.LastName.ToUpperInvariant() == lastNameUpperCase);

            return records.ToArray();
        }

        public FileCabinetRecord[] FindByDateOfBirth(DateTime dateOfBirth)
        {
            var records = this._list.Where(x => x.DateOfBirth == dateOfBirth);

            return records.ToArray();
        }

        // Dictionary methods
        private void AddToFirstNameDictionary(FileCabinetRecord record)
        {
            var firstNameUpperCase = record.FirstName.ToUpperInvariant();

            if (this._firstNameDictionary.ContainsKey(firstNameUpperCase))
            {
                this._firstNameDictionary[record.FirstName].Add(record);
            }
            else
            {
                this._firstNameDictionary.Add(firstNameUpperCase, new List<FileCabinetRecord>() { record });
            }
        }

        private void UpdateFirstNameDictionary(FileCabinetRecord record, string uneditedFirstname)
        {
            var oldFirstNameUpperCase = uneditedFirstname.ToUpperInvariant();
            var newNameUpperCase = record.FirstName.ToUpperInvariant();

            if (oldFirstNameUpperCase == newNameUpperCase)
            {
                var recordDictionary = this._firstNameDictionary[oldFirstNameUpperCase]
                    .FirstOrDefault(x => x.Id == record.Id);

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
            else if (oldFirstNameUpperCase != newNameUpperCase)
            {
                var recordDictionary = this._firstNameDictionary[oldFirstNameUpperCase]
                    .FirstOrDefault(x => x.Id == record.Id);

                if (recordDictionary != null)
                {
                    this._firstNameDictionary[oldFirstNameUpperCase].Remove(recordDictionary);
                    this.AddToFirstNameDictionary(record);
                }
            }
        }
    }
}
