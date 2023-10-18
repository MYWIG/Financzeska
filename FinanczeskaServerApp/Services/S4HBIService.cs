using FinanczeskaServerApp.Data;
using Newtonsoft.Json;
using System.Dynamic;

namespace FinanczeskaServerApp.Services
{
    public class S4HBIService
    {

        private readonly IWebHostEnvironment _webHostEnvironment;
        private const string s4hContextFolderName = "S4HContext";

        public S4HBIService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
        }

        /// <summary>
        /// Analizing raz s4h data and return text description for AI context
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string analyzeData(List<StatisticalDataFromS4H> data)
        {
            try
            {

                Dictionary<string, List<StatisticalDataFromS4H>> myDictionary = data
                    .GroupBy(o => o.OPR_Nazwa)
                    .ToDictionary(g => g.Key, g => g.ToList());

                string contextData = string.Empty;

                //Dictionary<string, decimal> operatorsGross = new();
                Dictionary<string, decimal> operatorsGrossMonth = new();
                Dictionary<string, decimal> operatorsGrossDay = new();

                // get gross by operator Total, day Month
                foreach (var key in myDictionary.Keys)
                {

                    //decimal totalOperatorsGross = myDictionary[key]
                    //    .Sum(x => x.totalGross);
                    //operatorsGross.Add(key, totalOperatorsGross);

                    decimal totalOperatorsGrossDay = myDictionary[key]
                        .Where(x => x.DSL_DataOperacji >= DateTime.Now.AddDays(-1))
                        .Sum(x => x.totalGross);
                    operatorsGrossDay.Add(key, totalOperatorsGrossDay);

                    decimal totalOperatorsGrossMonth = myDictionary[key]
                        .Where(x => x.DSL_DataOperacji >= DateTime.Now.AddDays(-1))
                        .Sum(x => x.totalGross);
                    operatorsGrossMonth.Add(key, totalOperatorsGrossMonth);
                }


                foreach (string key in myDictionary.Keys)
                {
                    if (operatorsGrossMonth[key] > 0)
                    {
                        contextData += $" W ostatnim miesiącu sprzedał na {operatorsGrossMonth[key]} zł co stanowi w średnim {operatorsGrossMonth[key] / 30} zł w dzień. \\n ";
                        contextData += $" a wczoraj około {operatorsGrossDay[key]} zł. \\n ";
                    }
                }

                // get average month information 
                decimal totalMonthIncome = operatorsGrossMonth.Values.Sum(x => x);
                if (totalMonthIncome > 0)
                {
                    contextData += $" W średnim operatory sprzedają na {operatorsGrossMonth.Keys.Count / operatorsGrossMonth.Values.Sum(x => x)} w miesiąc. \\n ";
                }

                // get Max
                var ordered = operatorsGrossMonth.OrderBy(x => x.Value);
                var maxSelesOperator = ordered.Last();
                contextData += $"Najwięcej zarobił {maxSelesOperator.Key} czyli {maxSelesOperator.Value} zł. \\n ";

                // get Min
                var minSelesOperator = ordered.First();
                contextData += $"A najmniej otrzymał {minSelesOperator.Key}, to tylko {minSelesOperator.Value} zł. \\n ";

                return contextData;
            }
            catch(Exception ex)
            {
                return string.Empty;
            }
        }

        public bool LoadData(string serializedData)
        {
            try
            {

                var data = JsonConvert.DeserializeObject<List<StatisticalDataFromS4H>>(serializedData);

                string analyzedDataAsText = analyzeData(data);
                if(string.IsNullOrEmpty(analyzedDataAsText))
                    return false;

                // folder with s4hContext 
                string folder = Path.Combine(_webHostEnvironment.ContentRootPath, s4hContextFolderName);
                string filePath  = Path.Combine(folder, "data.json");

                if(!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                if(!File.Exists(filePath))
                    File.Create(filePath)
                        .Close();

                File.WriteAllText(filePath, analyzedDataAsText);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public async Task<string> GetS4HContextData()
        {
            string folder = Path.Combine(_webHostEnvironment.ContentRootPath, s4hContextFolderName);
            string filePath = Path.Combine(folder, "data.json");
            return await File.ReadAllTextAsync(filePath);
        }

    }
}
