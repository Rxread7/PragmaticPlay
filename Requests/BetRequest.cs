using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Integration.PragmaticPlay.Requests
{
    public class BetRequest : Request
    {
        /// <summary>
        /// Identifier of the user within the Casino Operator’s system.
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// ID of the game.
        /// </summary>
        public string GameId { get; set; }
        /// <summary>
        /// Id of the round.
        /// </summary>
        public long RoundId { get; set; }
        /// <summary>
        /// Amount of the bet. Minimum is 0.00.
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// Unique reference of this transaction.
        /// </summary>
        public string Reference { get; set; }
        /// <summary>
        /// Date and time when the transaction is processedon the Pragmatic Playside.
        /// (Unix epoch time in milliseconds, for example : 1470926696715)
        /// </summary>
        public long TimeStamp { get; set; }
        /// <summary>
        /// Additional information about the current game round.
        /// </summary>
        public string RoundDetails { get; set; }
        // Mandatory for FRB
        /// <summary>
        /// Id of the bonus in Casino Operator system.
        /// (*is mandatory in case of FRB API is implemented)
        /// </summary>
        public string BonusCode { get; set; }


        //dedi
        /*
        /// <summary>
        /// Token of the player from Authenticate response.
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// Game Provider ID.
        /// </summary>
        public string ProviderId { get; set; }
        /// <summary>
        /// Hash code of the request.
        /// </summary>
        public string Hash { get; set; }
        */


        //zbytecne
        /*
        /// <summary>
        /// The platform type (channel) on which the game is played.
        /// Possible values: “MOBILE” – mobile device, “WEB” – desktop device, “DOWNLOAD” – downloadable client.
        /// </summary>
        public string Platform { get; set; }
        /// <summary>
        /// Language on which the game was opened.
        /// </summary>
        public string Language { get; set; }
        /// <summary>
        /// Amount of the contribution to the jackpot.
        /// If there is a multi-tier jackpot, this field will contain the total amount of contributions to all jackpots.
        /// The field is optional and should be sent together with jackpotId.
        /// </summary>
        public decimal JackpotContribution { get; set; }
        /// <summary>
        /// Amounts of the contribution for multi-tier jackpot, separated by tiers.
        /// The field is optional and should be sent together with jackpotId and jackpotContribution.
        /// VALID JSON (Example:       "jackpotDetails":"{"2701":       {"amount":1.00,"prizeType": "monetary"},  "2702":{"amount":2.00,"prizeType": "monetary"}})
        /// </summary>
        public string JackpotDetails { get; set; }
        /// <summary>
        /// Id of the active jackpot to contribute.
        /// The field is optional and should be sent together with jackpotContribution.
        /// </summary>
        public long JackpotId { get; set; }
        /// <summary>
        /// IP address of the player.
        /// </summary>
        public string ipAddress { get; set; }
        */
    }
}
