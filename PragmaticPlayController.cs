using Casino.Common.Diagnostics;
using Casino.Core;
using Casino.Core.BusinessFacades;
using Casino.Core.BusinessLayer;
using Casino.Core.Tokens;
using Casino.Integration.Common;
using Casino.Integration.PragmaticPlay.Requests;
using Casino.Integration.PragmaticPlay.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using JackpotWinRequest = Casino.Integration.PragmaticPlay.Requests.JackpotWinRequest;

namespace Casino.Integration.PragmaticPlay
{
    public class PragmaticPlayController : ApiController
    {
        private IIntegrationFacade _integrationFacade;
        private PragmaticPlaySettings _settings;

        public PragmaticPlayController(IIntegrationFacade integrationFacade)
        {
            _integrationFacade = integrationFacade;
            _settings = PragmaticPlaySettings.GetSettings();
        }

        public PragmaticPlayController(IIntegrationFacade integrationFacade, PragmaticPlaySettings settings)
        {
            _integrationFacade = integrationFacade;
            _settings = settings;
        }

        [HttpPost]
        [HttpGet]
        [PerformanceCounter("EASIT.Casino.Api.PragmaticPlay", "Authenticate")]
        [Route("api/pragmaticplay/authenticate.html")]
        public AuthenticateResponse Authenticate([FromBody]AuthenticateRequest request)
        {
            if (!_integrationFacade.ValidateApiKey(GameProviderName.PragmaticPlay, request.ProviderId))
            {
                return new AuthenticateResponse()
                {
                    Error = (int)PragmaticPlayWalletApiErrorCode.BadParameters,
                    Description = PragmaticPlayWalletApiErrorCode.BadParameters.ToString()
                };
            }
            
            var reqKey = $"providerId={request.ProviderId}&token={request.Token}{_settings.SecretKey}";
            var reqKeyHash = CalculateHash(reqKey);
            if (reqKeyHash != request.Hash)
            {
                return new AuthenticateResponse()
                {
                    Error = (int)PragmaticPlayWalletApiErrorCode.InvalidHashCode,
                    Description = PragmaticPlayWalletApiErrorCode.InvalidHashCode.ToString()
                };
            }

            AuthenticateResponse response;

            var authenticateResult = _integrationFacade.Authenticate(request.Token);

            if (authenticateResult.IsSuccess)
            {
                response = new AuthenticateResponse
                {
                    UserId = authenticateResult.Token.Player.Id.ToString(),
                    Currency = Currency.GetCurrency(CasinoSettings.DefaultCurrencyId).Code,
                    Cash = _integrationFacade.GetBalance(authenticateResult.Token.Player, authenticateResult.Token.Game),
                    Bonus = 0.00M,
                    Error = (int)PragmaticPlayWalletApiErrorCode.Success,
                    Description = PragmaticPlayWalletApiErrorCode.Success.ToString(),
                    Token = authenticateResult.Token?.Id,
                };
            }
            else
            {
                response = new AuthenticateResponse
                {
                    Error = (int)PragmaticPlayWalletApiErrorCode.TokenInvalid,
                    Description = PragmaticPlayWalletApiErrorCode.TokenInvalid.ToString()
                };
            }
            return response;
        } 

