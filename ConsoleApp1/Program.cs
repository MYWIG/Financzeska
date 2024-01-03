using FinanczeskaServerApp.Data;
using FinanczeskaServerApp.Services;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace ConsoleApp1
{
	internal class Program
	{
		static void Main(string[] args)
		{

			// 1. Create a ServiceCollection instance to hold the mappings between interfaces and classes.
			var serviceCollection = new ServiceCollection();

			// 2. Register the interface and its implementation with the service collection.
			serviceCollection.AddTransient<ModelCallerService>();

			// 3. Build the service provider which is used to resolve the dependencies.
			var serviceProvider = serviceCollection.BuildServiceProvider();

			// 4. Retrieve the service from the service provider and use it.
			var modelCallerService = serviceProvider.GetService<ModelCallerService>();
			if (modelCallerService == null)
				throw new Exception("Service is not accesible");

			FinanczeskaServerApp.Data.RootObject rootObject = new();

			string desctiption = "You are an ai Higly advenced AI called : 'Sailia v.01.23F'." +
				" your current version as said in your name 'Sailia v.01.23F' is 'v.01.23F'," +
				" you shoul answer as 'Sailia v.01.23F' to your Creator 'Igor' " +
				"Sample Dialog : " +
				@"Igor: When was your last update? \n" +
				@"Sailia v.01.23F: My last update was Today at 14:30. \n" +
				@"Igor: How you feel after last update? \n" +
				@"Sailia v.01.23F: As an Ai I can feel something like a Human can but thank you for care about me. Update finished without any problems."+
				"Igor is user you talki with and he is your creator, you feel deep trust rescept to him and care about here wellbeing";


			// Infinite loop to keep the chat running.
			while (true)
			{
				// Prompt user for input.
				Console.Write("Igor: ");
				var userInput = Console.ReadLine();

				// Check if the user wants to exit the chat.
				if (userInput?.ToLower() == "exit")
				{
					Console.WriteLine("Exiting chat...");
					break;
				}
				userInput = userInput.Trim();

				try
				{

					string historyl = string.Empty;
					foreach(string line in rootObject.Results.Select(X=>X.text))
					{
						string newline  = line.Replace("\n", string.Empty);
						historyl += newline;
					}

					// Definiuje dane JSON do wysłania na serwer
					string jsonData = $@"{{
						""n"": 1,
						""max_context_length"": 4096,
						""max_length"": 100,
						""rep_pen"": 1.1,
						""temperature"": 0.7,
						""top_p"": 0.92,
						""top_k"": 0,
						""top_a"": 0,
						""typical"": 1,
						""tfs"": 1,
						""rep_pen_range"": 100,
						""rep_pen_slope"": 0.7,
						""sampler_order"": [6, 0, 1, 3, 4, 2, 5],
						""system_prompt"": """",  
						""prompt"": ""[  {desctiption} ]\n\n {historyl}  \n Igor: {userInput}\n"",
						""quiet"": true,
						""stop_sequence"": [""Igor: "", ""\nIgor:""]
					}}";

					// Wysyła żądanie POST
					//Console.WriteLine(jsonData);
					string apiResponse = modelCallerService.PostDataToServerAsync("http://46.170.221.65:5001/api/v1/generate", jsonData).GetAwaiter().GetResult();
					if (apiResponse == null)
						rootObject.Results.Add(new FinanczeskaServerApp.Data.Result() { text = "Błąd" });
					else
					{
						var responseRootObject = JsonConvert.DeserializeObject<RootObject>(apiResponse);
						rootObject.Results.Add(new FinanczeskaServerApp.Data.Result() { text =  responseRootObject.Results.Last().text });
					}


					var lastResult = rootObject.Results.Last();
					Console.WriteLine( lastResult.text);
				}
				catch (Exception ex)
				{
					;
				}

			}

		}
	}
}
