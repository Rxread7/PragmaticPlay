using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Integration.PragmaticPlay.Responses
{
    public class BalanceResponse : Response
    {
        [JsonProperty("currency")]
        public string Currency { get; set; }
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


        /*
        //Optional
        [JsonProperty("totalBalance")]
        public decimal TotalBalance { get; set; }
        */
    }
}
