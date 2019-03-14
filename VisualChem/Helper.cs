using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualChem
{
    static class Helper
    {
        public static bool isNumeric(char c)
        {
            return (c >= '0' && c <= '9');
        }
        public static bool isAlpha(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
        }
        public static bool LookForward(string s,string t,int loc)
        {
            return s.Substring(loc, t.Length) == t;
        }
        public static string GetNumForward(string s,ref int loc)
        {
            string ret = s[loc].ToString();
            while (loc + 1 < s.Length && isNumeric(s[loc + 1]))
            {
                ret += s[loc];
                loc++;
            }
            return ret;
        }
        public static int ToInt(this string s)
        {
            return int.Parse(s);
        }
        public static string ToDString(this Enum val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val
               .GetType()
               .GetField(val.ToString())
               .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : val.ToString();
        }
    }
}