        [HttpPost]
        [HttpGet]
        [PerformanceCounter("EASIT.Casino.Api.PragmaticPlay", "Balance")]
        [Route("api/pragmaticplay/balance.html")]
        public BalanceResponse Balance([FromBody]BalanceRequest request)
        {
            if (!_integrationFacade.ValidateApiKey(GameProviderName.PragmaticPlay, request.ProviderId))
            {
                return new BalanceResponse()
                {
                    Error = (int)PragmaticPlayWalletApiErrorCode.BadParameters,
                    Description = PragmaticPlayWalletApiErrorCode.BadParameters.ToString()
                };
            }

            string reqKey;
            reqKey = $"providerId={request.ProviderId}&token={request.Token}&userId={request.UserId}{_settings.SecretKey}";
            var reqKeyHash = CalculateHash(reqKey);
            if (reqKeyHash != request.Hash)
            {
                return new BalanceResponse()
                {
                    Error = (int)PragmaticPlayWalletApiErrorCode.InvalidHashCode,
                    Description = PragmaticPlayWalletApiErrorCode.InvalidHashCode.ToString()
                };
            }

            var token = _integrationFacade.GetToken(request.Token);
            if (!_integrationFacade.Validate(token))
            {
                return new BalanceResponse()
                {
                    Error = (int)PragmaticPlayWalletApiErrorCode.TokenInvalid,
                    Description = PragmaticPlayWalletApiErrorCode.TokenInvalid.ToString()
                };
            }

            var balance = _integrationFacade.GetBalance(token.Player, token.Game);
            var response = new BalanceResponse
            {
                Currency = Currency.GetCurrency(CasinoSettings.DefaultCurrencyId).Code,
                Cash = balance,
                Bonus = 0.00M,
                Error = (int)PragmaticPlayWalletApiErrorCode.Success,
                Description = PragmaticPlayWalletApiErrorCode.Success.ToString()
            };


            return response;
        } 

        [HttpPost]
        [HttpGet]
        [PerformanceCounter("EASIT.Casino.Api.PragmaticPlay", "Bet")]
        [Route("api/pragmaticplay/bet.html")]
        public BetResponse Bet([FromBody]BetRequest request) 
        {
            if (!_integrationFacade.ValidateApiKey(GameProviderName.PragmaticPlay, request.ProviderId))
            {
                return new BetResponse()
                {
                    Error = (int)PragmaticPlayWalletApiErrorCode.BadParameters,
                    Description = PragmaticPlayWalletApiErrorCode.BadParameters.ToString()
                };
            }

            var reqKey = $"amount={request.Amount.ToString(System.Globalization.CultureInfo.InvariantCulture)}&gameId={request.GameId}&providerId={request.ProviderId}&reference={request.Reference}&roundDetails={request.RoundDetails}&roundId={request.RoundId}&timestamp={request.TimeStamp}&token={request.Token}&userId={request.UserId}{_settings.SecretKey}";
            if (!string.IsNullOrEmpty(request.BonusCode))
            {
                reqKey = $"amount={request.Amount.ToString(System.Globalization.CultureInfo.InvariantCulture)}&bonusCode={request.BonusCode}&gameId={request.GameId}&providerId={request.ProviderId}&reference={request.Reference}&roundDetails={request.RoundDetails}&roundId={request.RoundId}&timestamp={request.TimeStamp}&token={request.Token}&userId={request.UserId}{_settings.SecretKey}";
            }
            var reqKeyHash = CalculateHash(reqKey);
            if (reqKeyHash != request.Hash)
            {
                return new BetResponse()
                {
                    Error = (int)PragmaticPlayWalletApiErrorCode.InvalidHashCode,
                    Description = PragmaticPlayWalletApiErrorCode.InvalidHashCode.ToString()
                };
            }


            var token = _integrationFacade.GetToken(request.Token);

            if (!_integrationFacade.Validate(token))
            {
                return new BetResponse()
                {
                    Error = (int)PragmaticPlayWalletApiErrorCode.TokenInvalid,
                    Description = PragmaticPlayWalletApiErrorCode.TokenInvalid.ToString()
                };
            }


            var ticketExternalId = request.RoundId.ToString();
            var transferExternalId = request.Reference;
            var amount = request.Amount;
            
            BetResult result;

            if (string.IsNullOrEmpty(request.BonusCode))
            {
                result = _integrationFacade.Bet(token.Player, token.Game, ticketExternalId, transferExternalId, amount);
            }
            else
            {
                result = _integrationFacade.Bet(token.Player, token.Game, ticketExternalId, transferExternalId, request.BonusCode, token.TicketContext);
            }

            var response = new BetResponse()
            {
                Error = (int)PragmaticPlayWalletApiErrorCode.InternalServerError,
                Description = PragmaticPlayWalletApiErrorCode.InternalServerError.ToString()
            };

            if (result.IsSuccess)
            {
                response.TransactionId = result.TransferId.ToString();
                response.Currency = Currency.GetCurrency(CasinoSettings.DefaultCurrencyId).Code;
                response.Cash = result.Balance;
                response.Bonus = 0.00M;
                response.UsedPromo = 0.00M;
                response.Error = (int)PragmaticPlayWalletApiErrorCode.Success;
                response.Description = PragmaticPlayWalletApiErrorCode.Success.ToString();
            }
            else
            {
                switch (result.PaymentResultCode)
                {
                    case PaymentFacadeResultCode.Failed:
                        response.Error = (int)PragmaticPlayWalletApiErrorCode.InternalServerError;
                        response.Description = PragmaticPlayWalletApiErrorCode.InternalServerError.ToString();
                        break;
                    case PaymentFacadeResultCode.InsufficientFunds:
                        response.Error = (int)PragmaticPlayWalletApiErrorCode.InsufficientBalance;
                        response.Description = PragmaticPlayWalletApiErrorCode.InsufficientBalance.ToString();
                        break;
                    case PaymentFacadeResultCode.ResponsibleGamingLimit:
                        response.Error = (int)PragmaticPlayWalletApiErrorCode.BetLimitReached;
                        response.Description = PragmaticPlayWalletApiErrorCode.BetLimitReached.ToString();
                        break;
                    default:
                        break;
                }
            }
            return response;
        } 

