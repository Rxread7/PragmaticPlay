using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Integration.PragmaticPlay.Responses
{
    public class EndRoundResponse : Response
    {
        [JsonProperty("cash")]
        public decimal Cash { get; set; }
        [JsonProperty("bonus")]
        public decimal Bonus { get; set; }


        //dedi
        /*
        public int Error { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        */
    }
}
