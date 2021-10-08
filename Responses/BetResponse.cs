using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Integration.PragmaticPlay.Responses
{
    public class BetResponse : Response
    {
        [JsonProperty("transactionId")]
        public string TransactionId { get; set; }
        [JsonProperty("currency")]
        public string Currency { get; set; }
        [JsonProperty("cash")]
        public decimal Cash { get; set; }
        [JsonProperty("bonus")]
        public decimal Bonus { get; set; }
        [JsonProperty("usedPromo")]
        public decimal UsedPromo { get; set; }


        //dedi
        /*
        public int Error { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        */
    }
}