        [HttpPost]
        [HttpGet]
        [PerformanceCounter("EASIT.Casino.Api.PragmaticPlay", "Result")]
        [Route("api/pragmaticplay/result.html")]
        public ResultResponse Result([FromBody]ResultRequest request)
        {
            if (!_integrationFacade.ValidateApiKey(GameProviderName.PragmaticPlay, request.ProviderId))
            {
                return new ResultResponse()
                {
                    Error = (int)PragmaticPlayWalletApiErrorCode.BadParameters,
                    Description = PragmaticPlayWalletApiErrorCode.BadParameters.ToString()
                };
            }

            var reqKey = $"amount={request.Amount.ToString(System.Globalization.CultureInfo.InvariantCulture)}&gameId={request.GameId}&providerId={request.ProviderId}&reference={request.Reference}&roundDetails={request.RoundDetails}&roundId={request.RoundId}&timestamp={request.TimeStamp}&token={request.Token}&userId={request.UserId}{_settings.SecretKey}";
            if (!string.IsNullOrEmpty(request.BonusCode))
            {
                reqKey = $"amount={request.Amount.ToString(System.Globalization.CultureInfo.InvariantCulture)}&bonusCode={request.BonusCode}&gameId={request.GameId}&providerId={request.ProviderId}&reference={request.Reference}&roundDetails={request.RoundDetails}&roundId={request.RoundId}&timestamp={request.TimeStamp}&token={request.Token}&userId={request.UserId}{_settings.SecretKey}";
            }

            var reqKeyHash = CalculateHash(reqKey);
            if (reqKeyHash != request.Hash)
            {
                return new ResultResponse()
                {
                    Error = (int)PragmaticPlayWalletApiErrorCode.InvalidHashCode,
                    Description = PragmaticPlayWalletApiErrorCode.InvalidHashCode.ToString()
                };
            }

            var token = _integrationFacade.GetToken(request.Token);

            if (!_integrationFacade.Validate(token))
            {
                return new ResultResponse()
                {
                    Error = (int)PragmaticPlayWalletApiErrorCode.TokenInvalid,
                    Description = PragmaticPlayWalletApiErrorCode.TokenInvalid.ToString()
                };
            }

            var ticketExternalId = request.RoundId.ToString();
            var transferExternalId = request.Reference;
            var amount = request.Amount;
            var terminationBehaviour =  TerminationBehaviour.Never;

            var ticketDetail = CreateTicketDetailResult(request);
            
            WinResult result;
            if (string.IsNullOrEmpty(request.BonusCode))
            {
                result = _integrationFacade.Win(token.Player, token.Game, ticketExternalId, transferExternalId, amount, terminationBehaviour, ticketDetail);
            }
            else
            {
                result = _integrationFacade.Win(token.Player, token.Game, ticketExternalId, transferExternalId, amount, false, ticketDetail, request.BonusCode);
            }
            
            var response = new ResultResponse();

            if (result.IsSuccess)
            {
                response.TransactionId = result.TransferId.ToString();
                response.Currency = Currency.GetCurrency(CasinoSettings.DefaultCurrencyId).Code;
                response.Cash = result.Balance;
                response.Bonus = 0.00M;
                response.Error = (int)PragmaticPlayWalletApiErrorCode.Success;
                response.Description = PragmaticPlayWalletApiErrorCode.Success.ToString();
            }
            else
            {
                response.Error = (int)PragmaticPlayWalletApiErrorCode.InternalServerErrorReconciliation;
                response.Description = PragmaticPlayWalletApiErrorCode.InternalServerErrorReconciliation.ToString();
            }

            return response;
        }

