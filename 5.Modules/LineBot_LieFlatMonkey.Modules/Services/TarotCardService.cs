using LineBot_LieFlatMonkey.Assets.Constant;
using LineBot_LieFlatMonkey.Assets.Model.Resp;
using LineBot_LieFlatMonkey.Entities.Models;
using LineBot_LieFlatMonkey.Modules.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URF.Core.Abstractions.Trackable;

namespace LineBot_LieFlatMonkey.Modules.Services
{
    /// <summary>
    /// 塔羅牌相關功能 Service
    /// </summary>
    public class TarotCardService : ITarotCardService
    {
        private readonly ITrackableRepository<TarotCard> tarotCardRepo;

        /// <summary>
        /// 塔羅牌正、逆位判斷字典
        /// true - 正位
        /// false - 逆位
        /// </summary>
        private readonly Dictionary<bool, string> tarotCardFaceTypeDic;

        public TarotCardService(ITrackableRepository<TarotCard> tarotCardRepo)
        {
            this.tarotCardRepo = tarotCardRepo;

            tarotCardFaceTypeDic = new Dictionary<bool, string>()
            {
                { true,TarotCardFaceType.Up },
                { false,TarotCardFaceType.Rev }
            };
        }

        /// <summary>
        /// 依占卜類別進行占卜
        /// </summary>
        /// <param name="fortuneTellingType">塔羅牌占卜方式</param>
        /// <returns>FortuneTellingResp 塔羅牌占卜結果</returns>
        public FortuneTellingResp FortuneTellingByType(string fortuneTellingType)
        {
            FortuneTellingResp res = null;

            switch (fortuneTellingType)
            {
                // 每日運勢占卜
                case FortuneTellingType.Daily:
                    res = this.FortuneTellingDaily();
                    break;
                // 一般運勢占卜
                case FortuneTellingType.Normal:
                    res = this.FortuneTellingNormal();
                    break;
            }

            return res;
        }

        /// <summary>
        /// 一般運勢占卜
        /// </summary>
        /// <returns>FortuneTellingResp 塔羅牌占卜結果</returns>
        private FortuneTellingResp FortuneTellingNormal()
        {
            // 取得塔羅牌
            var tarotCardNo = this.GetTarotCardNo(FortuneTellingType.Normal);

            // 查詢對應塔羅牌資料
            var tarotCard = this.tarotCardRepo.Queryable()
                .FirstOrDefault(t => t.SeqNo == Math.Abs(tarotCardNo));

            // 查無就返回
            if (tarotCard == null) return null;

            return new FortuneTellingResp
            {
                FaceType = this.tarotCardFaceTypeDic[tarotCardNo > 0],
                Name = tarotCard.Name,
                NameEn = tarotCard.NameEn,
                Mean = tarotCard.Mean,
                Desc = tarotCardNo < 0 
                ? tarotCard.DescRev 
                : tarotCard.DescUp,
                MeanWord = tarotCardNo < 0 
                ? tarotCard.MeanRev 
                : tarotCard.MeanUp,
                ImageUrl = tarotCardNo < 0 
                ? tarotCard.ImageUrlRev 
                : tarotCard.ImageUrlUp
            };
        }

        /// <summary>
        /// 每日運勢占卜
        /// </summary>
        /// <returns>FortuneTellingResp 塔羅牌占卜結果</returns>
        private FortuneTellingResp FortuneTellingDaily() 
        {
            // 取得塔羅牌
            var tarotCardNo = this.GetTarotCardNo(FortuneTellingType.Daily);

            // 查詢對應塔羅牌資料
            var tarotCard = this.tarotCardRepo.Queryable()
                .FirstOrDefault(t => t.SeqNo == tarotCardNo);

            // 查無就返回
            if(tarotCard == null) return null;

            return new FortuneTellingResp
            {
                FaceType = this.tarotCardFaceTypeDic[tarotCardNo > 0],
                Name = tarotCard.Name,
                NameEn = tarotCard.NameEn,
                Mean = tarotCard.Mean,
                Desc = tarotCard.DescDaily,
                MeanWord = tarotCard.MeanUp,
                ImageUrl = tarotCard.ImageUrlUp
            };
        }

        /// <summary>
        /// 取得塔羅牌對應牌號
        /// </summary>
        /// <param name="fortuneTellingType">塔羅牌占卜方式</param>
        /// <returns></returns>
        private int GetTarotCardNo(string fortuneTellingType)
        {
            // 以 Guid 的 HashCode 作為亂數種子
            Random rnd = new Random(Guid.NewGuid().GetHashCode());

            // 取得編碼 1-78 任一數字
            var cardNo = rnd.Next(1, 79);

            // 若為每日運勢不執行轉換負號邏輯
            if (fortuneTellingType == FortuneTellingType.Daily) 
                return cardNo;

            DateTime startTime = new DateTime(1970, 1, 1, 8, 0, 0);

            // 取得時間戳
            var timeStamp = 
                Convert.ToInt32((DateTime.Now - startTime).TotalSeconds);

            // 時間戳若為偶數設為負數
            if(timeStamp % 2 == 0)
            {
                cardNo *= -1;
            }

            return cardNo;
        }
    }
}
