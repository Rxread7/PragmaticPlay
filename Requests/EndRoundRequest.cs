using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Integration.PragmaticPlay.Requests
{
    public class EndRoundRequest : Request
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
        /// ID of the round.
        /// </summary>
        public long RoundId { get; set; }



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
        /// <summary>
        /// Token of the player from Authenticate response.
        /// </summary>
        public string Token { get; set; }
        */


        //zbytecne
        /*
        //optional
        /// <summary>
        /// The platform type (channel) on which the game is played.
        /// Possible values: “MOBILE” – mobile device, “WEB” – desktop device, “DOWNLOAD” – downloadable client.
        /// </summary>
        public string Platform { get; set; }
        /// <summary>
        /// Additional information about the current game round.
        /// </summary>
        public string RoundDetails { get; set; }
        /// <summary>
        /// Win amount in round. Intended to notify Operator about amount won in round.
        /// This is notification parameter, and should not beused fortransactionwithin round.
        /// </summary>
        public decimal Win { get; set; }
        */
    }
}
