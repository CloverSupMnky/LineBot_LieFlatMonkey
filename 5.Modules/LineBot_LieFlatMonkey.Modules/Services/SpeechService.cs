using LineBot_LieFlatMonkey.Assets.Constant;
using LineBot_LieFlatMonkey.Assets.Model.AppSetting;
using LineBot_LieFlatMonkey.Modules.Interfaces;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Modules.Services
{
    /// <summary>
    /// 智能語音使用 Service
    /// </summary>
    public class SpeechService : ISpeechService
    {
        private readonly IOptions<SpeechSetting> speechSetting;

        public SpeechService(IOptions<SpeechSetting> speechSetting)
        {
            this.speechSetting = speechSetting;
        }

        /// <summary>
        /// 產生對應文字音檔並儲存對應使用者檔案路徑
        /// </summary>
        /// <param name="text">輸入文字</param>
        /// <param name="userId">使用者 Id</param>
        /// <returns></returns>
        public async Task<bool> GenAudioAndSave(string text, string userId)
        {
            // 設定語音服務設定
            var config = this.GetSpeechConfig();

            try 
            {
                var dirPath = Path.Combine(
                        Environment.CurrentDirectory,
                        EnglishSenteceDirName.Domain,
                        userId);

                // 資料夾不存在就新增
                if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);

                var audioPath = 
                    Path.Combine(dirPath,EnglishSenteceFileNameType.Normal);

                // 建立語音合成器類別
                using (var fileOutput = AudioConfig.FromWavFileOutput(audioPath))
                using (var synthesizer = new SpeechSynthesizer(config, fileOutput))
                {
                    // 將文字合成為語音
                    using (var result = await synthesizer.SpeakTextAsync(text))
                    {
                        return result.Reason == ResultReason.SynthesizingAudioCompleted;
                    }
                }
            }
            catch 
            {
                return false;
            }
        }

        /// <summary>
        /// 設定語音服務設定
        /// </summary>
        /// <returns></returns>
        private SpeechConfig GetSpeechConfig()
        {
            // 相關語音設定可參考 https://github.com/MicrosoftDocs/azure-docs.zh-tw/blob/master/articles/cognitive-services/Speech-Service/language-support.md#text-to-speech

            // 設定 訂用帳戶金鑰 與 訂用帳戶服務的所在區域
            var speechConfig = SpeechConfig.FromSubscription(
                this.speechSetting.Value.Subkey, this.speechSetting.Value.Region);

            // 要辨識的語言
            speechConfig.SpeechRecognitionLanguage = this.speechSetting.Value.Language;

            // 設定語音名稱
            speechConfig = this.SetVoiceName(speechConfig);

            return speechConfig;
        }

        /// <summary>
        /// 設定語音名稱
        /// </summary>
        /// <param name="speechConfig">語音服務設定</param>
        /// <returns></returns>
        private SpeechConfig SetVoiceName(SpeechConfig speechConfig)
        {
            // 分別為女性、男性聲音
            // en-US-JennyNeural、en-US-GuyNeural

            // 預設女性聲音
            speechConfig.SpeechSynthesisVoiceName = "en-US-JennyNeural";

            DateTime startTime = new DateTime(1970, 1, 1, 8, 0, 0);

            // 取得時間戳
            var timeStamp =
                Convert.ToInt32((DateTime.Now - startTime).TotalSeconds);

            // 時間戳若為奇數設為男性聲音
            if (timeStamp % 2 != 0)
            {
                speechConfig.SpeechSynthesisVoiceName = "en-US-GuyNeural";
            }

            return speechConfig;
        }
    }
}
