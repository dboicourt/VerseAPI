using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VerseAPI.Models
{
    public class PurchasePayload
    {
        public long ShipID { get; set; }
        public long PlanetID { get; set; }
        public string Resource { get; set; }
        public long Quantity { get; set; }
    }
}
