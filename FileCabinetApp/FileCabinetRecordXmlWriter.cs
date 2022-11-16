using System.Globalization;
using System.Xml;

namespace FileCabinetApp
{
    /// <summary>
    /// This class declares a method that writes a record from stream to a XML file.
    /// </summary>
    public class FileCabinetRecordXmlWriter
    {
        private readonly XmlWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlWriter"/> class.
        /// </summary>
        /// <param name="writer">Stream writer.</param>
        public FileCabinetRecordXmlWriter(XmlWriter writer)
        {
            this.writer = writer;
        }

        /// <summary>
        /// Serializes a record into a XML file.
        /// </summary>
        /// <param name="record">A record.</param>
        /// <exception cref="ArgumentNullException">Thrown if the record is null.</exception>
        public void Write(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record), $"{record} cannot be null.");
            }

            this.writer.WriteStartElement("record");
            this.writer.WriteAttributeString("id", $"{record.Id}");
            this.writer.WriteStartElement("name");
            this.writer.WriteAttributeString("first", $"{record.FirstName}");
            this.writer.WriteAttributeString("last", $"{record.LastName}");
            this.writer.WriteEndElement();
            this.writer.WriteStartElement("dateofBirth");
            this.writer.WriteString($"{record.DateOfBirth.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)}");
            this.writer.WriteEndElement();
            this.writer.WriteStartElement("areaCode");
            this.writer.WriteString($"{record.AreaCode}");
            this.writer.WriteEndElement();
            this.writer.WriteStartElement("savings");
            this.writer.WriteString($"{record.Savings}");
            this.writer.WriteEndElement();
            this.writer.WriteStartElement("gender");
            this.writer.WriteString($"{record.Gender}");
            this.writer.WriteEndElement();
            this.writer.WriteEndElement();
        }
    }
}
