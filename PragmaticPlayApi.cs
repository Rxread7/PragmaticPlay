using Casino.Core;
using Casino.Core.BusinessLayer;
using Casino.Core.Tokens;
using Casino.Integration.Common;
using Casino.Integration.Gameplay;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Casino.Integration.PragmaticPlay
{
    public class PragmaticPlayApi : IGameProviderApi
    {
        private readonly ITokenManager _tokenManager;
        private PragmaticPlaySettings _settings;

        public PragmaticPlayApi(ITokenManager tokenManager) : this(tokenManager, PragmaticPlaySettings.GetSettings())
        {
        }

        public PragmaticPlayApi(ITokenManager tokenManager, PragmaticPlaySettings settings)
        {
            _tokenManager = tokenManager;
            _settings = settings;
        }

        public void InitializeSession(HttpRequest httpRequest)
        {
            
            var bytes = new byte[httpRequest.InputStream.Length];
            httpRequest.InputStream.Read(bytes, 0, bytes.Length);
            httpRequest.InputStream.Position = 0;
            string requestString = Encoding.ASCII.GetString(bytes);
            var request = HttpUtility.ParseQueryString(requestString);

            if (request != null)
            { 
                var token = string.Empty;
                if (!string.IsNullOrEmpty(request.Get("Token")))
                {
                    token = request.Get("Token");
                }
                _tokenManager.InitializeHttpContext(token);
            }
        }
        
        public string CreateGameUrl(GameUrlParameters parameters)
        {
            return parameters.Mode ? CreateGameUrlForReal(parameters) : CreateGameUrlForFun(parameters);
        }

        public string CreateGameUrlForReal(GameUrlParameters parameters)
        {
            // Example of game opening link:
            // https://{game_server_domain}/gs2c/playGame.do?key=token={token}&symbol={symbol}&technology={technology}&platform={platform}&language={language}&cashierUrl={cashierUrl}&lobbyUrl={lobbyUrl}&stylename={secureLogin}

            var game = parameters.Game;
            Token token = _tokenManager.CreateAuthToken(parameters.Player, game, parameters.TicketContext);
            var gameProvider = GameProvider.All.Find(gp => gp.Id == game.GameProviderId);
            
            var symbol = game.ExternalId;
            var language = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
            var technology = "H5";
            var platform = game.IsMobile ? "MOBILE" : "WEB";
            var cashierUrl = CasinoSettings.CasinoUrl;
            var lobbyUrl = CasinoSettings.CasinoUrl;
            var stylename = _settings.Stylename;

            var casinoUrl = Regex.Replace(CasinoSettings.CasinoUrl, @"[^a-zA-z0-9!@#]+", "");
            var url =  gameProvider.GameUrl;
            url = url.Replace("{TOKEN}", token.Id);
            url = url.Replace("{SYMBOL}", symbol);
            url = url.Replace("{LANGUAGE}", language);
            url = url.Replace("{TECHNOLOGY}", technology);
            url = url.Replace("{PLATFORM}", platform);
            url = url.Replace("{CASHIERURL}", cashierUrl);
            url = url.Replace("{LOBBYURL}", lobbyUrl);
            url = url.Replace("{STYLENAME}", stylename);

            Logging.LogToFile($";URL FOR REAL;{url}", "PragmaticPlayApi");
            return url;
        }

        public string CreateGameUrlForFun(GameUrlParameters parameters)
        {
            // Example of game opening link:
            // http(s)://{demo_games_domain}/gs2c/openGame.do?gameSymbol={game_symbol}&lang={language}&cur={currency_symbol}&jurisdiction={jurisdiction}&lobbyUrl={lobbyUrl}&stylename={secureLogin}

            var game = parameters.Game;
            var gameProvider = GameProvider.All.Find(gp => gp.Id == game.GameProviderId);

            var game_symbol = game.ExternalId;
            var language = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
            var currency_symbol = Currency.GetCurrency(CasinoSettings.DefaultCurrencyId).Code;
            var jurisdiction = CasinoSettings.DefaultLanguage;
            var lobbyUrl = CasinoSettings.CasinoUrl;
            var stylename = _settings.Stylename;

            var casinoUrl = Regex.Replace(CasinoSettings.CasinoUrl, @"[^a-zA-z0-9!@#]+", "");
            var url = gameProvider.GameForFunUrl;
            url = url.Replace("{GAMESYMBOL}", game_symbol);
            url = url.Replace("{LANGUAGE}", language);
            url = url.Replace("{CURRENCY}", currency_symbol);
            url = url.Replace("{JURISDICTION}", jurisdiction);
            url = url.Replace("{LOBBYURL}", lobbyUrl);
            url = url.Replace("{STYLENAME}", stylename);

            Logging.LogToFile($";URL FOR FUN;{url}", "PragmaticPlayApi");
            return url;
        }
    }
}
