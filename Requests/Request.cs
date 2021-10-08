using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Integration.PragmaticPlay.Requests
{
    public class Request
    {
        /// <summary>
        /// Hash code of the request.
        /// </summary>
        public string Hash { get; set; }
        /// <summary>
        /// Token of the player.
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// Game Provider identifier.
        /// </summary>
        public string ProviderId { get; set; }
    }
}
