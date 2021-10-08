using Casino.Core.BusinessLayer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Integration.PragmaticPlay
{
    public class PragmaticPlaySettings
    {
        public string Stylename { get; set; }
        public string SecretKey { get; set; }

        public static PragmaticPlaySettings GetSettings()
        {
            var settings = new PragmaticPlaySettings();
            var gameProvider = GameProvider.All.FirstOrDefault(gp => gp.Id == (int)GameProviderName.PragmaticPlay);

            if (gameProvider != null && gameProvider.Data != null && gameProvider.SecretKey != null)
            {
                var data = JsonConvert.DeserializeObject<PragmaticPlaySettings>(gameProvider.Data);
                if (settings != null)
                {
                    settings.Stylename = data.Stylename;
                    settings.SecretKey = gameProvider.SecretKey;
                }
            }
            return settings;
        }
    }
}
