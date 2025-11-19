using Microsoft.AspNetCore.Mvc;
using PlantPlacesSpecimen;
using System.Collections;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyPlantDiary25FS001
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET: api/<ValuesController>
        [HttpGet]
        public IEnumerable<Specimen> Get()
        {
            Specimen pawpaw = new Specimen();
            pawpaw.Genus = "Asimina";
            pawpaw.Species = "triloba";
            pawpaw.Common = "Paw Paw";
            pawpaw.PlantId = 83;
            pawpaw.SpecimenId = 1;

            Specimen redbud = new Specimen();
            redbud.Genus = "Cercis";
            redbud.Species = "canadensis";
            redbud.Common = "Eastern Redbud";
            redbud.PlantId = 50;
            redbud.SpecimenId = 2;

            List<Specimen> specimens = new List<Specimen>();
            specimens.Add(redbud);
            specimens.Add(pawpaw);

            return specimens;

        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public Specimen Get(int id)
        {
            Specimen pawpaw = new Specimen();
            pawpaw.Genus = "Asimina";
            pawpaw.Species = "triloba";
            pawpaw.Common = "Paw Paw";
            pawpaw.PlantId = 83;
            pawpaw.SpecimenId = 1;
            return pawpaw;
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] Specimen value)
        {
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Specimen value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
