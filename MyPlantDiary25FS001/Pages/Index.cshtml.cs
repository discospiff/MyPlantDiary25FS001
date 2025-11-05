using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlantPlacesPlants;
using PlantPlacesSpecimen;

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
                string specimentJSON = readString.Result;
                specimens = Specimen.FromJson(specimentJSON);
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

        }
    }
}
