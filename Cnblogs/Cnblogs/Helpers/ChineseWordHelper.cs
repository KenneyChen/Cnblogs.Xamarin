// ===============================================================================
// ChineseWordHelper.cs
// 功能:汉字长度帮助类
// 作者:shuifeng
// 时间:2011-12-31
// ===============================================================================

using System.Text.RegularExpressions;
using System;

namespace Cnblogs.Service
{
    public class ChineseWordHelper
    {
        /// <summary>
        /// 获取文字所有的数据（汉字为2，英文数字为1）
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static int GetWordCount(string word)
        {
            Regex regNickname = new Regex(@"[\u4e00-\u9fa5]", RegexOptions.IgnoreCase);

            int k = 0;//名字的所有字节的数目（汉字为2，英文数字为1）
            for (int i = 0; i < word.Length; i++)
            {
                if (regNickname.IsMatch(word[i].ToString()))//如果是汉字
                {
                    k += 2;
                }
                else if (Char.IsUpper(word[i]) || word[i] == 'm' || word[i] == 'w')//如果是大写字母、或是m、w
                {
                    k += 2;
                }
                else
                {
                    k += 1;
                }
            }
            return k;
        }

        /// <summary>
        /// 对超过长度个数的的汉字加省略号
        /// </summary>
        /// <param name="orginString">原始字符串</param>
        /// <param name="n">汉字个数</param>
        /// <returns></returns>
        public static string GetString(string orginString, int n)
        {
            if (orginString.Length>n)
            {
                int doubleCount = 2 * n;
                Regex regNickname = new Regex(@"[\u4e00-\u9fa5]", RegexOptions.IgnoreCase);

                int k = 0;//名字的所有字节的数目（汉字为2，英文数字为1）
                int j = 0;//显示长度最大的索引
                for (int i = 0; i < orginString.Length; i++)
                {
                    if (regNickname.IsMatch(orginString[i].ToString()))//如果是汉字
                    {
                        k += 2;
                    }
                    else if (Char.IsUpper(orginString[i]) || orginString[i] == 'm' || orginString[i] == 'w')//如果是大写字母、或是m、w
                    {
                        k += 2;
                    }
                    else
                    {
                        k += 1;
                    }
                    if (k == doubleCount - 1 || k == doubleCount)
                    {
                        j = i;
                    }
                }
                if (k == doubleCount)//如果正好为n个汉字，直接返回改昵称
                {
                    return orginString.Substring(0, j + 1);
                }
                if (k > doubleCount)
                {
                    return orginString.Substring(0, j) + "...";
                }  
            }
            return orginString;            
        }
    }
}
