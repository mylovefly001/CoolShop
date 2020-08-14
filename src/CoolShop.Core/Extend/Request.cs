using System.Linq;
using Microsoft.AspNetCore.Http;

namespace CoolShop.Core.Extend
{
    public static class Request
    {
        /// <summary>
        /// 获取真实的IP4地址
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string RealIp4(this HttpRequest request)
        {
            var ip = "";
            if (request.Headers.ContainsKey("X-Real-IP"))
            {
                ip = request.Headers["X-Real-IP"].FirstOrDefault();
            }

            if (request.Headers.ContainsKey("X-Forwarded-For"))
            {
                ip = request.Headers["X-Forwarded-For"].FirstOrDefault();
            }

            if (string.IsNullOrWhiteSpace(ip))
            {
                ip = request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            }

            return ip;
        }
    }
}