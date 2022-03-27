using LineBot_LieFlatMonkey.Modules.Interfaces;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Modules.Services
{
    /// <summary>
    /// LUIS 意圖判斷 Service
    /// </summary>
    public class LUISService : ILUISService
    {
        private readonly IHttpClientService httpClientService;

        public LUISService(IHttpClientService httpClientService)
        {
            this.httpClientService = httpClientService;
        }

        /// <summary>
        /// 取得意圖判斷結果
        /// </summary>
        /// <param name="text">需要意圖判斷的文字</param>
        /// <returns></returns>
        public async Task<string> GetIntent(string text)
        {
           return await this.httpClientService.GetIntentAsync(text);
        }
    }
}
