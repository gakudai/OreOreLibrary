using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OreOreLibrary
{
    public static class ParserExtesions
    {
        /// <summary>
        /// 文字列をDateTime型へ変換
        /// </summary>
        /// <param name="dateString">変換したい文字列</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string dateString)
        {
            DateTime temp;
            if (DateTime.TryParse(dateString, out temp))
            {
                return temp;
            }
            return default(DateTime);
        }

        /// <summary>
        /// 文字列をInt型へ変換
        /// </summary>
        /// <param name="intString"></param>
        /// <returns></returns>
        public static int ToInt(this string intString)
        {
            int temp;
            if (int.TryParse(intString, out temp))
            {
                return temp;
            }
            return default(int);
        }
    }
}
