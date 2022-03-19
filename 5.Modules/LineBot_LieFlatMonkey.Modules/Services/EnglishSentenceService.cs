using LineBot_LieFlatMonkey.Assets.Constant;
using LineBot_LieFlatMonkey.Assets.Model.Resp;
using LineBot_LieFlatMonkey.Entities.Models;
using LineBot_LieFlatMonkey.Modules.Interfaces;
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

        public EnglishSentenceService(ITrackableRepository<EnglishSentence> englishSentenceRepo)
        {
            this.englishSentenceRepo = englishSentenceRepo;
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

            Random random = new Random(Guid.NewGuid().GetHashCode());

            return random.Next(1,totalCount + 1);
        }

        /// <summary>
        /// 取得英文句子英頻檔
        /// </summary>
        /// <param name="userId">使用者 Id</param>
        /// <returns></returns>
        public byte[] GetAudioByUserId(string userId)
        {
            // https://translate.google.com/translate_tts?ie=UTF-8&tl=zh_tw&client=tw-ob&ttsspeed=1&q=文字轉語音連結參考連結

            var audioPath = Path.Combine(
                    Environment.CurrentDirectory,
                    EnglishSenteceDirName.Domain, 
                    $"{userId}", 
                    EnglishSenteceFileNameType.Normal
                    );

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
                        EnglishSenteceDirName.Domain,
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
            using (var fileStream = new FileStream(audioPath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new BinaryReader(fileStream))
                {
                    var length = Convert.ToInt32(new FileInfo(audioPath).Length);

                    return reader.ReadBytes(length);
                }
            }
        }
    }
}
