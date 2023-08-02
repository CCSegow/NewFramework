/*
 * 可以用来检测输入是否合法
 * 或者屏蔽敏感词
 */

using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
namespace ZFramework
{
    public class RegexUtil 
    {
        public class Patterns {
            public const string AssetPath2BundleName = "/";
            public const string RemoveExtension =@"\.\S+";//移除文件扩展名

            public const string OnlyNumber = "^[0-9]*$";
            public const string OnlyCharacter = "^[A-Za-z]+$";
        }

        public static string ConvertToBundleName(string input) 
        {                        
            //var result = Regex.Replace(input, Patterns.AssetPath2BundleName, "_");
            var result = Regex.Replace(input, Patterns.RemoveExtension, "");
            return result.ToLower();
        }
        public static bool IsMatch(string input,string pattern) {
            Regex regex = new Regex(pattern);
            return regex.IsMatch(input);
        }
    }
}