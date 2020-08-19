using System;
using CoolShop.Admin.Entities;
using CoolShop.Core.Enum;
using CoolShop.Core.Extend;
using CoolShop.Core.Library;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Exception = CoolShop.Core.Extend.Exception;

namespace CoolShop.Admin.Attributes
{
    public class AuthAttribute : Attribute, IActionFilter
    {
        public string Ctr { get; set; } = "Index";

        public string Act { get; set; } = "Index";

        /// <summary>
        /// 是否是超级管理员
        /// </summary>
        public bool IsRoot { get; set; }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                var sessionJson = context.HttpContext.Session.GetString(Session.AdminKey);
                if (string.IsNullOrWhiteSpace(sessionJson))
                {
                    throw new Exception("该管理员未登录", StatusCodeEnum.LogicErr);
                }

                var sessionObj = JsonConvert.DeserializeObject<AdminSessionEntity>(sessionJson!);
                if (sessionObj.IsRoot == 0)
                {
                    if (IsRoot)
                    {
                        throw new Exception("该管理员未登录", StatusCodeEnum.LogicErr);
                    }

                    if (Ctr.ToLower() != "index" || Act.ToLower() != "index")
                    {
                        var key = Ctr.Md5(Act);
                        if (!sessionObj.MenuKeys.Contains(key))
                        {
                            throw new Exception("该管理员未登录", StatusCodeEnum.LogicErr);
                        }
                    }
                }

                sessionObj.CurrentCtr = Ctr;
                sessionObj.CurrentAct = Act;
                if (context.Controller is Controller controller)
                {
                    controller.ViewBag.AdminSession = sessionObj;
                }
            }
            catch (Exception e)
            {
                throw new Exception("该管理员未登录", StatusCodeEnum.LogicErr);
            }
        }
    }
}