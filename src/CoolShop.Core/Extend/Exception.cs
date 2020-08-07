using System;
using CoolShop.Core.Enum;

namespace CoolShop.Core.Extend
{
    public class Exception : ApplicationException
    {
        public override string Message { get; }

        public StatusCodeEnum Code { get; set; }

        public Exception(string msg, StatusCodeEnum code = StatusCodeEnum.AbNormal) : base(msg)
        {
            Message = msg;
            Code = code;
        }
    }
}