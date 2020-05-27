using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Solar
{
    public partial class SolarTrackers
    {
        protected List<string> Istring(string macname, string backstring)
        {
            int a = macname.Length;
            List<string> list = new List<string>();
            list.Add(backstring.Substring(1, a));
            list.Add(backstring.Substring(a + 5, 10));
            for (int i = a + 11; i + a < backstring.Length; a += 4)
            {
                list.Add(backstring.Substring(i + a, 4));
            }
            list[1] = "20" + list[1].Substring(8, 2) + "-" + list[1].Substring(6, 2) + "-" + list[1].Substring(4, 2) + " " + list[1].Substring(2, 2) + ":" + list[1].Substring(0, 2);
            return list;
        }

        protected List<double> Workdouble(List<string> list)
        {
            List<double> vs = new List<double>();
            for (int i = 2; i < 8; i++)
            {
                if (list[i].Substring(0, 2) != "80")
                    vs.Add((Convert.ToInt32(list[i], 16) * 0.1));
                else
                    vs.Add((Convert.ToInt32(list[i].Substring(2, 2), 16) * -0.1));
            }
            vs.Add((Convert.ToInt32(list[8], 16) * 0.01));
            for (int i = 9; i < 13; i++)
            {
                vs.Add((Convert.ToInt32(list[i], 16) * 0.1));
            }
            for (int i = 13; i < 24; i++)
            {
                vs.Add(Convert.ToInt32(list[i], 16));
            }
            vs[13] = vs[13] * 0.1 + 1;
            for (int i = 24; i < 28; i++)
            {
                vs.Add((Convert.ToInt32(list[i], 16) * 0.1));
            }
            for (int i = 28; i < 45; i++)
            {
                vs.Add(Convert.ToInt32(list[i], 16));
            }
            return vs;
        }

        protected void Workstring(List<string> _list, List<double> vs)
        {
            for (int i = 0; i < 26; i++)
                _list[i + 2] = vs[i].ToString("f2");
            for (int i = 26; i < vs.Count; i++)
                _list[i + 2] = vs[i].ToString();

            if (vs[21] > 360)
                vs[21] -= (int)vs[21] / 360 * 360;

            if (vs[21] == 0)
                _list[23] = "正北";
            else if (vs[21] < 90)
                _list[23] = "东北 " + vs[21].ToString() + "°";
            else if (vs[21] == 90)
                _list[23] = "正东";
            else if (vs[21] > 90 && vs[21] < 180)
                _list[23] = "东南 " + (vs[21] - 90).ToString() + "°";
            else if (vs[21] == 180)
                _list[23] = "正南";
            else if (vs[21] > 180 && vs[21] < 270)
                _list[23] = "西南 " + (vs[21] - 180).ToString() + "°";
            else if (vs[21] == 270)
                _list[23] = "正西";
            else if (vs[21] > 270)
                _list[23] = "西北 " + (vs[21] - 270).ToString() + "°";
        }
    }
}
