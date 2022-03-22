using LineBot_LieFlatMonkey.Assets.Constant;
using LineBot_LieFlatMonkey.Assets.Model;
using LineBot_LieFlatMonkey.Assets.Model.Resp;
using LineBot_LieFlatMonkey.Modules.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Modules.Services
{
    /// <summary>
    /// 音樂推薦 Service
    /// </summary>
    public class MusicRecommandService : IMusicRecommandService
    {
        private readonly IHttpClientService httpClientService;
        private readonly ICommonService commonService;

        /// <summary>
        /// 曲風判斷字典
        /// 1-華語
        /// 2-西洋
        /// 3-日語
        /// 4-韓語
        /// 5-台語
        /// </summary>
        private readonly Dictionary<int, string> musicCateTypeDic;
        private readonly Dictionary<int, string> musicCateStrTypeDic;


        public MusicRecommandService(
            IHttpClientService httpClientService, 
            ICommonService commonService)
        {
            this.httpClientService = httpClientService;
            this.commonService = commonService;

            musicCateTypeDic = new Dictionary<int, string>()
            {
                {1,MusicCateType.Chinese},
                {2,MusicCateType.Western},
                {3,MusicCateType.Japan},
                {4,MusicCateType.Korea},
                {5,MusicCateType.Taiwanese}
            };

            musicCateStrTypeDic = new Dictionary<int, string>()
            {
                {1,MusicCateStrType.Chinese},
                {2,MusicCateStrType.Western},
                {3,MusicCateStrType.Japan},
                {4,MusicCateStrType.Korea},
                {5,MusicCateStrType.Taiwanese}
            };
        }

        /// <summary>
        /// 推薦音樂
        /// </summary>
        public async Task<MusicRecommandResp> Recommand()
        {
            var no = this.commonService.GetRandomNo(this.musicCateTypeDic.Count);

            var musicCate = this.musicCateTypeDic[no];

            List<Song> songList = 
                await this.httpClientService.GetSongInfoByMusicCateType(musicCate);

            Song song = new Song();
            //MusicRecommandVideoInfo videoInfo = new MusicRecommandVideoInfo();
            if (songList.Count > 0)
            {
                song = songList[this.commonService.GetRandomNo(songList.Count) - 1];
                // videoInfo = this.webDriverService.GetVideoInfoByMusicInfo(musicInfo);
            }

            var res = new MusicRecommandResp();

            res.Artist = song.artist_name;
            res.Song = song.song_name;
            //res.VideoUrl = videoInfo.VideoUrl;
            //res.ImageUrl = videoInfo.ImageUrl;
            res.SongType = this.musicCateStrTypeDic[no];

            return res;
        }
    }
}
