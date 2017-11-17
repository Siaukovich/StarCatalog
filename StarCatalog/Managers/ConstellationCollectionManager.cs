using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace StarCatalog
{
    public static class ConstellationCollectionManager
    {
        #region Collection

        public static ObservableCollection<Constellation> Constellations { get; private set; }

        public static int Current { get; set; }
        public static Constellation GetCurrectConstellation() => Constellations[Current];
        public static IList<Star> GetCurrentStars() => Constellations[Current].Stars;

        #endregion

        static ConstellationCollectionManager()
        {
            Constellations = new ObservableCollection<Constellation>();
            Constellations.CollectionChanged += (s, e) => 
            {
                for (int i = 0; i < Constellations.Count; i++)
                {
                    Constellations[i].Index = i;
                }
            };

            var constellation1 =
                new Constellation(
                    name: "Gemini",
                    coordinates: new Coordinates(
                        new Angle(20.23f),
                        new Angle(10.643f)
                    ));


            constellation1.AddStar("Castor", 1.1e8f, 1.4e29, 4.43e28f);
            constellation1.AddStar("Pollux", 2.1e8f, 1.1e29, 3.3e28f);

            constellation1.Stars[0].AddPlanet("PlanetCastor1", 1.1e4f, 4.8e21, 1.8e4f, 7.3e5f, 1.1e8f);
            constellation1.Stars[0].AddPlanet("PlanetCastor2", 2.2e4f, 3.21e21, 1.28e4f, 17.3e5f, 4.1e8f);

            constellation1.Stars[1].AddPlanet("PlanetPollux", 2.1e4f, 3.1e21, 3.48e4f, 1.3e5f, 4e4f);

            AddConstellation(constellation1);

            var constellation2 =
                new Constellation(
                    name: "Orion",
                    coordinates: new Coordinates(
                        new Angle(-10.231f),
                        new Angle(67.98f)
                    ));

            constellation2.AddStar("Betelgeuse", 1.1e8f, 1.4e29, 4.43e28f);
            constellation2.AddStar("Mintaka", 2.1e8f, 1.1e29, 3.3e28f);

            constellation2.Stars[1].AddPlanet("PlanetMintaka", 2.2e4f, 3.21e21, 1.28e4f, 17.3e5f, 4.1e8f);

            AddConstellation(constellation2);

            var constellation3 =
                new Constellation(
                    name: "Bootes",
                    coordinates: new Coordinates(
                        new Angle(20.23f),
                        new Angle(10.643f)
                    ));


            AddConstellation(constellation3);

            var constellation4 =
                new Constellation(
                    name: "Cancer",
                    coordinates: new Coordinates(
                        new Angle(20.23f),
                        new Angle(10.643f)
                    ));

            AddConstellation(constellation4);

            var constellation5 =
                new Constellation(
                    name: "Canes Venatici",
                    coordinates: new Coordinates(
                        new Angle(20.23f),
                        new Angle(10.643f)
                    ));

            constellation5.AddStar("Heart", 2.1e8f, 1.1e29, 3.3e28f);

            AddConstellation(constellation5);
        }

        public static void SaveToFile(string fullPathToFile)
        {
            Serializer.SerializeCollectionXml(Constellations, fullPathToFile);
        }

        public static void LoadFromFile(string fullPathToFile)
        {
            if (!File.Exists(fullPathToFile))
                throw new FileNotFoundException();

            var tempCollection = Serializer.DeserializeXml<ObservableCollection<Constellation>>(fullPathToFile);
            ClearCollection();
            AddRange(tempCollection);
        }

        public static void AddRange(IEnumerable<Constellation> collection)
        {
            foreach (var constellation in collection)
            {
                AddConstellation(constellation);
            }
        }


        public static List<Constellation> GetAllConstellations() => Constellations.OrderBy(constellation => constellation.Name)
                                                                                  .ToList();

        public static List<Star> GetAllStars() => Constellations.SelectMany(constellation => constellation.Stars)
                                                                .OrderBy(star => star.Name)
                                                                .ToList();

        public static List<Planet> GetAllPlanets() => GetAllStars()
                                                     .SelectMany(star => star.Planets)
                                                     .OrderBy(planet => planet.Name)
                                                     .ToList();

        public static List<Constellation> GetConstellationByNameStart(string nameStart)
        {
            nameStart = nameStart.ToLower();
            return Constellations.Where(constellation => constellation.Name.ToLower().StartsWith(nameStart))
                                 .ToList();
        }

        public static List<Star> GetStarByNameStart(string nameStart)
        {
            nameStart = nameStart.ToLower();
            return GetAllStars().Where(star => star.Name.ToLower().StartsWith(nameStart))
                                .ToList();
        }

        public static List<Planet> GetPlanetByNameStart(string nameStart)
        {
            nameStart = nameStart.ToLower();

            return GetAllPlanets().Where(planet => planet.Name.ToLower().StartsWith(nameStart))
                                  .ToList();
        }

        public static List<Constellation> GetConstellationsSortedBy(string sortTypeString)
        {
            var returnList = new List<Constellation>();
            switch (sortTypeString)
            {
                case "Name":
                    returnList = Constellations.OrderBy(c => c.Name).ToList();
                    break;
                case "Amount of stars":
                    var orderedByStarsAmount = from constellation in Constellations
                                               orderby constellation.Stars.Count descending
                                               select constellation;

                    returnList = orderedByStarsAmount.ToList();

                    //returnList = Constellations.OrderByDescending(c => c.Stars.Count).ToList();
                    break;
                case "Amount of planets":

                    var orderedByPlanetsAmount = Constellations
                                                .OrderByDescending(constellation => constellation.Stars
                                                                                                 .Sum(star => star.AmountOfPlanets));

                    returnList = orderedByPlanetsAmount.ToList();
                    break;
            }

            return returnList;
        }

        public static void AddConstellation(Constellation c)
        {
            Constellations.Add(c);
        }

        public static void ClearCollection()
        {
            Constellations.Clear();
            Current = -1;
        }

        public static void RemoveCurrent()
        {
            Constellations.RemoveAt(Current);
            Current--;
        }

        public static bool IsEmpty() => Constellations.Count == 0;
    }
}