        [HttpPost]
        [HttpGet]
        [PerformanceCounter("EASIT.Casino.Api.PragmaticPlay", "BonusWin")]
        [Route("api/pragmaticplay/bonusWin.html")]
        public BonusWinResponse BonusWin([FromBody]BonusWinRequest request)
        {
            if (!_integrationFacade.ValidateApiKey(GameProviderName.PragmaticPlay, request.ProviderId))
            {
                return new BonusWinResponse()
                {
                    Error = (int)PragmaticPlayWalletApiErrorCode.BadParameters,
                    Description = PragmaticPlayWalletApiErrorCode.BadParameters.ToString()
                };
            }

            var reqKey = $"amount={request.Amount.ToString(System.Globalization.CultureInfo.InvariantCulture)}&gameId={request.GameId}&providerId={request.ProviderId}&reference={request.Reference}&roundId={request.RoundId}&timestamp={request.TimeStamp}&token={request.Token}&userId={request.UserId}{_settings.SecretKey}";
            if (!string.IsNullOrEmpty(request.BonusCode))
            {
                reqKey = $"amount={request.Amount.ToString(System.Globalization.CultureInfo.InvariantCulture)}&bonusCode={request.BonusCode}&gameId={request.GameId}&providerId={request.ProviderId}&reference={request.Reference}&roundId={request.RoundId}&timestamp={request.TimeStamp}&token={request.Token}&userId={request.UserId}{_settings.SecretKey}";
            }

            var reqKeyHash = CalculateHash(reqKey);
            if (reqKeyHash != request.Hash)
            {
                return new BonusWinResponse()
                {
                    Error = (int)PragmaticPlayWalletApiErrorCode.InvalidHashCode,
                    Description = PragmaticPlayWalletApiErrorCode.InvalidHashCode.ToString()
                };
            }

            var token = _integrationFacade.GetToken(request.Token);

            if (!_integrationFacade.Validate(token))
            {
                return new BonusWinResponse()
                {
                    Error = (int)PragmaticPlayWalletApiErrorCode.TokenInvalid,
                    Description = PragmaticPlayWalletApiErrorCode.TokenInvalid.ToString()
                };
            }

            var ticketExternalId = request.RoundId.ToString();
            var transferExternalId = request.Reference;
            var amount = request.Amount;
            var terminationBehaviour = TerminationBehaviour.Never;
            var ticketDetail = CreateTicketDetailBonusWin(request);
            
            WinResult result;
            if (string.IsNullOrEmpty(request.BonusCode))
            {
                result = _integrationFacade.Win(token.Player, token.Game, ticketExternalId, transferExternalId, amount, terminationBehaviour, ticketDetail);
            }
            else
            {
                result = _integrationFacade.Win(token.Player, token.Game, ticketExternalId, transferExternalId, amount, false, ticketDetail, request.BonusCode);
            }
            
            //var result = _integrationFacade.Win(token.Player, token.Game, ticketExternalId, transferExternalId, amount, terminationBehaviour, ticketDetail);
            var response = new BonusWinResponse();

            if (result.IsSuccess)
            {
                response.TransactionId = result.TransferId.ToString();
                response.Currency = Currency.GetCurrency(CasinoSettings.DefaultCurrencyId).Code;
                response.Cash = result.Balance;
                response.Bonus = 0.00M;
                response.Error = (int)PragmaticPlayWalletApiErrorCode.Success;
                response.Description = PragmaticPlayWalletApiErrorCode.Success.ToString();
            }
            else
            {
                response.Error = (int)PragmaticPlayWalletApiErrorCode.InternalServerErrorReconciliation;
                response.Description = PragmaticPlayWalletApiErrorCode.InternalServerErrorReconciliation.ToString();
            }

            return response;
        }
        
