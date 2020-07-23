using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CoolShop.Core.Entity;
using CoolShop.Core.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace CoolShop.Core.Library
{
    public static class Common
    {
         /// <summary>
        /// 返回统一的数据格式
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static ResultEntity Result(dynamic obj = null, ErrorEnum error = ErrorEnum.Normal)
        {
            var result = new ResultEntity();
            if (obj != null && obj.GetType().ToString() != typeof(bool).FullName)
            {
                switch (obj)
                {
                    case string _:
                        result.Code = error;
                        result.Msg = obj.ToString();
                        break;
                    default:
                        result.Data = obj;
                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="val"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string Md5(this string val, string salt = "")
        {
            using var md5 = MD5.Create();
            var result = md5.ComputeHash(Encoding.UTF8.GetBytes($"{val}:{salt}"));
            return BitConverter.ToString(result).Replace("-", "");
        }

        /// <summary>
        /// 生成指定位数的随机数
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string CreateRandom(int length = 6)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < length; i++)
            {
                var r = new Random(Guid.NewGuid().GetHashCode());
                sb.Append(r.Next(0, 10));
            }

            return sb.ToString();
        }

        /// <summary>
        /// 时间戳转日期
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime ToLocalTimeTime(this long dt)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(dt).ToLocalTime().DateTime;
        }

        /// <summary>
        /// 日期转时间戳
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public static long ToUnixTimeStamp(this DateTime dt, bool milliseconds = false)
        {
            var dto = new DateTimeOffset(dt);
            return milliseconds ? dto.ToUnixTimeMilliseconds() : dto.ToUnixTimeSeconds();
        }

        /// <summary>
        /// JSON格式化数据
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string EncoderJson(this object o)
        {
            o ??= new JArray();
            return JsonConvert.SerializeObject(o, new JsonSerializerSettings
            {
                DateFormatString = "yyyy-MM-dd HH:mm:ss",
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }

        /// <summary>
        /// 返格式化JSON
        /// </summary>
        /// <param name="json"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T DecoderJson<T>(this string json) where T : new()
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// 输出字符串
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static async Task EchoMsgAsync(string msg)
        {
            await Console.Out.WriteLineAsync(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + $"\t{msg}");
        }

        public static void EchoMsg(string msg)
        {
            Console.Out.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + $"\t{msg}");
        }
    }
}