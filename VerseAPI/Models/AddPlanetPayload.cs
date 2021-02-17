using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VerseAPI.Models
{
    public class AddPlanetPayload
    {
        public string Name { get; set; }
        public long OreCount { get; set; }
        public long WaterCount { get; set; }
        public long FuelCount { get; set; }
        public long ComponentsCount { get; set; }
    }
}
