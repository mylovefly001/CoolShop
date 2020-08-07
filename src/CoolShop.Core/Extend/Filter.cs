using CoolShop.Core.Entity;
using CoolShop.Core.Enum;
using CoolShop.Core.Library;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CoolShop.Core.Extend
{
    public class Filter : IExceptionFilter, IResultFilter, IActionFilter
    {
        /// <summary>
        /// 返回并设置状态码
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private static JsonResult SetStatusCode(ResultEntity entity)
        {
            return new JsonResult(entity)
            {
                StatusCode = (int) entity.Code
            };
        }

        public void OnException(ExceptionContext context)
        {
            var entity = Common.Result(context.Exception.Message, context.Exception is Exception ex ? ex.Code : StatusCodeEnum.AbNormal);
            context.Result = SetStatusCode(entity);
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (!(context.Result is JsonResult result))
            {
                return;
            }

            if (!(result.Value is ResultEntity entity))
            {
                return;
            }

            context.Result = SetStatusCode(entity);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }
    }
}