using LineBot_LieFlatMonkey.Assets.Constant;
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
    /// 共用方法 Service
    /// </summary>
    public class CommonService : ICommonService
    {
        private readonly ITrackableRepository<QuickReply> quickReplyRepo;

        public CommonService(ITrackableRepository<QuickReply> quickReplyRepo)
        {
            this.quickReplyRepo = quickReplyRepo;
        }

        /// <summary>
        /// 取得亂數值
        /// </summary>
        /// <param name="maxLength">範圍最大值</param>
        /// <returns></returns>
        public int GetRandomNo(int maxLength)
        {
            // 以 Guid 的 HashCode 作為亂數種子
            Random random = new Random(Guid.NewGuid().GetHashCode());

            return random.Next(1, maxLength + 1);
        }

        /// <summary>
        /// 依模板檔案名稱取得對應模板 Json 字串
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetMessageTemplateByName(string name)
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, DirName.Template, name);

            if (!File.Exists(filePath)) return string.Empty;

            string res = string.Empty;
            using (var streamReader = new StreamReader(filePath, Encoding.UTF8))
            {
                res = await streamReader.ReadToEndAsync();
            }

            return res;
        }

        /// <summary>
        /// 依類型取得 QuickReply 資料
        /// </summary>
        /// <param name="type">QuickReply 類型</param>
        /// <returns></returns>
        public List<QuickReply> GetQuickReplyByType(string type)
        {
            return this.quickReplyRepo.Queryable()
                .Where(q => q.ItemType == type)
                .ToList();
        }
    }
}
