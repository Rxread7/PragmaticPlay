namespace Casino.Integration.PragmaticPlay
{
    public enum PragmaticPlayWalletApiErrorCode
    {
        /// <summary>
        /// Success.
        /// </summary>
        Success = 0,
        /// <summary>
        /// Insufficient balance.
        /// The error should be returned in the response on the Bet request.
        /// </summary>
        InsufficientBalance = 1,
        /// <summary>
        /// Player not found or is logged out.
        /// Should be returned in the response on any request sent by Pragmatic Play if the player can’t be found or is logged out at Casino Operator’s side.
        /// </summary>
        PlayerNotFound = 2,
        /// <summary>
        /// Bet is not allowed. Should be returned in any case when the player is not allowed to play a specific game.
        /// For example, because of special bonus.
        /// </summary>
        BetNotAllowed = 3,
        /// <summary>
        /// Player authentication failed due to invalid, not found or expired token.
        /// </summary>
        TokenInvalid = 4,
        /// <summary>
        /// Invalid hash code.
        /// Should be returned in the response on any request sent by Pragmatic Play if the hash code validation is failed.
        /// </summary>
        InvalidHashCode = 5,
        /// <summary>
        /// Player is frozen.
        /// Casino Operator  will  return this  error in  the  response  of any request if player account if banned or frozen.
        /// </summary>
        PlayerFrozen = 6,
        /// <summary>
        /// Bad parameters in the request, please check post parameters.
        /// </summary>
        BadParameters = 7,
        /// <summary>
        /// Game  is  not  found  or  disabled.
        /// This  error  should  be  returned  on  Bet request if the game cannot be played by some reason.
        /// Bet result request with winning amount should be processed as intended, even if the game is disabled.
        /// </summary>
        GameNotFound = 8,
        /// <summary>
        /// Bet limit has been reached. The code is relevant for regulated markets.
        /// </summary>
        BetLimitReached = 50,
        /// <summary>
        /// Internal  server  error.
        /// Casino  Operator  will  return  this  error  code  if  their system  has  internal  problem  and  cannot  process  the  request  at  the moment  and  Operator  logic  requires  a  retry  of  the  request.
        /// Request  will follow Reconciliation process.
        /// </summary>
        InternalServerErrorReconciliation = 100,
        /// <summary>
        /// Internal  server  error.
        /// Casino  Operator  will  return  this  error  code  if  their system has internal problem and cannot process the request and Operator logic  does  not  require  a  retry  of  the  request.
        /// Request  will  NOT  follow Reconciliation process.
        /// </summary>
        InternalServerError = 120,
        /// <summary>
        /// Internal server error on EndRound processing.
        /// Casino Operator will return this error code if their system has internal problem and cannot process the EndRound request, and  Operator  logic requires a  retry  of  the  request.
        /// This error code should be used for Endround method only and not for other methods
        /// </summary>
        EndRoundInternalServerError = 130,
        /// <summary>
        /// Reality check warning.
        /// </summary>
        RealityCheckWarning = 210,
        /// <summary>
        /// Player’s bet is out of his bet limits.
        /// Should be returned if player’s limits have been changed, and the betis out of new limit levels.
        /// Game client will show a  proper  error  message,  and  ask  player  to  reopen  the game.
        /// After  game reopening new bet limits will be applied.
        /// The error is relevant for operators that send player’s bet limits in response on Authenticate request.
        /// </summary>
        BetOutOfLimits = 310
    }
}
