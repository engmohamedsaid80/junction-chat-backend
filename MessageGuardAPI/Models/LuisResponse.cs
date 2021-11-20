using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageGuardAPI.Models
{
    public class LuisResponse
    {
        public string Query { get; set; }
        public Prediction prediction { get; set; }
    }

    public class Prediction
    {
        public string TopIntent { get; set; }
    }
}
