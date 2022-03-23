﻿using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using LineBot_LieFlatMonkey.Assets.Constant;
using LineBot_LieFlatMonkey.Assets.Model;
using LineBot_LieFlatMonkey.Assets.Model.AppSetting;
using LineBot_LieFlatMonkey.Assets.Model.Resp;
using LineBot_LieFlatMonkey.Modules.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
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
        private readonly IOptions<GoogleDriverSetting> googleDriverSetting;

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
            ICommonService commonService,
            IOptions<GoogleDriverSetting> googleDriverSetting)
        {
            this.httpClientService = httpClientService;
            this.commonService = commonService;
            this.googleDriverSetting = googleDriverSetting;

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

            Song song = null;
            SearchResult video = null;
            if (songList.Count > 0)
            {
                song = songList[this.commonService.GetRandomNo(songList.Count) - 1];
                video = await this.GetYTInfo(song);
            }

            var res = new MusicRecommandResp();

            if (song != null) 
            {
                res.Artist = song.artist_name;
                res.Song = song.song_name;
            }

            if (video != null && 
                video.Id != null && 
                video.Snippet != null && 
                video.Snippet.Thumbnails != null &&
                video.Snippet.Thumbnails.Medium != null)
            {
                res.VideoUrl = $"https://www.youtube.com/watch?v={video.Id.VideoId}";
                res.ImageUrl = video.Snippet.Thumbnails.Medium.Url;
            }

            res.SongType = this.musicCateStrTypeDic[no];

            return res;
        }

        /// <summary>
        /// 依音樂資訊取得 YT 影片資訊
        /// </summary>
        /// <param name="song"></param>
        /// <returns></returns>
        private async Task<SearchResult> GetYTInfo(Song song) 
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = this.googleDriverSetting.Value.YTAccessToKen,
                ApplicationName = this.GetType().ToString()
            });

            var searchListRequest = youtubeService.Search.List("snippet");

            // 限制歌名在 10 字內
            if(song.song_name.Length > 10) 
            {
                song.song_name = song.song_name.Substring(0, 10);
            }

            // 設定查詢值
            searchListRequest.Q = $"{song.artist_name}-{song.song_name}";

            // 取得 1 筆結果
            searchListRequest.MaxResults = 1;

            SearchListResponse searchListResponse = 
                await searchListRequest.ExecuteAsync();

            return searchListResponse.Items.FirstOrDefault();
        }
    }
}
