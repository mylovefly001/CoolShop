using System;
using CoolShop.Core.Enum;
using Newtonsoft.Json.Linq;

namespace CoolShop.Core.Entity
{
    public class ResultEntity
    {
        public StatusCodeEnum Code { get; set; } = StatusCodeEnum.Normal;

        public string Msg { get; set; } = string.Empty;

        public string DebugMsg { get; set; } = string.Empty;

        public DateTime Time { get; set; } = DateTime.Now.ToLocalTime();

        public object Data { get; set; } = new JArray();
    }
}