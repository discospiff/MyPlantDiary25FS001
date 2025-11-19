using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using PlantPlacesPlants;
using PlantPlacesSpecimen;
using WeatherFeed;

namespace MyPlantDiary25FS001.Pages
{
    public class IndexModel : PageModel
    {
        HttpClient httpClient = new HttpClient();

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            String brand = "My Plant Diary";
            String inBrand = Request.Query["Brand"];
            if (inBrand != null && inBrand.Length > 0)
            {
                brand = inBrand;
            }
            ViewData["Brand"] = brand;

            Task<HttpResponseMessage> plantTask = httpClient.GetAsync("https://raw.githubusercontent.com/discospiff/data/refs/heads/main/thirstyplants.json");
            HttpResponseMessage plantResult = plantTask.Result;

            Task<string> plantStringTask = plantResult.Content.ReadAsStringAsync();
            string plantJSON = plantStringTask.Result;

            List<Plant> plants = Plant.FromJson(plantJSON);

            // declare our dictionary.
            IDictionary<long, Plant> waterLovingPlants = new Dictionary<long, Plant>();

            // populate dictionary from JSON feed of only water loving plants.
            foreach (Plant plant in plants)
            {
                waterLovingPlants[plant.Id] = plant;
            }


            Task<HttpResponseMessage> task = httpClient.GetAsync("https://raw.githubusercontent.com/discospiff/data/refs/heads/main/specimens.json");
            HttpResponseMessage result = task.Result;



            List<Specimen> specimens = new List<Specimen>();
            if (result.IsSuccessStatusCode)
            {
                Task<string> readString = result.Content.ReadAsStringAsync();
                string specimenJSON = readString.Result;
                JSchema jsonSchema = JSchema.Parse(System.IO.File.ReadAllText("specimenschema.json"));
                JArray specimenArray = JArray.Parse(specimenJSON);

                IList<string> validationEvents = new List<string>();

                if (specimenArray.IsValid(jsonSchema, out validationEvents)) {
                    specimens = Specimen.FromJson(specimenJSON);
                } else
                {
                    foreach(string evt in validationEvents)
                    {
                        Console.WriteLine(evt);
                    }

                }


                    int foo = specimens.Count;

            }
            

            List<Specimen> waterLovingSpecimens = new List<Specimen>();
            foreach(Specimen specimen in specimens)
            {
                if (waterLovingPlants.ContainsKey(specimen.PlantId))
                {
                    waterLovingSpecimens.Add(specimen);
                }
            }
            ViewData["Specimens"] = waterLovingSpecimens;

            var config = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();
            string weatherApiKey = config["weatherApiKey"];

            Task<HttpResponseMessage> weatherTask = httpClient.GetAsync("https://api.weatherbit.io/v2.0/current?city=Cincinnati,OH&key=" + weatherApiKey);
            HttpResponseMessage weatherResult = weatherTask.Result;

            Task<string> weatherStringTask = weatherResult.Content.ReadAsStringAsync();
            string weatherJSON = weatherStringTask.Result;

            Weather weather = Weather.FromJson(weatherJSON);
            List<Datum> data = weather.Data;
            long precip = 0;
            foreach (Datum datum in data)
            {

                precip = datum.Precip;
            }

            if (precip < 1)
            {
                ViewData["WeatherMessage"] = "It's dry! Water these plants.";
            } else
            {
                ViewData["WeatherMessage"] = "Rain Expected.  No need to water.";
            }
        }

    }
}
