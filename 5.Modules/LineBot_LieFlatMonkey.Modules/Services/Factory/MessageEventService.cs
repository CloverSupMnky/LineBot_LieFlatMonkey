using LineBot_LieFlatMonkey.Assets.Constant;
using LineBot_LieFlatMonkey.Assets.Model.AppSetting;
using LineBot_LieFlatMonkey.Assets.Model.LineBot;
using LineBot_LieFlatMonkey.Assets.Model.Resp;
using LineBot_LieFlatMonkey.Modules.Interfaces;
using LineBot_LieFlatMonkey.Modules.Interfaces.Factory;
using Microsoft.Extensions.Options;
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
        private readonly IOptions<ApiDomainSetting> apiDomainSetting;
        private readonly ICommonService commonService;
        private readonly ILUISService luisService;

        public MessageEventService(
            IHttpClientService httpClientService,
            ITarotCardService tarotCardService,
            IEnglishSentenceService englishSentenceService,
            ISpeechService speechService,
            IOptions<ApiDomainSetting> apiDomainSetting,
            ICommonService commonService,
            ILUISService luisService)
        {
            this.httpClientService = httpClientService;
            this.tarotCardService = tarotCardService;
            this.englishSentenceService = englishSentenceService;
            this.speechService = speechService;
            this.apiDomainSetting = apiDomainSetting;
            this.commonService = commonService;
            this.luisService = luisService;
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
                case MessageType.Location:
                    await this.LocationMessage(eventInfo.Message, eventInfo.ReplyToken);
                    break;
            }
        }

        /// <summary>
        /// 回傳地圖探索功能的 QuickReply
        /// </summary>
        /// <param name="message">Line Bot Message 物件</param>
        /// <param name="replyToken">回覆訊息的 replyToken</param>
        /// <returns></returns>
        private async Task LocationMessage(Message message, string replyToken)
        {
            var quickReply = new QuickReplyMessage()
            {
                Items = this.GetLocationReplyItems(message.Latitude,message.Longitude)
            };

            var messages = new List<ResultMessage>()
            {
                new TextResultMessage(){ Text = "請選擇要探索的項目" , QuickReply = quickReply}
            };

            await this.httpClientService.ReplyMessageAsync(messages, replyToken);
        }

        /// <summary>
        /// 取得 QuickReply 內容項目
        /// </summary>
        /// <param name="latitude">緯度</param>
        /// <param name="longitude">經度</param>
        /// <returns></returns>
        private List<QuickReplyItem> GetLocationReplyItems(string latitude, string longitude) 
        {
            var res = new List<QuickReplyItem>();

            var quickItems = this.commonService.GetQuickReplyByType(QuickReplyType.SearchMap);

            foreach (var item in quickItems) 
            {
                res.Add(new QuickReplyItem
                {
                    Action = new QuickReplyAction()
                    {
                        Type = ActionType.Postback,
                        Label = item.Description,
                        Data = $"{QueryStringPropertyType.Type}={QuickReplyType.SearchMap}&{QueryStringPropertyType.Word}={item.ItemValue}&{QueryStringPropertyType.Latitude}={latitude}&{QueryStringPropertyType.Longitude}={longitude}"
                    },
                    ImageUrl = item.ImageUrl
                });
            }

            return res;
        }

        /// <summary>
        /// 文字訊息
        /// </summary>
        /// <param name="message">Line Bot Message 物件</param>
        /// <param name="replyToken">回覆訊息的 replyToken</param>
        private async Task TextMessage(Message message,string replyToken)
        {
            List<ResultMessage> messages = null;

            // 意圖判斷
            var intent = await this.luisService.GetIntent(message.Text);

            switch (intent)
            {
                case TextMessageType.TarotCardDaily:
                    messages = await this.TarotCardMessage();
                    break;
                case TextMessageType.TarotCardNormal:
                    messages = this.GetTarotCardNormalQuickReply();
                    break;
                case TextMessageType.EnglishStence:
                    messages = await this.GetEnglishSentence(replyToken);
                    break;
                case TextMessageType.MusicRecommand:
                    messages = this.GetMusicRecommandQuickReply();
                    break;
                case TextMessageType.ArticleRecommand:
                    messages = this.GetArticleRecommandQuickReply();
                    break;
                default:
                    messages = await this.GetCommandMessage();
                    break;
            }

            if(messages == null) 
            {
                await this.commonService.ReplyErrorMessage(SystemMessageType.NoData, replyToken);

                return;
            }

            await this.httpClientService.ReplyMessageAsync(messages,replyToken);
        }

        /// <summary>
        /// 取得指令列訊息
        /// </summary>
        /// <returns></returns>
        private async Task<List<ResultMessage>> GetCommandMessage()
        {
            string jsonString =
                await this.commonService.GetMessageTemplateByName("CommandTemplate.json");

            if (string.IsNullOrEmpty(jsonString)) return null;

            var obj = JsonConvert.DeserializeObject<object>(jsonString);

            return new List<ResultMessage>()
            {
                new FlexResultMessage(){ Contents = obj ,AltText = "指令列"},
                new StickerResultMessage(){ StickerId = "11087931", PackageId = "6362"}
            };
        }

        /// <summary>
        /// 塔羅牌訊息處理
        /// </summary>
        /// <returns></returns>
        private async Task<List<ResultMessage>> TarotCardMessage()
        {
            var tarotCard =
                this.tarotCardService.FortuneTellingByType(FortuneTellingType.Daily);

            string jsonString =
                await this.commonService.GetMessageTemplateByName("TarotCardTemplate.json");

            if (string.IsNullOrEmpty(jsonString)) return null;

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
                new FlexResultMessage(){ Contents = obj ,AltText = "運勢占卜結果"}
            };
        }

        /// <summary>
        /// 取得塔羅牌直覺運勢占卜 QuockReply 訊息
        /// </summary>
        /// <returns></returns>
        private List<ResultMessage> GetTarotCardNormalQuickReply() 
        {
            var quickReply = new QuickReplyMessage()
            {
                Items = this.GetTarotCardQuickReplyItems()
            };

            return new List<ResultMessage>()
            {
                new TextResultMessage(){ Text = "請想著你的問題，然後點選抽牌" , QuickReply = quickReply}
            };
        }

        /// <summary>
        /// 取得塔羅牌直覺運勢占卜 QuockReply 訊息內容模板
        /// </summary>
        /// <returns></returns>
        private List<QuickReplyItem> GetTarotCardQuickReplyItems()
        {
            var res = new List<QuickReplyItem>();

            var quickItems = this.commonService.GetQuickReplyByType(QuickReplyType.TarotCard);

            foreach (var item in quickItems)
            {
                res.Add(new QuickReplyItem
                {
                    Action = new QuickReplyAction()
                    {
                        Type = ActionType.Postback,
                        Label = item.Description,
                        Data = $"{QueryStringPropertyType.Type}={QuickReplyType.TarotCard}&{QueryStringPropertyType.Word}={item.ItemValue}"
                    },
                    ImageUrl = item.ImageUrl
                });
            }

            return res;
        }

        /// <summary>
        /// 取得英文句子
        /// </summary>
        /// <param name="replyToken">回覆訊息的 replyToken</param>
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
        /// <param name="replyToken">回覆訊息的 replyToken</param>
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
        /// <param name="replyToken">回覆訊息的 replyToken</param>
        /// <returns></returns>
        private AudioResultMessage GetSuccessAudioMessage(string replyToken)
        {
            var audioPath = Path.Combine(
                    Environment.CurrentDirectory,
                    DirName.Media,
                    replyToken,
                    EnglishSenteceFileNameType.Normal
                    );

            var apiUrl = $"{this.apiDomainSetting.Value.Product}/api/EnglishSentence/GetAudioByReplyToken/";

#if DEBUG
            apiUrl = $"{this.apiDomainSetting.Value.Develop}/api/EnglishSentence/GetAudioByReplyToken/";
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

            var apiUrl = @$"{this.apiDomainSetting.Value.Product}/api/EnglishSentence/GetNotFoundAudio";

#if DEBUG
            apiUrl = @$"{this.apiDomainSetting.Value.Develop}/api/EnglishSentence/GetNotFoundAudio";
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
            string jsonString = 
                await this.commonService.GetMessageTemplateByName("EnglishSentenceTemplate.json");

            if (string.IsNullOrEmpty(jsonString)) return null;

            jsonString = jsonString.Replace("{#Sentence}", sentenceResp.Sentence);
            jsonString = jsonString.Replace("{#Translation}", sentenceResp.Translation);
            jsonString = jsonString.Replace("{#Source}", sentenceResp.Source);
            jsonString = jsonString.Replace("{#SourceType}", sentenceResp.SourceType);

            var obj = JsonConvert.DeserializeObject<object>(jsonString);

            return new FlexResultMessage() { Contents = obj, AltText = "英文佳句" };
        }

        /// <summary>
        /// 取得音樂推薦模板訊息
        /// 取得音樂推薦 QuickReply 訊息
        /// </summary>
        /// <param name="replyToken">回覆訊息的 replyToken</param>
        /// <returns></returns>
        private List<ResultMessage> GetMusicRecommandQuickReply()
        {
            var quickReply = new QuickReplyMessage()
            {
                Items = this.GetMusicRecommandQuickReplyItems()
            };

            return new List<ResultMessage>()
            {
                new TextResultMessage(){ Text = "請選擇曲風" , QuickReply = quickReply}
            };
        }

        /// <summary>
        /// 取得音樂推薦 QuickReply 訊息內容模板
        /// </summary>
        /// <returns></returns>
        private List<QuickReplyItem> GetMusicRecommandQuickReplyItems()
        {
            var res = new List<QuickReplyItem>();

            var quickItems = this.commonService.GetQuickReplyByType(QuickReplyType.MusicRecommand);

            foreach (var item in quickItems)
            {
                res.Add(new QuickReplyItem
                {
                    Action = new QuickReplyAction()
                    {
                        Type = ActionType.Postback,
                        Label = item.Description,
                        Data = $"{QueryStringPropertyType.Type}={QuickReplyType.MusicRecommand}&{QueryStringPropertyType.Word}={item.ItemValue}"
                    },
                    ImageUrl = item.ImageUrl
                });
            }

            return res;
        }

        /// <summary>
        /// 取得熱門文章推薦 QuickReply 項目
        /// </summary>
        /// <param name="replyToken">回覆訊息的 replyToken</param>
        /// <returns></returns>
        private List<ResultMessage> GetArticleRecommandQuickReply()
        {
            var quickReply = new QuickReplyMessage()
            {
                Items = this.GetArticleRecommandQuickReplyItems()
            };

            return new List<ResultMessage>()
            {
                new TextResultMessage(){ Text = "請選擇 PTT 看板" , QuickReply = quickReply}
            };
        }

        /// <summary>
        /// 取得熱門文章推薦 QuickReply 內容項目
        /// </summary>
        /// <returns></returns>
        private List<QuickReplyItem> GetArticleRecommandQuickReplyItems()
        {
            var res = new List<QuickReplyItem>();

            var quickItems = this.commonService.GetQuickReplyByType(QuickReplyType.SearchPTT);

            foreach (var item in quickItems)
            {
                res.Add(new QuickReplyItem
                {
                    Action = new QuickReplyAction()
                    {
                        Type = ActionType.Postback,
                        Label = item.Description,
                        Data = $"{QueryStringPropertyType.Type}={QuickReplyType.SearchPTT}&{QueryStringPropertyType.Word}={item.ItemValue}"
                    },
                    ImageUrl = item.ImageUrl
                });
            }

            return res;
        }
    }
}
