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
        public long OrePrice { get; set; }
        public long OreAmount { get; set; }
        public long WaterPrice { get; set; }
        public long WaterAmount { get; set; }
        public long FuelPrice { get; set; }
        public long FuelAmount { get; set; }
        public long ComponentsPrice { get; set; }
        public long ComponentsAmount { get; set; }
    }
}
