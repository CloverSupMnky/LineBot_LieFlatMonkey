using LineBot_LieFlatMonkey.Assets.Constant;
using LineBot_LieFlatMonkey.Assets.Model.LineBot;
using LineBot_LieFlatMonkey.Assets.Model.Resp;
using LineBot_LieFlatMonkey.Modules.Interfaces;
using LineBot_LieFlatMonkey.Modules.Interfaces.Factory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Modules.Services.Factory
{
    /// <summary>
    /// Line Bot 訊息事件處理 Service
    /// </summary>
    public class MessageEventService : IEventFactoryService
    {
        private readonly IHttpClientService httpClientService;
        private readonly ITarotCardService tarotCardService;
        private readonly IEnglishSentenceService englishSentenceService;
        private readonly ISpeechService speechService;

        public MessageEventService(
            IHttpClientService httpClientService,
            ITarotCardService tarotCardService,
            IEnglishSentenceService englishSentenceService,
            ISpeechService speechService)
        {
            this.httpClientService = httpClientService;
            this.tarotCardService = tarotCardService;
            this.englishSentenceService = englishSentenceService;
            this.speechService = speechService;
        }

        /// <summary>
        /// 處理訊息
        /// </summary>
        /// <param name="eventInfo">Line Bot Event 物件</param>
        public async Task Invoke(Event eventInfo)
        {
            switch (eventInfo.Message.Type) 
            {
                case MessageType.Text:
                    await this.TextMessage(eventInfo.Message,eventInfo.ReplyToken);
                    break;
            }
        }

        /// <summary>
        /// 文字訊息
        /// </summary>
        /// <param name="message">Line Bot Message 物件</param>
        private async Task TextMessage(Message message,string replyToken)
        {
            List<ResultMessage> messages = null;

            switch (message.Text)
            {
                case TextMessageType.TarotCardDaily:
                    messages = await this.TarotCardMessage(FortuneTellingType.Daily);
                    break;
                case TextMessageType.TarotCardNormal:
                    messages = await this.TarotCardMessage(FortuneTellingType.Normal);
                    break;
                case TextMessageType.EnglishStence:
                    messages = await this.GetEnglishSentence(replyToken);
                    break;
            }

            if(messages == null) 
            {
                // TODO 錯誤處理
                return;
            }

            await this.httpClientService.ReplyMessageAsync(messages,replyToken);
        }

        /// <summary>
        /// 塔羅牌訊息處理
        /// </summary>
        /// <param name="fortuneTellingType">塔羅牌占卜方式</param>
        /// <returns></returns>
        private async Task<List<ResultMessage>> TarotCardMessage(string fortuneTellingType) 
        {
            var filePath = Path.Combine(
                Environment.CurrentDirectory, DirName.Template, "TarotCardTemplate.json");

            if (!File.Exists(filePath)) return null;

            var tarotCard =
                this.tarotCardService.FortuneTellingByType(fortuneTellingType);

            string jsonString = string.Empty;
            using (var streamReader = new StreamReader(filePath,Encoding.UTF8))
            {
                jsonString = await streamReader.ReadToEndAsync();
            }

            jsonString = jsonString.Replace("{#ImageUrl}", tarotCard.ImageUrl);
            jsonString = jsonString.Replace("{#Name}", tarotCard.Name);
            jsonString = jsonString.Replace("{#NameEN}", tarotCard.NameEn);
            jsonString = jsonString.Replace("{#FaceType}", tarotCard.FaceType);
            jsonString = jsonString.Replace("{#Mean}", tarotCard.Mean);
            jsonString = jsonString.Replace("{#MeanWord}", tarotCard.MeanWord);
            jsonString = jsonString.Replace("{#Desc}", tarotCard.Desc);

            var obj = JsonConvert.DeserializeObject<object>(jsonString);

            return new List<ResultMessage>()
            {
                new TextResultMessage(){ Text = "運勢占卜結果"},
                new FlexResultMessage(){ Contents = obj ,AltText = "運勢占卜結果"},
                new StickerResultMessage(){ StickerId = "16581294", PackageId = "8525"}
            };
        }

        /// <summary>
        /// 取得英文句子
        /// </summary>
        /// <param name="replyToken">回覆訊息的 replyToken </param>
        /// <returns></returns>
        private async Task<List<ResultMessage>> GetEnglishSentence(string replyToken)
        {
            // 取得英文語句
            var sentence = this.englishSentenceService.GetSentence();

            // 取得英文語句模板訊息
            var flexMessage = await this.GetEnglishSentenceFlexMessage(sentence);

            // 取得英文語句音頻訊息
            var audioMessage = 
                await this.GetEnglishSentenceAudioMessage(sentence.Sentence,replyToken);

            return new List<ResultMessage>()
            {
                flexMessage,
                audioMessage
            };
        }

        /// <summary>
        /// 取得英文語句音頻訊息
        /// </summary>
        /// <param name="sentenceText">英文語句</param>
        /// <param name="replyToken">回覆訊息的 replyToken </param>
        /// <returns></returns>
        private async Task<AudioResultMessage> GetEnglishSentenceAudioMessage(
            string sentenceText, string replyToken)
        {
            if (string.IsNullOrEmpty(sentenceText)) 
            {
                return this.GetErrorAudioMessage();
            }

            var genRes = await this.speechService.GenAudioAndSave(sentenceText, replyToken);
            if(!genRes) 
            {
                return this.GetErrorAudioMessage();
            }

            return this.GetSuccessAudioMessage(replyToken);
        }

        /// <summary>
        /// 取得成功音頻訊息
        /// </summary>
        /// <param name="replyToken">回覆訊息的 replyToken </param>
        /// <returns></returns>
        private AudioResultMessage GetSuccessAudioMessage(string replyToken)
        {
            var audioPath = Path.Combine(
                    Environment.CurrentDirectory,
                    DirName.Media,
                    replyToken,
                    EnglishSenteceFileNameType.Normal
                    );

            var apiUrl = @"https://linebotlieflatmonkey.herokuapp.com/api/EnglishSentence/GetAudioByReplyToken/";

#if DEBUG
            apiUrl = @"https://localhost:44346/api/EnglishSentence/GetAudioByReplyToken/";
#endif

            return this.GetAudioResultMessage(
                $"{apiUrl}{replyToken}",
                this.speechService.GetAudioLength(audioPath));
        }

        /// <summary>
        /// 取得失敗音頻訊息
        /// </summary>
        /// <returns></returns>
        private AudioResultMessage GetErrorAudioMessage()
        {
            var audioPath = Path.Combine(
                        Environment.CurrentDirectory,
                        DirName.Media,
                        EnglishSenteceFileNameType.NotFound
                        );

            var apiUrl = @"https://linebotlieflatmonkey.herokuapp.com/api/EnglishSentence/GetNotFoundAudio";

#if DEBUG
            apiUrl = @"https://localhost:44346/api/EnglishSentence/GetNotFoundAudio";
#endif

            return this.GetAudioResultMessage(
                apiUrl,
                this.speechService.GetAudioLength(audioPath));
        }

        /// <summary>
        /// 取得音頻訊息
        /// </summary>
        /// <param name="url">檔案連結網址</param>
        /// <param name="duration">檔案播放時間長度</param>
        /// <returns></returns>
        private AudioResultMessage GetAudioResultMessage(string url,int duration) 
        {
            return new AudioResultMessage()
            {
                OriginalContentUrl = url,
                Duration = duration
            };
        }

        /// <summary>
        /// 取得英文語句模板訊息
        /// </summary>
        /// <param name="sentenceResp">英文語句取得結果</param>
        /// <returns></returns>
        private async Task<FlexResultMessage> GetEnglishSentenceFlexMessage(
            EnglishSentenceResp sentenceResp)
        {
            var filePath = Path.Combine(
                Environment.CurrentDirectory, DirName.Template, "EnglishSentenceTemplate.json");

            if (!File.Exists(filePath)) return null;

            string jsonString = string.Empty;
            using (var streamReader = new StreamReader(filePath, Encoding.UTF8))
            {
                jsonString = await streamReader.ReadToEndAsync();
            }

            jsonString = jsonString.Replace("{#Sentence}", sentenceResp.Sentence);
            jsonString = jsonString.Replace("{#Translation}", sentenceResp.Translation);
            jsonString = jsonString.Replace("{#Source}", sentenceResp.Source);
            jsonString = jsonString.Replace("{#SourceType}", sentenceResp.SourceType);

            var obj = JsonConvert.DeserializeObject<object>(jsonString);

            return new FlexResultMessage() { Contents = obj, AltText = "雞湯來啦~" };
        }
    }
}