        [HttpPost]
        [HttpGet]
        [PerformanceCounter("EASIT.Casino.Api.PragmaticPlay", "JackpotWin")]
        [Route("api/pragmaticplay/jackpotWin.html")]
        public JackpotWinResponse JackpotWin([FromBody]JackpotWinRequest request) 
        {
            if (!_integrationFacade.ValidateApiKey(GameProviderName.PragmaticPlay, request.ProviderId))
            {
                return new JackpotWinResponse()
                {
                    Error = (int)PragmaticPlayWalletApiErrorCode.BadParameters,
                    Description = PragmaticPlayWalletApiErrorCode.BadParameters.ToString()
                };
            }

            string reqKey;
            reqKey = $"amount={request.Amount.ToString(System.Globalization.CultureInfo.InvariantCulture)}&gameId={request.GameId}&jackpotId={request.JackpotId}&providerId={request.ProviderId}&reference={request.Reference}&roundId={request.RoundId}&timestamp={request.TimeStamp}&token={request.Token}&userId={request.UserId}{_settings.SecretKey}";
            var reqKeyHash = CalculateHash(reqKey);
            if (reqKeyHash != request.Hash)
            {
                return new JackpotWinResponse()
                {
                    Error = (int)PragmaticPlayWalletApiErrorCode.InvalidHashCode,
                    Description = PragmaticPlayWalletApiErrorCode.InvalidHashCode.ToString()
                };
            }

            var token = _integrationFacade.GetToken(request.Token);

            if (!_integrationFacade.Validate(token))
            {
                return new JackpotWinResponse()
                {
                    Error = (int)PragmaticPlayWalletApiErrorCode.TokenInvalid,
                    Description = PragmaticPlayWalletApiErrorCode.TokenInvalid.ToString()
                };
            }

            var jackpotGame = _integrationFacade.GetGame(token.Game.GameProviderName, "Jackpot", GameType.None);
            if (jackpotGame == null || jackpotGame.Id == 0)
            {
                return new JackpotWinResponse()
                {
                    Error = (int)PragmaticPlayWalletApiErrorCode.GameNotFound,
                    Description = PragmaticPlayWalletApiErrorCode.GameNotFound.ToString()
                };
            }
            
            var ticketExternalId = request.RoundId.ToString();
            var transferExternalId = request.Reference;
            var amount = request.Amount;

            var result = _integrationFacade.WinJackpot(token.Player, jackpotGame, ticketExternalId, transferExternalId, amount, TerminationBehaviour.Never);
            var response = new JackpotWinResponse();

            if (result.IsSuccess)
            {
                response.TransactionId = result.TransferId.ToString();
                response.Currency = Currency.GetCurrency(CasinoSettings.DefaultCurrencyId).Code;
                response.Cash = result.Balance;
                response.Bonus = 0.00M;
                response.Error = (int)PragmaticPlayWalletApiErrorCode.Success;
                response.Description = PragmaticPlayWalletApiErrorCode.Success.ToString();
            }
            else
            {
                response.Error = (int)PragmaticPlayWalletApiErrorCode.InternalServerErrorReconciliation;
                response.Description = PragmaticPlayWalletApiErrorCode.InternalServerErrorReconciliation.ToString();
            }

            return response;
        }

