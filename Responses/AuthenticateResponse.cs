using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Integration.PragmaticPlay.Responses
{
    public class AuthenticateResponse : Response
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }
        [JsonProperty("currency")]
        public string Currency { get; set; }
        [JsonProperty("cash")]
        public decimal Cash { get; set; }
        [JsonProperty("bonus")]
        public decimal Bonus { get; set; }
        [JsonProperty("token")]
        public string Token { get; set; }


        //dedi
        /*
        [JsonProperty("error")]
        public int Error { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        */


        /*
        //nechceme
        [JsonProperty("country")]
        public string Country { get; set; }
        [JsonProperty("jurisdiction")]
        public string Jurisdiction { get; set; }
        [JsonProperty("betLimits")]
        public betLimitsModel BetLimits { get; set; }
        */
    }

}
