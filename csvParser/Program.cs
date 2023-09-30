using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace csvParser
{
    internal class Program
    {
        static void Main()
        {
            // Read the CSV file
            string[] lines = File.ReadAllLines("E:\\Downloads\\1.csv");

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
                    // Replace spaces with dots and remove non-digit characters
                    string cleanedValue = Regex.Replace(parts[i], @"[^\d.]", "").Replace(".", "").Insert(1, ".");
                    dataValues.Add(cleanedValue);
                }

                string formattedData = string.Join("; ", dataValues.Zip(Enumerable.Range(1995, dataValues.Count), (v, y) => $"{v} {unit}, {y}"));
                string result = $"{indicator} w {unit} wynosiło; {formattedData}";

                parsedData.Add(result);
            }

            string parsedDataStr = string.Join("\n", parsedData);

            // Print or save the parsed data as needed
            Console.WriteLine(parsedDataStr);
        }
    }
}
