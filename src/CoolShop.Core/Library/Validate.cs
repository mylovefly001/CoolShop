using System.Text.RegularExpressions;
using CoolShop.Core.Enum;
using CoolShop.Core.Extend;

namespace CoolShop.Core.Library
{
    public static class Validate
    {
        /// <summary>
        /// 邮箱的正则
        /// </summary>
        private static readonly Regex Email = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");

        /// <summary>
        /// 帐号(用户名)的正则
        /// </summary>
        private static readonly Regex Account = new Regex(@"/^[a-z0-9_-]{4,16}$/");

        /// <summary>
        /// 手机号正则
        /// </summary>
        private static readonly Regex Mobile = new Regex(@"/^1[3456789]\d{9}$/");

        /// <summary>
        /// 验证密码
        /// </summary>
        /// <param name="str"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string CheckPass(this string str, string msg)
        {
            if (str.Length < 8 || str.Length > 16)
            {
                throw new Exception(msg, StatusCodeEnum.ArgumentErr);
            }

            return str;
        }

        /// <summary>
        /// 验证帐号(用户名)
        /// </summary>
        /// <param name="str"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string CheckAccount(this string str, string msg)
        {
            if (Account.Match(str).Success)
            {
                throw new Exception(msg, StatusCodeEnum.ArgumentErr);
            }

            return str;
        }


        /// <summary>
        /// 检验邮箱
        /// </summary>
        /// <param name="str"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string CheckEmail(this string str, string msg)
        {
            if (!Email.Match(str).Success)
            {
                throw new Exception(msg, StatusCodeEnum.ArgumentErr);
            }

            return str;
        }

        /// <summary>
        /// 校验必填值
        /// </summary>
        /// <param name="str"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string CheckRequired(this string str, string msg)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                throw new Exception(msg, StatusCodeEnum.ArgumentErr);
            }

            return str;
        }

        /// <summary>
        /// 校验ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static int CheckId(this int id, string msg)
        {
            if (id <= 0)
            {
                throw new Exception(msg, StatusCodeEnum.ArgumentErr);
            }

            return id;
        }

        /// <summary>
        /// 校验手机号
        /// </summary>
        /// <param name="str"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string CheckMobile(this string str, string msg)
        {
            if (!Mobile.Match(str).Success)
            {
                throw new Exception(msg, StatusCodeEnum.ArgumentErr);
            }

            return str;
        }
    }
}