        [HttpPost]
        [HttpGet]
        [PerformanceCounter("EASIT.Casino.Api.PragmaticPlay", "PromoWin")]
        [Route("api/pragmaticplay/promoWin.html")]
        public PromoWinResponse PromoWin([FromBody]PromoWinRequest request)    
        {
            if (!_integrationFacade.ValidateApiKey(GameProviderName.PragmaticPlay, request.ProviderId))
            {
                return new PromoWinResponse()
                {
                    Error = (int)PragmaticPlayWalletApiErrorCode.BadParameters,
                    Description = PragmaticPlayWalletApiErrorCode.BadParameters.ToString()
                };
            }

            string reqKey;
            reqKey = $"amount={request.Amount.ToString(System.Globalization.CultureInfo.InvariantCulture)}&campaignId={request.CampaignId}&campaignType={request.CampaignType}&currency={request.Currency}&providerId={request.ProviderId}&reference={request.Reference}&timestamp={request.TimeStamp}&token={request.Token}&userId={request.UserId}{_settings.SecretKey}";
            var reqKeyHash = CalculateHash(reqKey);
            if (reqKeyHash != request.Hash)
            {
                return new PromoWinResponse()
                {
                    Error = (int)PragmaticPlayWalletApiErrorCode.InvalidHashCode,
                    Description = PragmaticPlayWalletApiErrorCode.InvalidHashCode.ToString()
                };
            }

            var transferExternalId = request.Reference;
            var ticketExternalId = $"Promo;{request.CampaignType};{transferExternalId}";
            var amount = request.Amount;
            var terminationBehaviour = TerminationBehaviour.Never;
            var playerId = Int32.Parse(request.UserId);

            var promoGame = _integrationFacade.GetGame(GameProviderName.PragmaticPlay, "Promo", GameType.None);
            if (promoGame == null || promoGame.Id == 0)
            {
                return new PromoWinResponse()
                {
                    Error = (int)PragmaticPlayWalletApiErrorCode.GameNotFound,
                    Description = PragmaticPlayWalletApiErrorCode.GameNotFound.ToString()
                };
            }
            
            var result = _integrationFacade.WinPromo(playerId, promoGame, ticketExternalId, transferExternalId, amount, terminationBehaviour);

            var response = new PromoWinResponse();

            if (result.IsSuccess)
            {
                response.TransactionId = result.TransferId.ToString();
                response.Currency = Currency.GetCurrency(CasinoSettings.DefaultCurrencyId).Code;
                response.Cash = result.Balance;
                response.Bonus = 0.00M;
                response.Error = (int)PragmaticPlayWalletApiErrorCode.Success;
                response.Description = PragmaticPlayWalletApiErrorCode.Success.ToString();
            }
            else
            {
                response.Error = (int)PragmaticPlayWalletApiErrorCode.InternalServerErrorReconciliation;
                response.Description = PragmaticPlayWalletApiErrorCode.InternalServerErrorReconciliation.ToString();
            }

            return response;
        }

