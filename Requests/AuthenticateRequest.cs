using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Integration.PragmaticPlay.Requests
{
    public class AuthenticateRequest : Request
    {
        //dedi
        /*
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
        */


        //zbytecne
        /*
        /// <summary>
        /// Value if the token that was received from operator for the game that should be closed.
        /// </summary>
        public string PreviousToken { get; set; }
        /// <summary>
        /// Type of game launch. Possible values: (“N” – normal launch) OR ("L -from in-game lobby”).
        /// </summary>
        public string LaunchingType { get; set; }
        /// <summary>
        /// Id of the game.
        /// </summary>
        public string GameId { get; set; }
        /// <summary>
        /// IP address of the player.
        /// </summary>
        public string IpAddress { get; set; }
        /// <summary>
        /// Amount of money player wants to spend in the game.
        /// </summary>
        public decimal ChosenBalance { get; set; }
        */
    }

}
