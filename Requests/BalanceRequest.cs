using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Integration.PragmaticPlay.Requests
{
    public class BalanceRequest : Request
    {
        /// <summary>
        /// Identifier of the user within the Casino Operator’s system.
        /// </summary>
        public string UserId { get; set; }


        //dedi
        /*
        /// <summary>
        /// Hash code of the request.
        /// </summary>
        public string Hash { get; set; }
        /// <summary>
        /// Game Provider identifier.
        /// </summary>
        public string ProviderId { get; set; }
        /// <summary>
        /// Token of the player from Authenticate response.
        /// </summary>
        public string Token { get; set; }
        */
    }
}
