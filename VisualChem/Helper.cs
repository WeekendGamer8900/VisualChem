using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace VisualChem
{
    public static class Helper
    {
        static Random rng = new Random();
        public static bool isNumeric(char c)
        {
            return (c >= '0' && c <= '9');
        }
        public static bool isAlpha(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
        }
        public static bool LookForward(string s, string t, int loc)
        {
            return s.Substring(loc, t.Length) == t;
        }
        public static string GetNumForward(string s, ref int loc)
        {
            string ret = s[loc].ToString();
            while (loc + 1 < s.Length && isNumeric(s[loc + 1]))
            {
                ret += s[loc + 1];
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
        public static PointF F(this Point P)
        {
            return new PointF(P.X, P.Y);
        }
        public static float Magnitude(this PointF p)
        {
            return (float)Math.Sqrt(p.X * p.X + p.Y * p.Y);
        }
        public static PointF Normalize(this PointF p)
        {
            if (p.Magnitude().Abs() < 0.01)
            {
                return new PointF(0, 0);
            }
            return new PointF(p.X / p.Magnitude(), p.Y / p.Magnitude());
        }
        public static PointF Scale(this PointF p, float x)
        {
            return new PointF(p.X * x, p.Y * x);
        }
        public static PointF Add(this PointF p, PointF q)
        {
            return new PointF(p.X + q.X, p.Y + q.Y);
        }
        public static PointF Invert(this PointF p)
        {
            return new PointF(-p.X, -p.Y);
        }
        public static PointF Subtract(this PointF p, PointF q)
        {
            return p.Add(q.Invert());
        }
        public static float Distance(this PointF p, PointF q)
        {
            return p.Subtract(q).Magnitude();
        }
        public static float Sqr(this float p)
        {
            return (float)Math.Pow(p, 2);
        }
        public static float Abs(this float p)
        {
            return Math.Abs(p);
        }
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        public static float Rnd(float low, float high)
        {
            return (float)(rng.NextDouble() * (high - low) + low);
        }
    }
}
