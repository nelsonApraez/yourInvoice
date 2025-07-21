///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using ClosedXML.Excel;
using CsvHelper;
using CsvHelper.Configuration;
using ExcelDataReader;
using yourInvoice.Common.Extension;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace yourInvoice.Common.Integration.Files
{
    public class FileOperation : IFileOperation
    {
        public byte[] CreateFileExcelAsync<T>(List<T> items, string sheetName) where T : class
        {
            DataTable dt = ToDataTable<T>(items);
            using (XLWorkbook workbook = new XLWorkbook())
            {
                var sheet = workbook.AddWorksheet(dt, sheetName);
                sheet.Columns(1, 12).Width = 25;
                using (MemoryStream ms = new MemoryStream())
                {
                    workbook.SaveAs(ms);
                    return ms.ToArray();
                }
            }
        }

        public byte[] CreateFileCsv<T>(List<T> data) where T : class
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream))
                using (var csvWriter = new CsvWriter(streamWriter, new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = false }))
                {
                    csvWriter.WriteRecords(data);
                    streamWriter.Flush();
                }
                return memoryStream.ToArray();
            }
        }

        public List<T> ReadFileCsv<T>(byte[] data) where T : class
        {
            var configCsv = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => RemoveSpace(args.Header),
                Delimiter = "|",
                HasHeaderRecord = true
            };
            using (var reader = new StreamReader(new MemoryStream(data)))
            using (var csvReader = new CsvReader(reader, configCsv))
            {
                var records = csvReader.GetRecords<T>();
                return records.ToList<T>();
            }
        }

        public List<T> ReadFileExcel<T>(byte[] data) where T : class
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using (var reader = ExcelReaderFactory.CreateReader(new MemoryStream(data)))
            {
                var result = reader.AsDataSet(new ExcelDataSetConfiguration
                {
                    ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration
                    {
                        UseHeaderRow = true,
                    }
                });
                return ConvertToList<T>(result.Tables[0]);
            }
        }

        private List<T> ConvertToList<T>(DataTable dt)
        {
            var columnNames = dt.Columns.Cast<DataColumn>()
                    .Select(c => c.ColumnName.Trim())
                    .ToList();
            var properties = typeof(T).GetProperties();
            return dt.AsEnumerable().Select(row =>
            {
                var objTInstance = Activator.CreateInstance<T>();
                properties.Where(c => columnNames.Contains(c.Name.Trim().Replace("_", " ")))
                          .ToList()
                          .ForEach(f =>
                            {
                                PropertyInfo property = objTInstance.GetType().GetProperty(f.Name.Trim());
                                var nameColumn = dt?.Columns?.Cast<DataColumn>().First(co => RemoveSpace(co.ColumnName.Trim()).Contains(RemoveSpace(f.Name.Trim().Replace("_", " "))))?.ColumnName ?? string.Empty;
                                var dataColumn = row[nameColumn];
                                var dataColumnValidated = GetDataValidated(property, dataColumn);
                                f.SetValue(objTInstance, dataColumn == DBNull.Value ? null : Convert.ChangeType(dataColumnValidated, property.PropertyType));
                            });
                return objTInstance;
            }).ToList();
        }

        private string RemoveSpace(string name)
        {
            return Regex.Replace(name, @"\s", "");
        }

        private DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        private object GetDataValidated(PropertyInfo property, object data)
        {
            var dataFile = Convert.ToString(data);           
            TypeCode typeCode = Type.GetTypeCode(property.PropertyType);
            switch (typeCode)
            {
                case TypeCode.DateTime:
                    var dateColumn = ExtensionFormat.GetDateAnyFormat(dataFile);
                    return dateColumn;

                case TypeCode.Int64:
                    var longColumn = ExtensionFormat.NumberLongWithoutPeriodOrCommas(dataFile);
                    return longColumn;

                case TypeCode.Int32:
                    var intColumn = ExtensionFormat.NumberIntWithoutPeriodOrCommas(dataFile);
                    return intColumn;

                case TypeCode.Decimal:
                    if (property.Name.ToLowerInvariant().Contains("tasaeap"))
                    {
                        var dataDecimal = ExtensionFormat.DecimalWhitCommaPoint(dataFile);
                        return dataDecimal;
                    }
                    var decimalColumn = ExtensionFormat.OnlyNumberTypeDecimal(dataFile);
                    return decimalColumn;

                default:
                    return data;
            }
        }
    }
}