        [HttpPost]
        [HttpGet]
        [PerformanceCounter("EASIT.Casino.Api.PragmaticPlay", "Adjustment")]
        [Route("api/pragmaticplay/adjustment.html")]
        public AdjustmentResponse Adjustment([FromBody]AdjustmentRequest request) 
        {
            if (!_integrationFacade.ValidateApiKey(GameProviderName.PragmaticPlay, request.ProviderId))
            {
                return new AdjustmentResponse()
                {
                    Error = (int)PragmaticPlayWalletApiErrorCode.BadParameters,
                    Description = PragmaticPlayWalletApiErrorCode.BadParameters.ToString()
                };
            }

            string reqKey;
            reqKey = $"amount={request.Amount.ToString(System.Globalization.CultureInfo.InvariantCulture)}&gameId={request.GameId}&providerId={request.ProviderId}&reference={request.Reference}&roundId={request.RoundId}&timestamp={request.TimeStamp}&token={request.Token}&userId={request.UserId}{_settings.SecretKey}"; //mozna dat do haje token
            var reqKeyHash = CalculateHash(reqKey);
            if (reqKeyHash != request.Hash)
            {
                return new AdjustmentResponse()
                {
                    Error = (int)PragmaticPlayWalletApiErrorCode.InvalidHashCode,
                    Description = PragmaticPlayWalletApiErrorCode.InvalidHashCode.ToString()
                };
            }

            var token = _integrationFacade.GetToken(request.Token);

            if (!_integrationFacade.Validate(token))
            {
                return new AdjustmentResponse()
                {
                    Error = (int)PragmaticPlayWalletApiErrorCode.TokenInvalid,
                    Description = PragmaticPlayWalletApiErrorCode.TokenInvalid.ToString()
                };
            }

            var ticketExternalId = request.RoundId.ToString();
            var transferExternalId = request.Reference;
            var amount = request.Amount;
            var terminationBehaviour = TerminationBehaviour.Never;

            var ticketDetail = CreateTicketDetailAdjustment(request);
            var result = _integrationFacade.Win(token.Player, token.Game, ticketExternalId, transferExternalId, amount, terminationBehaviour, ticketDetail);

            var response = new AdjustmentResponse();

            if (result.IsSuccess)
            {
                response.TransactionId = result.TransferId.ToString();
                response.Currency = Currency.GetCurrency(CasinoSettings.DefaultCurrencyId).Code;
                response.Cash = result.Balance;
                response.Bonus = 0.00M;
                response.Error = (int)PragmaticPlayWalletApiErrorCode.Success;
                response.Description = PragmaticPlayWalletApiErrorCode.Success.ToString();
            }
            else
            {
                response.Error = (int)PragmaticPlayWalletApiErrorCode.InternalServerErrorReconciliation;
                response.Description = PragmaticPlayWalletApiErrorCode.InternalServerErrorReconciliation.ToString();
            }

            return response;
        }

        [HttpPost]
        [HttpGet]
        [PerformanceCounter("EASIT.Casino.Api.PragmaticPlay", "Refund")] 
        [Route("api/pragmaticplay/refund.html")]
        public RefundResponse Refund([FromBody]RefundRequest request)
        {
            if (!_integrationFacade.ValidateApiKey(GameProviderName.PragmaticPlay, request.ProviderId))
            {
                return new RefundResponse()
                {
                    Error = (int)PragmaticPlayWalletApiErrorCode.BadParameters,
                    Description = PragmaticPlayWalletApiErrorCode.BadParameters.ToString()
                };
            }

            string reqKey;
            reqKey = $"providerId={request.ProviderId}&reference={request.Reference}&token={request.Token}&userId={request.UserId}{_settings.SecretKey}";
            var reqKeyHash = CalculateHash(reqKey);
            if (reqKeyHash != request.Hash)
            {
                return new RefundResponse()
                {
                    Error = (int)PragmaticPlayWalletApiErrorCode.InvalidHashCode,
                    Description = PragmaticPlayWalletApiErrorCode.InvalidHashCode.ToString()
                };
            }

            var token = _integrationFacade.GetToken(request.Token);

            if (!_integrationFacade.Validate(token))
            {
                return new RefundResponse()
                {
                    Error = (int)PragmaticPlayWalletApiErrorCode.TokenInvalid,
                    Description = PragmaticPlayWalletApiErrorCode.TokenInvalid.ToString()
                };
            }

            var transferExternalId = request.Reference;
            var refundResult = _integrationFacade.Refund(token.Player, token.Game, transferExternalId, TerminationBehaviour.Never);
            //promo (FGO)
            if (refundResult.PaymentResultCode == PaymentFacadeResultCode.InvalidGameRound)
            {
                refundResult = _integrationFacade.RegisterEmptyRefund(token.Player, token.Game, transferExternalId, false);
            }

            var refundResponse = new RefundResponse();

            if (refundResult.IsSuccess)
            {
                refundResponse.TransactionId = refundResult.TransferId.ToString();
                refundResponse.Error = (int)PragmaticPlayWalletApiErrorCode.Success;
                refundResponse.Description = PragmaticPlayWalletApiErrorCode.Success.ToString();
            }
            else
            {
                refundResponse.Error = (int)PragmaticPlayWalletApiErrorCode.InternalServerErrorReconciliation;
                refundResponse.Description = PragmaticPlayWalletApiErrorCode.InternalServerErrorReconciliation.ToString();
            }

            return refundResponse;
        }

