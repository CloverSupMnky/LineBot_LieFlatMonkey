using LineBot_LieFlatMonkey.Assets.Model;
using LineBot_LieFlatMonkey.Modules.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Modules.Services
{
    public class WebDriverService : IWebDriverService
    {
        /// <summary>
        /// 依曲風類型取得音樂推薦列表
        /// </summary>
        /// <param name="musicCate">曲風類型</param>
        /// <returns></returns>
        public List<MusicRecommandMusicInfo> GetMusicListByMusicCateType(string musicCate)
        {
            string url = $"https://kma.kkbox.com/charts/daily/song?cate={musicCate}&lang=tc&terr=tw";

            var res = new List<MusicRecommandMusicInfo>();

            try 
            {
                //using (IWebDriver driver = new ChromeDriver(Path.Combine(Environment.CurrentDirectory, "WebDriver")))
                //{
                //    driver.Navigate().GoToUrl(url);

                //    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

                //    var divElements = driver
                //        .FindElements(By.CssSelector("div.flipper-item.layer1 > span.charts-list-desc"));

                //    string song = string.Empty;
                //    var artist = string.Empty;
                //    foreach (var divElement in divElements)
                //    {
                //        song = divElement.FindElement(By.ClassName("charts-list-song")).Text;

                //        artist = divElement.FindElement(By.ClassName("charts-list-artist")).Text;

                //        res.Add(new MusicRecommandMusicInfo() { Song = song, Artist = artist });
                //    }

                //    driver.Quit();
                //}

                return res;
            }
            catch 
            {
                return res;
            }
        }

        /// <summary>
        /// 依音樂資訊取得影片資訊
        /// </summary>
        /// <param name="info">音樂資訊</param>
        public MusicRecommandVideoInfo GetVideoInfoByMusicInfo(MusicRecommandMusicInfo info)
        {
            string url = $"https://www.youtube.com/results?search_query={info.Song}-{info.Artist}";

            var res = new MusicRecommandVideoInfo();

            try
            {
                //using (IWebDriver driver = new ChromeDriver(Path.Combine(Environment.CurrentDirectory, "WebDriver")))
                //{
                //    driver.Navigate().GoToUrl(url);

                //    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

                //    var ytdElement = driver
                //        .FindElements(By.CssSelector("ytd-video-renderer.style-scope.ytd-item-section-renderer > div > ytd-thumbnail")).FirstOrDefault();

                //    if(ytdElement != null) 
                //    {
                //        var aElement = ytdElement.FindElement(By.TagName("a"));
                //        res.VideoUrl = $"https://www.youtube.com{aElement.GetDomAttribute("href")}";

                //        var imgElement = ytdElement.FindElement(By.TagName("img"));
                //        res.ImageUrl = imgElement.GetDomAttribute("src");
                //    }

                //    driver.Quit();
                //}

                return res;
            }
            catch
            {
                return res;
            }
        }
    }
}
