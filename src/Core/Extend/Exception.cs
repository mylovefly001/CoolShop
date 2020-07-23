using System;
using CoolShop.Core.Enum;

namespace CoolShop.Core.Extend
{
    public class Exception : ApplicationException
    {
        public override string Message { get; }

        public ErrorEnum Code { get; set; }

        public Exception(string msg, ErrorEnum code = ErrorEnum.AbNormal) : base(msg)
        {
            Message = msg;
            Code = code;
        }
    }
}