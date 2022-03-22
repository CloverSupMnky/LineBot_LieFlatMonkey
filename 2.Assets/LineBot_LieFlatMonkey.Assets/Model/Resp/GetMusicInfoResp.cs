using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Assets.Model.Resp
{
    /// <summary>
    /// KKBox 音樂排行榜資訊
    /// </summary>
    public class GetMusicInfoResp
    {
        public string code { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public string date { get; set; }
        public Charts charts { get; set; }
        public Playlist_Id playlist_id { get; set; }
    }

    public class Charts
    {
        public List<Song> song { get; set; }
    }

    public class Song
    {
        public Rankings rankings { get; set; }
        public string type { get; set; }
        public string song_id { get; set; }
        public bool is_auth { get; set; }
        public string song_name { get; set; }
        public string artist_name { get; set; }
        public string album_name { get; set; }
        public Cover_Image cover_image { get; set; }
        public string song_url { get; set; }
        public string artist_url { get; set; }
        public string album_url { get; set; }
        public bool is_artist_va { get; set; }
        public int release_date { get; set; }
        public int status { get; set; }
    }

    public class Rankings
    {
        public int this_period { get; set; }
        public int? last_period { get; set; }
    }

    public class Cover_Image
    {
        public string normal { get; set; }
        public string small { get; set; }
    }

    public class Playlist_Id
    {
        public string song { get; set; }
    }
}
