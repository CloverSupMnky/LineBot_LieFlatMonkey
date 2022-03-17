using LineBot_LieFlatMonkey.Assets.Model.AppSetting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.WebHost.Filters
{
    /// <summary>
    /// 1. Using the HMAC-SHA256 algorithm with the channel secret as the secret key, compute the digest for the request body.
    /// 2. Confirm that the Base64-encoded digest matches the signature in the x-line-signature request header.
    /// </summary>
    public class VerifySignatureFilter : Attribute, IAsyncAuthorizationFilter
    {
        private readonly IOptions<LineBotSetting> lineBotSetting;

        public VerifySignatureFilter(
            IOptions<LineBotSetting> lineBotSetting)
        {
            this.lineBotSetting = lineBotSetting;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            context.HttpContext.Request.EnableBuffering();

            var req = context.HttpContext.Request;

            // 取得 Req Header 指定欄位值 X-Line-Signature
            if (!req.Headers.TryGetValue(
                "X-Line-Signature", out StringValues strValues))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var signature = strValues.FirstOrDefault();
            if (signature == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // 讀取 Req Body 資料
            StreamReader reader = new StreamReader(req.Body);

            var bodyStr = await reader.ReadToEndAsync();

            req.Body.Seek(0, SeekOrigin.Begin);

            var channelSecretBytes =
                Encoding.UTF8.GetBytes(this.lineBotSetting.Value.ChannelSecret);

            var bodyBytes = Encoding.UTF8.GetBytes(bodyStr);

            var checkSignature = Convert.ToBase64String(
                new HMACSHA256(channelSecretBytes).ComputeHash(bodyBytes));

            if (signature != checkSignature)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }
    }
}
