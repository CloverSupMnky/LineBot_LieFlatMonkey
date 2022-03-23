using LineBot_LieFlatMonkey.Assets.Model.Resp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Modules.Comparer
{
    public class SearchPttResComparer : IComparer<SearchPttResp>
    {
        public int Compare(SearchPttResp x, SearchPttResp y)
        {
            if (x == null || y == null) return 0;

            if (x.ThumbsUp == "爆") return -1;

            if (y.ThumbsUp == "爆") return 1;

            var xInt = Convert.ToInt32(x.ThumbsUp);
            var yInt = Convert.ToInt32(y.ThumbsUp);
            
            if (xInt > yInt) return -1;

            if (yInt > xInt) return 1;

            return 0;
        }
    }
}
