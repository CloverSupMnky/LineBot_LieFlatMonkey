using LineBot_LieFlatMonkey.Assets.Constant;
using LineBot_LieFlatMonkey.Assets.Model.Resp;
using LineBot_LieFlatMonkey.Entities.Models;
using LineBot_LieFlatMonkey.Modules.Interfaces;
using MediaToolkit;
using MediaToolkit.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URF.Core.Abstractions.Trackable;

namespace LineBot_LieFlatMonkey.Modules.Services
{
    /// <summary>
    /// 英文句子產出 Service
    /// </summary>
    public class EnglishSentenceService : IEnglishSentenceService
    {
        private readonly ITrackableRepository<EnglishSentence> englishSentenceRepo;
        private readonly ICommonService commonService;

        public EnglishSentenceService(
            ITrackableRepository<EnglishSentence> englishSentenceRepo,
            ICommonService commonService)
        {
            this.englishSentenceRepo = englishSentenceRepo;
            this.commonService = commonService;
        }

        /// <summary>
        /// 取得英文句子
        /// </summary>
        /// <returns></returns>
        public EnglishSentenceResp GetSentence()
        {
            var sentenceNo = this.GetSentenceNo();

            var englishSentence = this.englishSentenceRepo.Queryable()
                .FirstOrDefault(e => e.SeqNo == sentenceNo);

            var res = new EnglishSentenceResp();

            if(englishSentence != null)
            {
                res.Sentence = englishSentence.Sentence;
                res.Translation = englishSentence.Translation;
                res.Source = englishSentence.Source;
                res.SourceType = englishSentence.SourceType;
            }

            return res;
        }

        /// <summary>
        /// 隨機取得一個英文句子的編號
        /// </summary>
        /// <returns></returns>
        private int GetSentenceNo()
        {
           var totalCount = this.englishSentenceRepo.Queryable().Count();

            return this.commonService.GetRandomNo(totalCount);
        }

        /// <summary>
        /// 取得英文句子英頻檔
        /// </summary>
        /// <param name="replyToken">使用者 Id</param>
        /// <returns></returns>
        public byte[] GetAudioByReplyToken(string replyToken)
        {
            // https://translate.google.com/translate_tts?ie=UTF-8&tl=zh_tw&client=tw-ob&ttsspeed=1&q=文字轉語音連結參考連結

            var audioPath = Path.Combine(
                    Environment.CurrentDirectory,
                    DirName.Media,
                    replyToken, 
                    EnglishSenteceFileNameType.Normal
                    );

            if (!File.Exists(audioPath)) return this.GetNotFoundAudio();

            return this.GetAudioBytes(audioPath);
        }

        /// <summary>
        /// 取得無法正常取得使用者音頻檔提示音檔
        /// </summary>
        /// <returns></returns>
        public byte[] GetNotFoundAudio()
        {
            // https://translate.google.com/translate_tts?ie=UTF-8&tl=zh_tw&client=tw-ob&ttsspeed=1&q=文字轉語音連結參考連結

            var audioPath = Path.Combine(
                        Environment.CurrentDirectory,
                        DirName.Media,
                        EnglishSenteceFileNameType.NotFound
                        );

            return this.GetAudioBytes(audioPath);
        }

        /// <summary>
        /// 取得音頻檔 Byte 資料
        /// </summary>
        /// <param name="audioPath">音頻檔路徑</param>
        /// <returns></returns>
        private byte[] GetAudioBytes(string audioPath) 
        {
            var ffmpegFilePath = Path.Combine(
                Environment.CurrentDirectory, DirName.ToolKit, "ffmpeg.exe");

            var tempFilePath = Path.Combine(
                        Environment.CurrentDirectory,
                        DirName.Media,
                        EnglishSenteceFileNameType.TempAAC
                        );

            using (var engine = new Engine(ffmpegFilePath))
            {
                engine.Convert(new MediaFile(audioPath), new MediaFile(tempFilePath));
            }

            byte[] res = null;

            using (var fileStream = new FileStream(
                tempFilePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new BinaryReader(fileStream))
                {
                    var length = Convert.ToInt32(new FileInfo(tempFilePath).Length);

                    res = reader.ReadBytes(length);
                }
            }

            File.Delete(tempFilePath);

            return res;
        }
    }
}
