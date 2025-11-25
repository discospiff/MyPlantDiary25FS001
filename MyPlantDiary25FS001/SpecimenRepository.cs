using PlantPlacesSpecimen;

namespace MyPlantDiary25FS001
{
    /// <summary>
    /// This is a static class that we are using to tranfer data around our application.
    /// </summary>
    public class SpecimenRepository
    {
        static SpecimenRepository()
        {
            allSpecimens = new List<Specimen>();
        }
        public static IList<Specimen> allSpecimens { get; set; }

    }
}
