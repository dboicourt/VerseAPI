using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VerseAPI.Models
{
    public class Ship
    {
        public long Id { get; set; }
        public string ShipName { get; set; }
        public string PilotName { get; set; }
        public string Class { get; set; }
        public long Ore { get; set; }
        public long Water { get; set; }
        public long Fuel { get; set; }
        public long Components { get; set; }
        public long Capacity { get; set; }
    }
}
