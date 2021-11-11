using System.Collections.Generic;
using System;
using System.Linq;
using System.IO;

namespace TestFramework
{
	public class ModFileData
	{
		private List<DataRow> m_Data;

		public ModFileData()
		{
			m_Data = new List<DataRow>();
		}

		public List<DataRow> GetTestData(double start, double end, string column, SegmentType st = SegmentType.Distance)
		{
			if (start > end) {
				throw new ArgumentException($"start delimiter ({start}) greater than end delimiter ({end})");
			}

			var testData = new List<DataRow>();
			var rangeType = st == SegmentType.Distance ? ModFileHeader.dist : ModFileHeader.time;

			foreach (var line in m_Data) {
				// we assume that the test data is already sorted
				if (line[rangeType] < start) {
					continue;
				} else if (line[rangeType] > end) {
					break;
				}

				var row = new DataRow {
					{ rangeType, line[rangeType] },
					{ column, line[column] }
				};

				testData.Add(row);
			}

			return testData;
		}

		public bool ParseCsv(string path)
		{
			// read all lines from the file
			var lines = File.ReadAllLines(path);

			var index = 0;

			if (lines[0].StartsWith("#")) {
				index = 1;
			}

			var headers_line = lines[index];
			var headers = headers_line.Split(',');

			foreach (var header in headers) {
				// check if the headers found in the mod file match the ones defined in Types.ModFileHeader class
				if (Utils.IsValidHeader(header) == false) {
					Console.Error.WriteLine("Column '{0}' is not defined in Types.ModFileHeaders class.", header);
					return false;
				}
			}

			index++;

			for (var line_idx = index; line_idx < lines.Length; line_idx++) {
				// Read data 
				var row = new DataRow();

				var line = lines[line_idx];
				var fields = line.Split(',');

				if (headers.Length != fields.Length) {
					Console.Error.WriteLine("headers.Length != fields.Length");
					return false;
				}

				var headersAndFields = headers.Zip(fields, (h, f) => new { Header = h, Field = f });
				foreach (var hf in headersAndFields) {
					double field_val;
					try {
						field_val = Convert.ToDouble(hf.Field);
					} catch (FormatException) {
						field_val = 0;
					} catch (OverflowException) {
						Console.Error.WriteLine("field '{0}' value: '{1}' is outside the range of a Double.", hf.Header, hf.Field);
						return false;
					}
					row.Add(hf.Header, field_val);
				}
				m_Data.Add(row);
			}
			return true;
		}
	}
}
