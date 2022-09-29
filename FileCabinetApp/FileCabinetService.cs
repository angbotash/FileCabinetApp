using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> _list = new List<FileCabinetRecord>();

        public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth, short areaCode, decimal savings, char gender)
        {
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

        public int GetStat()
        {
            return this._list.Count;
        }
    }
}
