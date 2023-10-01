using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace csvParser
{
    public class StringParser
    {
        public static string ParseInput(string input)
        {
            string cleanedInput = Regex.Replace(input, "\"\";\"\"", ",");

            // Remove leading and trailing commas
            cleanedInput = cleanedInput.Trim(',');

            // Remove double quotes and escaped double quotes
            cleanedInput = Regex.Replace(cleanedInput, "\"", "");

            // Remove any remaining double quotes
            cleanedInput = cleanedInput.Replace("\"", "");
            cleanedInput = cleanedInput.Trim();
            
            return cleanedInput;
        }
    }
}
