using LineBot_LieFlatMonkey.Assets.Model.AppSetting;
using LineBot_LieFlatMonkey.Modules.Interfaces;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime.Models;
using Microsoft.Extensions.Options;
using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Modules.Services
{
    /// <summary>
    /// LUIS 意圖判斷 Service
    /// </summary>
    public class LUISService : ILUISService
    {
        private readonly IOptions<LUISSetting> luisSetting;
        private readonly Prediction prediction;

        public LUISService(IOptions<LUISSetting> luisSetting)
        {
            this.luisSetting = luisSetting;
            var luisRuntimeClient = new LUISRuntimeClient(
                new ApiKeyServiceClientCredentials(this.luisSetting.Value.PrimaryKey));

            luisRuntimeClient.Endpoint = this.luisSetting.Value.EndpointURL;

            this.prediction = new Prediction(luisRuntimeClient);
        }

        /// <summary>
        /// 取得意圖判斷結果
        /// </summary>
        /// <param name="text">需要意圖判斷的文字</param>
        /// <returns></returns>
        public async Task<string> GetIntent(string text)
        {
            try 
            {
                LuisResult luisResult = await this.prediction.ResolveAsync(
                    appId: this.luisSetting.Value.AppID,
                    query: text,
                    timezoneOffset: null,
                    verbose: true,
                    staging: false,
                    spellCheck: false,
                    bingSpellCheckSubscriptionKey: null,
                    log: false,
                    cancellationToken: CancellationToken.None);

                if (luisResult == null || luisResult.TopScoringIntent == null) return text;

                return luisResult.TopScoringIntent.Intent;
            }
            catch 
            {
                return text;
            }
        }
    }
}
