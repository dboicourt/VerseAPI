using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VerseAPI.Models;

namespace VerseAPI.Models
{
    public class Planet
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public PlanetResource Ore { get; set; }
        public PlanetResource Water { get; set; }
        public PlanetResource Fuel { get; set; }
        public PlanetResource Components { get; set; }

    }
}
