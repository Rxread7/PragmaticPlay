using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Integration.PragmaticPlay.Requests
{
    public class PromoWinRequest : Request
    {

        /// <summary>
        /// Date and time when the transaction is processed on the Pragmatic Playside.
        /// (Unix epoch time in milliseconds, for example : 1470926696715)
        /// </summary>
        public long TimeStamp { get; set; }
        /// <summary>
        /// Identifier of the user within the Casino Operator’s system.
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Id of the campaign.
        /// </summary>
        public long CampaignId { get; set; }
        /// <summary>
        /// Type of the campaign.
        /// Available values are: T – Tournament, C – Chat game winning in bingo, B – Bonus award
        /// </summary>
        public string CampaignType { get; set; }
        /// <summary>
        /// Prize amount that the player is awarded with.
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// Player's currency.
        /// </summary>
        public string Currency { get; set; }
        /// <summary>
        /// Unique reference of the transaction within the Pragmatic Play system.
        /// </summary>
        public string Reference { get; set; }


        //dedi
        /*
        /// <summary>
        /// Hash code of the request.
        /// </summary>
        public string Hash { get; set; }
        /// <summary>
        /// Game Provider ID.
        /// </summary>
        public string ProviderId { get; set; }
        */


        /*
        //Optional
        /// <summary>
        /// Portfolio type of promo campaign.
        /// </summary>
        public string DataType { get; set; }
        */
    }
}
