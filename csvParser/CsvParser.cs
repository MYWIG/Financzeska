using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;


namespace csvParser
{
    public class CsvParser
    {
        public static List<string> ParseCsv(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            List<string> parsedData = new List<string>();

            foreach (string line in lines)
            {
                string[] parts = line.Split(';');

                if (parts.Length < 4)
                    continue; // Skip lines with insufficient data

                string indicator = parts[0].Trim();
                string unit = parts[2].Trim();

                List<string> dataValues = new List<string>();

                for (int i = 3; i < parts.Length; i++)
                {
                    string cleanedValue = Regex.Replace(parts[i], @"[^\d.]", "").Replace(".", "").Insert(1, ".");
                    dataValues.Add(cleanedValue);
                }

                string formattedData = string.Join("; ", dataValues.Zip(Enumerable.Range(1995, dataValues.Count), (v, y) => $"{v} {unit}, {y}"));

                // Replace semicolons with periods in the formattedData
                formattedData = formattedData.Replace(";", ".");

                string result = $"{indicator} w {unit} wynosiło; {formattedData}";

                // Remove question marks and double quotes from the result
                result = result.Replace("?", "").Replace("\"", "");

                parsedData.Add(result);
            }

            return parsedData;
        }
    }
}
