using System.Xml;

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

        /// <summary>
        /// Saves records to a XML file through <see cref="FileCabinetRecordXmlWriter.Write"/>.
        /// </summary>
        /// <param name="writer">StreamWriter instance.</param>
        public void SaveToXml(StreamWriter writer)
        {
            var xmlWriterSettings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "\t",
            };

            var writerXml = XmlWriter.Create(writer, xmlWriterSettings);
            var xmlWriter = new FileCabinetRecordXmlWriter(writerXml);

            writerXml.WriteStartElement("records");

            foreach (var record in this.records)
            {
                xmlWriter.Write(record);
            }

            writerXml.WriteEndElement();
            writerXml.Close();
        }
    }
}
