using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CoolShop.Core.Enum;
using CoolShop.Core.Library;
using Microsoft.AspNetCore.Mvc;

namespace CoolShop.Web.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {
            
        }
        
         /// <summary>
        /// 返回数据
        /// </summary>
        /// <param name="val"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        protected static JsonResult Result(object val = null, ErrorEnum code = ErrorEnum.Normal)
        {
            return new JsonResult(Common.Result(val, code));
        }
        
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetParamVal(string key)
        {
            if (HttpContext.Request.Method.ToUpper(CultureInfo.CurrentCulture) == "POST")
            {
                return HttpContext.Request.Form.ContainsKey(key) ? HttpContext.Request.Form[key].FirstOrDefault() : null;
            }

            return HttpContext.Request.Query.ContainsKey(key) ? HttpContext.Request.Query[key].FirstOrDefault() : null;
        }


        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T GetParam<T>(string key)
        {
            var newVal = GetParamVal(key);
            if (string.IsNullOrWhiteSpace(newVal))
            {
                return default;
            }

            return (T) Convert.ChangeType(newVal.Trim(), typeof(T));
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="action"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T GetParam<T>(Action<T> action = null) where T : new()
        {
            var obj = new T();
            foreach (var property in typeof(T).GetProperties())
            {
                var val = GetParamVal(property.Name);
                if (!string.IsNullOrWhiteSpace(val))
                {
                    property.SetValue(obj, Convert.ChangeType(val.Trim(), property.PropertyType));
                }
            }

            action?.Invoke(obj);
            return obj;
        }
    }
}