        [HttpPost]
        [HttpGet]
        [PerformanceCounter("EASIT.Casino.Api.PragmaticPlay", "EndRound")]
        [Route("api/pragmaticplay/endRound.html")]
        public EndRoundResponse EndRound([FromBody]EndRoundRequest request)
        {
            if (!_integrationFacade.ValidateApiKey(GameProviderName.PragmaticPlay, request.ProviderId))
            {
                return new EndRoundResponse()
                {
                    Error = (int)PragmaticPlayWalletApiErrorCode.BadParameters,
                    Description = PragmaticPlayWalletApiErrorCode.BadParameters.ToString()
                };
            }

            string reqKey;
            reqKey = $"gameId={request.GameId}&providerId={request.ProviderId}&roundId={request.RoundId}&token={request.Token}&userId={request.UserId}{_settings.SecretKey}";
            var reqKeyHash = CalculateHash(reqKey);
            if (reqKeyHash != request.Hash)
            {
                return new EndRoundResponse()
                {
                    Error = (int)PragmaticPlayWalletApiErrorCode.InvalidHashCode,
                    Description = PragmaticPlayWalletApiErrorCode.InvalidHashCode.ToString()
                };
            }

            var token = _integrationFacade.GetToken(request.Token);

            if (!_integrationFacade.Validate(token))
            {
                return new EndRoundResponse()
                {
                    Error = (int)PragmaticPlayWalletApiErrorCode.TokenInvalid,
                    Description = PragmaticPlayWalletApiErrorCode.TokenInvalid.ToString()
                };
            }

            var response = new EndRoundResponse();
            var ticketExternalId = request.RoundId.ToString();
            var result = _integrationFacade.TerminateTicket(token.Player, token.Game, ticketExternalId);

            if (result.IsSuccess)
            {
                response.Cash = result.Balance;
                response.Bonus = 0.00M;
                response.Error = (int)PragmaticPlayWalletApiErrorCode.Success;
                response.Description = PragmaticPlayWalletApiErrorCode.Success.ToString();
            }
            else
            {
                response.Error = (int)PragmaticPlayWalletApiErrorCode.EndRoundInternalServerError;
                response.Description = PragmaticPlayWalletApiErrorCode.EndRoundInternalServerError.ToString();
            }

            return response;
        }

        private string CalculateHash(string input) 
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder(hash.Length * 2);
                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }

        //todo az provider upravi API, zatim prichystano, staci potom vymenit 0 za promene
        private TicketDetail CreateTicketDetailResult(ResultRequest request)
        {
            TicketDetail ticketDetail = null;
            if (request.Amount.ToString() != null)
            {
                ticketDetail = new TicketDetail
                {
                    BaseWin = request.Amount,
                    GambleCount = 0,
                    GambleBet = 0,
                    GambleWin = 0,
                    FreeSpinCount = 0,
                    FreeSpinWin = 0
                };
            }
            return ticketDetail;
        }

        private TicketDetail CreateTicketDetailBonusWin(BonusWinRequest request)
        {
            TicketDetail ticketDetail = null;
            if (request.Amount.ToString() != null)
            {
                ticketDetail = new TicketDetail
                {
                    BaseWin = request.Amount,
                    GambleCount = 0,
                    GambleBet = 0,
                    GambleWin = 0,
                    FreeSpinCount = 0,
                    FreeSpinWin = 0
                };
            }
            return ticketDetail;
        }

        private TicketDetail CreateTicketDetailAdjustment(AdjustmentRequest request)
        {
            TicketDetail ticketDetail = null;
            if (request.Amount.ToString() != null)
            {
                ticketDetail = new TicketDetail
                {
                    BaseWin = request.Amount,
                    GambleCount = 0,
                    GambleBet = 0,
                    GambleWin = 0,
                    FreeSpinCount = 0,
                    FreeSpinWin = 0
                };
            }
            return ticketDetail;
        }
    }
}
