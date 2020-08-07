using Microsoft.AspNetCore.Http;

namespace CoolShop.Core.Enum
{
    public enum StatusCodeEnum
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal = StatusCodes.Status200OK,

        /// <summary>
        /// 参数错误
        /// </summary>
        ArgumentErr = StatusCodes.Status403Forbidden,

        /// <summary>
        /// 逻辑错误
        /// </summary>
        LogicErr = StatusCodes.Status503ServiceUnavailable,

        /// <summary>
        /// 异常错误
        /// </summary>
        AbNormal = StatusCodes.Status500InternalServerError,
    }
}